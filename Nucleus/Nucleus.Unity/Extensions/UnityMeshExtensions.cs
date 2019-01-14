using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nucleus.Unity.Extensions
{
    /// <summary>
    /// Extension methods for the Unity Mesh type
    /// </summary>
    public static class UnityMeshExtensions
    {
        /// <summary>
        /// Set the vertex colours of this mesh based on the specified fieldOfView map.
        /// For this to do anything at all reasonable, the mesh should have been generated
        /// as a regular grid with mapXSize + 2 vertices in the first direction via a function
        /// such as MeshBuilder.AddQuadGridMesh.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="fieldOfView"></param>
        /// <param name="mapUSize"></param>
        public static void SetVertexColoursFromVisionMap(this Mesh mesh, Geometry.ICellMap<int> fieldOfView, int mapUSize)
        {
            Color32[] vertexColours = new Color32[mesh.vertexCount];
            int uSizeMesh = mapUSize + 2;
            for (int i = 0; i < fieldOfView.CellCount; i++)
            {
                int vMesh = i / mapUSize + 1;
                int uMesh = i % mapUSize + 1;
                int iMesh = uSizeMesh * vMesh + uMesh;
                vertexColours[iMesh] = new Color32(0, 0, 0, (byte)(255 * (10.0 / fieldOfView[i])));
            }
            mesh.colors32 = vertexColours;
        }
    }
}
