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

using FreeBuild.Base;
using FreeBuild.Events;
using FreeBuild.Exceptions;
using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract base class for Elements - objects which represent physical model
    /// entities and which are defined by a set-out geometry which describes the
    /// overall abstract form of the element and by a volumetric property which
    /// determines how that design representation converts into a 3D solid object.
    /// </summary>
    [Serializable]
    public abstract class Element : DataOwner<DataStore, object>, IElement
    {

        #region Properties

        /// <summary>
        /// Private backing field for Data property
        /// </summary>
        private ElementDataStore _Data = null;

        /// <summary>
        /// The non-geometric additional data attached to this element.
        /// </summary>
        public ElementDataStore Data
        {
            get
            {
                if (_Data != null) _Data = new ElementDataStore();
                return _Data;
            }
        }

        /// <summary>
        /// Get a collection containing all of the nodes attached to this element's
        /// geometry.  This collection will be generated as necessary and adding or removing
        /// objects from it will not have any effect on the geometry.
        /// </summary>
        public NodeCollection Nodes
        {
            get
            {
                NodeCollection result = new NodeCollection();
                Shape geometry = GetGeometry();
                foreach (Vertex v in geometry.Vertices)
                {
                    if (v.Node != null && !result.Contains(v.Node.GUID)) result.Add(v.Node);
                }
                return result;
            }
        }

        /// <summary>
        /// IElement Geometry implementation
        /// </summary>
        Shape IElement.Geometry
        {
            get
            {
                return GetGeometry();
            }
        }

        /// <summary>
        /// IElement Property implementation
        /// </summary>
        VolumetricProperty IElement.Property
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Notify this element that one or more of its vertices or another aspect
        /// of its geometric definition has been altered.
        /// or has been updated
        /// </summary>
        public void NotifyGeometryUpdated()
        {
            NotifyPropertyChanged("Geometry");
        }

        /// <summary>
        /// Protected internal function to return this element's geometry as a shape
        /// </summary>
        /// <returns></returns>
        public abstract Shape GetGeometry();

        /// <summary>
        /// IElement Property implementation
        /// </summary>
        public abstract VolumetricProperty GetProperty();

        /// <summary>
        /// Generate nodes for this element's vertices, if they do not already posess them
        /// them.
        /// </summary>
        /// <param name="connectionTolerance"></param>
        /// <param name="model"></param>
        public virtual void RegenerateNodes(NodeGenerationParameters options)
        {
            Shape geometry = GetGeometry();
            foreach (Vertex v in geometry.Vertices)
            {
                v.GenerateNode(options);
            }
        }

        /// <summary>
        /// Does this element's geometry contain a reference to the specified node?
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool ContainsNode(Node node)
        {
            Shape geometry = GetGeometry();
            foreach (Vertex v in geometry.Vertices)
            {
                if (v.Node == node) return true;
            }
            return false;
        }

        #endregion

    }

    /// <summary>
    /// Generic base class for elements - objects which represent physical model
    /// entities and which are defined by a set-out geometry which describes the
    /// overall abstract form of the element and by a volumetric property which
    /// determines how that design representation converts into a 3D solid object.
    /// </summary>
    [Serializable]
    public abstract class Element<TShape, TProperty> : Element
        where TShape : Shape
        where TProperty : VolumetricProperty
    {

        #region Properties

        /// <summary>
        /// Private backing member variable for the Geometry property
        /// </summary>
        private TShape _Geometry;

        /// <summary>
        /// The set-out geometry of the element.
        /// Describes the editable control geometry that primarily defines
        /// the overall geometry of this object.
        /// The set-out curve of 1D Elements, the surface of slabs, etc.
        /// The assigned object should not already be assigned to any other
        /// element.
        /// When setting, the previous geometry (if any) will have any attached nodes removed
        /// to prevent memory leaks.
        /// If you wish to set the geometry of this element but retain existing attached
        /// data such as vertex nodes, use the ReplaceGeometry function instead of directly
        /// setting this property.
        /// </summary>
        public TShape Geometry
        {
            get { return _Geometry; }
            set
            {
                if (value.Element != null && value.Element != this)
                {
                    throw new NonExclusiveGeometryException(
                        "The set-out geometry of an element cannot be assigned because the geometry object already belongs to another element.");
                }
                else
                {
                    if (_Geometry != null && _Geometry.Element == this)
                    {
                        _Geometry.DettachNodes();
                        _Geometry.Element = null;
                    }
                    _Geometry = value;
                    _Geometry.Element = this;
                    NotifyPropertyChanged("Geometry");
                }
            }
        }

        /// <summary>
        /// Replace the set-out geometry of this element, automatically copying over any relevant data
        /// attached to the original geometry such as vertex nodes and cleaning up any residual data on
        /// the old geometry to prevent memory leaks.
        /// </summary>
        /// <param name="newGeometry"></param>
        public void ReplaceGeometry(TShape newGeometry)
        {
            TShape oldGeometry = Geometry;
            if (oldGeometry != null)
            {
                newGeometry.CopyAttachedDataFrom(oldGeometry);
            }
            Geometry = newGeometry;
        }

        /// <summary>
        /// Private backing member variable for the Property property
        /// </summary>
        private TProperty _Property;

        /// <summary>
        /// The volumetric property that describes how the editable set-out 
        /// geometry of this element should be interpreted to produce a 
        /// full 3D solid object
        /// </summary>
        public TProperty Property
        {
            get { return _Property; }
            set
            {
                _Property = value;
                NotifyPropertyChanged("Property");
            }
        }

        /// <summary>
        /// The orientation description of this element - determines the relative orientation
        /// of the local coordinate system of this element.
        /// </summary>
        public ElementOrientation Orientation { get; set; } = 0.0;

        #endregion

        #region Methods

        public override Shape GetGeometry()
        {
            return Geometry;
        }

        public override VolumetricProperty GetProperty()
        {
            return Property;
        }

        #endregion

    }
}
