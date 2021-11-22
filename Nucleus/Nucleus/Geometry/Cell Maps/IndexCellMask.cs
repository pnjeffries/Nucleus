using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Abstract base class for cell masks which filter based on the cell index
    /// </summary>
    [Serializable]
    public abstract class IndexCellMask : CellMask
    {
        public override void Apply<TMapCell>(ICellMap<bool> bitField, ICellMap<TMapCell> map)
        {
            for (int i = 0; i < bitField.CellCount; i++)
            {
                if (bitField.Exists(i) && bitField[i])
                {
                    bitField[i] = Pass(i, map);
                }
            }
        }

        protected abstract bool Pass<TMapCell>(int index, ICellMap<TMapCell> map);
    }
}
