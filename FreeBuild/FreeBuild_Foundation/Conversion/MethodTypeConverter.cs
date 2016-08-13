using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// Converter which will convert from one type to another via a static method
    /// </summary>
    [Serializable]
    public class MethodTypeConverter : ITypeConverter
    {
        #region Properties

        /// <summary>
        /// The method used to perform the conversion
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// The description text of this converter
        /// </summary>
        public string Description
        {
            get
            {
                return Method.ToString(); //Does this do what I want?
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor creating a new MethodTypeConverter by specifying the method to use
        /// </summary>
        public MethodTypeConverter(MethodInfo method)
        {
            Method = method;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Apply the conversion by calling the method
        /// </summary>
        /// <param name="fromObject"></param>
        /// <returns></returns>
        public object Convert(object fromObject)
        {
            return Method.Invoke(null, new object[] { fromObject });
        }

        #endregion

    }
}
