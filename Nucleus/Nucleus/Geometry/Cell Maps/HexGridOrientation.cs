using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// An enumerated value representing different orientations of a hexagonal grid
    /// </summary>
    public enum HexGridOrientation
    {
        /// <summary>
        /// AKA 'Flat topped'.  The grid is arranged such that hexes form aligned vertical columns
        /// </summary>
        VerticalColumns,

        /// <summary>
        /// AKA 'Pointy topped'.  The grid is arranged such that hexes form aligned horizontal rows.
        /// </summary>
        HorizontalRows
    }
}
