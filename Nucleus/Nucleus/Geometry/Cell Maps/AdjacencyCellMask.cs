using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry.Cell_Maps
{
    /// <summary>
    /// Cell mask which passes cells which have a certain number of contiguous adjacent cells
    /// which pass a particular condition.
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

        private bool _Invert = false;

        /// <summary>
        /// Invert the filter, so that only cells with lower than the contiguous limit of
        /// adjacent cells pass the filter
        /// </summary>
        public bool Invert
        {
            get { return _Invert; }
            set { _Invert = value; }
        }

        /// <summary>
        /// The condition which determines whether a cell should pass or be filtered out
        /// </summary>
        public Func<IMapCell, bool> Condition { get; set; }

        public AdjacencyCellMask(Func<IMapCell, bool> condition, int contiguousLimit = 1, bool invert = false)
        {
            Condition = condition;
            _ContiguousLimit = contiguousLimit;
            _Invert = invert;
        }

        public override void Apply<TMapCell>(ICellMap<bool> bitField, ICellMap<TMapCell> map)
        {
            for (int i = 0; i < bitField.CellCount; i++)
            {
                if (bitField.Exists(i) && bitField[i])
                {
                    int count = map.CountContiguousAdjacentCellsWhere(i, Condition);
                    if ((count < ContiguousLimit) ^ Invert) bitField[i] = false;
                }
            }
        }
    }
}
