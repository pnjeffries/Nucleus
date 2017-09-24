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
using Nucleus.Events;
using Nucleus.Exceptions;
using Nucleus.Geometry;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Abstract base class for Elements - objects which represent physical model
    /// entities and which are defined by a set-out geometry which describes the
    /// overall abstract form of the element and by a volumetric property which
    /// determines how that design representation converts into a 3D solid object.
    /// </summary>
    [Serializable]
    public abstract class Element : DataOwner<ElementDataStore, IElementDataComponent, Element>, IElement
    {

        #region Properties

        /// <summary>
        /// Get a collection containing all of the nodes attached to this element's
        /// geometry.  This collection will be generated as necessary and adding or removing
        /// objects from it will not have any effect on the geometry.
        /// </summary>
        [AutoUI(Order = 310)]
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
        [AutoUI(Order = 300)]
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
        [AutoUI(Order = 305)]
        Family IElement.Family
        {
            get
            {
                return GetFamily();
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
        [AutoUI(430)]
        public Angle Orientation
        {
            get { return _Orientation; }
            set { _Orientation = value; NotifyPropertyChanged("Orientation"); }
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

        /// <summary>
        /// Get a collection of element vertex wrappers for this element
        /// </summary>
        public IList<ElementVertex> ElementVertices
        {
            get
            {
                var result = new List<ElementVertex>();
                var vertices = GetGeometry().Vertices;
                for (int i = 0; i < vertices.Count; i++)
                {
                    Vertex v = vertices[i];
                        result.Add(new ElementVertex(this, v, GetElementVertexDescription(i, vertices)));
                }
                return result;
            }
        }

        /// <summary>
        /// Generate a text string to be used to describe the position of the element vertex
        /// in the specified position  in the specified collection.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="vertices"></param>
        /// <returns></returns>
        protected virtual string GetElementVertexDescription(int index, VertexCollection vertices)
        {
            return index.ToString();
        }

        /// <summary>
        /// Get the element vertex wrapper for the specified vertex index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public ElementVertex this[int i]
        {
            get
            {
                var vertices = GetGeometry()?.Vertices;
                return new ElementVertex(this, vertices?[i], GetElementVertexDescription(i, vertices));
            }
        }

        #endregion

        #region Methods

        protected override ElementDataStore NewDataStore()
        {
            return new ElementDataStore(this);
        }

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
        /// Get a point in space which nominally describes the position of this element,
        /// to be used for display attachments and the like.
        /// </summary>
        /// <returns></returns>
        public abstract Vector GetNominalPosition();

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

        /// <summary>
        /// Modify the orientation of this element so that the appropriate axis
        /// of the local coordinate system (Z for linear elements, X for panels)
        /// points as closely as possible towards the specified guide vector.
        /// </summary>
        /// <param name="vector"></param>
        public abstract void OrientateToVector(Vector vector);

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
        [AutoUI(410)]
        [Copy(CopyBehaviour.DUPLICATE)]
        public TShape Geometry
        {
            get { return _Geometry; }
            set
            {
                /*if (value.Element != null && value.Element != this)
                {
                    throw new NonExclusiveGeometryException(
                        "The set-out geometry of an element cannot be assigned because the geometry object already belongs to another element.");
                }
                else
                {*/
                    if (_Geometry != null && _Geometry.Element == this && _Geometry != value)
                    {
                        _Geometry.DettachNodes();
                        _Geometry.Element = null;
                    }
                    _Geometry = value;
                    _Geometry.Element = this;
                    NotifyPropertyChanged("Geometry");
                //}
            }
        }

        /// <summary>
        /// Replace the set-out geometry of this element, automatically copying over any relevant data
        /// attached to the original geometry such as vertex nodes and cleaning up any residual data on
        /// the old geometry to prevent memory leaks.
        /// </summary>
        /// <param name="newGeometry"></param>
        /// <param name="copyOnlyIfCoincident">If true, data will only be copied between
        /// vertices if the old and new vertex are within tolerance of one another.
        /// If false (default) data will be copied regardless.</param>
        public void ReplaceGeometry(TShape newGeometry, bool copyOnlyIfCoincident = false)
        {
            TShape oldGeometry = Geometry;
            if (oldGeometry != null)
            {
                newGeometry.CopyAttachedDataFrom(oldGeometry, copyOnlyIfCoincident);
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
        /// The element family to which this element belongs.  The element family
        /// is a volumetric property that describes how the editable set-out 
        /// geometry of this element should be interpreted to produce a 
        /// full 3D solid object and which may be shared by multiple elements.
        /// </summary>
        [AutoUIComboBox(Order = 420)]
        public TFamily Family
        {
            get { return _Family; }
            set
            {
                _Family = value;
                NotifyPropertyChanged("Family");
            }
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
