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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{

    /// <summary>
    /// A generic, observable, keyed collection of vertex geometry objects
    /// </summary>
    /// <typeparam name="TShape"></typeparam>
    [Serializable]
    public class VertexGeometryCollection<TShape> : UniquesCollection<TShape>
        where TShape : VertexGeometry
    {
        #region Properties

        /// <summary>
        /// Get the bounding box of the geometry contained within this collection
        /// </summary>
        public BoundingBox BoundingBox { get { return new BoundingBox(this); } }

        #endregion

        #region Methods

        /// <summary>
        /// Move all geometry along the specified translation vector
        /// </summary>
        /// <param name="translation"></param>
        public void MoveAll(Vector translation)
        {
            foreach (var shape in this)
            {
                shape.Move(translation);
            }
        }

        /// <summary>
        /// Get a collection containing all of the vertices belonging to the geometry in this collection
        /// </summary>
        /// <returns></returns>
        public VertexCollection AllVertices()
        {
            var result = new VertexCollection();
            foreach (VertexGeometry geom in this)
            {
                foreach (Vertex v in geom.Vertices)
                {
                    if (!result.Contains(v.GUID))
                        result.Add(v);
                }
            }
            return result;
        }

        /// <summary>
        /// Get the geometry in this collection that falls within the specified axis-aligned bounding box
        /// </summary>
        /// <param name="bounds">The bounding box to check</param>
        /// <param name="inclusive">If true, the output can include objects that lie partially within the bounds, else
        /// the geometry must be entirely contained by the box to be included</param>
        /// <param name="addToThis">The output collection, to which passing geometry should be added</param>
        public void GeometryInBounds(BoundingBox bounds, bool inclusive, VertexGeometryCollection<TShape> addToThis)
        {
            foreach (TShape vG in this)
            {
                if (!addToThis.Contains(vG.GUID) && (inclusive && bounds.Overlaps(vG)) || (!inclusive && bounds.Contains(vG)))
                    addToThis.Add(vG);
            }
        }

        /// <summary>
        /// Set the display 'Weight' attribute of all geometry in this collection
        /// </summary>
        /// <param name="weight"></param>
        public void SetAllWeights(double weight)
        {
            foreach (TShape vG in this)
            {
                if (vG.Attributes == null) vG.Attributes = new GeometryAttributes();
                vG.Attributes.Weight = weight;
            }
        }

        #endregion
    }

    /// <summary>
    /// An observable, keyed collection of vertex geometry objects
    /// </summary>
    [Serializable]
    public class VertexGeometryCollection : VertexGeometryCollection<VertexGeometry>
    {
        #region Constructors

        public VertexGeometryCollection() : base() { }

        public VertexGeometryCollection(VertexGeometryCollection collection) : base()
        {
            foreach (VertexGeometry vG in collection) Add(vG);
        }

        public VertexGeometryCollection(CurveCollection collection) : base()
        {
            foreach (VertexGeometry vG in collection) Add(vG);
        }

        public VertexGeometryCollection(VertexGeometry geometry) : base()
        {
            Add(geometry);
        }

        public VertexGeometryCollection(params VertexGeometry[] geometry)
        {
            AddRange(geometry);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Extract from this collection all geometry objects that are marked as lying on
        /// a layer with the specified name
        /// </summary>
        /// <param name="layerName">The layer name to search for.  Case sensitive.</param>
        /// <returns></returns>
        public VertexGeometryCollection AllOnLayer(string layerName)
        {
            VertexGeometryCollection result = new VertexGeometryCollection();
            foreach (VertexGeometry geometry in this)
            {
                if (geometry.Attributes?.LayerName == layerName) result.Add(geometry);
            }
            return result;
        }

        /// <summary>
        /// Extract a dictionary of geometry objects keyed by the layer name of their attributes
        /// </summary>
        /// <returns></returns>
        public GeometryLayerTable Layered()
        {
            GeometryLayerTable result = new GeometryLayerTable();
            foreach (VertexGeometry geometry in this)
            {
                string layerName = geometry.Attributes?.LayerName;
                if (layerName == null) layerName = "Default";
                if (!result.Contains(layerName))
                    result.Add(new GeometryLayer(layerName));
                result[layerName].Add(geometry);
            }
            return result;
        }

        /// <summary>
        /// Get the geometry in this collection that falls within the specified axis-aligned bounding box
        /// </summary>
        /// <param name="bounds">The bounding box to check</param>
        /// <param name="inclusive">If true, the output can include objects that lie partially within the bounds, else
        /// the geometry must be entirely contained by the box to be included</param>
        public VertexGeometryCollection GeometryInBounds(BoundingBox bounds, bool inclusive)
        {
            VertexGeometryCollection result = new Geometry.VertexGeometryCollection();
            GeometryInBounds(bounds, inclusive, result);
            return result;
        }

        /// <summary>
        /// Convert any polycurves in this collection that can be expressed accurately as polylines
        /// into PolyLine objects
        /// </summary>
        public void RationalisePolycurves()
        {
            for (int i = 0; i < Count; i++)
            {
                VertexGeometry vG = this[i];
                if (vG is PolyCurve)
                {
                    PolyCurve pC = (PolyCurve)vG;
                    if (pC.IsPolyline())
                    {
                        this[i] = pC.ToPolyLine();
                    }
                }
                else if (vG is PlanarRegion)
                {
                    PlanarRegion pR = (PlanarRegion)vG;
                    if (pR.Perimeter is PolyCurve)
                    {
                        PolyCurve pC = (PolyCurve)pR.Perimeter;
                        if (pC.IsPolyline()) pR.Perimeter = pC.ToPolyLine();
                    }

                }
            }
        }

        #endregion
    }

}
