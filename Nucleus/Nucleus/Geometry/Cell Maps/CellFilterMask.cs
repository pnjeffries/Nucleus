using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Abstract base class for CellMasks which filter cells based on whether they pass or fail a particular condition.
    /// </summary>
    [Serializable]
    public abstract class CellFilterMask : CellMask
    {
        public CellFilterMask() { }

        /// <summary>
        /// Filter the specified bitField through the mask
        /// </summary>
        /// <typeparam name="TMapCell"></typeparam>
        /// <param name="bitField">The map of boolean values</param>
        /// <param name="map">The base map to use to determine masking</param>
        /// <returns></returns>
        public override void Apply<TMapCell>(ICellMap<bool> bitField, ICellMap<TMapCell> map)
        {
            for (int i = 0; i < bitField.CellCount; i++)
            {
                if (bitField.Exists(i) && bitField[i])
                {
                    bitField[i] = Pass(map[i]);
                }
            }
        }

        public abstract bool Pass<TMapCell>(TMapCell cell)
            where TMapCell : IMapCell;
    }
}
