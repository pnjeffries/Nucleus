using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Excel
{
    /// <summary>
    /// A collection of helper utility functions relevant to interaction with Excel
    /// (but not actually requiring Excel itself to be accessed)
    /// </summary>
    public static class ExcelHelper
    {
        #region Static Methods

        /// <summary>
        /// Extract the row number from the specified Excel cell reference as an integer
        /// </summary>
        /// <param name="cellRef">The excel cell reference in column-letter, row-number format</param>
        /// <returns>The row number as an integer</returns>
        public static int RowNumber(string cellRef)
        {
            string numbers = cellRef.TrimNonNumeric();
            return int.Parse(numbers);
        }

        /// <summary>
        /// Extract the column number form the specified Excel cell reference as an integer
        /// </summary>
        /// <param name="cellRef">The excel cell reference in column-letter, row-number format</param>
        /// <returns>The column number as an integer</returns>
        public static int ColumnNumber(string cellRef)
        {
            string letters = cellRef.TrimNonLetters();
            int result = 0;
            for (int i = 0; i < letters.Length; i++)
            {
                result *= 26;
                result += (letters[i] - 'A' + 1);
            }
            return result;
        }

        /// <summary>
        /// Convert the cell coordinates into an Excel letter-number cell reference
        /// </summary>
        /// <param name="columnIndex">The column index (starting from 1)</param>
        /// <param name="rowIndex">The row index (starting from 1)</param>
        /// <returns>The converted cell reference</returns>
        public static string CellReference(int columnIndex, int rowIndex)
        {
            string result = "";
            while (columnIndex > 0)
            {
                int modulo = (columnIndex - 1) % 26;
                result = Convert.ToChar('A' + modulo).ToString() + result;
                columnIndex = (int)((columnIndex - modulo) / 26);
            }
            result = result + rowIndex;
            return result;
        }

        #endregion
    }
}
