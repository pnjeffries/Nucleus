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
using FreeBuild.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A collection of vertex geometry that acts equivalently to a layer in a CAD package
    /// </summary>
    [Serializable]
    public class GeometryLayer : VertexGeometryCollection, INamed
    {
        #region Properties

        /// <summary>
        /// Private backing field for the Name property
        /// </summary>
        [Copy(CopyBehaviour.COPY)]
        private string _Name;

        /// <summary>
        /// The name of the layer
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Private backing field for the Visible property
        /// </summary>
        [Copy(CopyBehaviour.COPY)]
        private bool _Visible = true;

        /// <summary>
        /// Whether or not this layer is visible and should be rendered
        /// </summary>
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;
                NotifyPropertyChanged("Visible");
            }
        }

        /// <summary>
        /// Private backing field for the Brush property
        /// </summary>
        [Copy(CopyBehaviour.DUPLICATE)]
        private DisplayBrush _Brush = ColourBrush.Black;

        /// <summary>
        /// The default brush to be used to display objects on this layer in
        /// the absense of any overriding display colour
        /// </summary>
        public DisplayBrush Brush
        {
            get { return _Brush; }
            set
            {
                _Brush = value;
                NotifyPropertyChanged("Brush");
            }
        }

        /// <summary>
        /// Private backing field for the Tag property
        /// </summary>
        [Copy(CopyBehaviour.COPY)]
        private string _Tag = null;

        /// <summary>
        /// A general-purpose text tag on this layer, used to designate
        /// additional information, such as what the geometry on this layer
        /// represents
        /// </summary>
        public string Tag
        {
            get { return _Tag; }
            set { _Tag = value;  NotifyPropertyChanged("Tag"); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public GeometryLayer()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the layer</param>
        public GeometryLayer(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name, brush constructor
        /// </summary>
        /// <param name="name">The name of the layer</param>
        /// <param name="brush">The default display brush of this layer</param>
        public GeometryLayer(string name, DisplayBrush brush) : this(name)
        {
            Brush = brush;
        }

        #endregion
    }
}
