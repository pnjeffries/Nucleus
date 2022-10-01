using Assimp;
using A = Assimp;
using Nucleus.Geometry;
using NG = Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Rendering;

namespace Nucleus.Assimp
{
    /// <summary>
    /// Converter helper functions to create Assimp equivalents of Nucleus geometry types
    /// </summary>
    public static class ToAssimp
    {

        /// <summary>
        /// Convert a Nucleus vector to an Assimp one
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3D Convert(Vector vector)
        {
            return new Vector3D((float)vector.X, (float)vector.Y, (float)vector.Z);
        }

        /// <summary>
        /// Convert a Nucleus mesh to an Assimp one
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static A.Mesh Convert(NG.Mesh mesh)
        {
            mesh.AssignVertexNumbers();

            var result = new A.Mesh("", PrimitiveType.Triangle);
            foreach (var v in mesh.Vertices)
            {
                result.Vertices.Add(Convert(v.Position));
            }
            foreach (var f in mesh.Faces)
            {
                var indices = f.ToIndexArray();
                // Write as tris:
                for (int i = 1; i < indices.Length - 2; i++)
                {
                    result.Faces.Add(new Face(new int[] {indices[0], indices[i], indices[i+1] }));
                }
            }
            return result;
        }

        /// <summary>
        /// Convert a Nucleus Colour to an Assimp Color4D
        /// </summary>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static Color4D Convert(Colour colour)
        {
            return new Color4D(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
        }

        /// <summary>
        /// Convert a Nucleus DisplayBrush to a Assimp Material
        /// </summary>
        /// <param name="brush"></param>
        /// <returns></returns>
        public static Material Convert(DisplayBrush brush)
        {
            var result = new Material();
            result.ColorDiffuse = Convert(brush.BaseColour);
            result.Opacity = brush.BaseColour.A / 255f;
            return result;
        }

        /// <summary>
        /// Convert a Nucleus GeometryLayerTable to an Assimp scene
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static Scene Convert(GeometryLayerTable geometry)
        {
            var result = new Scene();
            var rootNode = new Node("Root");
            result.RootNode = rootNode;
            var defaultMaterial = new Material();
            /*result.Materials.Add(new Material()); //Default material
            foreach (var layer in geometry)
            {
                //var layerNode = new Node(layer.Name, rootNode);
                int layerMaterialID = 0;
                if (layer.Brush != null)
                {
                    Material layerMaterial = Convert(layer.Brush);
                    layerMaterialID = result.MaterialCount + 1;
                    result.Materials.Add(layerMaterial);
                }
                foreach (var geoObj in layer)
                {
                    if (geoObj is NG.Mesh mesh)
                    {
                        var assMesh = Convert(mesh);
                        // TODO: Allow for individual mesh materials as well
                        if (assMesh.MaterialIndex == 0) assMesh.MaterialIndex = layerMaterialID;
                        int meshIndex = result.MeshCount + 1;
                        result.Meshes.Add(assMesh);
                        var meshNode = new Node("", rootNode);
                        meshNode.MeshIndices.Add(meshIndex);
                    }
                }
            }*/
            return result;
        }
    }
}
