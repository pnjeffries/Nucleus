using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    [Serializable]
    public class Mesh : Surface
    {
        /// <summary>
        /// Is this mesh valid?
        /// </summary>
        public override bool IsValid
        {
            get
            {
                //TODO!
                return true;
            }
        }

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// The mesh may contain as many vertices as you like, with the connecting topology
        /// described by the Faces collection.
        /// </summary>
        public override VertexCollection Vertices { get; }

        /// <summary>
        /// Private backing field for Faces property
        /// </summary>
        private MeshFaceCollection _Faces;

        /// <summary>
        /// The collection of faces which describe the topology of the mesh.
        /// </summary>
        public MeshFaceCollection Faces
        {
            get
            {
                if (_Faces == null) _Faces = new MeshFaceCollection(this);
                return _Faces;
            }
        }
    }
}
