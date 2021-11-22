using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Abstract base class for cell masks
    /// </summary>
    [Serializable]
    public abstract class CellMask
    {
        /// <summary>
        /// Filter the specified bitField through the mask
        /// </summary>
        /// <typeparam name="TMapCell"></typeparam>
        /// <param name="bitField">The map of boolean values</param>
        /// <param name="map">The base map to use to determine masking</param>
        /// <returns></returns>
        public abstract void Apply<TMapCell>(ICellMap<bool> bitField, ICellMap<TMapCell> map)
            where TMapCell : IMapCell;
    }
}
