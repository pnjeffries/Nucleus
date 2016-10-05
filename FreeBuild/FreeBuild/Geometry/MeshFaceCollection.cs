using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A collection of mesh face objects
    /// </summary>
    [Serializable]
    public class MeshFaceCollection : OwnedCollection<MeshFace, Mesh>
    {
        public MeshFaceCollection(Mesh owner) : base(owner)
        {
        }

        protected override void ClearItemOwner(MeshFace item)
        {
            item.Owner = null;
        }

        protected override void SetItemOwner(MeshFace item)
        {
            item.Owner = Owner;
        }
    }
}
