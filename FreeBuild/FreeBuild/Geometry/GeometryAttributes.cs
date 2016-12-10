﻿// Copyright (c) 2016 Paul Jeffries
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

using FreeBuild.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// An object which holds additional non-geometric attributes about how a
    /// piece of geometry should be displayed or organised.
    /// </summary>
    [Serializable]
    public class GeometryAttributes
    {
        #region Properties

        /// <summary>
        /// The name of the layer (if any) on which this object should be displayed
        /// </summary>
        public string LayerName { get; set; } = null;

        /// <summary>
        /// The ID of the source object from which this geometry was generated or
        /// to which it is otherwise linked.
        /// </summary>
        public string SourceID { get; set; } = null;

        /// <summary>
        /// The brush which determines how this geometry should be drawn.
        /// </summary>
        public DisplayBrush Brush { get; set; } = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public GeometryAttributes() { }

        /// <summary>
        /// Initialise an attributes object with the specified data
        /// </summary>
        /// <param name="sourceID"></param>
        /// <param name="layerName"></param>
        public GeometryAttributes(string sourceID, string layerName = null, DisplayBrush brush = null)
        {
            SourceID = sourceID;
            LayerName = layerName;
            Brush = brush;
        }

        /// <summary>
        /// Brush constructor.  Initialises a new GeometryAttributes object with the specified display brush.
        /// </summary>
        /// <param name="brush"></param>
        public GeometryAttributes(DisplayBrush brush)
        {
            Brush = brush;
        }

        #endregion
    }
}
