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
    public abstract class Element : ModelObject, IElement
    {

        #region Properties

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
                VertexGeometry geometry = GetGeometry();
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
        VertexGeometry IElement.Geometry
        {
            get
            {
                return GetGeometry();
            }
        }

        /// <summary>
        /// IElement Family implementation
        /// </summary>
        Family IElement.Family
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Get a description of this element.
        /// Will be the element's name if it has one or will return "Element {ID}"
        /// if not.
        /// </summary>
        public override string Description
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name) && NumericID > 0) return "Element " + NumericID;
                else return Name;
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
        public abstract VertexGeometry GetGeometry();

        /// <summary>
        /// IElement Family implementation
        /// </summary>
        public abstract Family GetFamily();

        /// <summary>
        /// Generate nodes for this element's vertices, if they do not already posess them
        /// them.
        /// </summary>
        /// <param name="connectionTolerance"></param>
        /// <param name="model"></param>
        public virtual void GenerateNodes(NodeGenerationParameters options)
        {
            VertexGeometry geometry = GetGeometry();
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
            VertexGeometry geometry = GetGeometry();
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
    public abstract class Element<TShape, TFamily> : Element
        where TShape : VertexGeometry
        where TFamily : Family
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
        /// Private backing field for the DerivedGeometry property
        /// </summary>
        [NonSerialized] //TEMP!
        private DerivedGeometryDictionary _DerivedGeometry = null;
        
        /// <summary>
        /// Storage for derived geometric objects attached to this element.
        /// These represent geometric forms that the element may be represented by
        /// or may attain under certain circumstances (such as structural deflection plots)
        /// but they do not form part of the idealised defining geometry of the element - for
        /// the set-out geometry of the element consult the Geometry property instead.
        /// NOTE: Currently Not Serialized - temporary data only
        /// </summary>
        public DerivedGeometryDictionary DerivedGeometry
        {
            get
            {
                if (_DerivedGeometry == null) _DerivedGeometry = new DerivedGeometryDictionary(this);
                return _DerivedGeometry;
            }
        }

        /// <summary>
        /// Private backing member variable for the Family property
        /// </summary>
        private TFamily _Family;

        /// <summary>
        /// The volumetric property that describes how the editable set-out 
        /// geometry of this element should be interpreted to produce a 
        /// full 3D solid object
        /// </summary>
        public TFamily Family
        {
            get { return _Family; }
            set
            {
                _Family = value;
                NotifyPropertyChanged("Family");
            }
        }

        /// <summary>
        /// Private backing field for Orientation property
        /// </summary>
        private Angle _Orientation = Angle.Zero;

        /// <summary>
        /// The orientation description of this element - determines the relative orientation
        /// of the local coordinate system of this element.
        /// </summary>
        public Angle Orientation
        {
            get { return _Orientation; }
            set { _Orientation = value; NotifyPropertyChanged("Orientation"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected Element() { }

        /// <summary>
        /// Duplication constructor
        /// </summary>
        /// <param name="other"></param>
        protected Element(Element<TShape,TFamily> other)
        {

        }

        #endregion

        #region Methods

        public override VertexGeometry GetGeometry()
        {
            return Geometry;
        }

        public override Family GetFamily()
        {
            return Family;
        }

        #endregion

    }
}
