using System.Linq;
using Generic.Abp.FileManagement.Resources;
using Microsoft.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore
{
    public static class FileManagementQueryableExtensions
    {
        /*

        public static IQueryable<TEntity> IncludeDetails(this IQueryable<TEntity> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Users);
        }

        */
        // public static IQueryable<Resource> IncludeDetails(this IQueryable<Resource> queryable, bool include = true)
        // {
        //     if (!include)
        //     {
        //         return queryable;
        //     }
        //
        //     return queryable
        //         .Include(m => m.Parent)
        //         .Include(m => m.Folder)
        //         .Include(m => m.FileInfoBase);
        // }
    }
}