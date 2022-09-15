using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Generic.Abp.Helper
{
    public interface IExcelHelper
    {
        Task<List<TT>> ReadData<TT>(MemoryStream stream, bool ignoreFirstRow,
            List<ExcelMapList> mapLists, int sheetIndex = 0, bool needNormalized = false) where TT : class;

        Task<List<TT>> ReadData<TT>(byte[] fileBytes, bool ignoreFirstRow,
            List<ExcelMapList> mapLists, int sheetIndex = 0, bool needNormalized = false) where TT : class;
    }
}
