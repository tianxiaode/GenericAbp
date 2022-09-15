using Generic.Abp.BusinessException.Exceptions;
using Generic.Abp.FileManagement.Exceptions;
using Generic.Abp.Helper.Extensions;
using Generic.Abp.Helper.MimeDetective;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Generic.Abp.FileManagement.Files;

public class FileManager: DomainService
{
    public FileManager(IFileRepository fileRepository, ICancellationTokenProvider cancellationTokenProvider)
    {
        FileRepository = fileRepository;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    protected IFileRepository FileRepository { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected CancellationToken CancellationToken => CancellationTokenProvider.Token;

    [UnitOfWork]
    public virtual Task<File> CreateAsync(File entity)
    {
        return FileRepository.InsertAsync(entity, true, CancellationToken);
    }

    [UnitOfWork]
    public virtual Task<File> UpdateAsync(File entity)
    {
        return FileRepository.UpdateAsync(entity, true, CancellationToken);
    }

    [UnitOfWork]
    public virtual Task DeleteAsync(File entity)
    {
        return FileRepository.DeleteAsync(m=>m.Id == entity.Id, true, CancellationToken);
    }

    [UnitOfWork]
    public virtual Task<List<File>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting,string filter)
    {
        return FileRepository.GetPagedListAsync(skipCount, maxResultCount, sorting, filter, CancellationToken);
    }

    [UnitOfWork]
    public virtual Task<List<File>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting,Expression<Func<File, bool>> predicate = null)
    {
        return FileRepository.GetPagedListAsync(skipCount, maxResultCount, sorting, predicate, CancellationToken);
    }

    [UnitOfWork]
    public virtual Task<File> GetAsync(Guid id)
    {
        return FileRepository.GetAsync(id, false, CancellationToken);
    }

    [UnitOfWork]
    public virtual Task<File> FindAsync(Expression<Func<File, bool>> predicate)
    {
        return FileRepository.FirstOrDefaultAsync(predicate, CancellationToken);
    }

    [UnitOfWork]
    public virtual async Task<File> FindByHashAsync(string hash, bool throwException = true)
    {
        if (!hash.IsAscii()) return null;
        var entity = await FileRepository.FirstOrDefaultAsync(m => m.Hash.Equals(hash), CancellationToken);
        if (entity != null) return entity;
        if (throwException) throw new EntityNotFoundException(typeof(File), hash);
        return null;
    }

    #region Get File

    public virtual async Task<byte[]> GetFileAsync(File entity, int chunkSize = FileConsts.DefaultChunkSize, int index = 0)
    {
        var filename = $"{entity.Hash}.{entity.FileType}";
        var file = Path.Combine(await GetPhysicalPathAsync(entity.Path), filename);
        if (!System.IO.File.Exists(file)) return null;
        var bytes = await System.IO.File.ReadAllBytesAsync(file, CancellationToken);
        return chunkSize == 0 ? bytes : bytes.Skip(chunkSize * index).Take(chunkSize).ToArray();
    }

    public virtual async Task<byte[]> GetThumbnailAsync(File entity)
    {
        var filename = await GetThumbnailFileNameAsync(entity.Hash);
        var dir = await GetPhysicalPathAsync(entity.Path);
        var file = Path.Combine(dir, filename);
        if (System.IO.File.Exists(file)) return await System.IO.File.ReadAllBytesAsync(file, CancellationToken);
        filename = $"{entity.Hash}.{entity.FileType}";
        file = Path.Combine(dir, filename);
        if (System.IO.File.Exists(file)) return await System.IO.File.ReadAllBytesAsync(file, CancellationToken);
        return null;

    }

    #endregion


    #region File upload 

    /// <summary>
    /// 检查文件是否已上传
    /// Check the file upload
    /// </summary>
    /// <typeparam name="TFileCheckResult"></typeparam>
    /// <param name="hash">The hash value of the file</param>
    /// <param name="size">File size</param>
    /// <param name="chunkSize">File chunk size</param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task<TFileCheckResult> CheckAsync<TFileCheckResult>(string hash, int size, int chunkSize = FileConsts.DefaultChunkSize) where TFileCheckResult: FileCheckResult
    {
        var file = await FindByHashAsync(hash,false);
        var result = new FileCheckResult(hash);
        if (file!=null)
        {
            result.SetFile(file);
            return result as TFileCheckResult;
        }

        var totalChunks = size / chunkSize;
        if (totalChunks * chunkSize < size) totalChunks++;

        result.Uploaded = new Dictionary<int, bool>();
        var dir = await GetTempPathAsync(hash);
        for (var i = 0; i < totalChunks; i++)
        {
            var filePath = Path.Combine(dir, $"{hash}_{i}").Replace("\\", "/");
            result.Uploaded.Add(i, System.IO.File.Exists(filePath));
        }

        return result as TFileCheckResult;

    }

    /// <summary>
    /// 上传文件块
    /// Upload a chunk of files
    /// </summary>
    /// <param name="hash">The hash value of the file</param>
    /// <param name="chunkBytes">A byte array of upload chunk</param>
    /// <param name="index">The index of the upload chunk</param>
    /// <returns></returns>
    /// <exception cref="TheFileNameCannotBeEmptyBusinessException"></exception>
    /// <exception cref="ValueExceedsFieldLengthBusinessException"></exception>
    [UnitOfWork]
    public virtual async Task UploadChunkAsync(string hash, byte[] chunkBytes, int index )
    {
        //如果文件存在，直接返回
        var exits = await FindByHashAsync(hash,false);
        if (exits != null) return;

        //保存文件
        var dir = await GetTempPathAsync(hash);
        Logger.LogInformation($"Chunk size:{chunkBytes.Length}");
        await SaveFileAsync(chunkBytes, dir, $"{hash}_{index}");

    }

    /// <summary>
    /// 合并文件块
    /// Merge file chunks
    /// </summary>
    /// <param name="hash">The hash value of the file</param>
    /// <param name="totalChunks">Total chunks</param>
    /// <param name="uploadPath">Save the path</param>
    /// <param name="filename">filename</param>
    /// <param name="allowTypes">Allowed file types</param>
    /// <param name="allowSize">Allowed file size</param>
    /// <param name="thumbnailSize">Thumbnail size, The default is 100k</param>
    /// <returns></returns>
    /// <exception cref="FileChunkErrorBusinessException"></exception>
    [UnitOfWork]
    public virtual async Task<File> MergeAsync(string hash, int totalChunks,string uploadPath,string filename, List<FileType> allowTypes, int allowSize, long thumbnailSize =102400)
    {

        var exits = await FindByHashAsync(hash,false);
        if (exits != null) return exits;

        var done = true;
        var dir = await GetTempPathAsync(hash);

        //检查文件块是否完整
        for (var i = 0; i < totalChunks; i++)
        {
            var filePath = Path.Combine(dir, $"{hash}_{i}");
            if (System.IO.File.Exists(filePath)) continue;
            done = false;
            break;
        }

        if (!done) throw new FileChunkErrorBusinessException();


        var dto = await SaveAsync(hash, totalChunks, uploadPath, filename,dir, allowTypes, allowSize, thumbnailSize);
        if (Directory.Exists(dir)) Directory.Delete(dir, true);


        return dto;
    }

    [UnitOfWork]
    protected virtual async Task<File> SaveAsync(string hash, int totalChunks, string uploadPath, string filename,
        string tempDir, List<FileType> allowTypes, int allowSize,long thumbnailSize)
    {
        await using var memorySteam = new MemoryStream();
        for (var i = 0; i < totalChunks; i++)
        {
            var filePath = Path.Combine(tempDir, $"{hash}_{i}");
            Logger.LogInformation($"文件合并:{filePath}");
            var bytes = await System.IO.File.ReadAllBytesAsync(filePath, CancellationToken);
            Logger.LogInformation($"文件合并:{bytes.Length}");
            await memorySteam.WriteAsync(bytes, 0, bytes.Length, CancellationToken);
        }

        var fileType = memorySteam.GetFileType(allowTypes);
        if (fileType == null) throw new InvalidFileTypeBusinessException();
        var fileSize = memorySteam.Length;
        if (fileSize > allowSize) throw new FileSizeOutOfRangeBusinessException(allowSize, fileSize);

        var filenamePath = await GetAccessPathAsync(hash);
        var path = Path.Combine(uploadPath, filenamePath).Replace("\\", "/");
        var storageDirectory = await GetPhysicalPathAsync(path, true);
        await SaveFileAsync(await memorySteam.GetAllBytesAsync(CancellationToken), storageDirectory,
            $"{hash}.{fileType.Extension}");
        memorySteam.Close();

        var entity = new File(GuidGenerator.Create(), hash, fileType.Mime, fileType.Extension, fileSize);
        entity.SetFilename(filename);
        entity.SetDescription(filename);
        entity.SetPath(path);
        await ThumbnailAsync(hash, storageDirectory,fileSize, fileType,thumbnailSize, memorySteam);

        await CreateAsync(entity);
        return entity;
    }

    public virtual async Task SaveFileAsync(byte[] bytes, string dir, string filename)
    {
        dir = dir.Replace("\\", "/");
        if(!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, filename).Replace("\\", "/");
        Logger.LogInformation($"保存文件：{path}");
        await using var file = System.IO.File.Create(path);
        await file.WriteAsync(bytes, CancellationToken);
        file.Close();
    }

    protected virtual async Task ThumbnailAsync(string hash,string storageDirectory, long size, FileType fileType,long thumbnailSize, MemoryStream stream)
    {
        if(!await IsImageAsync(fileType)) return;
        if(size < thumbnailSize) return;
        using var original = SKBitmap.Decode(stream);
        var width = original.Width;
        var height = original.Height;
        var factor = Math.Sqrt(width * height / 34000d);
        if (factor < 1)
        {
            return;
        }
        width = (int) (width / factor);
        height = (int) (height / factor);
        using var resized = original.Resize(new SKImageInfo(width, height),SKFilterQuality.High);
        if (resized == null)
        {
            return;
        }

        using var image = SKImage.FromBitmap(resized);
        var path = Path.Combine(storageDirectory, await GetThumbnailFileNameAsync(hash)).Replace("\\", "/");
        var thumbnail = new FileStream(path, FileMode.OpenOrCreate);
        image.Encode(SKEncodedImageFormat.Png,100).SaveTo(thumbnail);
        stream.Close();

    }

    public virtual Task<bool> IsImageAsync(FileType fileType)
    {
        return Task.FromResult(fileType.IsIn(new[] {MimeTypes.JPEG, MimeTypes.BMP, MimeTypes.PNG, MimeTypes.GIF}));
    }

    #endregion
    public virtual Task<string> GetAccessPathAsync(string hash)
    {
        return Task.FromResult($"{hash[..2]}/{hash.Substring(2, 2)}/{hash.Substring(4, 2)}");
    }

    public virtual Task<string> GetPhysicalPathAsync(string path, bool isCreated = false)
    {
        var dir = $"{Directory.GetCurrentDirectory().Replace("\\", "/")}/{path}/".Replace("//","/");
        if (isCreated && !Directory.Exists(path)) Directory.CreateDirectory(path);
        return Task.FromResult(dir);
    }

    protected virtual Task<string> GetTempPathAsync(string hash)
    {
        return Task.FromResult(Path.Combine(Directory.GetCurrentDirectory(), "temp",  hash));
    }

    protected virtual Task<string> GetThumbnailFileNameAsync(string hash)
    {
        return Task.FromResult($"{hash}_thumbnail.png");
    }

}