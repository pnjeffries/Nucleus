using Nucleus.Base;
using Nucleus.DDTree;
using Nucleus.Exceptions;
using Nucleus.Extensions;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Interface for geometric objects which are linear and follow
    /// a spine curve but that have sufficient width on plan for the left and right
    /// edges to be represented by separate curves.  For example; roads, building
    /// wings, beam representations in GAs, etc.
    /// This interface provides attached extension functions to allow the generation
    /// of geometry representing the boundaries of a network of paths.
    /// </summary>
    public interface IWidePath
    {
        /// <summary>
        /// The spine curve from which the outer curves are to be derived
        /// </summary>
        Curve Spine { get; set; }

        /// <summary>
        /// The left edge curve
        /// </summary>
        Curve LeftEdge { get; set; }

        /// <summary>
        /// The right edge curve
        /// </summary>
        Curve RightEdge { get; set; }

        /// <summary>
        /// The offset distance of the left edge from the spine
        /// </summary>
        double LeftOffset { get; }

        /// <summary>
        /// The offset distance of the right edge from the spine
        /// </summary>
        double RightOffset { get; }

        /// <summary>
        /// The 'pinch' distance applied to the left-hand edge ends
        /// in order to induce curvature in the left-hand edge curve
        /// </summary>
        double LeftEndPinch { get; }

        /// <summary>
        /// The 'pinch' distance applied to the right-hand edge ends
        /// in order to induce curvature in the right-hand edge curve
        /// </summary>
        double RightEndPinch { get; }

        /// <summary>
        /// The curve capping the left side of the starting end of the path.
        /// Will usually be null.
        /// </summary>
        Curve StartCapLeft { get; set; }

        /// <summary>
        /// The curve capping the right side of the starting end of the path.
        /// Will usually be null.
        /// </summary>
        Curve StartCapRight { get; set; }

        /// <summary>
        /// The curve capping the left side of the end of the path.
        /// Will usually be null.
        /// </summary>
        Curve EndCapLeft { get; set; }

        /// <summary>
        /// The curve capping the right side of the end of the path.
        /// Will usually be null.
        /// </summary>
        Curve EndCapRight { get; set; }
    }

    /// <summary>
    /// Extension methods for the IWidePath interface
    /// </summary>
    public static class IWidePathExtensions
    {
        /// <summary>
        /// Generate initial (untrimmed) left and right edge curves for
        /// this path.
        /// </summary>
        /// <param name="path"></param>
        public static void GenerateInitialPathEdges(this IWidePath path)
        {
            if (path.Spine != null)
            {
                path.RightEdge = path.Spine.Offset(path.RightOffset);
                path.LeftEdge = path.Spine.Offset(-path.LeftOffset);
            }
        }

        /// <summary>
        /// Replace the initially generated path edges with arcs with offset ends to induce curvature in the
        /// outer edges.
        /// </summary>
        /// <param name="path"></param>
        public static void CurveInitialPathEdges(this IWidePath path)
        {

            if (path.Spine != null)
            {
                double rightEndOffset = path.RightEndPinch;
                double leftEndOffset = path.LeftEndPinch;

                if (path.RightEdge != null && rightEndOffset > 0)
                {
                    rightEndOffset = Math.Min(rightEndOffset, path.RightOffset);
                    Vector vRS = (path.Spine.StartPoint - path.RightEdge.StartPoint)/path.RightOffset;
                    Vector vRE = (path.Spine.EndPoint - path.RightEdge.EndPoint)/path.RightOffset;
                    path.RightEdge = new Arc(path.RightEdge.StartPoint + vRS * rightEndOffset, path.RightEdge.PointAt(0.5), path.RightEdge.EndPoint + vRE * rightEndOffset);
                }
                if (path.LeftEdge != null && leftEndOffset > 0)
                {
                    leftEndOffset = Math.Min(leftEndOffset, path.LeftOffset);
                    Vector vLS = (path.Spine.StartPoint - path.LeftEdge.StartPoint) / path.LeftOffset;
                    Vector vLE = (path.Spine.EndPoint - path.LeftEdge.EndPoint) / path.LeftOffset;
                    path.LeftEdge = new Arc(path.LeftEdge.StartPoint + vLS * leftEndOffset, path.LeftEdge.PointAt(0.5), path.LeftEdge.EndPoint + vLE * leftEndOffset);
                }
            }
        }

        /// <summary>
        /// Find the point on this path specified by the given parameters.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="u">A normalised parameter along the path (where 0 = Start, 1 = End)</param>
        /// <param name="v">A normalised parameter across the path (where 0 = Left, 1 = Right)</param>
        /// <returns></returns>
        public static Vector PointAt(this IWidePath path, double u, double v)
        {
            if (path.RightEdge != null && path.LeftEdge != null)
            {
                Vector vR = path.RightEdge.PointAt(u);
                Vector vL = path.LeftEdge.PointAt(u);
                return vL.Interpolate(vR, v);
            }
            else if (path.Spine != null) return path.Spine.PointAt(u);
            else return Vector.Unset;
        }

        /// <summary>
        /// Find the points on this path specified by the given parameters.
        /// The left and right edges of the path must have been previously
        /// populated in order for this function to work correctly.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="u">A normalised parameter along the path 
        /// (where 0 = Start, 1 = End)</param>
        /// <param name="v">A list of normalised parameters across the path 
        /// (where 0 = Left, 1 = Right)</param>
        /// <returns></returns>
        public static Vector[] PointsAt(this IWidePath path, double u, IList<double> v)
        {
            if (path.RightEdge != null && path.LeftEdge != null)
            {
                Vector vR = path.RightEdge.PointAt(u);
                Vector vL = path.LeftEdge.PointAt(u);
                var result = new Vector[v.Count];
                for (int i = 0; i < v.Count; i++)
                {
                    result[i] = vL.Interpolate(vR, v[i]);
                }
                return result;
            }
            else if (path.Spine != null) return new Vector[] { path.Spine.PointAt(u) };
            else return null;
        }

        /// <summary>
        /// Find the points on this path specified by the given parameters.
        /// The right and left edges of the path must have been previously
        /// populated in order for this function to return successfully.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="u">A list of normalised parameters along the path 
        /// (where 0 = Start, 1 = End)</param>
        /// <param name="v">A list of normalised parameters across the path 
        /// (where 0 = Left, 1 = Right)</param>
        /// <returns></returns>
        public static Vector[,] PointsAt(this IWidePath path, IList<double> u, IList<double> v)
        {
            if (path.RightEdge != null && path.LeftEdge != null)
            {
                Vector[,] result = new Vector[u.Count, v.Count];
                for (int i = 0; i < u.Count; i++)
                {
                    var vs = path.PointsAt(u[i], v);
                    for (int j = 0; j < v.Count; j++)
                    {
                        result[i, j] = vs[j];
                    }
                }
                return result;
            }
            /*else if (path.Spine != null)
            {
                //TODO?
            }*/
            else return null;
        }

        /// <summary>
        /// Get a closed polycurve containing the edges of this section of the path.
        /// The edges of the path must have been generated first.
        /// </summary>
        /// <returns></returns>
        public static PolyCurve GetBoundaryCurve<TPath>(this TPath path)
            where TPath:IWidePath
        {
            var result = new PolyCurve();

            if (path.StartCapLeft != null)
                result.Add(path.StartCapLeft);

            if (path.LeftEdge != null)
                result.Add(path.LeftEdge, true);

            if (path.EndCapLeft != null)
                result.Add(path.EndCapLeft, true);

            if (path.EndCapRight != null)
                result.Add(path.EndCapRight.Reversed(), true);

            if (path.RightEdge != null) 
                result.Add(path.RightEdge.Reversed(), true);

            if (path.StartCapRight != null)
                result.Add(path.StartCapRight.Reversed(), true);

            result.Close();

            return result;
        }

        /// <summary>
        /// Generate the left and right edges of the paths in this network,
        /// automatically joining them at shared end nodes.  Nodes should have been
        /// generated for the network prior to calling this function.
        /// </summary>
        /// <typeparam name="TPath"></typeparam>
        /// <param name="paths"></param>
        public static void GenerateNetworkPathEdges<TPath>(this IList<TPath> paths)
            where TPath : IWidePath
        {
            var pathMap = new Dictionary<Guid, TPath>();

            // Generate initial curves + build map:
            foreach (TPath path in paths)
            {
                if (path.Spine != null)
                {
                    pathMap[path.Spine.GUID] = path;
                    path.GenerateInitialPathEdges();
                    path.CurveInitialPathEdges();
                }
            }

            NodeCollection nodes = paths.ExtractNetworkPathNodes();

            // Trim edges at nodes:
            foreach (Node node in nodes)
            {
                if (node.Vertices.Count > 0)
                {
                    // Sort connected vertices by the angle pointing away from the node
                    var angleSorted = new SortedList<double, Vertex>(node.Vertices.Count);
                    foreach (var v in node.Vertices)
                    {

                        if (v.Owner != null && v.Owner is Curve && pathMap.ContainsKey(v.Owner.GUID))
                        {
                            Curve crv = (Curve)v.Owner;
                            if (v.IsStart) angleSorted.AddSafe(crv.TangentAt(0).Angle, v);
                            else if (v.IsEnd) angleSorted.AddSafe(crv.TangentAt(1).Reverse().Angle, v);
                        }
                    }
                    if (angleSorted.Count > 1)
                    {
                        for (int i = 0; i < angleSorted.Count - 1; i++)
                        {
                            // Reference case is path leading away from node
                            Vertex vR = angleSorted.Values.GetWrapped(i - 1);
                            Vertex v = angleSorted.Values[i];
                            Vertex vL = angleSorted.Values.GetWrapped(i + 1);

                            Angle a = new Angle(angleSorted.Keys[i]).NormalizeTo2PI();
                            Angle aR = new Angle(angleSorted.Keys.GetWrapped(i - 1) - a);
                            Angle aL = new Angle(angleSorted.Keys.GetWrapped(i + 1) - a).Explement();

                            TPath pathR = pathMap[vR.Owner.GUID];
                            TPath path = pathMap[v.Owner.GUID];
                            TPath pathL = pathMap[vL.Owner.GUID];

                            // Work out correct edges to match up based on direction:
                            Vertex edgeVR;
                            Vertex edgeVL;
                            double offsR;
                            double offsL;

                            if (v.IsStart)
                            {
                                // Curve is pointing away from the node
                                edgeVR = path.RightEdge.Start;
                                edgeVL = path.LeftEdge.Start;
                                offsR = path.RightOffset;
                                offsL = path.LeftOffset;
                            }
                            else
                            {
                                // Curve is pointing towards the node - flip everything!
                                edgeVR = path.LeftEdge.End;
                                edgeVL = path.RightEdge.End;
                                offsR = path.LeftOffset;
                                offsL = path.RightOffset;
                            }

                            Vertex edgeVR2;
                            double offsR2;
                            if (vR.IsStart)
                            {
                                edgeVR2 = pathR.LeftEdge.Start;
                                offsR2 = pathR.LeftOffset;
                            }
                            else
                            {
                                edgeVR2 = pathR.RightEdge.End;
                                offsR2 = pathR.RightOffset;
                            }

                            Vertex edgeVL2;
                            double offsL2;
                            if (vL.IsStart)
                            {
                                edgeVL2 = pathL.RightEdge.Start;
                                offsL2 = pathL.RightOffset;
                            }
                            else
                            {
                                edgeVL2 = pathL.LeftEdge.End;
                                offsL2 = pathL.LeftOffset;
                            }

                            bool canTrimR = true;
                            bool canTrimR2 = true;
                            if (aR.IsReflex)
                            {
                                if (offsR > offsR2) canTrimR = false;
                                else if (offsR2 > offsR) canTrimR2 = false;
                            }

                            if (!Curve.MatchEnds(edgeVR, edgeVR2, true, canTrimR, canTrimR2))
                            {
                                if (offsR > offsR2)
                                {
                                    Curve.ExtendToLineXY(edgeVR2, node.Position, edgeVR.Position - node.Position);
                                    path.SetEndEdge(edgeVR, new Line(edgeVR.Position, edgeVR2.Position));
                                }
                                else
                                {
                                    Curve.ExtendToLineXY(edgeVR, node.Position, edgeVR2.Position - node.Position);
                                    pathR.SetEndEdge(edgeVR2, new Line(edgeVR2.Position, edgeVR.Position));
                                }
                                /*(Curve crv = Curve.Connect(edgeVR, edgeVR2.Position);
                                if (crv != null)
                                {
                                    if (v.IsStart) path.RightEdge = crv;
                                    else path.LeftEdge = crv;
                                }*/
                            }


                            bool canTrimL = true;
                            bool canTrimL2 = true;
                            if (aL.IsReflex)
                            {
                                if (offsL > offsL2) canTrimL = false;
                                else if (offsL2 > offsL) canTrimL2 = false;
                            }

                            if (!Curve.MatchEnds(edgeVL, edgeVL2, true, canTrimL, canTrimL2))
                            {
                                if (offsL > offsL2)
                                {
                                    Curve.ExtendToLineXY(edgeVL2, node.Position, edgeVL.Position - node.Position);
                                    path.SetEndEdge(edgeVL, new Line(edgeVL.Position, edgeVL2.Position));
                                }
                                else
                                {
                                    Curve.ExtendToLineXY(edgeVL, node.Position, edgeVL2.Position - node.Position);
                                    pathL.SetEndEdge(edgeVL2, new Line(edgeVL2.Position, edgeVL.Position));
                                }
                                /*Curve crv = Curve.Connect(edgeVL, edgeVL2.Position);
                                if (crv != null)
                                {
                                    if (v.IsStart) path.LeftEdge = crv;
                                    else path.RightEdge = crv;
                                }*/
                            }
                        }
                    }
                    else if (angleSorted.Count == 1)
                    {
                        // Close off end:
                        Vertex v = angleSorted.Values[0];
                        TPath path = pathMap[v.Owner.GUID];
                        if (v.IsStart)
                        {
                            path.StartCapLeft = new Line(path.LeftEdge.StartPoint, path.RightEdge.StartPoint);
                        }
                        else
                        {
                            path.EndCapLeft = new Line(path.LeftEdge.EndPoint, path.RightEdge.EndPoint);
                        }
                    }
                }
                
            }
        }

        private static void SetEndEdge(this IWidePath path, Vertex edgeEnd, Curve endCrv)
        {
            if (path.LeftEdge.Start == edgeEnd) path.StartCapLeft = endCrv;
            else if (path.LeftEdge.End == edgeEnd) path.EndCapLeft = endCrv;
            else if (path.RightEdge.Start == edgeEnd) path.StartCapRight = endCrv;
            else path.EndCapRight = endCrv;
        }

        /// <summary>
        /// Extract from this collection of paths all of the unique nodes at the ends of the
        /// path spine geometry.
        /// </summary>
        /// <typeparam name="TPath"></typeparam>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static NodeCollection ExtractNetworkPathNodes<TPath>(this IList<TPath> paths)
            where TPath : IWidePath
        {
            var result = new NodeCollection();
            foreach (TPath path in paths)
            {
                var nodeS = path.Spine?.Start?.Node;
                if (nodeS != null && !result.Contains(nodeS.GUID))
                    result.Add(nodeS);
                var nodeE = path.Spine?.End?.Node;
                if (nodeE != null && !result.Contains(nodeE.GUID))
                    result.Add(nodeE);
            }
            return result;
        }

        /// <summary>
        /// Generate nodes at the ends of the paths in this collection.
        /// It is not necessary for the geometry to be part of a Model for this
        /// function, nor will any newly created nodes be added automatically
        /// to the current model - this should be done subsequently if necessary.
        /// </summary>
        /// <typeparam name="TPath"></typeparam>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static NodeCollection GenerateNetworkPathNodes<TPath>(this IList<TPath> paths, NodeGenerationParameters gParams)
            where TPath : IWidePath
        {
            var nodes = new NodeCollection();
            foreach (TPath path in paths)
            {
                Node sNode = path.Spine?.Start?.Node;
                if (sNode != null &&
                    !nodes.Contains(sNode.GUID))
                    nodes.Add(sNode);
                Node eNode = path.Spine?.End?.Node;
                if (eNode != null &&
                    !nodes.Contains(eNode.GUID))
                    nodes.Add(eNode);
            }
            var tree = new NodeDDTree(nodes);
            foreach (TPath path in paths)
            {
                path.Spine?.Start.GenerateNode(gParams, nodes, tree);
                path.Spine?.End.GenerateNode(gParams, nodes, tree);
            }
            return nodes;
        }

        /// <summary>
        /// Extract all of the left edges from the path segments in this collection
        /// </summary>
        /// <typeparam name="TPath"></typeparam>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static CurveCollection ExtractNetworkPathLeftEdges<TPath>(this IList<TPath> paths)
            where TPath : IWidePath
        {
            var result = new CurveCollection();
            foreach (TPath path in paths) result.Add(path.LeftEdge);
            return result;
        }

        /// <summary>
        /// Extract all of the left edges from the path segments in this collection
        /// </summary>
        /// <typeparam name="TPath"></typeparam>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static CurveCollection ExtractNetworkPathRightEdges<TPath>(this IList<TPath> paths)
            where TPath : IWidePath
        {
            var result = new CurveCollection();
            foreach (TPath path in paths) result.Add(path.RightEdge);
            return result;
        }

        /// <summary>
        /// Calculate the total length of the spines of all 
        /// in this collection.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double TotalSpineLength<TPath>(this IList<TPath> list)
            where TPath : IWidePath
        {
            double result = 0;
            foreach (var element in list)
            {
                result += element.Spine.Length;
            }
            return result;
        }

        /// <summary>
        /// Count the number of dead-ends in the paths in this collection
        /// </summary>
        /// <typeparam name="TPath"></typeparam>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static int DeadEndCount<TPath>(this IList<TPath> paths)
            where TPath : IWidePath
        {
            int result = 0;

            var pathMap = new Dictionary<Guid, TPath>();

            // Generate initial curves + build map:
            foreach (TPath path in paths)
            {
                if (path.Spine != null)
                {
                    pathMap[path.Spine.GUID] = path;
                    path.GenerateInitialPathEdges();
                    path.CurveInitialPathEdges();
                }
            }

            NodeCollection nodes = paths.ExtractNetworkPathNodes();

            // Trim edges at nodes:
            foreach (Node node in nodes)
            {
                int nCount = 0;
                foreach (var v in node.Vertices)
                {
                    if (v.Owner != null && v.Owner is Curve && pathMap.ContainsKey(v.Owner.GUID))
                    {
                        nCount++;
                    }
                }
                if (nCount == 1) result++;
            }

            return result;
        }

    }
}
