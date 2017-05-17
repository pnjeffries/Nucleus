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

        /// <summary> Character used to denot the closing of a block of data </summary>
        private const char CLOSE_DATABLOCK = '}';

        /// <summary> Character used to separate fields and items in a block of data </summary>
        private const char SEPARATOR = ';';

        /// <summary> String that denotes the start of format definition </summary>
        private const string FORMAT = "FORMAT:";

        /// <summary> String that denotes the start of data records </summary>
        private const string DATA = "DATA:";

        #endregion

        #region Fields

        /// <summary>
        /// The type:format dictionary
        /// </summary>
        private Dictionary<string, IList<FieldInfo>> _Format;

        /// <summary>
        /// The list of Unique objects to be written or reconstructed
        /// </summary>
        private UniquesCollection _Uniques;

        #endregion

        #region Methods

        public void Serialize(Stream stream, object source)
        {
            _Format = new Dictionary<string, IList<FieldInfo>>();
            _Uniques = new UniquesCollection();

            // Write base item:
            SerializeItem(source);

            // Write uniques:
            int i = 0;
            while (i < _Uniques.Count)
            {
                IUnique unique = _Uniques[i];
                SerializeItem(unique);
                i++;
            }
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
                    // TODO
                }
                else
                {
                    string typeName = type.AssemblyQualifiedName;

                    IList<FieldInfo> fields = null;
                    if (!_Format.ContainsKey(typeName))
                    {
                        fields = type.GetAllFields(true);
                        // Add type to format:
                        _Format.Add(typeName, fields);
                    }
                    else
                        fields = _Format[typeName];

                    // Write fields:
                    foreach (FieldInfo field in fields)
                    {
                        object value = field.GetValue(source);
                        WriteValue(value);
                    }

                    // Write items:
                    if (source is IEnumerable)
                    {
                        foreach (object item in (IEnumerable)source)
                        {
                            WriteValue(item);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Construct a format descriptor from a type name and a list of serialisable fields
        /// </summary>
        /// <param name="typeName">The type's FullName</param>
        /// <param name="fields">all fields of the type which are to be serialized</param>
        /// <returns></returns>
        public string ToFormatDescription(string typeName, IList<FieldInfo> fields)
        {
            var sb = new StringBuilder();
            sb.Append(typeName);
            sb.Append(OPEN_DATABLOCK);
            for (int i = 0; i < fields.Count; i++)
            {
                if (i > 0) sb.Append(SEPARATOR);
                FieldInfo field = fields[i];
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
            foreach (KeyValuePair<string, IList<FieldInfo>> kvp in _Format)
            {
                sb.AppendLine(ToFormatDescription(kvp.Key, kvp.Value));
            }
            return sb.ToString();
        }

        public void ReadFormat(string line)
        {
            int i = 0;
            string typeName = line.NextChunk(ref i, OPEN_DATABLOCK);
            if (!_Format.ContainsKey(typeName))
            {
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
                _Format.Add(typeName, fields);
            }
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

        public IUnique Deserialize()
        {
            //TODO
            return null;
        }

        #endregion
    }
}
