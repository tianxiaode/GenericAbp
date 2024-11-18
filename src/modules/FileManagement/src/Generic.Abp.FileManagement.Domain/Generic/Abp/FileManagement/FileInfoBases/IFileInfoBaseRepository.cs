using System;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.FileInfoBases;

public interface IFileInfoBaseRepository : IRepository<FileInfoBase, Guid>
{
}