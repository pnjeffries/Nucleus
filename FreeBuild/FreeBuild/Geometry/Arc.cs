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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A planar arc between two points
    /// </summary>
    public class Arc : Curve
    {
        #region Properties

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// The arc will be defined by the start and end points of this collection and a vertex
        /// lying on the arc in between them.
        /// </summary>
        public override VertexCollection Vertices { get; }

        /// <summary>
        /// The full circle that this arc forms part of
        /// </summary>
        public Circle Circle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Is this Arc closed?  (i.e. does it represent a circle?
        /// </summary>
        public override bool Closed { get { return StartPoint == EndPoint; } protected set { } }

        public override bool IsValid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default private constructor
        /// </summary>
        protected Arc()
        {
            Vertices = new Geometry.VertexCollection(this);
        }

        #endregion
    }
}
