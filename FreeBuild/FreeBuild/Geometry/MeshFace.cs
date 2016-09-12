using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Mesh face representation.  Describes a face which connects three or more
    /// vertices in a mesh.  The face is defined as a series of vertex indices in the
    /// parent mesh.
    /// </summary>
    [Serializable]
    public class MeshFace : Unique, IOwned<Mesh>
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Owner property
        /// </summary>
        [Copy(CopyBehaviour.MAP)]
        private Mesh _Owner = null;

        /// <summary>
        /// The shape (if any) that this vertex belongs to.
        /// </summary>
        public Mesh Owner
        {
            get { return _Owner; }
            internal set { _Owner = value; }
        }

        /// <summary>
        /// The collection of vertex indices that describe the topology of this
        /// face.
        /// </summary>
        public IList<int> Indices { get; } = new List<int>();

        #endregion



        #region Methods

        #endregion
    }
}
