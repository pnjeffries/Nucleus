using FreeBuild.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// Base class for conversion contexts - objects that contain data on the context of a conversion operation
    /// </summary>
    public abstract class ConversionContext
    {
        /// <summary>
        /// The current execution information
        /// </summary>
        public ExecutionInfo ExInfo { get; set; } = null;
    }
}
