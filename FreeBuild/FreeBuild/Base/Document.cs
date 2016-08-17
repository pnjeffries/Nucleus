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
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Abstract base class for documents -
    /// objects which store persistent data and allow it to
    /// be loaded and saved to files.
    /// </summary>
    [Serializable]
    public abstract class Document : Unique
    {
        #region Properties

        /// <summary>
        /// The filepath to which the document was last saved
        /// </summary>
        public string FilePath { get; protected set; } = null;

        #endregion

        #region Methods

        /// <summary>
        /// Save this document to the specified location
        /// in binary format.
        /// </summary>
        /// <param name="filePath">The filepath to save the document to</param>
        /// <returns>True if the file was successfully saved</returns>
        public virtual bool SaveAs(string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath,
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this);
            stream.Close();
            return true;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Load a document from a file stored in binary format
        /// </summary>
        /// <param name="filePath">The path of the file to be loaded.</param>
        /// <returns>The loaded document, if a document could indeed be loaded.
        /// Else, null.</returns>
        public static Document Load(string filePath)
        {
            Document result = null;
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath,
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read);
            result = formatter.Deserialize(stream) as Document;
            stream.Close();
            return result;
        }

        #endregion
    }
}
