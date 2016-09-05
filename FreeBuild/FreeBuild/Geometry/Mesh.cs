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
        public override bool IsValid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// The mesh may contain as many vertices as you like, with the connecting topology
        /// described by the Faces collection.
        /// </summary>
        public override VertexCollection Vertices { get; }
    }
}
