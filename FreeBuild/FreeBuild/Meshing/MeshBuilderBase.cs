using FreeBuild.Geometry;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Meshing
{
    /// <summary>
    /// An abstract base class used to generically construct meshes in an application-specific format
    /// </summary>
    public abstract class MeshBuilderBase
    {
        /// <summary>
        /// Add a new vertex to the mesh
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>The new vertex index</returns>
        /// <remarks>The returned vertex indices should be sequential</remarks>
        public abstract int AddVertex(Vector pt);

        /// <summary>
        /// Add a new tri face to the mesh.
        /// By default, the winding order is counter-clockwise.
        /// </summary>
        /// <param name="v1">The first vertex index</param>
        /// <param name="v2">The second vertex index</param>
        /// <param name="v3">The third vertex index</param>
        /// <returns>The new face index</returns>
        public abstract int AddFace(int v1, int v2, int v3);

        /// <summary>
        /// Add a new quad face to the mesh.
        /// By default, the winding order is counter-clockwise.
        /// </summary>
        /// <param name="v1">The first vertex index</param>
        /// <param name="v2">The second vertex index</param>
        /// <param name="v3">The third vertex index</param>
        /// <param name="v4">The fourth vertex index</param>
        /// <returns>The new face index</returns>
        public abstract int AddFace(int v1, int v2, int v3, int v4);

        /// <summary>
        /// Add a new tri face to the mesh
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pt3"></param>
        /// <returns>The new face index</returns>
        public int AddFace(Vector pt1, Vector pt2, Vector pt3)
        {
            return AddFace(AddVertex(pt1), AddVertex(pt2), AddVertex(pt3));
        }

        /// <summary>
        /// Add a new quad face to the mesh
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pt3"></param>
        /// <param name="pt4"></param>
        /// <returns>The new face index</returns>
        public int AddFace(Vector pt1, Vector pt2, Vector pt3, Vector pt4)
        {
            return AddFace(AddVertex(pt1), AddVertex(pt2), AddVertex(pt3), AddVertex(pt4));
        }

        /// <summary>
        /// Add a new mesh face specified by an ordered list of vertex indices
        /// </summary>
        /// <param name="indices">A list of vertex indices to be converted into a face.  Should contain at least three values.</param>
        /// <returns>The new face index, or -1 if there were insufficient indices to build a face</returns>
        public int AddFace(IList<int> indices)
        {
            if (indices.Count == 3 || indices.Count == 6) //TRI
            {
                return AddFace(indices[0], indices[1], indices[2]);
            }
            else if (indices.Count >= 4) //QUAD
            {
                return AddFace(indices[0], indices[1], indices[2], indices[3]);
            }
            return -1;
        }

        /// <summary>
        /// Add a new face to the mesh based on a structural 2D Element's geometry
        /// </summary>
        /// <param name="element"></param>
        /// <returns>The new face index</returns>
        public int AddFace(PanelElement element)
        {
            //TODO!
            //if (element.Nodes.Count == 3 || element.Nodes.Count == 6) //TRI
            //{
            //    return AddFace(element.OffsetNodePosition(0), element.OffsetNodePosition(1), element.OffsetNodePosition(2));
            //}
            //else if (element.Nodes.Count >= 4) //QUAD
            //{
            //    return AddFace(element.OffsetNodePosition(0), element.OffsetNodePosition(1),
            //        element.OffsetNodePosition(2), element.OffsetNodePosition(3));
            //}
            return -1;
        }

        /// <summary>
        /// Add a set of points to the mesh as new vertices
        /// </summary>
        /// <param name="points">The points to be added as vertices</param>
        /// <returns>The vertex index of the last added point</returns>
        public int AddVertices(IEnumerable<Vector> points)
        {
            int lastVI = 0;
            foreach (Vector point in points)
            {
                lastVI = AddVertex(point);
            }
            return lastVI;
        }

        /// <summary>
        /// Add a set of faces to the mesh described as a set of Lists of vertex indices
        /// </summary>
        /// <param name="faces">The collection of lists of vertex indices to be used to describe the face topology</param>
        /// <returns>The face index of the last valid added face</returns>
        public int AddFaces(IEnumerable<IList<int>> faces)
        {
            int lastFI = 0;
            foreach (IList<int> face in faces)
            {
                int FI = AddFace(face);
                if (FI >= 0) lastFI = FI;
            }
            return lastFI;
        }

        /// <summary>
        /// Add strips of faces swept between a set of polylines defined as point lists
        /// </summary>
        /// <param name="pointLists">A list of lists of points representing polylines.  Each list should have the same number of points contained within.</param>
        /// <param name="close"></param>
        public void AddLoft(IList<IList<Vector>> pointLists, bool close = false)
        {
            for (int i = 0; i < pointLists.Count; i++)
            {
                IList<Vector> points = pointLists[i];
                int stripSize = points.Count;
                for (int j = 0; j < stripSize; j++)
                {
                    int vi = AddVertex(points[j]);
                    if (i > 0 && j > 0)
                    {
                        AddFace(vi - 1 - stripSize, vi - 1, vi, vi - stripSize);
                        if (close && j == stripSize - 1)
                        {
                            AddFace(vi + 1 - 2 * stripSize, vi - stripSize, vi, vi + 1 - stripSize); //Close meshing
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Add strips of faces sweep between lines of points lying on a set of frames
        /// </summary>
        /// <param name="frames">A set of coordinate systems representing frames to be swept between</param>
        /// <param name="pointStrip">A polyline of points in frame-coordinates that will be used to create vertices on each frame</param>
        /// <param name="close">If true, the strip of points will be closed (i.e. the last point in each strip will be meshed with the first)</param>
        public void AddSweep(IList<CartesianCoordinateSystem> frames, IList<Vector> pointStrip, bool close = false)
        {
            int stripSize = pointStrip.Count;
            for (int i = 0; i < frames.Count; i++)
            {
                ICoordinateSystem cs = frames[i];
                for (int j = 0; j < stripSize; j++)
                {
                    Vector pt = pointStrip[j];
                    Vector position = cs.LocalToGlobal(pt);
                    int vi = AddVertex(position);
                    if (i > 0 && j > 0)
                    {
                        AddFace(vi - 1 - stripSize, vi - 1, vi, vi - stripSize);
                        if (close && j == stripSize - 1)
                        {
                            AddFace(vi + 1 - 2 * stripSize, vi - stripSize, vi, vi + 1 - stripSize); //Close meshing
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add a set of faces generated by sweeping a curve between a set of frames.
        /// </summary>
        /// <param name="frames">The local coordinate frames between which faces will be swept</param>
        /// <param name="profile">The curve to sweep.</param>
        /// <param name="useYZ"></param>
        public void AddSweep(IList<CartesianCoordinateSystem> frames, Curve profile, bool useYZ = false)
        {
            if (profile is PolyCurve) //Polycurves are a special case which will be recursively broken up to subcurves
            {
                PolyCurve pC = (PolyCurve)profile;
                foreach (Curve subCrv in pC.SubCurves) AddSweep(frames, subCrv, useYZ);
            }
            else
            {
                Vector[] pointStrip = profile.Facet(Math.PI / 6);
                if (useYZ) pointStrip = pointStrip.RemapZnegXY();
                AddSweep(frames, pointStrip, profile.Closed);
            }
        }

        /// <summary>
        /// Create faces out of points from the specified list, starting by joining the start points to the end points
        /// and moving towards the middle of the list
        /// </summary>
        /// <param name="pointStrip"></param>
        /// <param name="startOffset"></param>
        public void FillStartToEnd(IList<Vector> pointStrip, int startOffset = 1)
        {
            for (int i = 0; i < pointStrip.Count / 2 - startOffset; i++)
            {
                Vector v0 = pointStrip[pointStrip.Count - 1 - i];
                Vector v1 = pointStrip[i + startOffset];
                Vector v2 = pointStrip[i + startOffset + 1];
                Vector v3 = pointStrip[pointStrip.Count - 2 - i];
                AddFace(v0, v1, v2, v3);
            }
        }

        /// <summary>
        /// Add a set of vertices and faces to this mesh representing the given element complete with
        /// a 3D representation of the assigned section profile
        /// </summary>
        /// <param name="element"></param>
        public void AddSectionPreview(LinearElement element)
        {
            Angle tolerance = Angle.FromDegrees(15); //TODO: Make adjustable
            SectionProperty section = element.Property;
            Curve geometry = element.Geometry;
            if (section != null && geometry != null && section.Profile != null)
            {
                Angle orientation = 0;
                if (element.Orientation != null) orientation = element.Orientation.Angle();
                IList<CartesianCoordinateSystem> frames = geometry.FacetCSystems(tolerance, orientation);
                if (frames.Count > 0)
                {
                    Curve perimeter = section.Profile.Perimeter;
                    AddSweep(frames, perimeter, true);

                    //Caps:
                    Vector[] pointStrip = perimeter.Facet(Math.PI / 6);
                    Vector[] startPts = frames[0].LocalToGlobal(pointStrip.RemapZnegXY());
                    FillStartToEnd(startPts);
                    Vector[] endPts = frames.Last().LocalToGlobal(pointStrip.RemapZXY());
                    FillStartToEnd(endPts);
                }
            }
        }

        /// <summary>
        /// Add a set of vertices and faces to this mesh representing a cone
        /// </summary>
        /// <param name="tip">The point at the tip of the cone</param>
        /// <param name="baseCircle">The base circle of the cone</param>
        /// <param name="baseResolution">The number of positions around the cone where mesh facets will be generated.</param>
        public void AddCone(Vector tip, Circle baseCircle, int baseResolution)
        {
            IList<Vector> basePoints = baseCircle.Divide(baseResolution - 1);
            //if (basePoints.Count >= baseResolution) basePoints.RemoveAt(baseResolution); //Get rid of duplicate last point
            IList<Vector> tipPoints = new List<Vector>();
            for (int i = 0; i < basePoints.Count; i++)
            {
                tipPoints.Add(tip);
            }
            IList<IList<Vector>> topology = new List<IList<Vector>>();
            topology.Add(basePoints);
            topology.Add(tipPoints);
            AddLoft(topology, true);
        }
    }
}
