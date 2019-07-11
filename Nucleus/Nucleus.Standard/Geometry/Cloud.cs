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

namespace Nucleus.Geometry
{
    /// <summary>
    /// A point cloud, consisting only of vertices with no connecting geometry.
    /// </summary>
    [Serializable]
    public class Cloud : VertexGeometry
    {
        #region Properties

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Private backing field for Vertices property
        /// </summary>
        private VertexCollection _Vertices;

        /// <summary>
        /// The vertices of this cloud
        /// </summary>
        public override VertexCollection Vertices { get { return _Vertices; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Cloud(GeometryAttributes attributes = null)
        {
            _Vertices = new VertexCollection(this);
            Attributes = attributes;
        }

        /// <summary>
        /// Initialise a point cloud using the specified point locations
        /// </summary>
        /// <param name="points"></param>
        public Cloud(IList<Vector> points, GeometryAttributes attributes = null) : this(attributes)
        {
            foreach (Vector point in points)
            {
                _Vertices.Add(new Vertex(point));
            }
        }

        /// <summary>
        /// Initialise a point cloud containing a single point
        /// </summary>
        /// <param name="point"></param>
        public Cloud(Vector point, GeometryAttributes attributes = null) : this(attributes)
        {
            _Vertices.Add(new Vertex(point));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a point to this cloud.
        /// </summary>
        /// <param name="point"></param>
        public Vertex Add(Vector point)
        {
            Vertex v = new Vertex(point);
            _Vertices.Add(v);
            return v;
        }

        /// <summary>
        /// Add a vertex to this cloud.
        /// If the specified vertex already belongs to another piece of geometry it will
        /// automatically be copied and the copy added to this cloud.
        /// </summary>
        /// <param name="vertex"></param>
        public Vertex Add(Vertex vertex)
        {
            if (vertex.Owner != null) vertex = new Vertex(vertex);
            _Vertices.Add(vertex);
            return vertex;
        }

        public override string ToString()
        {
            return "Point Cloud";
        }

        #endregion

    }
}
