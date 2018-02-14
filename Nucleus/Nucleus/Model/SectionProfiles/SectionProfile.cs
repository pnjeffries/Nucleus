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
using Nucleus.Extensions;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Base class for objects representing the profile of a SectionProperty.
    /// </summary>
    [Serializable]
    public abstract class SectionProfile : Unique, IOwned<SectionFamily>
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
        /// The shorthand designating the type of catalogue section, obtained
        /// from the starting letters of the CatalogueName.  For example, for
        /// Universal Beams this would be "UB".
        /// </summary>
        public string CatalogueTypeDesignation
        {
            get { return _CatalogueName?.StartingLetters(); }
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
        [Copy(CopyBehaviour.MAP)]
        private SectionFamily _Section = null;

        /// <summary>
        /// The section to which this profile belongs
        /// </summary>
        public SectionFamily Section
        {
            get { return _Section; }
            internal set { _Section = value; }
        }

        SectionFamily IOwned<SectionFamily>.Owner
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

        /// <summary>
        /// Private backing field for the Area property
        /// </summary>
        private double _Area = double.NaN;

        /// <summary>
        /// The cross-sectional area of the profile, in m².
        /// Will be calculated automatically where necessary if not
        /// populated manually.
        /// </summary>
        public double Area
        {
            get
            {
                if (double.IsNaN(_Area)) _Area = CalculateArea();
                return _Area;
            }
            protected set
            {
                ChangeProperty(ref _Area, value, "Area");
            }
        }

        /// <summary>
        /// Private backing field for the Ixx property.
        /// </summary>
        private double _Ixx = double.NaN;

        /// <summary>
        /// The second moment of area of the profile about the major 
        /// (X-X) axis, in m^4.
        /// Will be calculated automatically where necessary if not
        /// populated manually.
        /// </summary>
        public double Ixx
        {
            get
            {
                if (double.IsNaN(_Ixx)) _Ixx = CalculateIxx();
                return _Ixx;
            }
            protected set
            {
                ChangeProperty(ref _Ixx, value, "Ixx");
            }
        }

        /// <summary>
        /// Private backing field for the Iyy property.
        /// </summary>
        private double _Iyy = double.NaN;

        /// <summary>
        /// The second moment of area of the profile about the minor
        /// (Y-Y) axis, in m^4.
        /// Will be calculated automatically where necessary if not
        /// populated manually.
        /// </summary>
        public double Iyy
        {
            get
            {
                if (double.IsNaN(_Iyy)) _Iyy = CalculateIyy();
                return _Iyy;
            }
            protected set
            {
                ChangeProperty(ref _Iyy, value, "Iyy");
            }
        }

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
            Area = double.NaN;
        }

        /// <summary>
        /// Calculate the cross-sectional area of this profile
        /// </summary>
        /// <returns></returns>
        public virtual double CalculateArea()
        {
            Curve perimeter = Perimeter;
            if (perimeter != null && perimeter.IsValid)
            {
                Vector centroid;
                return perimeter.CalculateEnclosedArea(out centroid, Voids).Abs();
            }
            else
                return double.NaN;
        }

        /// <summary>
        /// Calculate the second moment of area of this profile about the X-X axis
        /// </summary>
        /// <returns></returns>
        public virtual double CalculateIxx()
        {
            Curve perimeter = Perimeter;
            if (perimeter != null && perimeter.IsValid)
            {
                return perimeter.CalculateEnclosedIxx(Voids).Abs();
            }
            else
                return double.NaN;
        }

        /// <summary>
        /// Calculate the second moment of area of this profile about the Y-Y axis
        /// </summary>
        /// <returns></returns>
        public virtual double CalculateIyy()
        {
            Curve perimeter = Perimeter;
            if (perimeter != null && perimeter.IsValid)
            {
                return perimeter.CalculateEnclosedIyy(Voids).Abs();
            }
            else
                return double.NaN;
        }

        /// <summary>
        /// Calculate the total combined offset of the centroid of the profile to the specified set-out location
        /// </summary>
        /// <returns></returns>
        public abstract Vector GetTotalOffset(HorizontalSetOut toHorizontal = HorizontalSetOut.Centroid,
            VerticalSetOut toVertical = VerticalSetOut.Centroid);

        public override string ToString()
        {
            if (CatalogueName != null) return CatalogueName;
            else return GenerateDescription();
        }

        /// <summary>
        /// Generate the string description of this profile
        /// </summary>
        /// <returns></returns>
        public abstract string GenerateDescription();

        #endregion

        #region Static Methods

        /// <summary>
        /// Create a new SectionProfile based on a string description.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="catalogue"></param>
        /// <returns></returns>
        public static SectionProfile FromDescription(string description, SectionProfileLibrary catalogue = null)
        {
            if (catalogue != null)
            {
                // Check in catalogue:
                SectionProfile profile = catalogue.GetByCatalogueName(description);
                if (profile != null) return profile.Duplicate();
            }

            string[] tokens = description.Split(' ');
            if (tokens.Length > 1)
            {
                switch(tokens[0].Trim().ToUpper())
                {
                    case "I":
                        return new SymmetricIProfile(tokens[1]);
                    case "T":
                        return new TProfile(tokens[1]);
                    case "A":
                        return new AngleProfile(tokens[1]);
                    case "CIRC":
                        return new CircularProfile(tokens[1]);
                    case "CHS":
                        return new CircularHollowProfile(tokens[1]);
                    case "RECT":
                        return new RectangularProfile(tokens[1]);
                    case "RHS":
                        return new RectangularProfile(tokens[1]);
                }
                throw new NotImplementedException("The specified section profile description could not be parsed successfully."); //If we're here, we haven't caught the case
            }

            return null;
        }

        /// <summary>
        /// Split the specified dimesion string into its constituent parts for subsequent
        /// parsing as dimension parameters
        /// </summary>
        /// <param name="dimensionString"></param>
        /// <returns></returns>
        protected string[] TokeniseDimensionString(string dimensionString)
        {
            return dimensionString.Split('x', '×', ' ');
        }

        #endregion
    }
}
