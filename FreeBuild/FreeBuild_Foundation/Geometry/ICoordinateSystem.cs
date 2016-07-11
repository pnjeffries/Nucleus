using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Interface for objects that define a local coordinate system.
    /// </summary>
    public interface ICoordinateSystem
    {
        /// <summary>
        /// Convert a vector defined in the global coordinate system into 
        /// one defined in local coordinates of this coordinate system.
        /// </summary>
        /// <param name="vector">A vector in the global coordinate system.</param>
        /// <returns>A vector in local coordinates</returns>
        Vector GlobalToLocal(Vector vector);

        /// <summary>
        /// Convert a vector defined in the local coordinate system into
        /// one defined in global coordinates
        /// </summary>
        /// <param name="vector">A vector in the local coordinate system.</param>
        /// <returns>A vector in global coordinates</returns>
        Vector LocalToGlobal(Vector vector);
    }
}
