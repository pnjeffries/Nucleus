using Nucleus.Base;
using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// Data about the format various types should be serialised to.
    /// For use with TextSerialiser
    /// </summary>
    public class TextFormat : Dictionary<Type, string>
    {
        #region Constants

        /// <summary>
        /// The symbol used within text formats to denote a step-out to the
        /// current context
        /// </summary>
        public const string CONTEXT = "*";

        /// <summary>
        /// The symbol used within text formats to denote the start of a conditional
        /// format
        /// </summary>
        public const string IF = "???";

        /// <summary>
        /// The symbol used within text formats to close the condition of a conditional
        /// section of a format
        /// </summary>
        public const string THEN = ":";

        /// <summary>
        /// The character sequence that denotes the start of a type name
        /// when loading this format from a file
        /// </summary>
        private static string _TYPE_START = ">>>";

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
        /// Write out this TextFormat to a text file
        /// </summary>
        /// <param name="filePath"></param>
        public void Save(FilePath filePath)
        {
            var writer = new StreamWriter(filePath);
            writer.Write(ToString());
            writer.Flush();
            writer.Close();
        }

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
                        if (currentFormat.Length > 0) currentFormat += Environment.NewLine;
                        currentFormat += line;
                    }
                }
            }

            // Store last:
            if (currentType != null && !string.IsNullOrWhiteSpace(currentFormat))
                this[currentType] = currentFormat;
        }

        /// <summary>
        /// Get the format string (if one exists) for the specified object
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public virtual string FormatFor(object source)
        {
            Type type = this.Keys.ClosestAncestor(source.GetType());
            if (type != null)
            {
                string format = this[type];
                return format;
            }
            else return null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<Type, string> kvp in this)
            {
                sb.Append(_TYPE_START).AppendLine(kvp.Key.Name); //TODO: Full type name?
                sb.AppendLine(kvp.Value);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        #endregion
    }
}
