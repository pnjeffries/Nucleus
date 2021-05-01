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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Nucleus.Extensions;
using System.ComponentModel;
using System.Globalization;

#if !JS
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
#endif

namespace Nucleus.Base
{
    /// <summary>
    /// A structure that represents a file path.
    /// Wraps a string path variable to provide additional
    /// file handling utility methods.  Essentially provides
    /// much of the same functionality as the System.IO.Path
    /// and File classes but in a non-static way that is 
    /// quicker to use and easier to bind to.
    /// Can be used interchangably with file paths stored
    /// as strings.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(FilePathConverter))]
    public struct FilePath
#if !JS
        : IXmlSerializable
#endif
    {
#region Constants

        /// <summary>
        /// Get a filepath representing the path of the current AppData directory
        /// </summary>
        public static FilePath AppData
        {
            get
            {
                return new FilePath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }

#if !JS

        /// <summary>
        /// Get a filepath representing the path of the user's Temp directory
        /// </summary>
        public static FilePath Temp
        {
            get
            {
                return new FilePath(System.IO.Path.GetTempPath());
            }
        }

        /// <summary>
        /// Get a filepath pointing to the current working directory.
        /// Will not include a trailing slash
        /// </summary>
        public static FilePath Current
        {
            get
            {
                return new FilePath(System.IO.Directory.GetCurrentDirectory());
            }
        }

#endif

        /// <summary>
        /// Get a filepath pointing to the local application data folder for the current user
        /// </summary>
        public static FilePath LocalAppData
        {
            get
            {
                return new FilePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            }
        }

#endregion

#region Properties

        /// <summary>
        /// Private backing field for Path property
        /// </summary>
        private string _Path;

        /// <summary>
        /// The full string value of the filepath
        /// </summary>
        public string Path
        {
            get { return _Path; }
        }

        /// <summary>
        /// Is the path a valid file location?
        /// </summary>
        public bool IsValid
        {
            get { return !string.IsNullOrWhiteSpace(_Path); } //TODO: Other checks?
        }

        /// <summary>
        /// Is the path set?  (i.e. is it non-null?)
        /// </summary>
        public bool IsSet
        {
            get { return _Path != null; }
        }

        /// <summary>
        /// Does this filepath point to a directory rather than a file?
        /// </summary>
        public bool IsDirectory
        {
            get { return (File.GetAttributes(_Path) & FileAttributes.Directory) == FileAttributes.Directory; }
        }

        /// <summary>
        /// Gets the extension of this filepath.
        /// Includes the preceding '.'.
        /// </summary>
        public string Extension
        {
            get
            {
#if !JS
                return System.IO.Path.GetExtension(_Path);
#else
                return _Path.AfterLast('.');
#endif
            }
        }

#if !JS

        /// <summary>
        /// Does the file that this path points to exist?
        /// </summary>
        public bool Exists { get { return File.Exists(_Path); } }

        /// <summary>
        /// Gets the name of the file at the end of the path,
        /// including its extension but not the preceding
        /// directory structure.
        /// </summary>
        public string FileName { get { return System.IO.Path.GetFileName(_Path); } }

        /// <summary>
        /// Gets the directory of the filepath
        /// </summary>
        public string Directory { get { return System.IO.Path.GetDirectoryName(_Path); } }

        /// <summary>
        /// Get this filepath shortened to 50 characters or less
        /// </summary>
        public string Shortened { get { return Directory.TruncateMiddle(80 - FileName.Length) + "\\" + FileName; } }

        /// <summary>
        /// Get the fileInfo for the file represented by this path, if it exists
        /// If the file does not exist, returns null.
        /// </summary>
        public FileInfo Info
        {
            get
            {
                if (Exists) return new FileInfo(Path);
                else return null;
            }
        }

#else
        /// <summary>
        /// Gets the directory of the filepath
        /// </summary>
        public string Directory { get { return _Path.BeforeLast('\\'); } }

#endif

        #endregion

        #region Constructors

        /// <summary>
        /// Path Constructor
        /// </summary>
        /// <param name="path"></param>
        public FilePath(string path)
        {
            _Path = path ?? "";
        }

#endregion

#region Methods

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Path;
        }

        /// <summary>
        /// GetHashCode override
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

#if !JS

        /// <summary>
        /// Returns a version of this FilePath with it's file extension trimmed off
        /// </summary>
        /// <returns></returns>
        public FilePath TrimExtension()
        {
            return new FilePath(Directory + "\\" + System.IO.Path.GetFileNameWithoutExtension(Path));
        }

        /// <summary>
        /// Returns a version of this FilePath with the specified new extension
        /// </summary>
        /// <param name="newExtension">The extension to use.  If not preceded with a '.' one will be added automatically</param>
        /// <returns></returns>
        public FilePath ChangeExtension(string newExtension)
        {
            return TrimExtension() + (newExtension.StartsWith(".") ? newExtension : "." + newExtension);
        }

        /// <summary>
        /// Returns a copy of this FilePath with a suffix appended to the filename, inserted between the
        /// file name and extension
        /// </summary>
        /// <param name="suffix">The suffix to add to the filename</param>
        /// <returns></returns>
        public FilePath AddNameSuffix(string suffix)
        {
            return new FilePath(TrimExtension() + suffix + Extension);
        }

        /// <summary>
        /// Returns a copy of this FilePath with a suffix appended to the filename, inserted between the
        /// file name and extension
        /// </summary>
        /// <param name="suffix">The suffix to add to the filename</param>
        /// <param name="newExtension">The new file extension.  If not preceded with a '.' one will be added automatically.</param>
        /// <returns></returns>
        public FilePath AddNameSuffix(string suffix, string newExtension)
        {
            return new FilePath(TrimExtension() + suffix + (newExtension.StartsWith(".") ? newExtension : "." + newExtension));
        }

        /// <summary>
        /// Obtain a list of all filePaths contained within this one.
        /// If this filePath points to a single file, a collection containing
        /// only that file will be returned.  If this filePath points to a folder
        /// the contents of the folder will be returned.  If the recursive option
        /// is set to true, any subfolders will be exploded as well and their
        /// contents included in the resulting list.
        /// </summary>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public IList<FilePath> Explode(bool recursive = false, string extension = null)
        {
            var result = new List<FilePath>();
            return Explode(recursive, result, extension);
        }

        /// <summary>
        /// Obtain a list of all filePaths contained within this one.
        /// If this filePath points to a single file, a collection containing
        /// only that file will be returned.  If this filePath points to a folder
        /// the contents of the folder will be returned.  If the recursive option
        /// is set to true, any subfolders will be exploded as well and their
        /// contents included in the resulting list.
        /// </summary>
        /// <param name="recursive"></param>
        /// <param name="addTo">Collection to which results are to be added.
        /// Should not be null.</param>
        /// <returns></returns>
        public IList<FilePath> Explode(bool recursive, IList<FilePath> addTo, string extension = null)
        {
            if (IsDirectory)
            {
                foreach (var file in FilesInFolder(this))
                {
                    FilePath filePath = file;
                    if (recursive)
                        filePath.Explode(recursive, addTo, extension);
                    else if (extension == null || file.EndsWith(extension, true, null))
                        addTo.Add(filePath);
                }
            }
            else if (extension == null || _Path.EndsWith(extension, true, null))
                addTo.Add(this);
            return addTo;
        }
#endif

#endregion

#region Static Methods

#if !JS

        /// <summary>
        /// Return the path of the directory within which the specified loaded assembly exists
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static FilePath DirectoryOf(Assembly assembly)
        {
            return new FilePath(assembly.Location).Directory;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            _Path = reader.ReadString();
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(_Path);
        }

#endif


        /// <summary>
        /// Get all of the files in the specified folder
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        private static IList<string> FilesInFolder(FilePath folderPath)
        {
            return System.IO.Directory.GetFiles(folderPath);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Implicit to string conversion operator
        /// </summary>
        /// <param name="path"></param>
        public static implicit operator string(FilePath path)
        {
            return path.Path;
        }

        /// <summary>
        /// Implicit from string conversion operator
        /// </summary>
        /// <param name="path"></param>
        public static implicit operator FilePath(string path)
        {
            return new FilePath(path);
        }

#endregion
    }

    /// <summary>
    /// FilePath to string TypeConverter
    /// </summary>
    public class FilePathConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
                return new FilePath((string)value);

            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return value.ToString();

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
