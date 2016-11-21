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

using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract base class for profiles which are defined by numerical
    /// parameters and have their geometry automatically generated based
    /// on them.
    /// </summary>
    [Serializable]
    public abstract class ParameterProfile : SectionProfile
    {
        #region Properties

        /// <summary>
        /// Private backing field for the Perimeter property
        /// </summary>
        private Curve _Perimeter = null;

        /// <summary>
        /// The (generated) shape of the outer perimeter of this profile
        /// </summary>
        public override Curve Perimeter
        {
            get
            {
                if (_Perimeter == null) _Perimeter = GeneratePerimeter();
                return _Perimeter;
            }
        }

        /// <summary>
        /// Private 
        /// </summary>
        private CurveCollection _Voids = null;

        /// <summary>
        /// The collection of curves which denote the edges of internal voids
        /// within this profile
        /// </summary>
        public override CurveCollection Voids
        {
            get
            {
                if (_Voids == null) _Voids = GenerateVoids();
                return _Voids;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generate the profile's perimeter
        /// </summary>
        /// <returns></returns>
        protected abstract Curve GeneratePerimeter();

        /// <summary>
        /// Generate the edge curves of the internal voids.
        /// </summary>
        /// <returns></returns>
        protected abstract CurveCollection GenerateVoids();

        /// <summary>
        /// Invalidate the stored generated geometry 
        /// </summary>
        public virtual void InvalidateCachedGeometry()
        {
            _Perimeter = null;
            _Voids = null;
            NotifyPropertyChanged("Perimeter");
            NotifyPropertyChanged("Voids");
            if (Section != null) Section.NotifyProfileChanged(this);
        }

        #endregion
    }
}
