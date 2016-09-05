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
    /// vertices in a mesh.
    /// </summary>
    [Serializable]
    public class MeshFace : Unique, IOwned<Shape>
    {
        /// <summary>
        /// Private backing member variable for the Shape property
        /// </summary>
        [Copy(CopyBehaviour.MAP)]
        private Shape _Owner = null;

        /// <summary>
        /// The shape (if any) that this vertex belongs to.
        /// </summary>
        public Shape Owner
        {
            get { return _Owner; }
            internal set { _Owner = value; }
        }
    }
}
