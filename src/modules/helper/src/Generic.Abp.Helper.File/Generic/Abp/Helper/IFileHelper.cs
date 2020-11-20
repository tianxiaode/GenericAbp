using System.IO;
using System.Threading.Tasks;

namespace Generic.Abp.Helper
{
    public interface IFileHelper
    {
        Task<string> CreateFileNameAsync();
        //Task<string> GetUploadPathAsync();
        Task<string> GetPathByNameAsync(string filename);

        Task<string> GetStorageDirectoryAsync(string uploadPath, string filenamePath, string savePath = null,
            string datePath = null);

        Task SaveFileAsync(MemoryStream file, string storageDirectory, string filename);
        Task SaveFileAsync(byte[] file, string storageDirectory, string filename);
    }
}
