// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Nucleus.Extensions;

namespace Nucleus.Excel
{
    /// <summary>
    /// Manager for data exchange with Microsoft Excel.
    /// Wraps basic Excel interop behaviour and allows interaction without
    /// the need to involve yourself with specialised Excel types.
    /// </summary>
    public class ExcelController
    {
        #region Properties

        /// <summary>
        /// Internal backing member for ExcelApp property
        /// </summary>
        private Application _ExcelApp;

        /// <summary>
        /// The Excel application that is currently being linked to
        /// </summary>
        public Application ExcelApp
        {
            get
            {
                if (_ExcelApp == null)
                {
                    _ExcelApp = new Application();
                }
                return _ExcelApp;
            }
        }

        /// <summary>
        /// Get the currently active excel workbook
        /// </summary>
        public Workbook ActiveWorkbook
        {
            get
            {
                if (ExcelApp.ActiveWorkbook == null) NewWorkbook();
                return ExcelApp.ActiveWorkbook;
            }
        }

        /// <summary>
        /// Get the currently active excel worksheet
        /// </summary>
        public Worksheet ActiveSheet
        {
            get
            {
                return ActiveWorkbook.ActiveSheet;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExcelController()
        {

        }

        /// <summary>
        /// Initialises the excel link and opens the specified workbook
        /// </summary>
        /// <param name="filePath">The path of the excel file to open</param>
        public ExcelController(string filePath) : this()
        {
            OpenWorkbook(filePath);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Make the Excel application window visible
        /// </summary>
        public void ShowExcel()
        {
            ExcelApp.Visible = true;
        }

        /// <summary>
        /// Make the Excel application window invisible
        /// </summary>
        public void HideExcel()
        {
            ExcelApp.Visible = false;
        }

        /// <summary>
        /// Release control over the Excel application.
        /// </summary>
        /// <param name="close">If true, the Excel application will be closed.
        /// If false, control will be released but the spreadsheet will remain open.</param>
        public void ReleaseExcelApp(bool close = false)
        {
            if (_ExcelApp != null)
            {
                _ExcelApp.Visible = true;
                _ExcelApp.UserControl = true;
                if (close) _ExcelApp.Quit();
                _ExcelApp = null;
            }
        }

        /// <summary>
        /// Open an excel workbook from a file
        /// </summary>
        /// <param name="filePath">The filepath to read the workbook from</param>
        /// <returns></returns>
        public bool OpenWorkbook(string filePath)
        {
            try
            {
                ExcelApp.Workbooks.Open(filePath);
                return true;
            }
            catch (COMException)
            { }
            return false;
        }

        /// <summary>
        /// Create a new excel workbook.
        /// Wraps ExcelApp.Workbooks.Add()
        /// </summary>
        /// <returns>The newly created workbook</returns>
        public void NewWorkbook(string title = null)
        {
            Workbook workbook = ExcelApp.Workbooks.Add();
            if (title != null) workbook.Title = title;
        }

        /// <summary>
        /// Save a workbook to the specified file path
        /// </summary>
        /// <param name="filePath">Optional.  The filepath to save the workbook to.
        /// If not specified the workbook will be saved to its previous location.</param>
        public void SaveWorkbook(string filePath = null)
        {
            SaveWorkbook(filePath, null);
        }

        /// <summary>
        /// Save a workbook to the specified file path
        /// </summary>
        /// <param name="filePath">Optional.  The filepath to save the workbook to.
        /// If not specified the workbook will be saved to its previous location.</param>
        /// <param name="workbook">Optional.  The workbook to save.
        /// If not specified, the active workbook will be used.</param>
        public void SaveWorkbook(string filePath, Workbook workbook)
        {
            ExcelApp.DisplayAlerts = false;
            if (workbook == null) workbook = ActiveWorkbook;
            if (filePath != null) workbook.SaveAs(filePath);
            else workbook.Save();
        }

        /// <summary>
        /// Make the worksheet with the specified name the currently active one.
        /// A new worksheet with that name will be created if it does not already exist
        /// </summary>
        /// <param name="sheetName"></param>
        public void OpenWorksheet(string sheetName)
        {
            Worksheet sheet;
            try
            {
                sheet = ActiveWorkbook.Worksheets[sheetName];
            }
            catch(COMException) //The sheet doesn't exist or couldn't be opened - so we'll make a new one
            {
                sheet = ActiveWorkbook.Worksheets.Add();
                sheet.Name = sheetName;
            }
            sheet.Select();
        }

        /// <summary>
        /// Find the index of the last used row in an Excel worksheet
        /// </summary>
        /// <param name="sheet">Optional.  The worksheet to count the number of rows in.
        /// If not specified, the currently active sheet will be used.</param>
        /// <returns>The last used row in the worksheet.  This will include rows 
        /// that have contained values at any time and so may be an overestimate.</returns>
        public long LastRow(Worksheet sheet = null)
        {
            if (sheet == null) sheet = ActiveSheet;
            Range usedRange = sheet.UsedRange;
            return usedRange.Row + usedRange.Rows.CountLarge;
        }

        /// <summary>
        /// Find the index of the last used column in an Excel worksheet
        /// </summary>
        /// <param name="sheet">Optional.  The worksheet to count the number of columns in.
        /// If not specified, the currently active sheet will be used.</param>
        /// <returns>The number of used columns in the worksheet.  This will include columns 
        /// that have contained values at any time and so may be an overestimate.</returns>
        public long LastColumn(Worksheet sheet = null)
        {
            if (sheet == null) sheet = ActiveSheet;
            Range usedRange = sheet.UsedRange;
            return usedRange.Column + usedRange.Columns.CountLarge;
        }

        /// <summary>
        /// Get the current value of the specified cell
        /// </summary>
        /// <param name="row">The row number of the cell</param>
        /// <param name="column">The column number of the cell</param>
        /// <returns>The data from within the specified cell</returns>
        public object GetCellValue(int row, int column)
        {
            return GetCellValue(row, column, null);
        }

        /// <summary>
        /// Get the current value of the specified cell
        /// </summary>
        /// <param name="row">The row number of the cell</param>
        /// <param name="column">The column number of the cell</param>
        /// <returns>The data from within the specified cell</returns>
        public TValue GetCellValue<TValue>(int row, int column)
        {
            return (TValue)Convert.ChangeType(GetCellValue(row, column), typeof(TValue));
        }

        /// <summary>
        /// Get the current value of the specified cell
        /// </summary>
        /// <param name="row">The row number of the cell</param>
        /// <param name="column">The column number of the cell</param>
        /// <param name="sheet">Optional.Specify the sheet the value is to be extracted from.</param>
        /// <returns>The data from within the specified cell</returns>
        public object GetCellValue(int row, int column, Worksheet sheet)
        {
            if (sheet == null) sheet = ActiveSheet;
            return sheet.Cells[row, column].Value;
        }

        /// <summary>
        /// Set the value of the specified cell
        /// </summary>
        /// <param name="row">The row number of the cell</param>
        /// <param name="column">The column number of the cell</param>
        /// <param name="value">The value to be placed in the cell</param>
        public void SetCellValue(int row, int column, object value)
        {
            SetCellValue(row, column, value, null);
        }

        /// <summary>
        /// Set the value of the specified cell
        /// </summary>
        /// <param name="row">The row number of the cell</param>
        /// <param name="column">The column number of the cell</param>
        /// <param name="value">The value to be placed in the cell</param>
        /// <param name="sheet">Optional.  The sheet the value is to be placed in.
        /// If not specified, the currently active sheet will be used.</param>
        public void SetCellValue(int row, int column, object value, Worksheet sheet)
        {
            if (sheet == null) sheet = ActiveSheet;
            sheet.Cells[row, column].Value = value;
        }

        /// <summary>
        /// Get all values from an Excel worksheet
        /// </summary>
        /// <param name="fromStart">If true, the returned array will include all cells starting from 0,0.
        /// Else, the start point will be the first cell (ever) used and the indexes of the array may be
        /// consequently difficult to relate back to the cell numbers.</param>
        /// <param name="sheet">Optional.  The sheet from which values are to be extracted.
        /// If not specified, the currently active worksheet will be used.</param>
        /// <returns>A two-dimensional array of objects containing the values of the specified set of cells.
        /// Note that unlike the Excel data itself, this will be zero-indexed.</returns>
        public object[,] GetCellValues(bool fromStart = false, Worksheet sheet = null)
        {
            if (sheet == null) sheet = ActiveSheet;
            Range range;
            if (fromStart)
                range = sheet.Cells[1, 1].Resize[LastRow(sheet), LastColumn(sheet)];
            else
                range = sheet.UsedRange;
            if (range.Cells.Count == 0) return new object[0, 0];
            else if (range.Cells.Count == 1) return new object[1, 1] { { range.Value } };
            else return MakeZeroIndexed(range.Value);
        }

        /// <summary>
        /// Get values from an Excel worksheet within a specified range
        /// </summary>
        /// <param name="startRow">The number of the starting row</param>
        /// <param name="startColumn">The number of the starting column</param>
        /// <param name="endRow">The number of the ending row</param>
        /// <param name="endColumn">The number of the ending column</param>
        /// <param name="sheet">Optional.  The sheet from which values are to be extracted.
        /// If not specified, the currently active worksheet will be used.</param>
        /// <returns>A two-dimensional array of objects containing the values of the specified set of cells.
        /// Note that unlike the Excel data itself, this will be zero-indexed.</returns>
        public object[,] GetCellValues(int startRow, int startColumn, int endRow, int endColumn)
        {
            return GetCellValues(startRow, startColumn, endRow, endColumn, null);
        }

        /// <summary>
        /// Get values from an Excel worksheet within a specified range
        /// </summary>
        /// <param name="startRow">The number of the starting row</param>
        /// <param name="startColumn">The number of the starting column</param>
        /// <param name="endRow">The number of the ending row</param>
        /// <param name="endColumn">The number of the ending column</param>
        /// <param name="sheet">Optional.  The sheet from which values are to be extracted.
        /// If not specified, the currently active worksheet will be used.</param>
        /// <returns>A two-dimensional array of objects containing the values of the specified set of cells.
        /// Note that unlike the Excel data itself, this will be zero-indexed.</returns>
        public object[,] GetCellValues(int startRow, int startColumn, int endRow, int endColumn, Worksheet sheet)
        {
            if (sheet == null) sheet = ActiveSheet;
            Range range = sheet.Cells[startRow, startColumn].Resize[endRow - startRow + 1, endColumn - startColumn + 1];
            if (range.Cells.Count == 0) return new object[0, 0];
            else if (range.Cells.Count == 1) return new object[1, 1] { { range.Value } };
            else return MakeZeroIndexed(range.Value);
        }

        /// <summary
        /// Set the values of a block of cells by copying in the values from an array
        /// </summary>
        /// <param name="startRow">The row number of the cell to put the first value into</param>
        /// <param name="startColumn">The column number of the cell to put the first value into</param>
        /// <param name="values">An array of values to be placed into the cells, ordered by row, column</param>
        /// <param name="sheet">Optional.  Specify the sheet the values are to be placed in.</param>
        public void SetCellValues<T>(int startRow, int startColumn, T[,] values, Worksheet sheet = null)
        {
            if (sheet == null) sheet = ActiveSheet;
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    sheet.Cells[startRow + i, startColumn + i] = values[i, j];
                }
            }
        }

        /// <summary>
        /// Set the values of a column of cells to the specifed set of values.
        /// </summary>
        /// <param name="startRow">The row number of the cell to begin from</param>
        /// <param name="column">The column number to place the values into.</param>
        /// <param name="values">The set of values to be written into the column of cells.</param>
        /// <param name="sheet">Optional.  Specify the sheet the values are to be placed into.</param>
        public void SetColumnValues(int startRow, int column, IEnumerable<object> values)
        {
            SetColumnValues(startRow, column, values, null);
        }

        /// <summary>
        /// Set the values of a column of cells to the specifed set of values.
        /// </summary>
        /// <param name="startRow">The row number of the cell to begin from</param>
        /// <param name="column">The column number to place the values into.</param>
        /// <param name="values">The set of values to be written into the column of cells.</param>
        /// <param name="sheet">Optional.  Specify the sheet the values are to be placed into.</param>
        public void SetColumnValues(int startRow, int column, IEnumerable<object> values, Worksheet sheet)
        {
            if (sheet == null) sheet = ActiveSheet;
            int count = 0;
            foreach (string value in values)
            {
                sheet.Cells[startRow + count, column] = value;
                count++;
            }
        }

        /// <summary>
        /// Set the values of a row of cells to the specifed set of values.
        /// </summary>
        /// <param name="row">The row number of the row to write the values into.</param>
        /// <param name="startColumn">The column number to begin from.</param>
        /// <param name="values">The set of values to be written into the row of cells.</param>
        /// <param name="sheet">Optional.  Specify the sheet the values are to be placed into.</param>
        public void SetRowValues(int row, int startColumn, IEnumerable<object> values, Worksheet sheet = null)
        {
            if (sheet == null) sheet = ActiveSheet;
            int count = 0;
            foreach (string value in values)
            {
                sheet.Cells[row, startColumn + count] = value;
                count++;
            }
        }

        /// <summary>
        /// Convert an object array obtained from an Excel range (which will be indexed starting from 1)
        /// into a normal .NET array starting from 0 (as nature intended)
        /// </summary>
        /// <param name="excelData">The data extracted from Range.Value</param>
        /// <returns></returns>
        public object[,] MakeZeroIndexed(object[,] excelData)
        {
            object[,] result = new object[excelData.GetLength(0), excelData.GetLength(1)];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = excelData[i + 1, j + 1];
                }
            }
            return result;
        }

        #endregion

    }
}
