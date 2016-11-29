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
using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Base class for objects representing the profile of a SectionProperty.
    /// </summary>
    [Serializable]
    public abstract class SectionProfile : Unique, IOwned<SectionProperty>
    {
        #region Properties

        /// <summary>
        /// Private backing field for CatalogueName property
        /// </summary>
        protected string _CatalogueName = null;

        /// <summary>
        /// The catalogue name of this profile, if this is a standard profile from
        /// a catalogue.  If this is a custom section this value will be null.
        /// </summary>
        public string CatalogueName
        {
            get { return _CatalogueName; }
            set { _CatalogueName = value;
                NotifyPropertyChanged("CatalogueName");
            }
        }

        /// <summary>
        /// The outer perimeter curve of this section profile.
        /// </summary>
        public abstract Curve Perimeter { get; }

        /// <summary>
        /// The collection of curves which denote the voids within this section profile.
        /// </summary>
        public abstract CurveCollection Voids { get; }

        /// <summary>
        /// Does this profile (potentially) have voids?
        /// </summary>
        public virtual bool HasVoids { get { return false; } }

        /// <summary>
        /// Private backing field for Material property.
        /// </summary>
        private Material _Material;

        /// <summary>
        /// The primary material assigned to this profile.
        /// </summary>
        public Material Material
        {
            get { return _Material; }
            set { _Material = value; NotifyPropertyChanged("Material"); }
        }

        /// <summary>
        /// Private backing field for HorizontalSetOut property
        /// </summary>
        private HorizontalSetOut _HorizontalSetOut = HorizontalSetOut.Centroid;

        /// <summary>
        /// The horizontal position of the base set-out point of the profile.
        /// This is the point on the profile which will be taken as running along 
        /// the element set-out curve when this profile is applied as a section property
        /// to a linear element, modified by the Offset vector.
        /// </summary>
        public HorizontalSetOut HorizontalSetOut
        {
            get { return _HorizontalSetOut; }
            set
            {
                _HorizontalSetOut = value;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("HorizontalSetOut");
            }
        }

        /// <summary>
        /// Private backing field for VerticalSetOut property
        /// </summary>
        private VerticalSetOut _VerticalSetOut = VerticalSetOut.Centroid;

        /// <summary>
        /// The vertical position of the base set-out point of the profile.
        /// This is the point on the profile which will be taken as running along
        /// the element set-out curve when this profile is applied as a section property
        /// to a linear element, modified by the Offset vector.
        /// </summary>
        public VerticalSetOut VerticalSetOut
        {
            get { return _VerticalSetOut; }
            set
            {
                _VerticalSetOut = value;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("VerticalSetOut");
            }
        }

        /// <summary>
        /// Private backing field for Offset property
        /// </summary>
        private Vector _Offset = Vector.Zero;

        /// <summary>
        /// The set-out offset vector of this profile.  This describes the position of the
        /// base set-out point defined by the VerticalSetOut and HorizontalSetOut properties
        /// relative to the actual point along which the set-out curve is assumed to run when
        /// this profile is applied as a section profile to a linear element.
        /// </summary>
        public Vector Offset
        {
            get { return _Offset; }
            set
            {
                _Offset = value;
                InvalidateCachedGeometry();
                NotifyPropertyChanged("Offset");
            }
        }

        /// <summary>
        /// Private backing field for Section property
        /// </summary>
        private SectionProperty _Section = null;

        /// <summary>
        /// The section to which this profile belongs
        /// </summary>
        public SectionProperty Section
        {
            get { return _Section; }
            internal set { _Section = value; }
        }

        SectionProperty IOwned<SectionProperty>.Owner
        {
            get
            {
                return _Section;
            }
        }

        /// <summary>
        /// Get the overall depth of this section profile.
        /// This is a utility property common to all profile types
        /// regardless of how their parameters are actually defined.
        /// It gives the overall bounding dimension of the 
        /// (i.e. the depth between extreme points).
        /// </summary>
        public abstract double OverallDepth { get; }

        /// <summary>
        /// Get the overall width of this section profile.
        /// This is a utility property common to all profile types
        /// regardless of how their parameters are actually defined.
        /// It gives the overall bounding dimension of the profile
        /// (i.e. the width between extreme points).
        /// </summary>
        public abstract double OverallWidth { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Set the set-out point of this profile defined by horizontal and vertical set-out rules.
        /// </summary>
        /// <param name="horizontal">The horizontal set-out rule</param>
        /// <param name="vertical">The vertical set-out rule</param>
        public void SetOut(HorizontalSetOut horizontal, VerticalSetOut vertical)
        {
            HorizontalSetOut = horizontal;
            VerticalSetOut = vertical;
        }

        /// <summary>
        /// Set the set-out point of this profile defined by horizontal and vertical set-out rules.
        /// </summary>
        /// <param name="horizontal">The horizontal set-out rule</param>
        /// <param name="vertical">The vertical set-out rule</param>
        /// <param name="offset">The offset of the section relative to the the base set-out point</param>
        public void SetOut(HorizontalSetOut horizontal, VerticalSetOut vertical, Vector offset)
        {
            SetOut(horizontal, vertical);
            Offset = offset;
        }

        /// <summary>
        /// Invalidate the stored generated geometry 
        /// </summary>
        public virtual void InvalidateCachedGeometry()
        {

        }

        public void CalculateGeometricProperties()
        {
            //TO BE REVIEWED!

            //Calculate combined area:
            Curve perimeter = Perimeter;
            if (perimeter != null && perimeter.IsValid)
            {
                Vector centroid;
                double area = perimeter.CalculateEnclosedArea(out centroid);
            }
        }

        #endregion
    }
}
