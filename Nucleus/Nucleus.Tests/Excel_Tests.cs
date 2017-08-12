using Nucleus.Excel;
using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Tests
{
    public static class Excel_Tests
    {
        public static void ProcessTimeSheet()
        {
            var excel = new ExcelController();
            excel.ShowExcel();
            excel.OpenWorkbook("\\\\ukramlonfiler01\\adm\\0000-0999\\00020\\Business Service Areas\\Buildings and Designs\\Ramboll Computational Design\\Admin\\MaconomyDataPJ.xls");
            excel.OpenWorksheet("Exported Data");
            object[,] values = excel.GetCellValues(2, 8, 2925, 10);
            var hashSet = new HashSet<string>();
            for (int i = 0; i < values.GetLength(0); i++)
            {
                string desc = values[i, 2]?.ToString();
                if (desc != null && !hashSet.Contains(desc)) hashSet.Add(desc);
            }
            excel.OpenWorksheet("Sheet1");
            excel.SetColumnValues(1, 1, hashSet);
            excel.SaveWorkbook();
            excel.ReleaseExcelApp();
        }
    }
}
