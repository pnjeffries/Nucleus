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
    /// Generic base class for elements - objects which represent physical model
    /// entities and which are defined by a set-out geometry which describes the
    /// overall abstract form of the element and by a volumetric property which
    /// determines how that design representation converts into a 3D solid object.
    /// </summary>
    [Serializable]
    public abstract class Element<TShape, TProperty> : Named, IElement
        where TShape : Shape
        where TProperty : VolumetricProperty
    {
        #region Events

        /// <summary>
        /// Event raised whenever a key part of this element's representative data
        /// has been updated.
        /// </summary>
        public EventHandler<ElementUpdateEventArgs> Updated;

        #endregion

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
                    if (_Geometry != null && _Geometry.Element == this) _Geometry.Element = null;
                    _Geometry = value;
                    _Geometry.Element = this;
                    NotifyPropertyChanged("Geometry");
                }
            }
        }

        /// <summary>
        /// IElement Geometry implementation
        /// </summary>
        Shape IElement.Geometry { get { return Geometry; } }

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
        /// IElement Property implementation
        /// </summary>
        VolumetricProperty IElement.Property { get { return Property; } }

        /// <summary>
        /// The orientation description of this element - determines the relative orientation
        /// of the local coordinate system of this element.
        /// </summary>
        public ElementOrientation Orientation { get; set; } = 0.0;

        #endregion

        #region Methods

        /// <summary>
        /// Notify this shape that one or more of its vertices or another aspect
        /// of it's geometric definition has been altered.
        /// or has been updated
        /// </summary>
        public void NotifyGeometryUpdated()
        {
            RaiseEvent(Updated, new ElementUpdateEventArgs());
        }

        #endregion

    }
}
