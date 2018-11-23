using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Meshing
{
    /// <summary>
    /// A vertex in a Half-Egde mesh
    /// </summary>
    public class HalfEdgeVertex : IPosition
    {

        #region Properties

        /// <summary>
        /// Private backing member variable for the Position property
        /// </summary>
        private Vector _Position;

        /// <summary>
        /// The position of the vertex
        /// </summary>
        public Vector Position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        /// <summary>
        /// Private backing member variable for the Edge property
        /// </summary>
        private HalfEdge _Edge;

        /// <summary>
        /// One of the half-edges emanating from this vertex
        /// </summary>
        public HalfEdge Edge
        {
            get { return _Edge; }
            set { _Edge = value; }
        }


        #endregion

    }
}
