using Assimp;
using A = Assimp;
using Nucleus.Geometry;
using NG = Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Assimp
{
    /// <summary>
    /// Static helper class to translate Assimp geometry types to equivalent Nucleus types
    /// </summary>
    public static class FromAssimp
    {
        /// <summary>
        /// Convert an Assimp scene to a Nucleus vertex geometry collection
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static VertexGeometryCollection Convert(Scene scene)
        {
            var result = new VertexGeometryCollection();
            foreach (var mesh in scene.Meshes)
            {
                var nMesh = Convert(mesh);
                if (nMesh != null) result.Add(nMesh);
            }
            return result;
        }

        /// <summary>
        /// Convert an Assimp vector to a Nucleus one
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector Convert(Vector3D vector)
        {
            return new Vector(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Convert an Assimp mesh to a Nucleus one
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static NG.Mesh Convert(A.Mesh mesh)
        {
            var result = new NG.Mesh();
            foreach (var v in mesh.Vertices)
            {
                result.AddVertex(Convert(v));
            }
            foreach (var face in mesh.Faces)
            {
                result.AddFace(face.Indices);
            }
            return result;
        }
    }
}
