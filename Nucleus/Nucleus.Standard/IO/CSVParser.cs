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

using Nucleus.Base;
using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// A class to parse CSV data and generate objects based upon it
    /// </summary>
    /// <typeparam name="TBase">The base type of the data to be constructed</typeparam>
    [Serializable]
    public class CSVParser<TBase> : CSVReader
        where TBase : class
    {
        #region Fields

        /// <summary>
        /// The current type to be created
        /// </summary>
        private Type _CurrentType;

        /// <summary>
        /// The dictionary of sub-types for the base type of object this parser generates
        /// </summary>
        private Dictionary<string, Type> _SubTypes = new Dictionary<string, Type>();

        #endregion

        #region Properties

        /// <summary>
        /// The map of columns to object properties
        /// </summary>
        public IList<PropertyInfo> Columns { get; set; } = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CSVParser()
        {
            Type baseType = typeof(TBase);
            if (!baseType.IsAbstract) _SubTypes.Add(baseType.Name, baseType);
            IList<Type> types = baseType.GetSubTypes(false);
            foreach (Type type in types)
            {
                _SubTypes.Add(type.Name, type);
            }
            _CurrentType = baseType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Parse CSV-format data from a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IList<TBase> Parse(FilePath filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                IList<TBase> result = Parse(reader);
                reader.Close();
                return result;
            }
        }

        /// <summary>
        /// Parse CSV-format data from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public IList<TBase> Parse(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                IList<TBase> result = Parse(reader);
                reader.Close();
                return result;
            }
        }

        /// <summary>
        /// Parse CSV-format data in a string
        /// </summary>
        /// <param name="csvString"></param>
        /// <returns></returns>
        public IList<TBase> Parse(string csvString)
        {
            using (var reader = new StringReader(csvString))
            {
                IList<TBase> result = Parse(reader);
                reader.Close();
                return result;
            }
        }

        /// <summary>
        /// Parse CSV-format data from a TextReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public IList<TBase> Parse(TextReader reader)
        {
            IList<TBase> result = new List<TBase>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                TBase obj = ParseLine(line);
                if (obj != null) result.Add(obj);
            }
            return result;
        }

        /// <summary>
        /// Parse a CSV line.  May do one of three things:
        /// - Set the object type (if only one entry and of a valid type)
        /// - Set the column properties (if not previously set)
        /// - Create a new object with the specified properties
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public TBase ParseLine(string line)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                string[] tokens = line.Split(Delimiter);
                int entryCount = tokens.NonEmptyCount();
                if (entryCount > 0)
                {
                    
                    if (entryCount == 1 && _SubTypes.ContainsKey(tokens[0]))
                    {
                        //Change type
                        _CurrentType = _SubTypes[tokens[0]];
                        Columns = null;
                        return null;
                    }
                    else if (Columns == null)
                    {
                        //Store column definitions
                        List<PropertyInfo> newColumns = new List<PropertyInfo>(tokens.Length);
                        foreach(string token in tokens)
                        {
                            PropertyInfo pInfo = _CurrentType.GetProperty(token);
                            newColumns.Add(pInfo);
                        }
                        Columns = newColumns;
                    }
                    else
                    {
                        TBase result = (TBase)Activator.CreateInstance(_CurrentType);
                        for (int i = Math.Min(tokens.Length, Columns.Count) - 1; i >= 0 ; i--)
                        {
                            string token = tokens[i];
                            PropertyInfo pInfo = Columns[i];
                            if (pInfo != null)
                            {
                                try
                                {
                                    object value = null;
                                    if (pInfo.PropertyType.IsAssignableFrom(typeof(string))) value = token;
                                    else if (pInfo.PropertyType.IsAssignableFrom(typeof(double))) value = double.Parse(token);
                                    else if (pInfo.PropertyType.IsAssignableFrom(typeof(bool))) value = bool.Parse(token);

                                    pInfo.SetValue(result, value, null);
                                }
                                catch (Exception ex)
                                {
                                    RaiseMessage(ex.Message);
                                }
                            }
                        }
                        return result;
                    }

                }
            }
            return null;
        }

        #endregion

    }
}
