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
        /// <summary>
        /// 使用Guid生产文件名
        /// </summary>
        /// <returns></returns>
        public Task<string> CreateFileNameAsync()
        {
            return Task.FromResult(Guid.NewGuid().ToString("N"));
        }


        /// <summary>
        /// 根据文件名获取访问路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Task<string> GetPathByNameAsync(string filename)
        {
            var path = Path.Combine(filename.Substring(0, 2), filename.Substring(2, 2), filename.Substring(4, 2));
            return Task.FromResult(path);
        }

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="uploadPath">上传路径</param>
        /// <param name="filenamePath">文件名路径</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="datePath">日期格式路径</param>
        /// <returns></returns>
        public async Task<string> GetStorageDirectoryAsync(string uploadPath, string filenamePath , string savePath = null ,  string datePath = null)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath);
            if (!string.IsNullOrEmpty(savePath)) path = Path.Combine(path, savePath);
            if (!string.IsNullOrEmpty(datePath)) path = Path.Combine(path, datePath);
            if (!string.IsNullOrEmpty(filenamePath)) path = Path.Combine(path, filenamePath);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return await Task.FromResult(path);

        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="storageDirectory">存储路径</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>

        public async Task SaveFileAsync(MemoryStream file, string storageDirectory, string filename)
        {
            var path = Path.Combine(storageDirectory, filename);
            using var fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="storageDirectory">存储路径</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public async Task SaveFileAsync(byte[] file, string storageDirectory, string filename)
        {
            var stream = new MemoryStream(file);
            await SaveFileAsync(stream, storageDirectory, filename);
        }

    }
}
