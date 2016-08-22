using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FreeBuild.Base
{
    /// <summary>
    /// A structure that represents a file path.
    /// Wraps a string path variable to provide additional
    /// file handling utility methods.  Essentially provides
    /// much of the same functionality as the System.IO.Path
    /// and File classes but in a non-static way that is 
    /// quicker to use and easier to bind to.
    /// </summary>
    [Serializable]
    public struct FilePath
    {
        #region Properties

        /// <summary>
        /// The full string value of the filepath
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Does the file that this path points to exist?
        /// </summary>
        public bool Exists { get { return File.Exists(Path); } }

        /// <summary>
        /// Gets the name of the file at the end of the path,
        /// including its extension but not the preceding
        /// directory structure.
        /// </summary>
        public string FileName { get { return System.IO.Path.GetFileName(Path); } }

        /// <summary>
        /// Gets the extension of this filepath
        /// </summary>
        public string Extension { get { return System.IO.Path.GetExtension(Path); } }

        /// <summary>
        /// Gets the directory of the filepath
        /// </summary>
        public string Directory { get { return System.IO.Path.GetDirectoryName(Path); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Path Constructor
        /// </summary>
        /// <param name="path"></param>
        public FilePath(string path)
        {
            Path = path;
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
}
