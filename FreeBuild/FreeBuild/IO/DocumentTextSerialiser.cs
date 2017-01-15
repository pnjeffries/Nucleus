using FreeBuild.Base;
using FreeBuild.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.IO
{
    /// <summary>
    /// Base class for text serialisers that write out documents
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public abstract class DocumentTextSerialiser<TSource> : TextSerialiser<TSource>
        where TSource : Document
    {
        #region constructors

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
