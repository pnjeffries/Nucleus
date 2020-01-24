using Nucleus.Base;
using Nucleus.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// Base class for text serialisers that write out documents
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    [Serializable]
    public abstract class DocumentTextSerialiser<TSource> : TextSerialiser<TSource>
        where TSource : Document
    {
        #region Properties

        /// <summary>
        /// Private backing field for CustomHeader property
        /// </summary>
        private string _CustomHeader = null;

        /// <summary>
        /// Get or set a custom header to be written before the main body of text.
        /// Can be used to include customised titles, extra header data etc.
        /// If null will be ignored.
        /// </summary>
        public string CustomHeader
        {
            get { return _CustomHeader; }
            set { _CustomHeader = value; }
        }


        /// <summary>
        /// Private backing field for CustomFooter property
        /// </summary>
        private string _CustomFooter = null;

        /// <summary>
        /// Get or set a custom footer to be written after the main body of text.
        /// Can be used to include additional data, closing tags, footer comments etc.
        /// If null will be ignored.
        /// </summary>
        public string CustomFooter
        {
            get { return _CustomFooter; }
            set { _CustomFooter = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DocumentTextSerialiser() : base() { }

        /// <summary>
        /// Format constructor
        /// </summary>
        /// <param name="format"></param>
        public DocumentTextSerialiser(TextFormat format, IStringConversionContext context = null) : base(format, context) { }

        #endregion
    }
}
