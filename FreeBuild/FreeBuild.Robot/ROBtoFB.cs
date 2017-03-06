using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using FreeBuild.Geometry;
using FreeBuild.Model;

namespace FreeBuild.Robot
{
    /// <summary>
    /// Helper class to convert from Robot to FreeBuild types
    /// </summary>
    public static class ROBtoFB
    {
        /// <summary>
        /// Convert a Robot 3D point to a FreeBuild Vector
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static Vector Convert(IRobotGeoPoint3D pt)
        {
            return new Vector(pt.X, pt.Y, pt.Z);
        }

        /// <summary>
        /// Convert a collection of Robot 3D points to a FreeBuild Vector array
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static Vector[] Convert(RobotGeoPoint3DCollection pts)
        {
            Vector[] result = new Vector[pts.Count];
            for (int i = 0; i < pts.Count; i++)
            {
                result[i] = pts.Get(i + 1);
            }
            return result;
        }

        /// <summary>
        /// Convert a Robot polyline geometry into a FreeBuild polyline
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
        public static PolyLine Convert(RobotGeoPolyline polyline)
        {
            PolyLine result = new PolyLine();
            RobotGeoSegmentCollection segments = polyline.Segments as RobotGeoSegmentCollection;
            for (int i = 1; i <= segments.Count; i++)
            {
                RobotGeoSegment segment = segments.Get(i);
                Vector pt = Convert(segment.P1);
                if (i == segments.Count && pt.Equals(result.StartPoint, Tolerance.Geometric))
                {
                    result.Close(true);
                }
                else result.Add(pt);
            }

            return result;
        }

        /// <summary>
        /// Convert a Robot contour geometry to a FreeBuild polycurve
        /// </summary>
        /// <param name="contour"></param>
        /// <returns></returns>
        public static PolyCurve Convert(RobotGeoContour contour)
        {
            PolyCurve result = new PolyCurve();
            RobotGeoSegmentCollection segments = contour.Segments as RobotGeoSegmentCollection;
            RobotGeoSegment firstSegment = segments.Get(1);
            Vector lastPt = Convert(firstSegment.P1);
            for (int i = 2; i <= segments.Count; i++)
            {
                RobotGeoSegment segment = segments.Get(i);
                if (segment.Type == IRobotGeoSegmentType.I_GST_ARC)
                {
                    RobotGeoSegmentArc arcSegment = (RobotGeoSegmentArc)segment;
                    Vector endPt = Convert(arcSegment.P2);
                    Arc arc = new Arc(lastPt, Convert(arcSegment.P1), endPt);
                    result.Add(arc);
                    lastPt = endPt;
                }
                else
                {
                    Vector endPt = Convert(segment.P1);
                    Line line = new Line(lastPt, endPt);
                    result.Add(line);
                    lastPt = endPt;
                }
            }
            result.Close();
            return result;
        }

        /// <summary>
        /// Convert a Robot geometry object into a FreeBuild curve
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static Curve Convert(RobotGeoObject geometry)
        {
            if (geometry.Type == IRobotGeoObjectType.I_GOT_POLYLINE)
            {
                return Convert((RobotGeoPolyline)geometry);
            }
            else if (geometry.Type == IRobotGeoObjectType.I_GOT_CONTOUR)
            {
                return Convert((RobotGeoContour)geometry);
            }
            // TODO: Others!
            else return null;
        }

        /// <summary>
        /// Convert a Robot bar end offset into a FreeBuild Vector 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Vector Convert(IRobotBarEndOffsetData offset)
        {
            return new Vector(offset.UX, offset.UY, offset.UZ);
        }

        /// <summary>
        /// Convert a Robot node to a FreeBuild one
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Node Convert(IRobotNode node)
        {
            Node result = new Node(node.X, node.Y, node.Z);
            return result;
        }

        /// <summary>
        /// Extract the position of a Robot Node as a FreeBuild Vector
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Vector PositionOf(IRobotNode node)
        {
            return new Vector(node.X, node.Y, node.Z);
        }

        /// <summary>
        /// Extract the position of a Robot bar end as a FreeBuild Vector,
        /// assembled from the position of the end's node and offset vector.
        /// </summary>
        /// <param name="barEnd">The bar end to extract the position of</param>
        /// <param name="structureNodes">The full collection of nodes within the structure that contains the bar</param>
        /// <returns></returns>
        public static Vector PositionOf(IRobotBarEnd barEnd, IRobotCollection structureNodes)
        {
            IRobotNode node = structureNodes.Get(barEnd.Node);
            return PositionOf(node) + Convert(barEnd.GetOffsetValue());
        }

        /// <summary>
        /// Extract the straight-line geometry of a Robot bar as a FreeBuild line
        /// </summary>
        /// <param name="bar">The bar to extract geometry for</param>
        /// <param name="structureNodes">The full collection of nodes within the structure that contains the bar</param>
        /// <returns></returns>
        public static Line GeometryOf(IRobotBar bar, IRobotCollection structureNodes)
        {
            return new Line(PositionOf(bar.Start, structureNodes), PositionOf(bar.End, structureNodes));
        }

        /// <summary>
        /// Convert RobotNodeSupportData to a FreeBuild node support
        /// </summary>
        /// <param name="support"></param>
        /// <returns></returns>
        public static NodeSupport Convert(RobotNodeSupportData support)
        {
            var result = new NodeSupport(
                new Base.Bool6D(support.UX != 0, support.UY != 0, support.UZ != 0,
                support.RX != 0, support.RY != 0, support.RX != 0));
            if (support.Alpha != 0 || support.Beta != 0 || support.Gamma != 0)
            {
                // TODO
            }
            return result;
        }
        
    }
}
