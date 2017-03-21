using FreeBuild.Conversion;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.IO
{
    /// <summary>
    /// Serialisation class that can write out a model document to text using a defined
    /// format.
    /// </summary>
    public class ModelDocumentTextSerialiser : DocumentTextSerialiser<ModelDocument>
    {

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ModelDocumentTextSerialiser() : base() { }

        /// <summary>
        /// Format constructor
        /// </summary>
        /// <param name="format"></param>
        public ModelDocumentTextSerialiser(TextFormat format, IStringConversionContext context = null) : base(format, context) { }

        #endregion
         
        #region Methods

        public override bool WriteAll(ModelDocument source)
        {
            if (CustomHeader != null) Write(CustomHeader);
            Write(source); // Document header
            WriteModel(source.Model); // Model data
            if (CustomFooter != null) Write(CustomFooter);
            return true;
        }

        #endregion
    }
}
