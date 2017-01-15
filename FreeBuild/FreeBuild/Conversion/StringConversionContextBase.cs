using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// Abstract base class for String conversion contexts.
    /// Implements a basic version of the required functionality.
    /// </summary>
    public abstract class StringConversionContextBase : IStringConversionContext
    {
        /// <summary>
        /// The current source object
        /// </summary>
        public object SourceObject { get; set; } = null;

        /// <summary>
        /// Set the source object.
        /// </summary>
        /// <param name="source"></param>
        public virtual void SetSourceObject(object source)
        {
            SourceObject = source;
        }
    }
}
