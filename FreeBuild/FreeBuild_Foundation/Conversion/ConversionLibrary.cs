using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        /// The classes containing conversion routines that have been loaded so far
        /// </summary>
        public TypeCollection LoadedConverters { get; } = new TypeCollection();

        /// <summary>
        /// The dictionary of conversion methods loaded from converters.
        /// Keyed by source type, then by target type.
        /// </summary>
        public IDictionary<Type, IDictionary<Type, IList<MethodInfo>>> ConversionMethods { get; }
            = new Dictionary<Type, IDictionary<Type, IList<MethodInfo>>>();

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
        public bool LoadConverter(Type converterClass)
        {
            if (!LoadedConverters.Contains(converterClass.GUID))
            {
                LoadedConverters.Add(converterClass);
                MethodInfo[] methods = converterClass.GetMethods(BindingFlags.Public | BindingFlags.Static);
                foreach (MethodInfo method in methods)
                {
                    //TODO
                }
                return true;
            }
            return false;
        }

        #endregion
    }
}
