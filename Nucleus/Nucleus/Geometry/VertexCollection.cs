﻿// Copyright (c) 2016 Paul Jeffries
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
using Nucleus.Debugging;
using Nucleus.Extensions;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// An obervable, keyed collection of vertices
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    [Serializable]
    [CollectionCopy(CopyBehaviour.DUPLICATE, CopyBehaviour.DUPLICATE)]
    [DebuggerTypeProxy(typeof(VertexCollectionDebugView))]
    [DebuggerDisplay("Count = {Count}")]
    public class VertexCollection : OwnedCollection<Vertex, VertexGeometry>
    {
        #region Properties

        /// <summary>
        /// Do any of the vertices in this collection have non-zero nodal offsets?
        /// </summary>
        public bool HasNodalOffsets
        {
            get
            {
                foreach (Vertex v in this)
                    if (!v.NodalOffset().IsZero()) return true;
                return false;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Owner constructor
        /// </summary>
        /// <param name="owner"></param>
        public VertexCollection(VertexGeometry owner): base(owner)
        {
        }

        /// <summary>
        /// Default, parameterless constructor.
        /// The owner of this vertex collection will be null.
        /// </summary>
        public VertexCollection() : this((VertexGeometry)null) { }

        /// <summary>
        /// Initialise a new vertex collection containing a single vertex
        /// </summary>
        /// <param name="vertex"></param>
        public VertexCollection(Vertex vertex) : this()
        {
            Add(vertex);
        }

        /// <summary>
        /// Initialise a new vertex collection, converting the set of position vectors passed in into
        /// new vertices.
        /// </summary>
        /// <param name="points"></param>
        public VertexCollection(IEnumerable<Vector> points, VertexGeometry owner = null) : this(owner)
        {
            foreach(Vector v in points)
            {
                Add(new Vertex(v));
            }
        }

        /// <summary>
        /// Initialise a new vertex collection, containing the specified set of vertices
        /// </summary>
        /// <param name="points"></param>
        public VertexCollection(params Vertex[] vertices) : this((IEnumerable<Vertex>)vertices)
        {

        }

        /// <summary>
        /// Initialise a new vertex collection, containing the specified set of vertices
        /// </summary>
        /// <param name="points"></param>
        public VertexCollection(IEnumerable<Vertex> vertices, VertexGeometry owner = null) : this(owner)
        {
            foreach (Vertex v in vertices)
            {
                Add(v);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the owning geometry of the specified vertex
        /// </summary>
        /// <param name="item"></param>
        protected override void SetItemOwner(Vertex item)
        {
            if (Owner != null)
            {
                if (item.Owner != null && item.Owner != Owner)
                {
                    throw new Exception("Vertex already has an owner.  The same vertex cannot belong to more than one piece of geometry.");
                }
                else
                    item.Owner = Owner;
            }
        }

        /// <summary>
        /// Clears the owning geometry of the specified
        /// </summary>
        /// <param name="item"></param>
        protected override void ClearItemOwner(Vertex item)
        {
            if (item.Owner == Owner) item.Owner = null;
        }

        /// <summary>
        /// Calculates and returns the bounding box of all of the vertices
        /// within this collection.
        /// </summary>
        /// <returns>A new bounding box containing all vertices in this collection.</returns>
        public BoundingBox BoundingBox()
        {
            return new BoundingBox(this);
        }

        /// <summary>
        /// Calculate the plane these vertices lie on, if they are planar.
        /// Returns null if the vertex collection is non-planar within the specified tolerance 
        /// or if there are insufficient points to describe a plane.
        /// </summary>
        /// <returns></returns>
        public Plane Plane(double tolerance = 0.0000001)
        {
            if (Count > 2)
            {
                Vector o = this[0].Position;
                Vector x = this[1].Position - o;
                int i = 1;
                while (i < Count - 1)
                {
                    i++;
                    if (!x.IsZero()) break;
                    else x = this[i].Position - o;
                }

                Vector xy = this[i].Position - o;
                while (i < Count - 1)
                {
                    i++;
                    if (!xy.IsZero() && !x.IsParallelTo(xy)) break;
                    else xy = this[i].Position - o;
                }
                Plane result = new Plane(o, x, xy);
                while (i < Count)
                {
                    if (result.DistanceTo(this[i].Position).Abs() > tolerance) return null;
                    i++;
                }
                return result;
            }
            else return null;
        }

        /// <summary>
        /// Do any of the vertices in this collection contain a reference to
        /// the specified node?
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool ContainsNode(Node node)
        {
            foreach (Vertex v in this)
            {
                if (v.Node == node) return true;
            }
            return false;
        }

        /// <summary>
        /// Populate the VertexIndex properties of the vertices in this collection,
        /// starting from the specified (optional) value.
        /// </summary>
        /// <param name="startingIndex">The index to assign to the first vertex in the collection.
        /// Subsequent vertices will be numbered incrementally.</param>
        public void AssignVertexIndices(int startingIndex = 0)
        {
            foreach (Vertex v in this)
            {
                v.Number = startingIndex;
                startingIndex++;
            }
        }

        /// <summary>
        /// Extract the position vectors of all vertices in this collection to
        /// an array.
        /// </summary>
        /// <returns></returns>
        public Vector[] ExtractPoints()
        {
            Vector[] result = new Vector[Count];
            for (int i = 0; i < Count; i++)
            {
                result[i] = this[i].Position;
            }
            return result;
        }

        /// <summary>
        /// Move the vertices in this collection along the specified translation vector
        /// </summary>
        /// <param name="translation"></param>
        public virtual void Move(Vector translation)
        {
            Transform(new Transform(translation));
        }

        /// <summary>
        /// Apply the specified transformation to all of the vertices in this collection
        /// </summary>
        public virtual void Transform(Transform transform)
        {
            foreach (Vertex v in this)
            {
                v.Transform(transform);
            }
        }

        /// <summary>
        /// Move all vertices in this collection from the global coordinate system
        /// to equivalent positions in the specified local coordinate system
        /// </summary>
        /// <param name="cSystem"></param>
        public void MoveGlobalToLocal(ICoordinateSystem cSystem)
        {
            foreach (Vertex v in this)
            {
                v.Position = cSystem.GlobalToLocal(v.Position);
            }
        }

        /// <summary>
        /// Move all vertices in this collection from the local coordinate system specified
        /// to equivalent positions in global 3D space
        /// </summary>
        /// <param name="cSystem"></param>
        public void MoveLocalToGlobal(ICoordinateSystem cSystem)
        {
            foreach (Vertex v in this)
            {
                v.Position = cSystem.LocalToGlobal(v.Position);
            }
        }

        /// <summary>
        /// Returns a sum value of (x1 - x0)(y1 + y0) for each vector between
        /// vertices in this collection, which can be used to test whether the
        /// vertices are overall stored in a clockwise or anti-clockwise direction.
        /// If the result is greater than 0, the collection is clockwise.
        /// If the result is less than 0, the collection is anticlockwise.
        /// If the result is 0, it is indeterminate.
        /// The final vector from the last vertex to the first one will also be included.
        /// </summary>
        /// <returns></returns>
        public double ClockwiseTestSum()
        {
            double result = 0;
            for (int i = 0; i < Count; i++)
            {
                Vector v0 = this[i].Position;
                Vector v1 = this.GetWrapped(i + 1).Position;
                result += (v1.X - v0.X) * (v1.Y + v0.Y); 
            }
            return result;
        }

        protected override void OnCollectionChanged()
        {
            if (Owner != null) Owner.NotifyGeometryUpdated();
            base.OnCollectionChanged();
        }

        /// <summary>
        /// Remove vertices from this collection which are within
        /// tolerance of the preceeding vertex
        /// </summary>
        /// <returns></returns>
        public void RemoveSequentialCoincident()
        {
            for (int i = Count - 1; i > 0; i--)
            {
                var v1 = this[i];
                var v0 = this[i - 1];
                if (v1.Position.ManhattenDistanceTo(v0.Position).IsTiny()) RemoveAt(i);
            }
        }

        #endregion
    }

    [Serializable]
    public class VertexCollectionDebugView
    { 
        private ICollection<Vertex> _Collection;

        public VertexCollectionDebugView(ICollection<Vertex> collection)
        {
            _Collection = collection ?? throw new ArgumentNullException("collection");
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public Vertex[] Items
        {
            get
            {
                var array = new Vertex[_Collection.Count];
                _Collection.CopyTo(array, 0);
                return array;
            }
        }
    }
}
