using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Interface for objects which can be named
    /// </summary>
    public interface INamed
    {
        /// <summary>
        /// The name of this object
        /// </summary>
        string Name { get; set; }
    }
}
