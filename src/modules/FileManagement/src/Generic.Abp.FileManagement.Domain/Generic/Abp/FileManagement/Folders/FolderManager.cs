using Generic.Abp.Extensions.Trees;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.Folders;

public class FolderManager : TreeManager<Folder, IFolderRepository>
{
    public FolderManager(IFolderRepository repository, ITreeCodeGenerator<Folder> treeCodeGenerator,
        ICancellationTokenProvider cancellationTokenProvider) : base(repository, treeCodeGenerator,
        cancellationTokenProvider)
    {
    }
}