using Nucleus.DDTree;
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
                }
            }

            NodeCollection nodes = paths.ExtractNetworkPathNodes();

            // Trim edges at nodes:
            foreach (Node node in nodes)
            {
                if (node.Vertices.Count > 1)
                {
                    // Sort connected vertices by the angle pointing away from the node
                    var angleSorted = new SortedList<double, Vertex>(node.Vertices.Count);
                    foreach (var v in node.Vertices)
                    {

                        if (v.Owner != null && v.Owner is Curve && pathMap.ContainsKey(v.Owner.GUID))
                        {
                            Curve crv = (Curve)v.Owner;
                            if (v.IsStart) angleSorted.Add(crv.TangentAt(0).Angle, v);
                            else if (v.IsEnd) angleSorted.Add(crv.TangentAt(1).Reverse().Angle, v);
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

                            TPath pathR = pathMap[vR.Owner.GUID];
                            TPath path = pathMap[v.Owner.GUID];
                            TPath pathL = pathMap[vL.Owner.GUID];

                            // Work out correct edges to match up based on direction:
                            Vertex edgeVR;
                            Vertex edgeVL;
                            
                            if (v.IsStart)
                            {
                                edgeVR = path.RightEdge.Start;
                                edgeVL = path.LeftEdge.Start;
                            }
                            else
                            {
                                edgeVR = path.LeftEdge.End;
                                edgeVL = path.RightEdge.End;
                            }

                            Vertex edgeVR2;
                            if (vR.IsStart) edgeVR2 = pathR.LeftEdge.Start;
                            else edgeVR2 = pathR.RightEdge.End;

                            Vertex edgeVL2;
                            if (vL.IsStart) edgeVL2 = pathL.RightEdge.Start;
                            else edgeVL2 = pathL.LeftEdge.End;

                            if (!Curve.MatchEnds(edgeVR, edgeVR2, true))
                            {
                                if (edgeVR.Position.XYDistanceToSquared(node.Position) > edgeVR2.Position.XYDistanceToSquared(node.Position))
                                    Curve.ExtendToLineXY(edgeVR2, node.Position, edgeVR.Position - node.Position);
                                else
                                    Curve.ExtendToLineXY(edgeVR, node.Position, edgeVR2.Position - node.Position);
                                /*(Curve crv = Curve.Connect(edgeVR, edgeVR2.Position);
                                if (crv != null)
                                {
                                    if (v.IsStart) path.RightEdge = crv;
                                    else path.LeftEdge = crv;
                                }*/
                            }
                            if (!Curve.MatchEnds(edgeVL, edgeVL2, true))
                            {
                                if (edgeVL.Position.XYDistanceToSquared(node.Position) > edgeVL2.Position.XYDistanceToSquared(node.Position))
                                    Curve.ExtendToLineXY(edgeVL2, node.Position, edgeVL.Position - node.Position);
                                else
                                    Curve.ExtendToLineXY(edgeVL, node.Position, edgeVL2.Position - node.Position);
                                /*Curve crv = Curve.Connect(edgeVL, edgeVL2.Position);
                                if (crv != null)
                                {
                                    if (v.IsStart) path.LeftEdge = crv;
                                    else path.RightEdge = crv;
                                }*/
                            }
                        }
                    }
                }
            }
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
    }
}
