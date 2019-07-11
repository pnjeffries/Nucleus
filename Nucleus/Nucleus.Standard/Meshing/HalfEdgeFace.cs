using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Meshing
{
    /// <summary>
    /// A face in a half-edge mesh
    /// </summary>
    [Serializable]
    public class HalfEdgeFace
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Edge property
        /// </summary>
        private HalfEdge _Edge;

        /// <summary>
        /// One of the half-edges bordering the face
        /// </summary>
        public HalfEdge Edge
        {
            get { return _Edge; }
            set { _Edge = value; }
        }

        #endregion
    }
}
