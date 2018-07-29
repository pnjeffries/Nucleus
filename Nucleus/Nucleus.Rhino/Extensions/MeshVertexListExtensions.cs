using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rhino
{
    /// <summary>
    /// Extension methods for the MeshVertexList class
    /// </summary>
    public static class MeshVertexListExtensions
    {
        /// <summary>
        /// Find the closest vertex to a given target point
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="target"></param>
        /// <param name="minDist"></param>
        /// <returns></returns>
        public static int ClosestVertex(this MeshVertexList vertices, Point3d target, out double minDist)
        {
            minDist = double.MaxValue;
            int iMin = -1;
            for (int i = 0; i < vertices.Count; i++)
            {
                Point3f v = vertices[i];
                double dX = target.X - v.X;
                double dY = target.Y - v.Y;
                double dZ = target.Z - v.Z;
                double distSqd = dX * dX + dY * dY + dZ * dZ;
                if (iMin < 0 || distSqd < minDist)
                {
                    iMin = i;
                    minDist = distSqd;
                }
            }
            return iMin;
        }
    }
}
