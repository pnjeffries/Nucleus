using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Modals
{
    /// <summary>
    /// A paragraph of text to be displayed as part of a document
    /// </summary>
    [Serializable]
    public class Paragraph : Unique
    {
        private string _Text = "";

        /// <summary>
        /// The text to be displayed in the paragraph
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { ChangeProperty(ref _Text, value); }
        }

        /// <summary>
        /// Text constructor
        /// </summary>
        /// <param name="text"></param>
        public Paragraph(string text) 
        {
            _Text = text;
        }
    }
}
