using FreeBuild.Base;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// An abstract base class for shapes.
    /// Shapes are geometry defined by and containing a set of vertices.
    /// Different types of shapes will require different numbers and types of vertices
    /// and will interpret them in different ways, however the basic data structure is 
    /// always the same.
    /// </summary>
    /// <typeparam name="TVertex">The type of vertex used to define this shape</typeparam>
    /// <typeparam name="TParameter">The type of the parameter used to indicate a specific position </typeparam>
    [Serializable]
    public abstract class Shape : Unique, IOwned<IElement>
    {
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
        /// The element, if any, that this shape describes geometry for.
        /// Shapes can exist independently of any element, but may only belong to
        /// a maximum of one at any one time.
        /// If this shape does not describe element geometry this will return null.
        /// </summary>
        public IElement Element { get; internal set; } = null;

        IElement IOwned<IElement>.Owner { get { return Element; } }

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// Different shapes will provide different means of editing this collection.
        /// DO NOT directly modify the collection returned from this property unless you are
        /// sure you know what you are doing.
        /// </summary>
        public abstract VertexCollection Vertices { get; }

        /// <summary>
        /// Is the definition of  shape valid?
        /// i.e. does it have the correct number of vertices, are all parameters within acceptable limits, etc.
        /// </summary>
        public abstract bool IsValid { get; }

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
        /// Notify this shape that one or more of its vertices or another aspect
        /// of it's geometric definition has been altered.
        /// or has been updated
        /// </summary>
        internal void NotifyGeometryUpdated()
        {
            if (!_SuppressChangedNotification)
            {
                if (Element != null)
                {
                    Element.NotifyGeometryUpdated();
                }
            }
        }

        #endregion


    }
}
