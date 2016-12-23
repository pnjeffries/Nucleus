using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.IO
{
    /// <summary>
    /// Data about the format various types should be serialised to.
    /// For use with TextSerialiser
    /// </summary>
    public class TextFormat : Dictionary<Type, string>
    {
        #region Constants

        /// <summary>
        /// The character sequence that denotes the start of a type name
        /// when loading this format from a file
        /// </summary>
        private static string _TYPE_START = ":::";

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public TextFormat() : base() { }

        /// <summary>
        /// Initialise a new TextFormat from the specified format data string
        /// </summary>
        /// <param name="format"></param>
        public TextFormat(string format) : this()
        {
            Load(format);
        }

        /// <summary>
        /// Initialise a new TextFormat, loading it from the specified filepath
        /// </summary>
        /// <param name="filePath"></param>
        public TextFormat(FilePath filePath) : this()
        {
            Load(filePath);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load type formats from a text file
        /// </summary>
        /// <param name="filePath"></param>
        public void Load(FilePath filePath)
        {
            var reader = new StreamReader(filePath);
            Load(reader);
            reader.Close();
        }

        /// <summary>
        /// Load type formats from a string
        /// </summary>
        /// <param name="formatString"></param>
        public void Load(string formatString)
        {
            Load(new StringReader(formatString));
        }

        /// <summary>
        /// Load the format via a text reader.
        /// </summary>
        /// <param name="reader"></param>
        public void Load(TextReader reader)
        {
            Type currentType = null;
            string currentFormat = "";

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (line.StartsWith(_TYPE_START))
                    {
                        // Store last:
                        if (currentType != null && !string.IsNullOrWhiteSpace(currentFormat))
                            this[currentType] = currentFormat;

                        // Start new type record:
                        line = line.Trim(':').Trim();
                        currentType = Type.GetType(line);
                        currentFormat = "";
                    }
                    else
                    {
                        currentFormat += line;
                    }
                }
            }

            // Store last:
            if (currentType != null && !string.IsNullOrWhiteSpace(currentFormat))
                this[currentType] = currentFormat;
        }

        #endregion
    }
}
