using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry.Cell_Maps
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AdjacencyCellMask : CellMask
    {
        private int _ContiguousLimit = 1;

        /// <summary>
        /// The minimum number of contiguous adjacent cells where the 
        /// </summary>
        public int ContiguousLimit
        {
            get { return _ContiguousLimit; }
            set { _ContiguousLimit = value; }
        }

        /// <summary>
        /// The condition which determines whether a cell should pass or be filtered out
        /// </summary>
        public Func<IMapCell, bool> Condition { get; set; }

        public AdjacencyCellMask(Func<IMapCell, bool> condition, int contiguousLimit = 1)
        {
            Condition = condition;
            _ContiguousLimit = contiguousLimit;
        }

        public override void Apply<TMapCell>(ICellMap<bool> bitField, ICellMap<TMapCell> map)
        {
            for (int i = 0; i < bitField.CellCount; i++)
            {
                if (bitField.Exists(i) && bitField[i])
                {
                    int count = map.CountContiguousAdjacentCellsWhere(i, Condition);
                    if (count < ContiguousLimit) bitField[i] = false;
                }
            }
        }
    }
}
