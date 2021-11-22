using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A cell mask which passes or filters cells based on a function delegate
    /// </summary>
    [Serializable]
    public class ConditionalCellMask : CellFilterMask
    {
        #region Constants

        /// <summary>
        /// Conditional cell mask which passes cells if they have no contents
        /// </summary>
        public static ConditionalCellMask Empty => new ConditionalCellMask(cell => cell is MapCell mCell && mCell.Contents.Count == 0);

        #endregion

        /// <summary>
        /// The condition which determines whether a cell should pass or be filtered out
        /// </summary>
        public Func<IMapCell, bool> Condition { get; set; }

        public ConditionalCellMask(Func<IMapCell, bool> condition)
        {
            Condition = condition;
        }

        public override bool Pass<TMapCell>(TMapCell cell)
        {
            return Condition.Invoke(cell);
        }
    }
}
