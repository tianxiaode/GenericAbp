using System.IO;
using System.Threading.Tasks;

namespace Generic.Abp.Helper
{
    public interface IFileHelper
    {
        Task<string> CreateFileNameAsync();
        Task<string> GetUploadPathAsync();
        Task<string> GetAccessPathAsync(string baseFilename, string additionalPath = null);
        Task<string> GetAbsolutePathAsync(string filename, string additionalPath = null);
        Task SaveFileAsync(FileStream file, string storageDirectory, string filename, string ext);
    }
}
