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

using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FreeBuild.Extensions;
using FreeBuild.Exceptions;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// A library of converter routines to take one type of data and convert
    /// it into another as closely as possible.
    /// </summary>
    public class ConversionLibrary
    {
        #region Properties

        /// <summary>
        /// The dictionary of conversion methods loaded from converters.
        /// Keyed by source type, then by target type.
        /// </summary>
        public IDictionary<Type, IDictionary<Type, IList<ITypeConverter>>> LoadedConverters { get; }
            = new Dictionary<Type, IDictionary<Type, IList<ITypeConverter>>>();

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConversionLibrary() { }

        /// <summary>
        /// Initialises a new ConversionLibrary, loading in converters from a specified assembly
        /// </summary>
        /// <param name="converterAssembly"></param>
        public ConversionLibrary(Assembly converterAssembly)
        {
            LoadConverters(converterAssembly);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load all classes containing conversion routines from the specified assembly 
        /// into the library
        /// </summary>
        /// <param name="converterAssembly"></param>
        public void LoadConverters(Assembly converterAssembly)
        {
            IEnumerable<Type> types = converterAssembly.ExportedTypes;
            foreach(Type type in types)
            {
                LoadConverters(type);
            }
        }

        /// <summary>
        /// Load a class containing conversion routines into the library
        /// </summary>
        /// <param name="converterClass"></param>
        /// <returns></returns>
        public void LoadConverters(Type converterClass)
        {
            MethodInfo[] methods = converterClass.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo method in methods)
            {
                ParameterInfo[] paras = method.GetParameters();
                if (paras.Length == 1) //TODO: Or 2, and second is a context object?
                {
                    Type targetType = method.ReturnType;
                    if (targetType != typeof(void))
                    {
                        Type sourceType = paras[0].ParameterType;
                        if (!LoadedConverters.ContainsKey(sourceType)) LoadedConverters.Add(sourceType, new Dictionary<Type, IList<ITypeConverter>>());
                        var targetDictionary = LoadedConverters[sourceType];
                        if (!targetDictionary.ContainsKey(targetType)) targetDictionary.Add(targetType, new List<ITypeConverter>());
                        var converterList = targetDictionary[targetType];
                        //TODO: Test if method already loaded?
                        converterList.Add(new MethodTypeConverter(method));
                    }
                }
            }
        }

        /// <summary>
        /// Convert the specified object to the specified type, using a previously loaded
        /// converter.
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="toType"></param>
        /// <returns></returns>
        public object Convert(object sourceObject, Type toType)
        {
            if (sourceObject == null) return null;

            Type sourceType = sourceObject.GetType();
            if (!LoadedConverters.ContainsKey(sourceType))
            {
                sourceType = LoadedConverters.Keys.ClosestAncestor(sourceType);
            }
            if (sourceType != null)
            {
                var targetDictionary = LoadedConverters[sourceType];
                Type targetType = toType;
                if (!targetDictionary.ContainsKey(targetType))
                {
                    targetType = targetDictionary.Keys.ClosestDescendent(targetType);
                }
                if (targetType != null)
                {
                    IList<ITypeConverter> tConverters = targetDictionary[targetType];
                    ITypeConverter tConverter = tConverters.First(); //TODO: Try multiple converters if the first is not successful?
                    return tConverter.Convert(sourceObject);
                }
            }
            if (sourceObject is IConvertible) return System.Convert.ChangeType(sourceObject, toType);
            return sourceObject; //TODO test if can convert first?
        }

        /// <summary>
        /// Find all loaded converters that could conceivably be applied to the given pair of types
        /// </summary>
        /// <param name="fromType">The type to convert from</param>
        /// <param name="toType">The type of convert to</param>
        /// <returns></returns>
        public IList<ITypeConverter> AllSuitableConverters(Type fromType, Type toType)
        {
            var result = new List<ITypeConverter>();
            foreach(Type sourceType in LoadedConverters.Keys)
            {
                if (sourceType.IsAssignableFrom(fromType))
                {
                    var targetConverters = LoadedConverters[sourceType];
                    foreach (Type targetType in targetConverters.Keys)
                    {
                        if (toType.IsAssignableFrom(targetType))
                        {
                            foreach (ITypeConverter converter in targetConverters[sourceType])
                            {
                                result.Add(converter);
                            }
                        }
                    }
                }
            }
            return result;
        }

        #endregion


    }
}
