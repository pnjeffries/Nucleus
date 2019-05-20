using Nucleus.Extensions;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nucleus.Unity
{
    /// <summary>
    /// Extension methods for the Unity Mesh type
    /// </summary>
    public static class UnityMeshExtensions
    {
        /// <summary>
        /// Set the vertex colours of this mesh based on the specified array of values, one
        /// for each vertex of the mesh, mapped to a colour gradient from a range.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="values"></param>
        /// <param name="range"></param>
        /// <param name="gradient"></param>
        public static void SetVertexColoursFromGradient(this Mesh mesh, IList<double> values,
            Interval range, Gradient gradient)
        {
            Color[] vertexColours = new Color[mesh.vertexCount];
            int iCount = Math.Min(values.Count, mesh.vertexCount);
            for (int i = 0; i < iCount; i++)
            {
                float value = (float)range.ParameterOf(values[i]);
                var c = gradient.Evaluate(value);
                vertexColours[i] = c;
            }
            mesh.colors = vertexColours;
        }

        /// <summary>
        /// Set the vertex colours of this mesh based on the specified fieldOfView map.
        /// For this to do anything at all reasonable, the mesh should have been generated
        /// as a regular grid with (mapXSize + 2) * 2 + 1 vertices in the first direction via a function
        /// such as MeshBuilder.AddQuadGridMesh.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="fieldOfView"></param>
        /// <param name="oldFieldOfView"></param>
        /// <param name="t"></param>
        /// <param name="tweening"></param>
        /// <param name="power">The power to use for interpolating values in-between cells</param>
        public static void SetVertexColoursFromVisionMap(this Mesh mesh, Geometry.SquareCellMap<int> fieldOfView,
            Geometry.SquareCellMap<int> oldFieldOfView = null, double t = 1.0, Interpolation tweening = Interpolation.Linear,
            double power = 1)
        {
            Color32[] vertexColours = new Color32[mesh.vertexCount];
            int uSizeMesh = (fieldOfView.SizeX + 2) * 2 + 1;
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                int u = i % uSizeMesh;
                int v = i / uSizeMesh;
                double x = (u - 3) / 2.0;
                double y = (v - 3) / 2.0;
                double viewValue = 0;
                if (x.IsWholeNumber() && y.IsWholeNumber())
                {
                    int iX = (int)x;
                    int iY = (int)y;
                    viewValue = fieldOfView[iX, iY];
                    if (oldFieldOfView != null)
                    {
                        double oldViewValue = oldFieldOfView[iX, iY];
                        viewValue = tweening.Interpolate(oldViewValue, viewValue, t);
                    }
                }
                else
                {
                    viewValue = AverageGridValue(x, y, fieldOfView, power);
                    if (oldFieldOfView != null)
                    {
                        double oldViewValue = AverageGridValue(x, y, oldFieldOfView, power);
                        viewValue = tweening.Interpolate(oldViewValue, viewValue, t);
                    }
                }
                byte alpha = (byte)(255.0 / 10 * (10 - viewValue));
                vertexColours[i] = new Color32(0, 0, 0, alpha);
            }

            /*
            int uSizeMesh = mapUSize + 2;
            for (int i = 0; i < fieldOfView.CellCount; i++)
            {
                int vMesh = i / mapUSize + 1;
                int uMesh = i % mapUSize + 1;
                int iMesh = uSizeMesh * vMesh + uMesh;
                if (iMesh < mesh.vertexCount)
                {
                    byte alpha = (byte)(255.0 / 10 * (10 - fieldOfView[i]));
                    vertexColours[iMesh] = new Color32(0, 0, 0, alpha);
                }
            }*/

            mesh.colors32 = vertexColours;
        }

        private static double AverageGridValue(double x, double y, Geometry.SquareCellMap<int> fieldOfView, double power)
        {
            double viewValue = 0;
            // Intermediate vertex - take the average of the adjoining cells:
            if (x >= 0)
            {
                if (y >= 0)
                    viewValue += fieldOfView[(int)x.Floor(), (int)y.Floor()];
                if (y < fieldOfView.SizeY)
                    viewValue += fieldOfView[(int)x.Floor(), (int)y.Ceiling()];
            }
            if (x < fieldOfView.SizeX)
            {
                if (y >= 0)
                    viewValue += fieldOfView[(int)x.Ceiling(), (int)y.Floor()];
                if (y < fieldOfView.SizeY)
                    viewValue += fieldOfView[(int)x.Ceiling(), (int)y.Ceiling()];
            }
            viewValue /= 4;
            return viewValue;
        }
    }
}
