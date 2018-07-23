using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Interface for objects which may be stored as map cells
    /// </summary>
    public interface IMapCell
    {
        /// <summary>
        /// The index of the cell in its owning map
        /// </summary>
        int Index { get; }

        /// <summary>
        /// The map to which this cell belongs
        /// </summary>
        ICellMap Map { get; }
    }
}
