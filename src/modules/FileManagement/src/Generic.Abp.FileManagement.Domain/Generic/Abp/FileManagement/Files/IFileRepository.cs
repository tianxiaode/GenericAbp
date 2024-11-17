using System;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Files;

public interface IFileRepository : IRepository<File, Guid>
{
}