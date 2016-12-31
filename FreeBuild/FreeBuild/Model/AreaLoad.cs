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
    /// A load applied over a planar area
    /// </summary>
    [Serializable]
    public class AreaLoad : Load<Element, ElementCollection>
    {
        #region Properties

        /// <summary>
        /// Private backing field for Region property
        /// </summary>
        private PlanarRegion _Region;

        /// <summary>
        /// The region over which the load is applied
        /// </summary>
        public PlanarRegion Region
        {
            get { return _Region; }
            set { _Region = value; NotifyPropertyChanged("Region"); }
        }

        /// <summary>
        /// Private backing field for Value property
        /// </summary>
        private Vector _Value;

        /// <summary>
        /// The value of the load to be applied.
        /// TODO: Review
        /// </summary>
        public Vector Value
        {
            get { return _Value; }
            set { _Value = value;  NotifyPropertyChanged("Value"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new blank area load.
        /// </summary>
        public AreaLoad() : base() { }

        /// <summary>
        /// Region constructor.  Initialises an area load applied over the specified region.
        /// </summary>
        /// <param name="region">The region of application of the load.</param>
        public AreaLoad(PlanarRegion region, Vector value) : this()
        {
            Region = region;
            Value = value;
        }

        /// <summary>
        /// Region constructor.  Initialises an area load applied over the region within the
        /// specified boundary.
        /// </summary>
        /// <param name="regionBoundary">The boundary of the applied area.  Should be closed.</param>
        public AreaLoad(Curve regionBoundary, Vector value) : this(new PlanarRegion(regionBoundary), value) { }

        #endregion
    }
}
