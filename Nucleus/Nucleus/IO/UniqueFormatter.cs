using Nucleus.Base;
using Nucleus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// A custom serialisation class which maintains IUnique-implementing objects as unique objects which
    /// may be referenced by others and inlines everything else.  The data format is kept malleable and forwards-compatible
    /// by being recorded within the file itself.
    /// </summary>
    public class UniqueFormatter
    {
        #region Constants

        /// <summary> Character used to denote the opening of a block of data </summary>
        private const char OPEN_DATABLOCK = '{';//'\u0002';

        /// <summary> Character used to denote the closing of a block of data </summary>
        private const char CLOSE_DATABLOCK = '}';//'\u0003';

        /// <summary> Character used to separate fields and items in a block of data </summary>
        private const char SEPARATOR = ';';//'\u001E';

        /// <summary> Character used to separate key-value pairs in a block of data </summary>
        private const char KEY_SEPARATOR = '|';//'\u001F';

        /// <summary> String that denotes the start of format definition </summary>
        private const string FORMAT = "FORM|"; //"FOR\u001D";

        /// <summary> String that denotes the start of data records </summary>
        private const string DATA = "DATA|";//"DAT\u001D";

        #endregion

        #region Fields

        /// <summary>
        /// The type alias:format dictionary
        /// </summary>
        private Dictionary<string, TypeFieldsFormat> _Format;

        /// <summary>
        /// The dictionar of type aliases
        /// </summary>
        private Dictionary<Type, string> _Aliases;

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

        public void Serialize(Stream stream, IUnique source)
        {
            _Format = new Dictionary<string, TypeFieldsFormat>();
            _Aliases = new Dictionary<Type, string>();
            _Uniques = new UniquesCollection();
            _Writer = new StreamWriter(stream);

            // Write base item:
            _Writer.WriteLine(SerializeItem(source));

            // Write uniques:
            int i = 0;
            while (i < _Uniques.Count)
            {
                IUnique unique = _Uniques[i];
                _Writer.WriteLine(SerializeItem(unique));
                i++;
            }

            //_Writer.WriteLine();
            //_Writer.WriteLine(FORMAT);

            //_Writer.Write(GenerateFormatDescription());

            _Writer.Flush();
        }

        /// <summary>
        /// Write 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sb"></param>
        protected void WriteValue(object value, StringBuilder sb)
        {
            if (value == null)
            {

            }
            else if (value is IntPtr[])
            {
                //Nope!
            }
            else if (value is IUnique)
            {
                //Write GUID, put in queue:
                IUnique unique = (IUnique)value;
                if (!_Uniques.Contains(unique.GUID)) _Uniques.Add(unique);

                //TODO: Write GUID
                sb.Append(unique.GUID);
            }
            else
            {
                WriteObject(value, sb);
            }
        }

        /// <summary>
        /// Serialize a unique item to a string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected string SerializeItem(IUnique source)
        {
            var sb = new StringBuilder();
            sb.Append(DATA);
            sb.Append(source.GUID);
            sb.Append(KEY_SEPARATOR);
            WriteObject(source, sb);
            return sb.ToString();
        }

        protected void WriteObject(object source, StringBuilder sb)
        {
            Type type = source.GetType();

            if (type.IsPrimitive || type.IsAssignableFrom(typeof(string)) || type == typeof(Guid))
            {
                sb.Append(source.ToString());
            }
            else if (type.IsSerializable)
            {
                string typeAlias = SerializeType(type);
                TypeFieldsFormat format = _Format[typeAlias];

                sb.Append(typeAlias);
                sb.Append(OPEN_DATABLOCK);

                int valueCount = 0;

                // Write fields:
                foreach (FieldInfo field in format.Fields)
                {
                    if (valueCount > 0) sb.Append(SEPARATOR);

                    object value = field.GetValue(source);
                    WriteValue(value, sb);

                    valueCount++;
                }

                // Write items:
                if (type.IsArray)
                {
                    foreach (object item in (IEnumerable)source)
                    {
                        if (valueCount > 0) sb.Append(SEPARATOR);

                        WriteValue(item, sb);

                        valueCount++;
                    }
                }
                else if (type.IsStandardDictionary())
                {
                    IDictionary dic = (IDictionary)source;
                    foreach (DictionaryEntry item in dic)
                    {
                        if (valueCount > 0) sb.Append(SEPARATOR);

                        WriteValue(item.Key, sb);
                        sb.Append(KEY_SEPARATOR);
                        WriteValue(item.Value, sb);

                        valueCount++;
                    }
                }

                sb.Append(CLOSE_DATABLOCK);
            }
            else
            {
                //Type not serializable - throw warning:
                RaiseError("Type '" + type.FullName + "' is not marked as serializable and could not be saved.");
            }
        }

        /// <summary>
        /// Get the alias of the specified type, writing the type format description
        /// to the stream if it has not already been done for this type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string SerializeType(Type type)
        {
            if (_Aliases.ContainsKey(type))
            {
                return _Aliases[type];
            }
            else
            {
                //Generate alias:
                string abbreviation = type.Name.TruncatePascal(6);
                if (type.Name.EndsWith("[]")) abbreviation = abbreviation.OverwriteEnd("[]");
                string alias = abbreviation;
                int i = 2;
                while (_Format.ContainsKey(alias))
                {
                    alias = abbreviation + i;
                    i++;
                }
                _Aliases.Add(type, alias);
                var format = new TypeFieldsFormat(alias, type);
                _Format.Add(alias, format);
                _Writer.WriteLine(ToFormatDescription(format));
                return alias;
            }
        }

        

        /*protected void SerializeItemX(object source)
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
                        WriteValue(value, sb);

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
        }*/

        /// <summary>
        /// Construct a format descriptor from a type name and a list of serialisable fields
        /// </summary>
        /// <param name="typeName">The type's FullName</param>
        /// <param name="fields">all fields of the type which are to be serialized</param>
        /// <returns></returns>
        public string ToFormatDescription(TypeFieldsFormat format)
        {
            var sb = new StringBuilder();
            sb.Append(FORMAT);
            sb.Append(format.Alias);
            sb.Append(KEY_SEPARATOR);
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
            int i = FORMAT.Length;
            string alias = line.NextChunk(ref i, KEY_SEPARATOR);
            string typeName = line.NextChunk(ref i, OPEN_DATABLOCK);
            Type type = GetType(typeName);
            var fields = new List<FieldInfo>();
            if (type != null)
            {
                while (i < line.Length)
                {
                    string fieldName = line.NextChunk(ref i, SEPARATOR, CLOSE_DATABLOCK);
                    if (!string.IsNullOrEmpty(fieldName))
                    {
                        FieldInfo field = type.GetBaseField(fieldName);
                        //TODO: check for mapped fields if null
                        if (field == null) RaiseError("Field '" + fieldName + "' cannot be found on type '" + type.Name + "'.");
                        fields.Add(field);
                    }
                }

                result = new TypeFieldsFormat(alias, type, fields);
                if (_Format != null) _Format.Add(alias, result);
                if (_Aliases != null) _Aliases.Add(type, alias);
                return result;
            }
            else return null;
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

        /// <summary>
        /// Read a record string, which consists of a prefix,
        /// an OPEN_DATABLOCK character, some sub-records and a 
        /// matching CLOSE_DATABLOCK character.
        /// </summary>
        /// <returns></returns>
        public string ReadRecord()
        {
            var sb = new StringBuilder();
            int levels = 0;
            int strLevels = 0;
            while (!_Reader.EndOfStream)
            {
                char c = Convert.ToChar(_Reader.Read());
                sb.Append(c);
                if (c == OPEN_DATABLOCK) levels += 1;
                else if (c == CLOSE_DATABLOCK)
                {
                    levels -= 1;
                    if (levels == 0) return sb.ToString();
                }
                //TODO: Open/Close strings?
            }
            if (sb.Length > 0)
                return sb.ToString();
            else
                return null;
        }

        public IUnique Deserialize(Stream stream)
        {
            IUnique result = null;

            _Format = new Dictionary<string, TypeFieldsFormat>();
            _Aliases = new Dictionary<Type, string>();
            _Uniques = new UniquesCollection();
            _Reader = new StreamReader(stream);

            //First, read through once to end:
            string line;

            while ((line = _Reader.ReadLine()) != null)
            {
                if (line.StartsWith(FORMAT))
                {
                    ReadFormat(line);
                }
                else if (line.StartsWith(DATA))
                {
                    // Initial pass: Create object
                    int i = DATA.Length;
                    string guid = line.NextChunk(ref i, KEY_SEPARATOR);
                    string typeAlias = line.NextChunk(ref i, OPEN_DATABLOCK);
                    if (_Format.ContainsKey(typeAlias))
                    {
                        TypeFieldsFormat format = _Format[typeAlias];

                        IUnique unique = format.Type.Instantiate() as IUnique;//FormatterServices.GetUninitializedObject(format.Type) as IUnique;
                        if (unique is IUniqueWithModifiableGUID)
                        {
                            var uniqueMG = (IUniqueWithModifiableGUID)unique;
                            uniqueMG.SetGUID(new Guid(guid));
                        }
                        else
                        {
                            FieldInfo fI = format.Type.GetBaseField("_GUID"); // Will not work if backing field is named differently!
                            fI.SetValue(unique, new Guid(guid));
                        }
                        _Uniques.Add(unique);
                        if (result == null) result = unique; // Set primary output object
                    }
                    else RaiseError("Formatting data for type alias '" + typeAlias + "' not found.");
                }
            }

            //Next: Second pass - populate fields with data

            //Rewind:
            stream.Seek(0, SeekOrigin.Begin);
            _Reader = new StreamReader(stream);

            while ((line = _Reader.ReadLine()) != null)
            {
                if (line.StartsWith(DATA))
                {
                    // Initial pass: Create object
                    int i = DATA.Length;
                    string guid = line.NextChunk(ref i, KEY_SEPARATOR);
                    string typeAlias = line.NextChunk(ref i, OPEN_DATABLOCK);
                    if (_Format.ContainsKey(typeAlias))
                    {
                        TypeFieldsFormat format = _Format[typeAlias];
                        object unique = _Uniques[new Guid(guid)];
                        PopulateFields(ref unique, format, ref i, line);
                    }
                }
            }

            //TODO
            return result;
        }

        /// <summary>
        /// Convert from a string to an object of the specified type,
        /// where for 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected object String2Object(string str, Type type)
        {
            if (typeof(IUnique).IsAssignableFrom(type))
            {
                Guid guid = new Guid(str);
                return _Uniques[guid];
            }
            if (type == typeof(Guid))
            {
                return new Guid(str);
            }
            try
            {
                return Convert.ChangeType(str, type);
            }
            catch
            {
                return null; //TODO: default(type)
            }
        }

        protected void PopulateFields(ref object target, TypeFieldsFormat format, ref int i, string line)
        {
            string chunk;
            int j = 0;
            IList items = null; //Items for array reinstantiation
            object currentKey = null; //Current key object for Dictionaries
            while (i < line.Length)// && j < format.Fields.Count())
            {
                char c;
                chunk = line.NextChunk(out c, ref i, SEPARATOR, OPEN_DATABLOCK, CLOSE_DATABLOCK, KEY_SEPARATOR);
                if (c == SEPARATOR || c == CLOSE_DATABLOCK)
                {
                    // Chunk is simple value
                    if (!string.IsNullOrEmpty(chunk))
                    {
                        if (j < format.Fields.Count())
                        {
                            FieldInfo fI = format.Fields[j];
                            object value = String2Object(chunk, fI.FieldType);
                            fI.SetValue(target, value);
                        }
                        else if (target is IDictionary && currentKey != null) //Dictionary value
                        {
                            //Add to dictionary
                            IDictionary dic = (IDictionary)target;
                            Type[] arguments = dic.GetType().GetGenericArguments();
                            if (arguments.Length > 1)
                            {
                                object value = String2Object(chunk, arguments[1]);
                                //TODO: What if value is complex object?
                                dic[currentKey] = value;
                            }
                            currentKey = null;
                        }
                        else if (format.Type.IsList())
                        {
                            Type type = format.Type.GetElementType();
                            if (format.Type.ContainsGenericParameters)
                            {
                                type = format.Type.GenericTypeArguments[0];
                            }
                            object value = String2Object(chunk, type);
                            if (format.Type.IsArray)
                            {
                                if (items == null)
                                {
                                    Type listType = typeof(List<>).MakeGenericType(type);
                                    items = Activator.CreateInstance(listType) as IList;
                                    //TODO!!! Create list of required type!
                                }
                                items.Add(value);
                            }
                            else
                            {
                                IList list = (IList)target;
                                list.Add(value);
                            }
                            //TODO

                        }

                    }
                    j++;

                    if (c == CLOSE_DATABLOCK)
                    {
                        // The current object definition is finished - can step out

                        if (items != null)
                        {
                            Type type = format.Type.GetElementType();
                            Array targetArray = Array.CreateInstance(type, items.Count);
                            for (int k = 0; k < items.Count; k++)
                            {
                                targetArray.SetValue(items[k],k);
                            }
                            target = targetArray;
                        }

                        return;
                    }
                }
                else if (c == OPEN_DATABLOCK)
                {
                    // Chunk is the type alias of an embedded object
                    if (_Format.ContainsKey(chunk))
                    {
                        TypeFieldsFormat subFormat = _Format[chunk];
                        object value = null;
                        if (!subFormat.Type.IsArray)
                        {
                            value = subFormat.Type.Instantiate();
                        }
                        else
                        {
                            value = Activator.CreateInstance(subFormat.Type, new object[] { 0 });
                        }
                        PopulateFields(ref value, subFormat, ref i, line);
                        if (j < format.Fields.Count)
                        {
                            // Is sub-object belonging to a field
                            FieldInfo fI = format.Fields[j];
                            fI.SetValue(target, value);
                        }
                        else if (target is Array)
                        {
                            if (items == null)
                            {
                                items = new List<object>();
                                //TODO: Create list of specified type
                            }
                            items.Add(value);
                        }
                        else if (target is IDictionary && currentKey != null) //Dictionary value
                        {
                            IDictionary dic = (IDictionary)target;
                            dic[currentKey] = value;
                            currentKey = null;
                        }
                        else if (target is IDictionary && line[i] == KEY_SEPARATOR) //Dictionary key
                        {
                            currentKey = value;
                            i++; //Skip the key separator character
                        }
                        else if (target is IList)
                        {
                            // Is an entry in the target collection
                            IList list = (IList)target;
                            list.Add(value);
                        }
                        //j++;
                        //i++; //Skip the next separator?
                    }
                    else
                    {
                        RaiseError("Formatting data for type alias '" + chunk + "' not found.");
                    }
                }
                else if (c == KEY_SEPARATOR && target is IDictionary)
                {
                    // Is a key for a dictionary:
                    Type[] genTypes = target.GetType().GetGenericArguments();
                    currentKey = String2Object(chunk, genTypes[0]);
                }
                else
                {
                    // Line is not closed correctly - must be a multiline string
                    // TODO
                }
            }
        }

        protected void RaiseError(string message)
        {
            throw new Exception(message);
        }
        

        #endregion
    }
}
