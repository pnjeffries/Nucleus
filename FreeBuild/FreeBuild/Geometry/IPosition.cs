using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Interface for entities which occupy a position in space
    /// represented by a vector - nodes, vertices etc.
    /// </summary>
    public interface IPosition
    {
        /// <summary>
        /// Get the position vector of this object
        /// </summary>
        Vector Position { get; }
    }
}
