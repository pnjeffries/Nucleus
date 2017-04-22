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
    /// A property which describes the cross-thickness
    /// makeup of a panel element in order to produce
    /// a 3D solid geometry
    /// </summary>
    [Serializable]
    public class PanelFamily : Family
    {
        #region Properties

        private BuildUpLayerCollection _BuildUp;

        /// <summary>
        /// The collection of build-up layers that define the through-thickness
        /// properties of this family
        /// </summary>
        public BuildUpLayerCollection BuildUp
        {
            get
            {
                if (_BuildUp == null) _BuildUp = new BuildUpLayerCollection();
                return _BuildUp;
            }
        }

        /// <summary>
        /// Private backing field for the SetOut property
        /// </summary>
        private VerticalSetOut _SetOut = VerticalSetOut.Centroid;

        /// <summary>
        /// The set-out position of the layers
        /// </summary>
        public VerticalSetOut SetOut
        {
            get { return _SetOut; }
            set { ChangeProperty(ref _SetOut, value, "SetOut"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new blank Panel Family
        /// </summary>
        public PanelFamily() : base() { }

        /// <summary>
        /// Initialse a new Panel Family with the given name
        /// </summary>
        /// <param name="name"></param>
        public PanelFamily(string name) : this()
        {
            Name = name;
        }

        #endregion
    }
}
