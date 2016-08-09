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

        #endregion

        #region Methods

        /// <summary>
        /// Load a class containing conversion routines into the library
        /// </summary>
        /// <param name="converterClass"></param>
        /// <returns></returns>
        public bool LoadConverters(Type converterClass)
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
            return true;
        }

        #endregion

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
                    ITypeConverter tConverter = tConverters.First();
                    return tConverter.Convert(sourceObject);
                }
            }
            if (sourceObject is IConvertible) return System.Convert.ChangeType(sourceObject, toType);
        }
    }
}
