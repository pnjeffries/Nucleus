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

using Nucleus.Base;
using Nucleus.Conversion;
using Nucleus.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A document which contains and stores a model
    /// </summary>
    [Serializable]
    public class ModelDocument : Document
    {
        #region Properties

        /// <summary>
        /// Private backing field for Model property.
        /// </summary>
        private Model _Model;

        /// <summary>
        /// The model contained within this document.
        /// </summary>
        public Model Model
        {
            get { return _Model; }
            protected set { _Model = value; }
        }

        /// <summary>
        /// Private backing field for IDMappings property
        /// </summary>
        private IDMappingsDictionary _IDMappings = null;

        /// <summary>
        /// A record of the ID mapping tables generated when reading this document from or
        /// writing it to files of other data types.
        /// This data can be used to synchronise with other data files and read/write data in a
        /// consistent form.
        /// </summary>
        public IDMappingsDictionary IDMappings
        {
            get
            {
                if (_IDMappings == null) _IDMappings = new IDMappingsDictionary();
                return _IDMappings;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// Will create a new model.
        /// </summary>
        public ModelDocument()
        {
            Model = new Model();
        }

        /// <summary>
        /// Model constructor.  Initialises a ModelDocument containing the specified model.
        /// </summary>
        /// <param name="model"></param>
        public ModelDocument(Model model)
        {
            Model = model;
        }

        /// <summary>
        /// FilePath, Model constructor.  Initialises a ModelDocument with the specified model
        /// loaded from the specified filePath.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        public ModelDocument(FilePath filePath, Model model)
        {
            FilePath = filePath;
            Model = model;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Save this document to the specified location
        /// in the specified text format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The filepath to save to</param>
        /// <param name="format">The text format to save in</param>
        /// <returns></returns>
        public virtual bool SaveAs(FilePath filePath, TextFormat format)
        {
            return SaveAs(filePath, new ModelDocumentTextSerialiser(format));
        }

        /// <summary>
        /// Merge the contents of another ModelDocument into this one.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool MergeIn(ModelDocument other)
        {
            return Model.Add(other?.Model?.Everything);
            // TODO: Merge ID maps, etc?
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Load a ModelDocument from a file stored in binary format
        /// </summary>
        /// <param name="filePath">The path of the file to be loaded.</param>
        /// <returns>The loaded document, if a document could indeed be loaded.
        /// Else, null.</returns>
        public static ModelDocument Load(FilePath filePath, DocumentSaveFileType type = DocumentSaveFileType.Binary)
        {
            return Load<ModelDocument>(filePath, type);
        }

        #endregion
    }
}
