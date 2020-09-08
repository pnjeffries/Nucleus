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
using Nucleus.Events;
using Nucleus.Model;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// An abstract base class for vertex geometry - geometry defined by and containing 
    /// a set of vertices.
    /// Different types of geometry will require different numbers and types of vertices
    /// and will interpret them in different ways, however the basic data structure is 
    /// always the same.
    /// </summary>
    /// specific position </typeparam>
    [Serializable]
    [Copy(CopyBehaviour.DUPLICATE)]
    public abstract class VertexGeometry : Unique, IOwned<Element>, IBoxBoundable
    {
        #region Events

        /// <summary>
        /// Event raised when any defining data of this geometry is changed
        /// </summary>
        [field: NonSerialized]
        [Copy(CopyBehaviour.DO_NOT_COPY)]
        public event EventHandler<GeometryUpdateEventArgs> GeometryChanged;

        #endregion

        #region Fields

        /// <summary>
        /// When true, changes to this geometry will be made silently without being
        /// reported.  Used to prevent multiple change notifications being sent when
        /// significant geometry changes are made that may result in a chain of partial
        /// update notifications being enacted.
        /// </summary>
        [NonSerialized]
        private bool _SuppressChangedNotification = false;

        #endregion

        #region Properties

        /// <summary>
        /// Private backing field for Element property
        /// </summary>
        [Copy(CopyBehaviour.MAP)]
        private Element _Element = null;

        /// <summary>
        /// The element, if any, that this shape describes geometry for.
        /// Shapes can exist independently of any element, but may only belong to
        /// a maximum of one at any one time.
        /// If this shape does not describe element geometry this will return null.
        /// </summary>
        public Element Element { get { return _Element; } internal set { _Element = value; } }

        Element IOwned<Element>.Owner { get { return Element; } }

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// Different shapes will provide different means of editing this collection.
        /// DO NOT directly modify the collection returned from this property unless you are
        /// sure you know what you are doing.
        /// </summary>
        public abstract VertexCollection Vertices { get; }

        /// <summary>
        /// Get the total number of vertices that define this geometry
        /// </summary>
        public virtual int VertexCount
        {
            get { return Vertices.Count; }
        }

        /// <summary>
        /// Is the definition of this shape valid?
        /// i.e. does it have the correct number of vertices, are all parameters within
        /// acceptable limits, etc.
        /// </summary>
        public abstract bool IsValid { get; }

        /// <summary>
        /// Private backing field for Attributes property
        /// </summary>
        [Copy(CopyBehaviour.DUPLICATE)]
        private GeometryAttributes _Attributes = null;

        /// <summary>
        /// Optional attached display attributes for this geometry, which determine how this
        /// object is drawn.  This property may be null, in which case default options should
        /// be used.
        /// </summary>
        public GeometryAttributes Attributes
        {
            get { return _Attributes; }
            set { _Attributes = value;  NotifyPropertyChanged("Attributes"); }
        }

        /// <summary>
        /// Private backing member variable for the BoundingBox property
        /// </summary>
        [NonSerialized()]
        private BoundingBox _BoundingBox = null;

        /// <summary>
        /// The axis-aligned bounding box that contains this shape.
        /// Will be calculated as-needed and cached until invalidated by a geometry update.
        /// </summary>
        public BoundingBox BoundingBox
        {
            get
            {
                if (_BoundingBox == null) _BoundingBox = GenerateBoundingBox();
                return _BoundingBox;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Suppress or unsuppress reporting of geometry changes to this shape.
        /// Call this before and after operations that may result in multiple changes
        /// to avoid each individual change prompting a separate notification.
        /// </summary>
        /// <param name="value">The new value of the suppression flag.
        /// If true, subsequent change notifications will be suppressed.
        /// If false, subsequent change notifications will be allowed.</param>
        protected void SuppressChangeNotifications(bool value = true)
        {
            _SuppressChangedNotification = value;
        }

        /// <summary>
        /// Transform this shape from local coordinates on the specified coordinate system
        /// to global coordinates.
        /// </summary> 
        public void MapTo(ICoordinateSystem cSystem)
        {
            SuppressChangeNotifications();
            foreach (Vertex v in Vertices)
            {
                v.MapTo(cSystem);
            }
            SuppressChangeNotifications(false);
            NotifyGeometryUpdated();
        }

        /// <summary>
        /// Move this shape along the specified translation vector
        /// </summary>
        /// <param name="translation"></param>
        public virtual void Move(Vector translation)
        {
            Transform(new Transform(translation));
        }

        /// <summary>
        /// Apply the specified transformation to this shape
        /// </summary>
        public virtual void Transform(Transform transform)
        {
            SuppressChangeNotifications();
            foreach (Vertex v in Vertices)
            {
                v.Transform(transform);
            }
            SuppressChangeNotifications(false);
            NotifyGeometryUpdated();
        }

        /// <summary>
        /// Internal function used to populate the BoundingBox property
        /// when necessary.
        /// The default implementation returns the bounding box of the vertices
        /// which define this shape.
        /// </summary>
        /// <returns>A new boundingbox</returns>
        protected virtual BoundingBox GenerateBoundingBox()
        {
            return new BoundingBox(Vertices);
        }

        /// <summary>
        /// Invalidate any temporarily cached geometric data in this shape, forcing
        /// it to be recalculated the next time it is needed
        /// </summary>
        protected virtual void InvalidateCachedGeometry()
        {
            _BoundingBox = null;
        }

        /// <summary>
        /// Notify this shape that one or more of its vertices or another aspect
        /// of it's geometric definition has been altered.  May prompt recalculation
        /// of certain other properties.
        /// </summary>
        internal void NotifyGeometryUpdated()
        {
            InvalidateCachedGeometry();
            if (!_SuppressChangedNotification)
            {
                if (Element != null)
                {
                    Element.NotifyGeometryUpdated();
                }
                GeometryChanged?.Invoke(this, new GeometryUpdateEventArgs());
            }
        }

        /// <summary>
        /// Copy attached data (for example, nodes connected to vertices)
        /// from another shape to this one.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="copyOnlyIfCoincident">If true, data will only be copied between
        /// vertices if the old and new vertex are within tolerance of one another.
        /// If false (default) data will be copied regardless.</param>
        public void CopyAttachedDataFrom(VertexGeometry other, bool copyOnlyIfCoincident = false)
        {
            for (int i = 0; i < Math.Min(Vertices.Count, other.Vertices.Count); i++)
            {
                Vertex vA = Vertices[i];
                Vertex vB = other.Vertices[i];
                if (!copyOnlyIfCoincident || vA.DistanceToSquared(vB) < Tolerance.Distance)
                    vA.CopyAttachedDataFrom(vB);
            }
            Attributes = other.Attributes; //TODO: Duplicate instead?
            //Add any other attached data to be copied here
        }

        /// <summary>
        /// Dettach all nodes from the vertices of this shape
        /// </summary>
        public void DettachNodes()
        {
            foreach (Vertex v in Vertices)
            {
                v.Node = null;
            }
        }

        /// <summary>
        /// Flatten all vertices of this geometry to the specified Z level
        /// </summary>
        /// <param name="Z"></param>
        public void Flatten(double Z)
        {
            foreach (Vertex v in Vertices)
            {
                v.Position = v.Position.WithZ(Z);
            }
        }

        /// <summary>
        /// Stretch a bounding box to fit around this geometry.
        /// </summary>
        /// <param name="box"></param>
        public virtual void StretchBoxAround(BoundingBox box)
        {
            box.Include(Vertices);
        }

        /// <summary>
        /// Determine the side of an infinite line on which this geometry lies.
        /// Will return right, left, or undefined if the geometry lies across the line.
        /// </summary>
        /// <param name="lineOrigin">A point which lies on the line</param>
        /// <param name="lineDir">The direction vector of the line</param>
        /// <returns></returns>
        public HandSide SideOf(Vector lineOrigin, Vector lineDir, double tolerance = 0)
        {
            return Vertices.SideOf(lineOrigin, lineDir, tolerance);
        }

        #endregion


    }

    /// <summary>
    /// Extension methods for the VertexGeometry class and collections thereof
    /// </summary>
    public static class VertexGeometryExtensions
    {
        /// <summary>
        /// Do all pieces of geometry in this collection have the specified attributes object?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="geometry"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static bool AllHaveAttributes<T>(this IList<T> geometry, GeometryAttributes attributes)
            where T:VertexGeometry
        {
            foreach (var vG in geometry) if (vG.Attributes != attributes) return false;
            return true;
        }
    }

}
