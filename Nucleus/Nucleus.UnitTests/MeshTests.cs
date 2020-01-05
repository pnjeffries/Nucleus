using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class MeshTests
    {
        [TestMethod]
        public void MeshFacePlaneIntersection()
        {
            var face = new MeshFace(
                new Vertex(0, 0, 0),
                new Vertex(2, 0, 10),
                new Vertex(8, 0, 10),
                new Vertex(10, 0, 0));
            var line = face.IntersectPlane(5);
            Assert.AreEqual(8, line.Length);
        }
    }
}
