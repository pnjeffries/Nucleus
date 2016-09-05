using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using W = Microsoft.Office.Interop.Word;
using FreeBuild.Base;

namespace FreeBuild.Word
{
    public class WordController
    {
        #region Properties

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
        /// Get all of the text from 
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

        #endregion
    }
}
