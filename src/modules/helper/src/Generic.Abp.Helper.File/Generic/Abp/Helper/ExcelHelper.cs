using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Generic.Abp.Helper
{
    public class ExcelHelper : ITransientDependency, IExcelHelper
    {
        public ExcelHelper(IGuidGenerator guidGenerator)
        {
            GuidGenerator = guidGenerator;
        }

        protected IGuidGenerator GuidGenerator { get; }
        public virtual Task<List<TT>> ReadData<TT>(MemoryStream stream, bool ignoreFirstRow,
            List<ExcelMapList> mapLists, int sheetIndex = 0, bool needNormalized = false) where TT : class
        {
            using var excelPackage = new ExcelPackage(stream);
            var sheet = excelPackage.Workbook.Worksheets.FirstOrDefault(m => m.Index == sheetIndex);
            var result = new List<TT>();
            if (sheet == null) return Task.FromResult(result);
            var startRow = sheet.Dimension.Start.Row + (ignoreFirstRow ? 1 : 0);
            var startColumn = sheet.Dimension.Start.Column;
            var type = typeof(TT);
            MethodInfo normalizedMethod = null;
            if (needNormalized)
            {
                var methodInfos = type.GetMethods();
                normalizedMethod = methodInfos.FirstOrDefault(m => m.Name.Equals("Normalized", StringComparison.Ordinal));

            }
            for (var i = startRow; i <= sheet.Dimension.End.Row; i++)
            {
                var tt = (TT)Activator.CreateInstance(type);
                foreach (var mapList in mapLists)
                {
                    var property = tt.GetType().GetProperty(mapList.PropertyName);
                    if (property == null) continue;
                    if (mapList.IsId)
                    {
                        property.SetValue(tt, GuidGenerator.Create(), null);
                        continue;
                    }

                    if (mapList.Column < 0)
                    {
                        property.SetValue(tt, Convert.ChangeType(mapList.Value, property.PropertyType), null);
                        continue;
                    }
                    var value = sheet.Cells[i, startColumn + mapList.Column].Value;
                    if (value == null) continue;
                    property.SetValue(tt, Convert.ChangeType(value, property.PropertyType), null);
                }

                if (normalizedMethod != null)
                {
                    normalizedMethod.Invoke(tt, null);
                }
                result.Add(tt);
            }

            return Task.FromResult(result);
        }

        public virtual Task<List<TT>> ReadData<TT>(byte[] fileBytes, bool ignoreFirstRow,
            List<ExcelMapList> mapLists, int sheetIndex = 0, bool needNormalized = false) where TT : class
        {
            var steam = new MemoryStream(fileBytes);
            return ReadData<TT>(steam, ignoreFirstRow, mapLists, sheetIndex, needNormalized);
        }
    }
}
