using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.DDTree
{
    /// <summary>
    /// A divided dimension tree to store geometry vertices
    /// </summary>
    public class VertexDDTree : PositionDDTree<Vertex>
    {
        #region Constructors

        public VertexDDTree(IList<Vertex> items, int maxDivisions = 10, double minCellSize = 1) : base(items, maxDivisions, minCellSize)
        {
        }

        #endregion
    }
}
