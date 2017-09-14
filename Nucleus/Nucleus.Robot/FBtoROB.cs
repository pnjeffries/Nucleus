using Nucleus.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Robot
{
    /// <summary>
    /// Library of static conversion functions to convert from Nucleus to Robot datatypes
    /// </summary>
    public static class FBtoROB
    {
        /// <summary>
        /// Convert a Nucleus point to a RobotGeoPoint3D
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static RobotGeoPoint3D Convert(Vector pt)
        {
            var result = new RobotGeoPoint3D();
            result.X = pt.X;
            result.Y = pt.Y;
            result.Z = pt.Z;
            return result;
        }

        /// <summary>
        /// Convert an array of Nucleus vectors to a RobotPointsArray
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static RobotPointsArray Convert(Vector[] pts)
        {
            var result = new RobotPointsArray();
            result.SetSize(pts.Length);
            for (int i = 0; i < pts.Length; i++)
            {
                Vector pt = pts[i];
                result.Set(i + 1, pt.X,pt.Y,pt.Z);
            }
            return result;
        }

        /// <summary>
        /// Add a line segment to the end of the specified Robot polyline
        /// </summary>
        /// <param name="result"></param>
        /// <param name="v"></param>
        private static void AddPolylinePoint(RobotGeoPolyline result, Vertex v)
        {
            var segment = new RobotGeoSegmentLine();
            segment.P1 = Convert(v.Position);
            result.Add((RobotGeoSegment)segment); // Wow!  RobotOM has an F'd up inheritance structure!
        }

        /// <summary>
        /// Convert a Nucleus polyline to a RobotGeoPolyline
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
        public static RobotGeoPolyline Convert(PolyLine polyline)
        {
            RobotGeoPolyline result = new RobotGeoPolyline();
            foreach (Vertex v in polyline.Vertices)
            {
                AddPolylinePoint(result, v);
            }
            if (polyline.Closed)
            {
                AddPolylinePoint(result, polyline.Start);
            }
            return result;
        }

        private static void AddContourSegment(RobotGeoContour result, Curve crv)
        {
            if (crv.IsValid)
            {
                if (crv is Arc)
                {
                    Arc arc = (Arc)crv;
                    var segment = new RobotGeoSegmentArc();
                    segment.P1 = Convert(arc.Vertices[1].Position);
                    segment.P2 = Convert(arc.EndPoint);
                    result.Add((RobotGeoSegment)segment);
                }
                else if (crv is PolyLine)
                {
                    foreach (Vertex v in crv.Vertices)
                    {
                        var segment = new RobotGeoSegmentLine();
                        segment.P1 = Convert(v.Position);
                        result.Add((RobotGeoSegment)segment);
                    }
                }
                else if (crv is PolyCurve)
                {
                    foreach (Curve subCrv in ((PolyCurve)crv).SubCurves)
                    {
                        AddContourSegment(result, subCrv);
                    }
                }
                else // Everything else treated as a line
                {
                    var segment = new RobotGeoSegmentLine();
                    segment.P1 = Convert(crv.EndPoint);
                    result.Add((RobotGeoSegment)segment);
                }
            }
        }

        /// <summary>
        /// Convert a Nucleus polyCurve to a Robot Contour
        /// </summary>
        /// <param name="polyCurve"></param>
        /// <returns></returns>
        public static RobotGeoContour Convert(PolyCurve polyCurve)
        {
            RobotGeoContour result = new RobotGeoContour();
            foreach (Curve subCrv in polyCurve.SubCurves)
            {
                AddContourSegment(result, subCrv);
            }
            return result;
        }

        /// <summary>
        /// Convert a Nucleus geometry object to a Robot one
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static RobotGeoObject Convert(VertexGeometry geometry)
        {
            if (geometry is PolyLine) return (RobotGeoObject)Convert((PolyLine)geometry);
            else if (geometry is PolyCurve) return (RobotGeoObject)Convert((PolyCurve)geometry);
            // TODO!
            else return null;
        }

        /// <summary>
        /// Convert Nucleus horizontal and vertical set-out enumerations into a Robot IRobotBarOffsetAutoPosition
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        public static IRobotBarOffsetAutoPosition Convert(HorizontalSetOut horizontal, VerticalSetOut vertical)
        {
            if (horizontal == HorizontalSetOut.Left)
            {
                if (vertical == VerticalSetOut.Top) return IRobotBarOffsetAutoPosition.I_BOAP_VY_VPZ;
                else if (vertical == VerticalSetOut.Bottom) return IRobotBarOffsetAutoPosition.I_BOAP_VY_VZ;
                else return IRobotBarOffsetAutoPosition.I_BOAP_VY_0;
            }
            else if (horizontal == HorizontalSetOut.Right)
            {
                if (vertical == VerticalSetOut.Top) return IRobotBarOffsetAutoPosition.I_BOAP_VPY_VPZ;
                else if (vertical == VerticalSetOut.Bottom) return IRobotBarOffsetAutoPosition.I_BOAP_VPY_VZ;
                else return IRobotBarOffsetAutoPosition.I_BOAP_VPY_0;
            }
            else
            {
                if (vertical == VerticalSetOut.Top) return IRobotBarOffsetAutoPosition.I_BOAP_0_VPZ;
                else if (vertical == VerticalSetOut.Bottom) return IRobotBarOffsetAutoPosition.I_BOAP_0_VZ;
                else return IRobotBarOffsetAutoPosition.I_BOAP_0_0;
            }
        }
    }
}
