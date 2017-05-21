using FreeBuild.Base;
using FreeBuild.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.IO
{
    /// <summary>
    /// A custom serialisation class which maintains IUnique-implementing objects as unique objects which
    /// may be referenced by others and inlines everything else.  The data format is kept malleable and forwards-compatible
    /// 
    /// </summary>
    public class UniqueFormatter
    {
        #region Constants

        /// <summary> Character used to denote the opening of a block of data </summary>
        private const char OPEN_DATABLOCK = '{';

        /// <summary> Character used to denote the closing of a block of data </summary>
        private const char CLOSE_DATABLOCK = '}';

        /// <summary> Character used to separate fields and items in a block of data </summary>
        private const char SEPARATOR = ';';

        /// <summary> Character used to separate key-value pairs in a block of data </summary>
        private const char KEY_SEPARATOR = ':';

        /// <summary> String that denotes the start of format definition </summary>
        private const string FORMAT = "FORMAT:";

        /// <summary> String that denotes the start of data records </summary>
        private const string DATA = "DATA:";

        #endregion

        #region Fields

        /// <summary>
        /// The type:format dictionary
        /// </summary>
        private Dictionary<string, TypeFieldsFormat> _Format;

        /// <summary>
        /// The list of Unique objects to be written or reconstructed
        /// </summary>
        private UniquesCollection _Uniques;

        /// <summary>
        /// The stream writer
        /// </summary>
        private StreamWriter _Writer;

        /// <summary>
        /// The stream reader
        /// </summary>
        private StreamReader _Reader;

        #endregion

        #region Methods

        public void Serialize(Stream stream, object source)
        {
            _Format = new Dictionary<string, TypeFieldsFormat>();
            _Uniques = new UniquesCollection();
            _Writer = new StreamWriter(stream);

            _Writer.WriteLine(DATA);

            // Write base item:
            SerializeItem(source);

            _Writer.WriteLine();

            // Write uniques:
            int i = 0;
            while (i < _Uniques.Count)
            {
                IUnique unique = _Uniques[i];
                SerializeItem(unique);
                i++;

                _Writer.WriteLine();
            }

            //_Writer.WriteLine();
            //_Writer.WriteLine(FORMAT);

            //_Writer.Write(GenerateFormatDescription());

            _Writer.Flush();
        }

        protected void WriteValue(object value)
        {
            if (value == null)
            {

            }
            else if (value is IUnique)
            {
                //Write GUID, put in queue:
                IUnique unique = (IUnique)value;
                if (!_Uniques.Contains(unique.GUID)) _Uniques.Add(unique);

                //TODO: Write GUID
                _Writer.Write(unique.GUID);
            }
            else
            {
                SerializeItem(value);
            }
        }


        protected void SerializeItem(object source)
        {
            if (source != null)
            {
                Type type = source.GetType();

                if (type.IsPrimitive)
                {
                    _Writer.Write(source.ToString());
                }
                else if (type.IsAssignableFrom(typeof(string)))
                {
                    _Writer.Write(source.ToString());
                }
                else if (type.IsSerializable)
                {
                    if (source is IUnique)
                    {
                        _Writer.Write(((IUnique)source).GUID);
                        _Writer.Write(KEY_SEPARATOR);
                    }
                    
                    string typeName = type.FullName;

                    _Writer.Write(typeName);

                    _Writer.Write(OPEN_DATABLOCK);

                    TypeFieldsFormat format = null;
                    if (!_Format.ContainsKey(typeName))
                    {
                        format = new TypeFieldsFormat(type, type.GetAllFields(true));
                        // Add type to format:
                        _Format.Add(typeName, format);
                    }
                    else
                        format = _Format[typeName];

                    int valueCount = 0;

                    // Write fields:
                    foreach (FieldInfo field in format.Fields)
                    {
                        if (valueCount > 0) _Writer.Write(SEPARATOR);

                        object value = field.GetValue(source);
                        WriteValue(value);

                        valueCount++;
                    }

                    // Write items:
                    if (type.IsArray)
                    {
                        foreach (object item in (IEnumerable)source)
                        {
                            if (valueCount > 0) _Writer.Write(SEPARATOR);

                            WriteValue(item);

                            valueCount++;
                        }
                    }

                    _Writer.Write(CLOSE_DATABLOCK);

                }

            }
        }

        /// <summary>
        /// Construct a format descriptor from a type name and a list of serialisable fields
        /// </summary>
        /// <param name="typeName">The type's FullName</param>
        /// <param name="fields">all fields of the type which are to be serialized</param>
        /// <returns></returns>
        public string ToFormatDescription(TypeFieldsFormat format)
        {
            var sb = new StringBuilder();
            sb.Append(format.Type.AssemblyQualifiedName);
            sb.Append(OPEN_DATABLOCK);
            for (int i = 0; i < format.Fields.Count; i++)
            {
                if (i > 0) sb.Append(SEPARATOR);
                FieldInfo field = format.Fields[i];
                sb.Append(field.Name);
            }
            sb.Append(CLOSE_DATABLOCK);
            return sb.ToString();
        }

        /// <summary>
        /// Generate the block of text that describes the data formats described within
        /// the document
        /// </summary>
        /// <returns></returns>
        public string GenerateFormatDescription()
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, TypeFieldsFormat> kvp in _Format)
            {
                sb.AppendLine(ToFormatDescription(kvp.Value));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Parse a line of text as a data format
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public TypeFieldsFormat ReadFormat(string line)
        {
            TypeFieldsFormat result;
            int i = 0;
            string typeName = line.NextChunk(ref i, OPEN_DATABLOCK);
            Type type = GetType(typeName);
            var fields = new List<FieldInfo>();
            if (type != null)
            {
                while (i < line.Length)
                {
                    string fieldName = line.NextChunk(ref i, SEPARATOR, CLOSE_DATABLOCK);
                    FieldInfo field = type.GetBaseField(fieldName);
                    //TODO: check for mapped fields if null
                    fields.Add(field);
                }
            }
            result = new TypeFieldsFormat(type, fields);
            if (_Format != null) _Format.Add(typeName, result);
            return result;
        }

        private Type GetType(string typeName)
        {
            Assembly current = Assembly.GetExecutingAssembly();
            Type result = Type.GetType(typeName);
            if (result == null)
            {
                //TODO!
            }
            return result;
        }

        public IUnique Deserialize(Stream stream)
        {
            IUnique result = null;

            _Reader = new StreamReader(stream);

            //First, read through once to end:
            string line;

            while ((line = _Reader.ReadLine()) != null)
            {
                int i = 0;
                string idChunk = line.NextChunk(ref i, KEY_SEPARATOR);
                string typeChunk = line.NextChunk(ref i, OPEN_DATABLOCK);

            }


            //TODO
            return null;
        }

        

        #endregion
    }
}
