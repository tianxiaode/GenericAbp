using System;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Folders;

public interface IFolderRepository : IRepository<Folder, Guid>
{
}