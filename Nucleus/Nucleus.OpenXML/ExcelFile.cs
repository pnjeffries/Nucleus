using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.OpenXML
{
    /// <summary>
    /// Class to read from and write to an OpenXML Excel file 
    /// </summary>
    [Serializable]
    public class ExcelFile : IDisposable
    {
        /// <summary>
        /// The currently active document
        /// </summary>
        private SpreadsheetDocument Document { get; set; }

        /// <summary>
        /// Initialise a new object and open a file
        /// </summary>
        /// <param name="filePath"></param>
        public ExcelFile(string filePath, bool isEditable = true)
        {
            Document = SpreadsheetDocument.Open(filePath, isEditable);
        }

        /// <summary>
        /// Get a worksheet by name
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public WorksheetPart GetWorksheet(string sheetName)
        {
            IEnumerable<Sheet> sheets = Document.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Count() == 0)
            {
                // The specified worksheet does not exist.
                return null;
            }

            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)Document.WorkbookPart.GetPartById(relationshipId);

            return worksheetPart;
        }
         
        /// <summary>
        /// Get an existing cell by reference
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="cellReference"></param>
        /// <returns></returns>
        public Cell GetCell(WorksheetPart worksheet, string cellReference)
        {
            return worksheet?.Worksheet?.Descendants<Cell>().Where(c => c.CellReference == cellReference).FirstOrDefault();
        }

        public Cell GetCell(string worksheetName, string cellReference)
        {
            var worksheet = GetWorksheet(worksheetName);
            if (worksheet == null) return null;
            return GetCell(worksheet, cellReference);
        }

        public void Dispose()
        {
            Document?.Dispose();
        }
    }
}
