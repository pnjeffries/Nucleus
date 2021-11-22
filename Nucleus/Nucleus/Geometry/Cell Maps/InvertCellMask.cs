using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A cell mask which will simply invert the current values of the bitfield
    /// </summary>
    [Serializable]
    public class InvertCellMask : CellMask
    {
        public override void Apply<TMapCell>(ICellMap<bool> bitField, ICellMap<TMapCell> map)
        {
            for (int i = 0; i < bitField.CellCount; i++)
            {
                if (bitField.Exists(i)) bitField[i] = !bitField[i];
            }
        }
    }
}
