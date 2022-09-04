using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// An enumerated value representing different layout options of a hexagonal grid
    /// </summary>
    public enum HexGridLayout
    {
        /// <summary>
        /// The grid is arranged with horizontal rows.  Every even-indexed row is offset to the right.
        /// </summary>
        EvenRows = 0,

        /// <summary>
        /// The grid is arranged with horizontal rows.  Every odd-indexed row is offset to the right.
        /// </summary>
        OddRows = 1,

        /// <summary>
        /// The grid is arranged with vertical columns.  Every even-indexed column is offset down.
        /// </summary>
        EvenColumns = 2,

        /// <summary>
        /// The grid is arranged with vertical columns.  Every odd-indexed column is offset down.
        /// </summary>
        OddColumns = 3
    }

    /// <summary>
    /// Extension methods for the HexGridLayout Enum
    /// </summary>
    public static class HexGridLayoutExtensions
    {
        /// <summary>
        /// Does this layout have aligned vertical columns (a.k.a. flat-topped cells)?
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        public static bool HasAlignedColumns(this HexGridLayout layout)
        {
            return layout == HexGridLayout.EvenColumns || layout == HexGridLayout.OddColumns;
        }

        /// <summary>
        /// Does this layout have aligned horixontal rows (a.k.a. pointy-topped cells)?
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        public static bool HasAlignedRows(this HexGridLayout layout)
        {
            return layout == HexGridLayout.EvenRows || layout == HexGridLayout.OddRows;
        }
    }
}
