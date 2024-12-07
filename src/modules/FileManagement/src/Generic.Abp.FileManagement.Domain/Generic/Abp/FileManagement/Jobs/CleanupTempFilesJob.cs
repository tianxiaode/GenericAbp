namespace Generic.Abp.FileManagement.Jobs;

/// <summary>
/// 清理临时文件任务，从临时文件夹中获取全部文件夹，然后获取文件夹里的meta文件，判断是否过期，过期就删除文件，并释放使用空间
/// 因为存在同文件夹的情况，因而需要用字典记录每个文件夹需要释放多少空间，并且在删除文件后，再调用ResourceManager.DecreaseUsedStorageAsync来更新使用额
/// </summary>
public class CleanupTempFilesJob
{
}