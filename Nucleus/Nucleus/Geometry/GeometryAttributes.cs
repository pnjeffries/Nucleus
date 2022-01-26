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

using Nucleus.Rendering;
using System;
using System.Collections.Generic;

namespace Nucleus.Geometry
{
    /// <summary>
    /// An object which holds additional non-geometric attributes about how a
    /// piece of geometry should be displayed or organised.
    /// </summary>
    [Serializable]
    public class GeometryAttributes : DisplayAttributes
    {
        #region Properties

        /// <summary>
        /// Private backing field for LayerName property
        /// </summary>
        private string _LayerName = null;

        /// <summary>
        /// The name of the layer (if any) on which this object should be displayed
        /// </summary>
        public string LayerName
        {
            get { return _LayerName; }
            set { ChangeProperty(ref _LayerName, value, "LayerName"); }
        }

        /// <summary>
        /// Private backing field for SourceID property
        /// </summary>
        private string _SourceID = null;

        /// <summary>
        /// The ID of the source object from which this geometry was generated or
        /// to which it is otherwise linked.
        /// </summary>
        public string SourceID
        {
            get { return _SourceID; }
            set { _SourceID = value; }
        }

        /// <summary>
        /// Private backing field for Interactive property
        /// </summary>
        private bool _Interactive = false;

        /// <summary>
        /// Raise mouse interaction events with this geometry?
        /// </summary>
        public bool Interactive
        {
            get { return _Interactive; }
            set { ChangeProperty(ref _Interactive, value, "Interactive"); }
        }

        /// <summary>
        /// Private backing field for Phases property
        /// </summary>
        private IList<int> _Phases;

        /// <summary>
        /// The development phases which this geometry should be included in.
        /// A value of -1 in the phase list indicates the list is unset.
        /// </summary>
        public IList<int> Phases
        {
            get
            {
                if (_Phases == null) _Phases = new List<int>() { -1 };
                return _Phases;
            }
            set { ChangeProperty(ref _Phases, value); }
        }
 
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public GeometryAttributes() { }

        /// <summary>
        /// Initialise an attributes object copying properties from another
        /// </summary>
        /// <param name="other">Another set of geometry attributes.  This may be null,
        /// in which case default values will be retained.</param>
        public GeometryAttributes(GeometryAttributes other)
        {
            if (other != null)
            {
                _SourceID = other.SourceID;
                _LayerName = other.LayerName;
                Brush = other.Brush;
                Weight = other.Weight;
                _Interactive = other.Interactive;
            }
        }

        /// <summary>
        /// Initialise an attributes object with the specified data
        /// </summary>
        /// <param name="sourceID"></param>
        /// <param name="layerName"></param>
        public GeometryAttributes(string sourceID, string layerName = null, DisplayBrush brush = null, double thickness = 1.0)
        {
            SourceID = sourceID;
            LayerName = layerName;
            Brush = brush;
            Weight = thickness;
        }

        /// <summary>
        /// Brush constructor.  Initialises a new GeometryAttributes object with the specified display brush.
        /// </summary>
        /// <param name="brush"></param>
        public GeometryAttributes(DisplayBrush brush, double thickness = 1.0)
        {
            Brush = brush;
            Weight = thickness;
        }

        /// <summary>
        /// Initialises a new GeometryAttributes object with the Brush initialised as 
        /// a ColourBrush of the specified colour
        /// </summary>
        /// <param name="colour"></param>
        public GeometryAttributes(Colour colour, double thickness = 1.0)
        {
            Brush = new ColourBrush(colour);
            Weight = thickness;
        }

        #endregion
    }
}
