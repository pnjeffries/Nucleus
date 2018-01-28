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

using Nucleus.IO;
using System;
using System.IO;
using System.Runtime.Serialization;
#if !JS
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace Nucleus.Base
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
        /// Private backing field for the FilePath property
        /// </summary>
        private FilePath _FilePath = null;

        /// <summary>
        /// The filepath to which the document was last saved
        /// </summary>
        public FilePath FilePath
        {
            get { return _FilePath; }
            protected set { _FilePath = value; NotifyPropertyChanged("FilePath"); }
        }

        /// <summary>
        /// Private backing field for LastSaved property
        /// </summary>
        private DateTime _LastSaved = DateTime.UtcNow;

        /// <summary>
        /// The time and date that this document was last saved.
        /// </summary>
        public DateTime LastSaved
        {
            get { return _LastSaved; }
            set { _LastSaved = value; }
        }

#endregion

#region Methods

        /// <summary>
        /// Save this document to the last-saved location, if possible,
        /// in binary format.
        /// </summary>
        /// <returns>True if the file was successfully saved, else false</returns>
        public virtual bool Save()
        {
            if (!string.IsNullOrWhiteSpace(FilePath))
            {
                return SaveAs(FilePath);
            }
            else return false;
        }

        /// <summary>
        /// Save this document to the specified location
        /// in binary format.
        /// </summary>
        /// <param name="filePath">The filepath to save the document to</param>
        /// <returns>True if the file was successfully saved, else false</returns>
        public virtual bool SaveAs(FilePath filePath, DocumentSaveFileType type = DocumentSaveFileType.Binary)
        {

#if !JS
            try
            {
                using (Stream stream = new FileStream(filePath,
                                         FileMode.Create,
                                         FileAccess.Write, FileShare.None))
                {
                    if (type == DocumentSaveFileType.Binary)
                    {
                        IFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, this);
                    }
                    else if (type == DocumentSaveFileType.ASS)
                    {
                        var formatter = new UniqueFormatter();
                        formatter.Serialize(stream, this);
                    }
                    stream.Flush();
                    stream.Close();
                }

                FilePath = filePath; //Store filepath
                LastSaved = DateTime.UtcNow;
                return true;
            }
            catch (Exception ex)
            {
                throw new SerializationException("An error was encountered while attempting to save the document.", ex);
                //TODO: Notify user of error
            }
#endif
            return false;
        }

        /// <summary>
        /// Save this document to the specified location
        /// in text format via the specified customisable text serialiser
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="textSerialiser"></param>
        /// <returns></returns>
        public virtual bool SaveAs<T>(FilePath filePath, DocumentTextSerialiser<T> textSerialiser)
            where T : Document
        {
#if !JS
            try
            {
                Stream stream = new FileStream(filePath,
                                         FileMode.Create,
                                         FileAccess.Write, FileShare.None);
                textSerialiser.Serialize(stream, this as T);
                //stream.Flush();
                //stream.Close();
                FilePath = filePath; //Store filepath
                LastSaved = DateTime.UtcNow;
                return true;
            }
            catch
            {
                //TODO: Notify user of error
            }
#endif
            return false;
        }

        /// <summary>
        /// Serialise this object to a Base64 Binary String
        /// </summary>
        /// <returns></returns>
        public virtual byte[] ToBinary()
        {
#if !JS
            using (MemoryStream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Flush();
                stream.Position = 0;
                return stream.ToArray();
            }
#else
            return null;
#endif
        }

#endregion

#region Static Methods

        /// <summary>
        /// Load a document from a file stored in binary format
        /// </summary>
        /// <param name="filePath">The path of the file to be loaded.</param>
        /// <returns>The loaded document, if a document could indeed be loaded.
        /// Else, null.</returns>
        public static T Load<T>(FilePath filePath, DocumentSaveFileType type = DocumentSaveFileType.Binary) where T : Document
        {
            T result = null;
#if !JS
            Stream stream = new FileStream(filePath,
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read);
            stream.Seek(0, SeekOrigin.Begin);
            if (type == DocumentSaveFileType.Binary)
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Binder = new CustomSerializationBinder();
                result = formatter.Deserialize(stream) as T;
            }
            else if (type == DocumentSaveFileType.ASS)
            {
                var formatter = new UniqueFormatter();
                result = formatter.Deserialize(stream) as T;
            }
            result.FilePath = filePath;
            stream.Close();
#endif
            return result;
        }

        /// <summary>
        /// Deserialize a document of the specified type from binary data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binaryData">The binary data to deserialize</param>
        /// <returns></returns>
        public static T FromBinary<T>(byte[] binaryData) where T : Document
        {
#if !JS
            using (var stream = new MemoryStream(binaryData))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream) as T;
            }
#else
            return null;
#endif
        }

#endregion
    }
}
