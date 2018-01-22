// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Nucleus.Base;
using Nucleus.Exceptions;
using Nucleus.Extensions;
using Nucleus.Geometry;
using Nucleus.Maths;
using Nucleus.Model;
using Nucleus.Model.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Meshing
{
    /// <summary>
    /// An abstract base class used to generically construct meshes in an application-specific format
    /// </summary>
    public abstract class MeshBuilderBase
    {
        #region Properties

        /// <summary>
        /// The limiting angle to be used when facetting curves for meshing
        /// </summary>
        public Angle FacetAngle { get; set; } = Angle.FromDegrees(20);

        #endregion

        #region Methods

        /// <summary>
        /// Finalize the mesh building.
        /// Will apply any necessary last steps to the mesh generation.
        /// </summary>
        /// <returns></returns>
        public virtual bool Finalize() { return true; }

        /// <summary>
        /// Add a new vertex to the mesh
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>The new vertex index</returns>
        /// <remarks>The returned vertex indices should be sequential</remarks>
        public abstract int AddVertex(Vector pt);

        /// <summary>
        /// Add a new vertex to the mesh.
        /// This operation will set the VertexIndex property of the vertex.
        /// </summary>
        /// <param name="v"></param>
        /// <returns>The new vertex index</returns>
        /// <remarks>The returned indices should be sequential and the 
        /// VertexIndex property of the input vertex should be set.</remarks> 
        public abstract int AddVertex(Vertex v);

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
        /// Add a new face to the mesh.
        /// The vertices within this face should have already had their indices set
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public int AddFace(MeshFace face)
        {
            if (face.Count == 3)
                return AddFace(face[0].Number, face[1].Number, face[2].Number); //TRI
            else if (face.Count > 3)
                return AddFace(face[0].Number, face[1].Number, face[2].Number, face[3].Number); //QUAD
            else
                return -1;
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
        /// Add a set of vertices to the mesh
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public int AddVertices(IEnumerable<Vertex> vertices)
        {
            int lastVI = 0;
            foreach (Vertex v in vertices)
            {
                lastVI = AddVertex(v);
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
        /// Add a set of faces to the mesh.
        /// The vertices referenced by these faces should have been previously
        /// added and the vertex indices set.
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        public int AddFaces(IEnumerable<MeshFace> faces)
        {
            int lastFI = 0;
            foreach (MeshFace face in faces)
            {
                int FI = AddFace(face);
                if (FI >= 0) lastFI = FI;
            }
            return lastFI;
        }

        /// <summary>
        /// Add a Nucleus mesh to the mesh.
        /// Copies of the input mesh vertices and faces will be appended
        /// to those already existing in the current mesh.
        /// </summary>
        /// <param name="mesh"></param>
        public void AddMesh(Mesh mesh)
        {
            AddVertices(mesh.Vertices);
            AddFaces(mesh.Faces);
        }

        /// <summary>
        /// Add vertices and faces to the mesh to represent a cuboid of the specified dimensions,
        /// with its base on the XY plane of the specified coordinate system.
        /// </summary>
        /// <param name="baseWidth"></param>
        /// <param name="baseDepth"></param>
        /// <param name="height"></param>
        /// <param name="cSystem"></param>
        public void AddCuboid(double width, double depth, double height, CartesianCoordinateSystem cSystem)
        {
            AddTruncatedPyramid(width, depth, width, depth, height, cSystem);
        }

        /// <summary>
        /// Add vertices and faces to the mesh to represent a flat-topped pyramid of the specified dimensions,
        /// with its base on the XY plane of the specified coordinate system.
        /// </summary>
        /// <param name="baseWidth"></param>
        /// <param name="baseDepth"></param>
        /// <param name="height"></param>
        /// <param name="cSystem"></param>
        public void AddTruncatedPyramid(double baseWidth, double baseDepth, double topWidth, double topDepth, double height, CartesianCoordinateSystem cSystem)
        {
            double bw2 = baseWidth / 2;
            double bd2 = baseDepth / 2;
            double tw2 = topWidth / 2;
            double td2 = topDepth / 2;
            //Base points:
            Vector b0 = new Vector(bw2, bd2);
            Vector b1 = new Vector(bw2, -bd2);
            Vector b2 = new Vector(-bw2, -bd2);
            Vector b3 = new Vector(-bw2, bd2);
            //Top points:
            Vector t0 = new Vector(tw2, td2, height);
            Vector t1 = new Vector(tw2, -td2, height);
            Vector t2 = new Vector(-tw2, -td2, height);
            Vector t3 = new Vector(-tw2, td2, height);

            if (cSystem != null)
            {
                b0 = cSystem.LocalToGlobal(b0);
                b1 = cSystem.LocalToGlobal(b1);
                b2 = cSystem.LocalToGlobal(b2);
                b3 = cSystem.LocalToGlobal(b3);

                t0 = cSystem.LocalToGlobal(t0);
                t1 = cSystem.LocalToGlobal(t1);
                t2 = cSystem.LocalToGlobal(t2);
                t3 = cSystem.LocalToGlobal(t3);
            }

            if (baseWidth != 0 && baseWidth != 0) AddFace(b0, b1, b2, b3); // Bottom
            if (topWidth != 0 && topDepth != 0) AddFace(t3, t2, t1, t0); // Top
            AddFace(t1, t2, b2, b1); // Front
            AddFace(t2, t3, b3, b2); // Left
            AddFace(t3, t0, b0, b3); // Back
            AddFace(t0, t1, b1, b0); // Right
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
        public void AddSweep(IList<CartesianCoordinateSystem> frames, Curve profile, 
            CoordinateSystemRemappingOption remapping = CoordinateSystemRemappingOption.None)
        {
            if (profile is PolyCurve) //Polycurves are a special case which will be recursively broken up to subcurves
            {
                PolyCurve pC = (PolyCurve)profile;
                foreach (Curve subCrv in pC.SubCurves) AddSweep(frames, subCrv, remapping);
            }
            else
            {
                Vector[] pointStrip = profile.Facet(FacetAngle);
                if (remapping == CoordinateSystemRemappingOption.RemapNegYZ) pointStrip = pointStrip.RemapZnegXY();
                else if (remapping == CoordinateSystemRemappingOption.RemapYZ) pointStrip = pointStrip.RemapZXY();
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
        /// Create faces out of points from the specified list, starting by joining the start points to the end points
        /// and moving towards the middle of the list
        /// </summary>
        /// <param name="pointStrip"></param>
        /// <param name="startOffset"></param>
        public void FillStartToEndReverse(IList<Vector> pointStrip, int startOffset = 1)
        {
            for (int i = 0; i < pointStrip.Count / 2 - startOffset; i++)
            {
                Vector v1 = pointStrip[pointStrip.Count - 1 - i];
                Vector v0 = pointStrip[i + startOffset];
                Vector v3 = pointStrip[i + startOffset + 1];
                Vector v2 = pointStrip[pointStrip.Count - 2 - i];
                AddFace(v0, v1, v2, v3);
            }
        }

        /// <summary>
        /// Create faces by filling between matching indices in two lists of points.
        /// </summary>
        /// <param name="pointStrip1">The first set of points</param>
        /// <param name="pointStrip2">The second set of points</param>
        /// <param name="reverse">If true, the second set of points will be reversed</param>
        /// <param name="close">If true, an additional face will be added closing the loop</param>
        public void FillBetween(IList<Vector> pointStrip1, IList<Vector> pointStrip2, bool reverse = false, bool close = false)
        {
            int max = Math.Max(pointStrip1.Count, pointStrip2.Count);
            // Calculate adjustment factors for the case where one strip has less items than the other:
            double factor1 = ((double)pointStrip1.Count) / max;
            double factor2 = ((double)pointStrip2.Count) / max;
            for (int i = 0; i < max - 1; i++)
            {
                Vector v1 = pointStrip2.GetBounded((int)Math.Round(i*factor2), reverse);
                Vector v2 = pointStrip1.GetBounded((int)Math.Round(i*factor1));
                Vector v3 = pointStrip1.GetBounded((int)Math.Round((i + 1)*factor1));
                Vector v4 = pointStrip2.GetBounded((int)Math.Round((i + 1)*factor2), reverse);
                AddFace(v1, v2, v3, v4);
            }
            if (close)
            {
                Vector v1 = pointStrip2.GetBounded(pointStrip2.Count - 1, reverse);
                Vector v2 = pointStrip1.GetBounded(pointStrip1.Count - 1);
                Vector v3 = pointStrip1.GetBounded(0);
                Vector v4 = pointStrip2.GetBounded(0, reverse);
                AddFace(v1, v2, v3, v4);
            }
        }

        /// <summary>
        /// Display a model geometry
        /// </summary>
        /// <param name="model"></param>
        public void AddModel(Model.Model model)
        {
            AddSectionPreviews(model.Elements.LinearElements);
            AddPanelPreviews(model.Elements.PanelElements);
        }

        /// <summary>
        /// Add mesh vertices and faces representing the section profiles of the given set of linear elements
        /// </summary>
        /// <param name="elements"></param>
        public void AddSectionPreviews(LinearElementCollection elements)
        {
            foreach (LinearElement element in elements) AddSectionPreview(element);
        }

        /// <summary>
        /// Add a set of vertices and faces to this mesh representing the given element complete with
        /// a 3D representation of the assigned section profile
        /// </summary>
        /// <param name="element"></param>
        public void AddSectionPreview(LinearElement element)
        {
            SectionFamily section = element.Family;
            Curve geometry = element.Geometry;
            Angle orientation = 0;
            orientation = element.Orientation;
            AddSectionPreview(geometry, section, orientation);
        }

        /// <summary>
        /// Add a set of vertices and faces to the mesh representing a sweep of the specified section
        /// along the specified curve with the specified orientation
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="section"></param>
        /// <param name="orientation"></param>
        public void AddSectionPreview(Curve geometry, SectionFamily section, Angle orientation)
        { 
            Angle tolerance = FacetAngle;
            if (section != null && geometry != null && section.Profile != null)
            {
                
                IList<CartesianCoordinateSystem> frames = geometry.FacetCSystems(Angle.FromDegrees(5), orientation);
                if (frames.Count > 0)
                {
                    Curve perimeter = section.Profile.Perimeter;
                    AddSweep(frames, perimeter, CoordinateSystemRemappingOption.RemapNegYZ);

                    if (section.Profile.HasVoids)
                    {
                        CurveCollection voids = section.Profile.Voids;
                        if (voids.Count > 0)
                        {
                            Vector[] outer = perimeter.Facet(tolerance);
                            Vector[] startOuter = frames[0].LocalToGlobal(outer.RemapZnegXY());
                            Vector[] endOuter = frames.Last().LocalToGlobal(outer.RemapZXY());
                            foreach (Curve voidCrv in voids)
                            {
                                AddSweep(frames, voidCrv, CoordinateSystemRemappingOption.RemapYZ);
                                Vector[] inner = voidCrv.Facet(tolerance);
                                Vector[] startInner = frames[0].LocalToGlobal(inner.RemapZnegXY());
                                Vector[] endInner = frames.Last().LocalToGlobal(inner.RemapZXY());
                                FillBetween(startOuter, startInner, false, (voidCrv is Arc && voidCrv.Closed));
                                FillBetween(endOuter, endInner, false, (voidCrv is Arc && voidCrv.Closed));
                            }
                        }
                    }
                    else
                    {
                        //Caps:
                        Vector[] pointStrip = perimeter.Facet(tolerance);
                        Vector[] startPts = frames[0].LocalToGlobal(pointStrip.RemapZnegXY());
                        Vector[] endPts = frames.Last().LocalToGlobal(pointStrip.RemapZnegXY());
                        if (perimeter is Arc)
                        {
                            FillStartToEnd(startPts, 0);
                            FillStartToEndReverse(endPts, 0);
                        }
                        else
                        {
                            FillStartToEnd(startPts, 2);
                            FillStartToEndReverse(endPts, 2);
                        }
                        //TODO: Implement other capping methods
                    }
                }
            }
        }

        /// <summary>
        /// Add a set of vertices and faces to the mesh representing a collection of panels with thickness
        /// </summary>
        /// <param name="elements"></param>
        public void AddPanelPreviews(PanelElementCollection elements)
        {
            foreach (PanelElement element in elements) AddPanelPreview(element);
        }

        /// <summary>
        /// Add a set of vertices and faces to the mesh representing a panel with thickness
        /// </summary>
        /// <param name="element"></param>
        public void AddPanelPreview(PanelElement element)
        {
            AddPanelPreview(element.Geometry, element.Family);
        }

        /// <summary>
        /// Add a set of vertices and faces to the mesh representing a panel with thickness.
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="family"></param>
        public virtual void AddPanelPreview(Surface geometry, BuildUpFamily family)
        {
            if (geometry.IsValid)
            {
                if (geometry is PlanarRegion)
                {
                    PlanarRegion region = (PlanarRegion)geometry;
                    if (family != null)
                    {
                        double thickness = family.Layers.TotalThickness;
                        AddPlanarRegion(region, thickness, thickness * family.SetOut.FactorFromTop());
                    }
                    else AddPlanarRegion(region);
                }
            }
        }

        /// <summary>
        /// Add a set of vertices and faces to the mesh representing the specified element's
        /// solid geometry.
        /// </summary>
        /// <param name="element"></param>
        public void AddFamilyPreview(Element element)
        {
            if (element is LinearElement) AddSectionPreview((LinearElement)element);
            else if (element is PanelElement) AddPanelPreview((PanelElement)element);
        }

        /// <summary>
        /// Add a set of vertices and faces to the mesh representing a planar region with optional thickness
        /// </summary>
        /// <param name="region"></param>
        /// <param name="thickness"></param>
        /// <param name="topOffset"></param>
        public void AddPlanarRegion(PlanarRegion region, double thickness = 0, double topOffset = 0, Vector offset = new Vector())
        {
            Plane plane = region.Plane;
            Vector[] perimeter = region.Perimeter.Facet(FacetAngle);
            Vector[] mappedPerimeter = perimeter.GlobalToLocal(plane);
            VertexCollection vertices = new VertexCollection(mappedPerimeter);
            List<Vector[]> voidPerimeters = null;
            List<Vector[]> mappedVoidPerimeters = null;
            if (region.HasVoids) //Voids if present
            {
                voidPerimeters = new List<Vector[]>();
                mappedVoidPerimeters = new List<Vector[]>();
                foreach (Curve voidCrv in region.Voids)
                {
                    Vector[] voidPerimeter = voidCrv.Facet(FacetAngle);
                    voidPerimeters.Add(voidPerimeter);
                    Vector[] mappedVoidPerimeter = voidPerimeter.GlobalToLocal(plane);
                    mappedVoidPerimeters.Add(mappedVoidPerimeter);
                    foreach (Vector pt in mappedVoidPerimeter) vertices.Add(new Vertex(pt));
                }
            }

            MeshFaceCollection faces = Mesh.DelaunayTriangulationXY(vertices);
            faces.CullOutsideXY(mappedPerimeter);

            if (mappedVoidPerimeters != null)
            {
                foreach (Vector[] voidPerimeter in mappedVoidPerimeters)
                {
                    faces.CullInsideXY(voidPerimeter);
                }
            }
            
            vertices.MoveLocalToGlobal(plane);
            if (offset.IsValidNonZero())
            {
                vertices.Move(offset);
            }

            if (topOffset != 0)
            {
                Vector topMove = plane.Z * topOffset;
                vertices.Move(topMove);
                perimeter = perimeter.Move(topMove);

                if (voidPerimeters != null)
                {
                    for (int i = 0; i < voidPerimeters.Count; i++)
                    {
                        voidPerimeters[i] = voidPerimeters[i].Move(topMove);
                    }
                }
            }
            Mesh mesh = new Mesh(vertices, faces);
            AddMesh(mesh);
            if (thickness != 0)
            {
                Vector bottomMove = plane.Z * -thickness;
                mesh.Move(bottomMove);
                AddMesh(mesh);
                Vector[] bottomPerimeter = perimeter.Move(bottomMove);
                FillBetween(perimeter, bottomPerimeter, false, true);

                if (voidPerimeters != null)
                {
                    foreach (Vector[] voidPerimeter in voidPerimeters)
                    {
                        FillBetween(voidPerimeter, voidPerimeter.Move(bottomMove));
                    }
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
            Vector[] basePoints = baseCircle.Divide(baseResolution);
            Vector[] reversedPts = new Vector[basePoints.Length];

            //if (basePoints.Count >= baseResolution) basePoints.RemoveAt(baseResolution); //Get rid of duplicate last point
            IList<Vector> tipPoints = new List<Vector>();
            for (int i = 0; i < basePoints.Length; i++)
            {
                reversedPts[basePoints.Length - 1 - i] = basePoints[i];
                tipPoints.Add(tip);
            }
            IList<IList<Vector>> topology = new List<IList<Vector>>();
            topology.Add(reversedPts);
            topology.Add(tipPoints);
            AddLoft(topology, true);
            FillStartToEndReverse(basePoints, 0); // Cap bottom
        }

        /// <summary>
        /// Add a set of vertices and faces to this mesh representing a facetted cone
        /// </summary>
        /// <param name="tip"></param>
        /// <param name="baseCircle"></param>
        /// <param name="baseResolution"></param>
        public void AddFacetCone(Vector tip, Circle baseCircle, int baseResolution)
        {
            IList<Vector> basePoints = baseCircle.Divide(baseResolution);
            for (int i = 0; i < basePoints.Count; i++)
            {
                AddFace(tip, basePoints[i], basePoints.GetWrapped(i + 1));
            }
            FillStartToEndReverse(basePoints, 0);
        }

        /// <summary>
        /// Add a set of vertices and faces to this mesh representing a cylinder
        /// by extruding a circle
        /// </summary>
        /// <param name="baseCircle">The base circle forming one end of the cylinder</param>
        /// <param name="height">The height of the cylinder - i.e. the extrusion distance of the
        /// base circle along it's own normal</param>
        /// <param name="resolution">The number of points around the circle at which to create
        /// vertices to define the cylinder.</param>
        public void AddCylinder(Circle baseCircle, double height, int resolution)
        {
            IList<Vector> basePoints = baseCircle.Divide(resolution);
            IList<Vector> topPoints = new Vector[basePoints.Count];
            for (int i = 0; i < basePoints.Count; i++)
            {
                topPoints[i] = basePoints[i] + baseCircle.L * height;
            }
            IList<IList<Vector>> topology = new List<IList<Vector>>();
            topology.Add(basePoints);
            topology.Add(topPoints);
            AddLoft(topology, true);
            FillStartToEnd(basePoints, 0); // Cap bottom
            FillStartToEndReverse(topPoints, 0); // Cap top
        }

        // TODO
        /*public void AddIcoSphere(Vector centre, double radius, int subDivs = 0)
        {
            //Create 12 vertices of an isohedron:

            double t = 1.6180339887498948482045868343656; // 1 + Sqrt(5)/2
            var pts = new List<Vector>();
            pts.Add(new Vector(-1, t, 0).;

        }*/

        /// <summary>
        /// Add a set of vertices and faces to this mesh representing a nodal support
        /// </summary>
        /// <param name="node"></param>
        /// <param name="support"></param>
        public void AddNodeSupport(Node node, NodeSupport support, double scale = 1.0)
        {
            if (support != null && !support.Fixity.AllFalse)
            {
                var tip = node.Position;

                Vector connections = node.AverageConnectionDirection();

                Direction dir = support.Fixity.PrimaryAxis(connections.PrimaryAxis());
                Vector direction = new Vector(dir);
                Bool6D fixity = support.Fixity.ReOrientate(dir);

                // TOOD: Orientate to support axes

                // Flip the direction vector if it will coincide with the connecting elements:
                if (!connections.IsZero() && direction.Dot(connections) < 0) direction = direction.Reverse();

                // Moment restraints
                double topWidth = 0;
                double topDepth = 0;
                if (fixity.XX) topDepth = scale;
                if (fixity.YY) topWidth = scale;

                CartesianCoordinateSystem cSystem = new CartesianCoordinateSystem(tip - direction * scale/2, direction);
                AddTruncatedPyramid(scale, scale, topWidth, topDepth, scale/2, cSystem);

                int rollerRes = 10;
                double rD = 6.0;
                // Translational rollers:
                if (!fixity.X && !fixity.Y)
                {
                    //2-way rollers
                    AddCylinder(new Circle(scale / rD, cSystem.LocalToGlobal(0, -scale/2, -scale / rD), cSystem.Y), scale, rollerRes);
                    AddCylinder(new Circle(scale / rD, cSystem.LocalToGlobal(-scale / 2, 0, -scale / rD), cSystem.X), scale, rollerRes);
                }
                else if (!fixity.X)
                {
                    //X-rollers
                    AddCylinder(new Circle(scale / rD, cSystem.LocalToGlobal(-scale / 4, -scale / 2, -scale / rD), cSystem.Y), scale, rollerRes);
                    AddCylinder(new Circle(scale / rD, cSystem.LocalToGlobal(scale / 4, - scale / 2, -scale / rD), cSystem.Y), scale, rollerRes);
                }
                else if (!fixity.Y)
                {
                    //Y-rollers
                    AddCylinder(new Circle(scale / rD, cSystem.LocalToGlobal(-scale / 2, -scale / 4, -scale / rD), cSystem.X), scale, rollerRes);
                    AddCylinder(new Circle(scale / rD, cSystem.LocalToGlobal(- scale / 2, scale / 4, -scale / rD), cSystem.X), scale, rollerRes);
                }

                //var circle = new Circle(scale, new CylindricalCoordinateSystem(tip - direction, direction, new Angle(Math.PI/4))); //new Vector(new Angle(Math.PI / 4)), Vector.UnitY));
                //AddFacetCone(tip, circle, 4);
            }
        }

        /// <summary>
        /// Add vertices and faces to this mesh to represent the specified load.
        /// </summary>
        /// <param name="load">The load to be represented</param>
        /// <param name="factor">The scaling factor by which the load's value will be multiplied by in order
        /// to determine the length of the tail of load representations.</param>
        /// <param name="scale">The overall scaling factor to be applied to the parts of the geometry
        /// not dependent on the magnitude of the load (i.e. arrow head width, etc.)</param>
        /// <param name="context"></param>
        public void AddLoad(Load load, double factor = 0.001, double scale = 1.0, IEvaluationContext context = null)
        {
            if (context == null) context = load?.Model?.Variables;

            if (load is NodeLoad)
            {
                NodeLoad nLoad = (NodeLoad)load;
                foreach (Node node in nLoad.AppliedTo.Items)
                {
                    var cSys = nLoad.Axes.GetCoordinateSystem(node);
                    Vector dir = cSys.DirectionVector(nLoad.Direction);
                    Vector sideways = cSys.DirectionVector(nLoad.Direction.FirstPerpendicular());
                    Vector sideways2 = cSys.DirectionVector(nLoad.Direction.SecondPerpendicular());
                    double value = nLoad.Value.Evaluate<double>(context);
                    AddArrow(node.Position, dir, sideways,  value * factor, scale * 0.4);
                    AddArrow(node.Position, dir, sideways2, value * factor, scale * 0.4);
                }
            }
            else if (load is LinearElementLoad)
            {
                LinearElementLoad eLoad = (LinearElementLoad)load;
                double value = eLoad.Value.Evaluate<double>(context);
                var distri = eLoad.Distribution;
                foreach (LinearElement element in eLoad.AppliedTo.Items)
                {
                    //TODO
                    KeyValuePair<double, double> last = new KeyValuePair<double, double>(double.NaN, double.NaN);
                    CartesianCoordinateSystem lastSys = null;
                    foreach (var kvp in distri)
                    {
                        CartesianCoordinateSystem cSys = 
                            eLoad.Axes.GetCoordinateSystem(element, kvp.Key) as CartesianCoordinateSystem; // TODO: Adjust to unitized length instead of parameter
                        if (cSys == null) cSys = CartesianCoordinateSystem.Global;
                        Vector dir = cSys.GetAxisVector(eLoad.Direction);
                        if (lastSys != null)
                        {
                            AddLineLoad(lastSys.Origin, cSys.Origin, dir, last.Value * value * factor, kvp.Value * value * factor, scale);
                        }
                        last = kvp;
                        lastSys = cSys;
                    }
                    // TODO: Deal with curved/kinked elements?
                    // TODO: Moments & Torsions
                }
            }
            else if (load is PanelLoad)
            {
                PanelLoad aLoad = (PanelLoad)load;
                double value = aLoad.Value.Evaluate<double>(context);
                foreach (PanelElement element in aLoad.AppliedTo.Items)
                {
                    CartesianCoordinateSystem cSys =
                        aLoad.Axes.GetCoordinateSystem(element) as CartesianCoordinateSystem;
                    if (cSys == null) cSys = CartesianCoordinateSystem.Global;
                    Vector dir = cSys.GetAxisVector(aLoad.Direction);
                    if (element.Geometry is PlanarRegion)
                    {
                        AddAreaLoad((PlanarRegion)element.Geometry, dir, value * factor);
                    }
                    else if (element.Geometry is Mesh)
                    {
                        AddAreaLoad((Mesh)element.Geometry, dir, value * factor);
                        // TODO: Deal with loads applied in local axes with non-planar meshes!
                        // Will require an offset rather than a simple translation...
                    }
                }
            }

        }

        /// <summary>
        /// Add faces and vertices to the mesh to represent a flat arrow with a head and a tail.
        /// </summary>
        /// <param name="tip">The position of the tip of the arrow</param>
        /// <param name="direction">The direction that the arrow is pointing</param>
        /// <param name="sideways">A vector perpendicular to the direction vector in
        /// the plane of the arrow</param>
        /// <param name="length">A scaling factor applied to the direction of the arrow</param>
        /// <param name="width">A scaling factor to be applied to the sideways vector.
        /// The </param>
        public void AddArrow(Vector tip, Vector direction, Vector sideways, double length, double width)
        {
            double headLength = Math.Min(length.Abs()/2, 4 * width) * length.Sign();
            Vector headEnd = tip - direction * headLength;
            Vector end = tip - direction * length;
            AddFace(tip, headEnd + sideways * width * 1.5, headEnd - sideways * width * 1.5);
            AddFace(
                headEnd + sideways * width / 2, 
                end + sideways * width / 2, 
                end - sideways * width / 2, 
                headEnd - sideways * width / 2);
        }

        /// <summary>
        /// Add faces and vertices to the mesh to represent a line load via a planar mesh with a serrated
        /// leading edge:
        /// ______
        /// |    |
        /// VVVVVV
        /// </summary>
        /// <param name="start">The start point of application of the line load</param>
        /// <param name="end">The end point of application of the line load</param>
        /// <param name="direction">The direction of application of the line load</param>
        /// <param name="startLength">The length of the arrow tail at the start of the line</param>
        /// <param name="endLength">The length of the arrow tail and the end of the line</param>
        /// <param name="zigZagLength">The width of the zig-zag 'arrow heads'</param>
        public void AddLineLoad(Vector start, Vector end, Vector direction, double startLength, double endLength, double zigZagLength = 1.0)
        {
            Vector line = end - start;
            double length = line.Magnitude();
            int zigZags = (int)Math.Ceiling(length / zigZagLength);
            double zigZagN = 1.0 / zigZags;

            int p0i = -1;
            int e0i = -1;
            for (int i = 0; i < zigZags; i++)
            {
                if (i == 0) //Initialise starting vertices
                {
                    Vector p0 = start.Interpolate(end, i * zigZagN);
                    Vector e0 = p0 - direction * startLength;
                    p0i = AddVertex(p0);
                    e0i = AddVertex(e0);
                }

                // Determine points               
                Vector p1 = start.Interpolate(end, (i + 0.5) * zigZagN);
                Vector p2 = start.Interpolate(end, (i + 1.0) * zigZagN);
                double l1 = startLength.Interpolate(endLength, (i + 0.5) * zigZagN);
                Vector e1 = p1 - direction * l1;
                Vector e2 = p2 - direction * startLength.Interpolate(endLength, (i + 1.0) * zigZagN);
                p1 -= direction * (Math.Min(zigZagLength, l1.Abs()/2) * l1.Sign());

                int p1i = AddVertex(p1);
                int p2i = AddVertex(p2);     
                int e1i = AddVertex(e1);
                int e2i = AddVertex(e2);

                AddFace(p0i, p1i, e1i, e0i);
                AddFace(p1i, p2i, e2i, e1i);

                p0i = p2i;
                e0i = e2i;
            }
        }

        /// <summary>
        /// Add faces and vertices to the mesh to represent an area load applied over a specified planar
        /// region.
        /// </summary>
        /// <param name="region">The region to which the load is applied</param>
        /// <param name="direction">The direction of the load</param>
        /// <param name="length">The length of the arrows representing the load</param>
        /// <param name="zigZagLength"></param>
        public void AddAreaLoad(PlanarRegion region, Vector direction, double length, double zigZagLength = 1.0)
        {
            Vector offset = direction * -length;
            AddPlanarRegion(region, 0, 0, offset);
            Vector[] pts = region.Perimeter.Facet(this.FacetAngle);
            for (int i = 0; i < pts.Length; i++)
            {
                Vector ptA = pts[i];
                Vector ptB = pts.GetWrapped(i + 1);
                AddLineLoad(ptA, ptB, direction, length, length, zigZagLength);
            }
        }

        /// <summary>
        /// Add faces and vertices to the mesh to represent an area load applied over a specified mesh
        /// region.
        /// </summary>
        /// <param name="region">The region to which the load is applied</param>
        /// <param name="direction">The direction of the load</param>
        /// <param name="length">The length of the arrows representing the load</param>
        /// <param name="zigZagLength"></param>
        public void AddAreaLoad(Mesh region, Vector direction, double length, double zigZagLength = 1.0)
        {
            var edges = region.Faces.ExtractNakedEdges();
            foreach (MeshEdge edge in edges)
            {
                AddLineLoad(edge.StartPoint, edge.EndPoint, direction, length, length, zigZagLength);
            }
            // TODO: optimise mesh offset
            Mesh m2 = region.Duplicate();
            m2.Move(direction * -length);
            AddMesh(m2);
        }

        #endregion
    }

    /// <summary>
    /// An abstract base class used to generically construct meshes in an application-specific format
    /// </summary>
    /// <typeparam name="TMesh">The type of mesh being generated</typeparam>
    public abstract class MeshBuilderBase<TMesh> : MeshBuilderBase
    {
        /// <summary>
        /// Protected backing field for Mesh property
        /// </summary>
        protected TMesh _Mesh;

        /// <summary>
        /// The mesh that is being built
        /// </summary>
        public TMesh Mesh
        {
            get { return _Mesh; }
            set { _Mesh = value; }
        }
    }
}
