using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using W = Microsoft.Office.Interop.Word;
using Nucleus.Base;
using Microsoft.Office.Core;
using Nucleus.Rendering;
using Nucleus.Logs;

namespace Nucleus.Word
{
    /// <summary>
    /// Controller class that wraps the Word API to facilitate easier
    /// sequential writing to a Word document.
    /// </summary>
    public class WordController : ILog
    {
        #region Fields

        /// <summary>
        /// Bold writing flag
        /// </summary>
        private bool _Bold = false;

        /// <summary>
        /// Italics writing flag
        /// </summary>
        private bool _Italics = false;

        /// <summary>
        /// Underline writing flag
        /// </summary>
        private bool _Underline = false;

        /// <summary>
        /// Superscript writing flag
        /// </summary>
        private bool _Superscript = false;

        /// <summary>
        /// Subscript writing flag
        /// </summary>
        private bool _Subscript = false;

        /// <summary>
        /// Bullet points writing flag
        /// </summary>
        private bool _Bullets = false;

        /// <summary>
        /// The currently active table
        /// </summary>
        private Table _Table = null;

        #endregion

        #region Properties

        /// <summary>
        /// Get or set whether text is written in bold
        /// </summary>
        public bool IsBold
        {
            get { return _Bold; }
            set { _Bold = value; }
        }

        /// <summary>
        /// Get or set whether text is written in italics
        /// </summary>
        public bool IsItalicised
        {
            get { return _Italics; }
            set { _Italics = value; }
        }

        /// <summary>
        /// Private backing field for the Word property
        /// </summary>
        private Application _Word;

        /// <summary>
        /// The Word Application
        /// </summary>
        protected Application Word
        {
            get
            {
                if (_Word == null) _Word = new Application();
                return _Word;
            }
        }

        /// <summary>
        /// The currently active document
        /// </summary>
        protected W.Document ActiveDocument
        {
            get { return Word.ActiveDocument; }
        }

        /// <summary>
        /// The current text colour
        /// </summary>
        public Colour TextColour { get; set; } = Colour.Black;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public WordController() { }

        /// <summary>
        /// Initialise the WordController, specifying a file to be opened
        /// </summary>
        /// <param name="filePath"></param>
        public WordController(FilePath filePath)
        {
            OpenDocument(filePath);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Release and close Word
        /// </summary>
        public void ReleaseWord()
        {
            if (_Word != null) _Word.Quit();
            _Word = null;
        }

        /// <summary>
        /// Create a new document and set it as the currently active document
        /// </summary>
        public void NewDocument()
        {
            Word.Documents.Add().Activate();
        }

        /// <summary>
        /// Create a new document from a template and set it as the currently active
        /// document
        /// </summary>
        /// <param name="template"></param>
        public void NewDocument(FilePath template)
        {
            Word.Documents.Add(template.ToString(), false, WdNewDocumentType.wdNewBlankDocument, true);
        }

        /// <summary>
        /// Clear the contents of the currently active document by deleting everything
        /// </summary>
        public void ClearDocument()
        {
            ActiveDocument.Content.Delete();
        }

        /// <summary>
        /// Open a word document from a file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>True if opened successfully, else false</returns>
        public bool OpenDocument(FilePath filePath)
        {
            try
            {
                object fileName = (string)filePath;
                Word.Documents.Open(ref fileName);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Save the currently open document to a file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool SaveDocument(FilePath filePath)
        {
            try
            {
                ActiveDocument.SaveAs2(filePath.Path);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Get all of the text from the currently open document
        /// </summary>
        /// <returns></returns>
        public string GetDocumentText()
        {
            var sb = new StringBuilder();
            W.Document doc = ActiveDocument;
            for (int i = 0; i < doc.Paragraphs.Count; i++)
            {
                sb.AppendLine(doc.Paragraphs[i + 1].Range.Text);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Set the following text to be bold (or not).
        /// Returns this controller, to allow chaining.
        /// </summary>
        /// <param name="value"></param>
        public WordController Bold(bool value = true)
        {
            _Bold = value;
            return this;
        }

        /// <summary>
        /// Set the following text to be italic (or not).
        /// Returns this controller, to allow chaining.
        /// </summary>
        /// <param name="value"></param>
        public WordController Italics(bool value = true)
        {
            _Italics = value;
            return this;
        }

        /// <summary>
        /// Set the following text to be superscript (or not).
        /// Returns this controller, to allow chaining.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WordController Super(bool value = true)
        {
            _Superscript = true;
            return this;
        }

        /// <summary>
        /// Set the following text to be subscript (or not).
        /// Returns this controller, to allow chaining.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WordController Sub(bool value = true)
        {
            _Subscript = true;
            return this;
        }

        /// <summary>
        /// Set the following text to be underlined (or not).
        /// Returns this controller, to allow chaining.
        /// </summary>
        /// <param name="value"></param>
        public WordController Underline(bool value = true)
        {
            _Underline = value;
            return this;
        }

        /// <summary>
        /// Set the following text to be bulleted (or not).
        /// Returns this controller, to allow chaining.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WordController Bullets(bool value = true)
        {
            _Bullets = value;
            return this;
        }

        /// <summary>
        /// Restore the current writing flags such that the following text
        /// will be written in normal style.
        /// Reverts any calls to Bold(), Italic(), Super(), etc.
        /// </summary>
        /// <returns></returns>
        public WordController Normal()
        {
            _Bold = false;
            _Italics = false;
            _Subscript = false;
            _Superscript = false;
            _Underline = false;
            _Bullets = false;
            return this;
        }

        private Range WriteInternal(string text)
        {
            W.Document doc = ActiveDocument;
            Range range = EndRange();
            range.InsertAfter(text);
            ApplyStyle(range);
            return range;
        }

        /// <summary>
        /// Write text to the end of the active document.
        /// Returns this controller, to allow chaining.
        /// </summary>
        /// <param name="text"></param>
        public WordController Write(string text)
        {
            WriteInternal(text);
            return this;
        }

        /// <summary>
        /// Write text in bold.
        /// </summary>
        /// <param name="boldText"></param>
        /// <returns></returns>
        public WordController WriteBold(string boldText)
        {
            bool original = _Bold;
            _Bold = true;
            Write(boldText);
            _Bold = original;
            return this;
        }

        /// <summary>
        /// Write text in italics.
        /// </summary>
        /// <param name="italicisedText"></param>
        /// <returns></returns>
        public WordController WriteItalics(string italicisedText)
        {
            bool original = _Italics;
            _Italics = true;
            Write(italicisedText);
            _Italics = original;
            return this;
        }

        /// <summary>
        /// Write text in superscript.
        /// </summary>
        /// <param name="superScript"></param>
        /// <returns></returns>
        public WordController WriteSuper(string superScript)
        {
            bool original = _Superscript;
            _Superscript = true;
            Write(superScript);
            _Superscript = original;
            return this;
        }

        /// <summary>
        /// Write text in subscript.
        /// </summary>
        /// <param name="subScript"></param>
        /// <returns></returns>
        public WordController WriteSub(string subScript)
        {
            bool original = _Subscript;
            _Subscript = true;
            Write(subScript);
            _Subscript = original;
            return this;
        }

        /// <summary>
        /// Write a line of text followed by the newline character
        /// </summary>
        /// <param name="text"></param>
        public WordController WriteLine(string text)
        {
            return Write(text + Environment.NewLine);
        }

        /// <summary>
        /// Write a carriage return to the document
        /// </summary>
        public WordController WriteLine()
        {
            return Write(Environment.NewLine);
        }

        /// <summary>
        /// Write multiple carriage returns to the document in one go
        /// </summary>
        /// <param name="number">The number of new lines to insert.</param>
        /// <returns></returns>
        public WordController WriteLines(int number)
        {
            for (int i = 0; i < number; i++)
            {
                WriteLine();
            }
            return this;
        }

        /// <summary>
        /// Write a page break to the document
        /// </summary>
        /// <returns></returns>
        public WordController PageBreak()
        {
            EndRange().InsertBreak(WdBreakType.wdPageBreak);
            return this;
        }

        /// <summary>
        /// Write text to the document as a title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public WordController Title(string title)
        {
            Range range = WriteInternal(title + Environment.NewLine);
            range.set_Style(WdBuiltinStyle.wdStyleTitle);
            return this;
        }

        /// <summary>
        /// Write a paragraph of text to the document as a heading
        /// </summary>
        /// <param name="heading"></param>
        /// <returns></returns>
        public WordController Heading1(string heading)
        {
            Range range = WriteInternal(heading + Environment.NewLine);
            range.set_Style(WdBuiltinStyle.wdStyleHeading1);
            return this;
        }

        /// <summary>
        /// Write a paragraph of text to the document as a heading
        /// </summary>
        /// <param name="heading"></param>
        /// <returns></returns>
        public WordController Heading2(string heading)
        {
            Range range = WriteInternal(heading + Environment.NewLine);
            range.set_Style(WdBuiltinStyle.wdStyleHeading2);
            return this;
        }

        /// <summary>
        /// Write a paragraph of text to the document as a heading
        /// </summary>
        /// <param name="heading"></param>
        /// <returns></returns>
        public WordController Heading3(string heading)
        {
            Range range = WriteInternal(heading + Environment.NewLine);
            range.set_Style(WdBuiltinStyle.wdStyleHeading3);
            return this;
        }



        /// <summary>
        /// Write a list of strings out as a set of bullet points.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public WordController WriteBulletPoints(IList<string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string item in list) sb.AppendLine(item);
            Range range = WriteInternal(sb.ToString());
            range.ListFormat.ApplyBulletDefault(Type.Missing);
            return this;
        }

        /// <summary>
        /// Write an image to the end of the active document
        /// </summary>
        /// <param name="filePath"></param>
        public WordController WriteImage(FilePath filePath)
        {
            W.Document doc = ActiveDocument;
            Range range = EndRange();
            InlineShape iShape = doc.InlineShapes.AddPicture(filePath.Path, false, true, range);
            W.Shape shape = iShape.ConvertToShape();
            shape.Left = (float)WdShapePosition.wdShapeCenter;
            shape.WrapFormat.Type = WdWrapType.wdWrapInline;
            iShape.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            return this;
        }

        /// <summary>
        /// Write text to a cell in the currently active table.
        /// You must have called BeforeTable before this in order to be
        /// able to use this function
        /// </summary>
        /// <param name="row">The row number (starting from 1)</param>
        /// <param name="column">The column number (starting from 1)</param>
        /// <param name="text">The text to write</param>
        /// <returns></returns>
        public WordController WriteCell(int row, int column, string text)
        {
            if (_Table != null)
            {
                Cell cell = _Table.Cell(row, column);
                ApplyStyle(cell.Range);
                cell.Range.InsertAfter(text);
            }
            else throw new Exception("No table initialised!  BeginTable must be called before writing to cells.");
            return this;
        }

        /// <summary>
        /// Write text to a row of cells in the currently active table.
        /// You must have called BeginTable first to provide a table to write to.
        /// </summary>
        /// <param name="row">The row number (starting from 1)</param>
        /// <param name="column">The starting column number (starting from 1)</param>
        /// <param name="text">The text to write</param>
        /// <returns></returns>
        public WordController WriteCells(int row, int column, params string[] text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                WriteCell(row, i + column, text[i]);
            }
            return this;
        }

        /// <summary>
        /// Create a table that subsequent calls to WriteCell will be able to populate
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="gridLines"></param>
        /// <returns></returns>
        public WordController BeginTable(int rows, int columns, bool gridLines = true)
        {
            W.Document doc = ActiveDocument;
            Table table = doc.Tables.Add(EndRange(), rows, columns);
            _Table = table;
            if (gridLines)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        Cell cell = table.Cell(i + 1, j + 1);

                        cell.Range.Borders[WdBorderType.wdBorderLeft].LineStyle = WdLineStyle.wdLineStyleSingle;
                        cell.Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
                        cell.Range.Borders[WdBorderType.wdBorderRight].LineStyle = WdLineStyle.wdLineStyleSingle;
                        cell.Range.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
                    }
                }
            }
            return this;
        }

        /// <summary>
        /// Write a table of data to the end of the active document
        /// </summary>
        /// <param name="data"></param>
        public WordController WriteTable(string[,] data, bool gridLines = true, bool boldTop = false, bool boldLeft = false)
        {
            W.Document doc = ActiveDocument;
            int rows = data.GetLength(0);
            int columns = data.GetLength(1);
            Table table = doc.Tables.Add(EndRange(), rows, columns);
            _Table = table;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Cell cell = table.Cell(i + 1, j + 1);
                    ApplyStyle(cell.Range);

                    if (i == 0 && boldTop || j == 0 && boldLeft)
                    {
                        cell.Range.Font.Bold = 1;
                    }

                    if (gridLines)
                    {
                        cell.Range.Borders[WdBorderType.wdBorderLeft].LineStyle = WdLineStyle.wdLineStyleSingle;
                        cell.Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
                        cell.Range.Borders[WdBorderType.wdBorderRight].LineStyle = WdLineStyle.wdLineStyleSingle;
                        cell.Range.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
                    }

                    cell.Range.InsertAfter(data[i, j]);
                }
            }
            return this;
        }

        /// <summary>
        /// NOT YET FULLY IMPLEMENTED!
        /// </summary>
        /// <returns></returns>
        public WordController WriteBox()
        {
            W.Document doc = ActiveDocument;
            W.Shape shape = doc.Shapes.AddShape((int)MsoAutoShapeType.msoShapeRectangle, 0, 0, 10, 10);
            throw new NotImplementedException();
            return this;
        }

        private void ApplyStyle(Range range)
        {
            
            Style style = range.ParagraphStyle as Style;
            string name = style.NameLocal;
            if (name != "Normal") //Revert style to normal
            {
                range.set_Style(WdBuiltinStyle.wdStyleNormal);
            }
            range.Bold = _Bold ? 1 : 0;
            range.Italic = _Italics ? 1 : 0;
            range.Font.Superscript = _Superscript ? 1 : 0;
            range.Font.Subscript = _Subscript ? 1 : 0;
            Colour c = TextColour;
            range.Font.TextColor.RGB = (c.R + 0x100 * c.G + 0x10000 * c.B);
            range.Underline = _Underline ? WdUnderline.wdUnderlineSingle : WdUnderline.wdUnderlineNone;
            if (_Bullets)
                range.ListFormat.ApplyBulletDefault(Type.Missing);
        }

        protected Range EndRange()
        {
            W.Document doc = ActiveDocument;
            int end = doc.Content.End;
            return doc.Range(end - 1, end);
        }

        void ILog.WriteText(string text)
        {
            Write(text);
        }

        #endregion
    }
}
