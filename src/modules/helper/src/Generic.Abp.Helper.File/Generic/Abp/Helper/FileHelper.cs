using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Helper
{
    public class FileHelper : ITransientDependency, IFileHelper
    {
        /// <summary>
        /// 使用Guid生产文件名
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> CreateFilenameAsync()
        {
            return Task.FromResult(Guid.NewGuid().ToString("N"));
        }


        /// <summary>
        /// 根据文件名获取访问路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public virtual Task<string> GetPathByNameAsync(string filename)
        {
            var path = Path.Combine(filename.Substring(0, 2), filename.Substring(2, 2), filename.Substring(4, 2))
                .Replace("\\", "/");
            return Task.FromResult(path);
        }

        /// <summary>
        /// 根据访问路径和文件名获取删除路径
        /// </summary>
        /// <param name="accessPath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public virtual async Task<string> GetDeletePathAsync(string accessPath, string filename)
        {
            var fileNamePath = await GetPathByNameAsync(filename);
            var first = filename.Substring(0, 2);
            return Path.Combine(accessPath.Replace(fileNamePath, "").Replace("//", "/"), first).Replace("\\", "/");
        }

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="uploadPath">上传路径</param>
        /// <param name="filenamePath">文件名路径</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="datePath">日期格式路径</param>
        /// <returns></returns>
        public virtual async Task<string> GetStorageDirectoryAsync(string uploadPath, string filenamePath , string savePath = null ,  string datePath = null)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath);
            if (!string.IsNullOrEmpty(savePath)) path = Path.Combine(path, savePath);
            if (!string.IsNullOrEmpty(datePath)) path = Path.Combine(path, datePath);
            if (!string.IsNullOrEmpty(filenamePath)) path = Path.Combine(path, filenamePath);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return await Task.FromResult(path.Replace("\\", "/"));

        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="storageDirectory">存储路径</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>

        public virtual async Task SaveFileAsync(MemoryStream file, string storageDirectory, string filename)
        {
            storageDirectory = storageDirectory.Replace("\\", "/");
            if(!Directory.Exists(storageDirectory)) Directory.CreateDirectory(storageDirectory);
            var path = Path.Combine(storageDirectory, filename).Replace("\\", "/");
            await using var fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="bytes">文件字节</param>
        /// <param name="storageDirectory">存储路径</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public virtual async Task SaveFileAsync(byte[] bytes, string storageDirectory, string filename)
        {
            storageDirectory = storageDirectory.Replace("\\", "/");
            if(!Directory.Exists(storageDirectory)) Directory.CreateDirectory(storageDirectory);
            var path = Path.Combine(storageDirectory, filename).Replace("\\", "/");
            using(var file = File.Create(path)){
                await file.WriteAsync(bytes);
            }
        }

        /// <summary>
        /// 删除文件及路径
        /// </summary>
        /// <param name="dir">要删除的路径</param>
        /// <returns></returns>
        public virtual async Task DeleteFolderAsync(string dir)
        {
            dir = dir.Replace("\\", "/");
            if (!Directory.Exists(dir)) return ;
            //如果存在这个文件夹,执行递归删除
            foreach (var d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    //直接删除其中的文件
                    File.Delete(d); 

                }
                else
                {
                    await DeleteFolderAsync(d); //递归删除子文件夹 
                }
            }
            Directory.Delete(dir, true); //删除已空文件夹                 
        }


    }
}
