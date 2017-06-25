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
using Nucleus.Geometry;

namespace Nucleus.Meshing
{
    /// <summary>
    /// Builder object for Nucleus meshes
    /// </summary>
    public class MeshBuilder : MeshBuilderBase<Mesh>
    {
        public MeshBuilder()
        {
            _Mesh = new Mesh();
        }

        public override int AddFace(int v1, int v2, int v3)
        {
            MeshFace face = new MeshFace(_Mesh.Vertices[v1], _Mesh.Vertices[v2], _Mesh.Vertices[v3]);
            _Mesh.Faces.Add(face);
            return _Mesh.Faces.Count - 1;
        }

        public override int AddFace(int v1, int v2, int v3, int v4)
        {
            MeshFace face = new MeshFace(_Mesh.Vertices[v1], _Mesh.Vertices[v2], _Mesh.Vertices[v3], _Mesh.Vertices[v4]);
            _Mesh.Faces.Add(face);
            return _Mesh.Faces.Count - 1;
        }

        public override int AddVertex(Vertex v)
        {
            _Mesh.Vertices.Add(new Vertex(v));
            return _Mesh.Vertices.Count - 1;
        }

        public override int AddVertex(Vector pt)
        {
            _Mesh.Vertices.Add(new Vertex(pt));
            return _Mesh.Vertices.Count - 1;
        }
    }
}
