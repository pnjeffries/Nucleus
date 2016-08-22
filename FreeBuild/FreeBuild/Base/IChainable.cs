using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Interface for objects which can be chained together one after the other
    /// </summary>
    public interface IChainable
    {
        /// <summary>
        /// The previous item in the chain
        /// </summary>
        IChainable Previous { get; }

        /// <summary>
        /// The next item in the chain
        /// </summary>
        IChainable Next { get; set; }
    }
}
