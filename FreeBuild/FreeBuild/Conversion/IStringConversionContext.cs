using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// Interface for objects used as a context when extracting values from
    /// object via the GetValue(path) object extension method.
    /// </summary>
    public interface IStringConversionContext
    {
        /// <summary>
        /// Set the source object about which this context object
        /// should return data.
        /// </summary>
        /// <param name="source"></param>
        void SetSourceObject(object source);
    }
}
