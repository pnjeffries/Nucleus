using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// A set of options to determine how a document ought to be deserialized.
    /// </summary>
    [Serializable]
    public class DocumentDeserializationOptions
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the DocumentType property
        /// </summary>
        private DocumentSaveFileType _DocumentType = DocumentSaveFileType.Binary;

        /// <summary>
        /// The document type
        /// </summary>
        public DocumentSaveFileType DocumentType
        {
            get { return _DocumentType; }
            set { _DocumentType = value; }
        }

        /// <summary>
        /// Private backing member variable for the Binder property
        /// </summary>
        private SerializationBinder _Binder = new CustomSerializationBinder();

        /// <summary>
        /// The serialization binder to be used to resolve types.
        /// </summary>
        public SerializationBinder Binder
        {
            get { return _Binder; }
            set { _Binder = value; }
        }

        #endregion
    }
}
