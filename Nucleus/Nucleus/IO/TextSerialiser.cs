using Nucleus.Base;
using Nucleus.Conversion;
using Nucleus.Extensions;
using Nucleus.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// Class that converts objects to text strings in a customisable format
    /// </summary>
    public abstract class TextSerialiser<TSource> : MessageRaiser
    {
        #region Fields

        /// <summary>
        /// The string builder that is used to produce the output string
        /// </summary>
        protected StringBuilder _OutputBuilder = new StringBuilder();

        #endregion

        #region Property

        /// <summary>
        /// Private backing field for Context property
        /// </summary>
        private IStringConversionContext _Context = null;

        /// <summary>
        /// The context object for string conversion
        /// </summary>
        public IStringConversionContext Context
        {
            get { return _Context; }
            set { _Context = value; }
        }

        /// <summary>
        /// Private backing field for Format property
        /// </summary>
        protected TextFormat _Format;

        /// <summary>
        /// The dictionary of format strings for different types
        /// </summary>
        public TextFormat Format
        {
            get
            {
                if (_Format == null) _Format = new TextFormat();
                return _Format;
            }
            set
            {
                _Format = value;
            }
        }

        /// <summary>
        /// Get the string value of the current output
        /// </summary>
        public string Output
        {
            get { return _OutputBuilder.ToString(); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public TextSerialiser() { }

        /// <summary>
        /// Format constructor
        /// </summary>
        /// <param name="format"></param>
        public TextSerialiser(TextFormat format, IStringConversionContext context = null)
        {
            Format = format;
            Context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the format for the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="format"></param>
        public void SetFormat(Type type, string format)
        {
            _Format[type] = format;
        }

        /// <summary>
        /// Write a line of text directly
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool Write(string line)
        {
            _OutputBuilder.AppendLine(line);
            return true;
        }

        /// <summary>
        /// Write a line of text representing an object, in the
        /// format pre-specified for that object's type.
        /// If no format is loaded for this object's type then it will be
        /// ignored.
        /// </summary>
        /// <param name="item">The item to be written.</param>
        /// <returns>True if successfully written, false if not.</returns>
        public bool Write(object item)
        {
            if (item == null) return false;
            string format = _Format.FormatFor(item);
            if (format != null)
            { 
                return Write(item.ToString(format, '{', '}', TextFormat.IF, TextFormat.THEN, Context));
            }
            else
            { 
                return false;
            }
        }

        /// <summary>
        /// Write all items in the specified collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public bool WriteAll(ICollection collection)
        {
            bool result = false;
            if (collection != null)
            {
                foreach (object obj in collection)
                {
                    if (Write(obj)) result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Write a model to text format
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool WriteModel(Model.Model model)
        {
            Write(model); // Write model header
            foreach (IEnumerable<ModelObject> table in model.AllTables)
            {
                Write(table); // Write table header
                WriteAll((ICollection)table); // Write table data
            }
            return true;
        }

        /// <summary>
        /// Write all data in the specified source object
        /// </summary>
        /// <param name="source">The source object to be written</param>
        public abstract bool WriteAll(TSource source);

        /// <summary>
        /// Clear all stored output text
        /// </summary>
        public void Clear()
        {
            _OutputBuilder.Clear();
        }

        /// <summary>
        /// Serialise the specified source object to text using
        /// the currently specified string formats
        /// </summary>
        /// <param name="source">The source object to be serialized</param>
        /// <returns></returns>
        public string Serialize(TSource source)
        {
            WriteAll(source);
            return Output;
        }

        /// <summary>
        /// Serialise the specified source object to text and
        /// write it to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="source">The source object to serialise</param>
        public void Serialize(Stream stream, TSource source, string openingText = null, string closingText = null)
        {
            if (openingText != null) Write(openingText);
            WriteAll(source);
            if (closingText != null) Write(closingText);
            var sw = new StreamWriter(stream);
            sw.Write(Output);
            sw.Dispose();
        }

        #endregion
    }
}
