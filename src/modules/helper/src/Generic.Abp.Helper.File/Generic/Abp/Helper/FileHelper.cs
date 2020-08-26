using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Generic.Abp.Helper
{
    public class FileHelper : ITransientDependency, IFileHelper
    {
        public FileHelper(IOptions<FileUploadOption> options)
        {
            FileUploadOption = options.Value;
        }

        protected FileUploadOption FileUploadOption { get; set; }

        protected IClock Clock { get; set; }
        public virtual Task<string> CreateFileNameAsync()
        {
            return Task.FromResult(Guid.NewGuid().ToString("N"));
        }

        public virtual async Task<string> GetUploadPathAsync()
        {
            return await Task.FromResult(FileUploadOption.FileUploadPath);
        }

        public virtual Task<string> GetAccessPathAsync(string baseFilename, string additionalPath = null)
        {
            var now = Clock.Now;
            var basePath =
                $"{now:yyyyMM}/{baseFilename.Substring(0, 2)}/{baseFilename.Substring(2, 2)}/{baseFilename.Substring(4, 2)}";
            return Task.FromResult(string.IsNullOrEmpty(additionalPath) ? basePath : $"{additionalPath}/{basePath}");
        }

        public virtual async Task<string> GetAbsolutePathAsync(string filename, string additionalPath = null)
        {
            var accessPath = (await GetAccessPathAsync(filename, additionalPath)).Replace("/", "\\\\");
            var path =
                $"{Directory.GetCurrentDirectory().Replace("\\", "\\\\")}\\\\{FileUploadOption.FileUploadPath}\\\\{accessPath}\\\\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;

        }

        public virtual async Task SaveFileAsync(FileStream file, string storageDirectory, string filename, string ext)
        {
            using var fileStream = new FileStream($"{storageDirectory}\\{filename}.{ext}", FileMode.Create);
            await file.CopyToAsync(fileStream);
        }

    }
}
