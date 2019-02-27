using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.DDTree;
using Nucleus.Geometry;
using Nucleus.Meshing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class DDTreeTests
    {
        [TestMethod]
        public void RaytraceTest()
        {
            var mB = new MeshBuilder();
            mB.AddCuboid(20, 20, 20);
            mB.Finalize();
            Mesh mesh = mB.Mesh;

            MeshFaceDDTree ddTree = new MeshFaceDDTree(mesh.Faces);
            var rayHit = ddTree.RayTrace(new Axis(new Vector(0, -30, 10), new Vector(0, 1)));
            Assert.AreEqual(20, rayHit.Parameter);

            var rayHit2 = ddTree.RayTrace(new Axis(new Vector(30, -30, 10),
                new Vector(-1, 1).Unitize()));
            Assert.AreEqual(28.28427, rayHit2.Parameter, 0.0001);

            var rayHit3 = ddTree.RayTrace(new Axis(new Vector(0, -30, 30), new Vector(0, 1)));
            Assert.AreEqual(null, rayHit3);
        }

        [TestMethod]
        public void RaytraceTest2()
        {
            // Test multiple cubes
            var mB = new MeshBuilder();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    mB.AddCuboid(20, 20, 20, new CartesianCoordinateSystem(new Vector(30 * j, 30 * i)));
                    mB.Finalize();
                }
            }
            
            Mesh mesh = mB.Mesh;

            MeshFaceDDTree ddTree = new MeshFaceDDTree(mesh.Faces);
            var rayHit = ddTree.RayTrace(new Axis(new Vector(0, -30, 10), new Vector(0, 1)));
            Assert.AreEqual(20, rayHit.Parameter);

            var rayHit2 = ddTree.RayTrace(new Axis(new Vector(30, -30, 10),
                new Vector(-1, 1).Unitize()));
            Assert.AreEqual(28.28427, rayHit2.Parameter, 0.0001);

            var rayHit3 = ddTree.RayTrace(new Axis(new Vector(0, -30, 30), new Vector(0, 1)));
            Assert.AreEqual(null, rayHit3);
        }

        [TestMethod]
        public void RaytraceTest3()
        {
            var meshFace = new MeshFace(
                new Vertex(-9.0, 0.0, 3.0),
                new Vertex(2.0, 0.0, 3.0),
                new Vertex(2.0, 0.0, -4.0),
                new Vertex(-9.0, 0.0, -4.0));

            MeshFaceDDTree ddTree = new MeshFaceDDTree(new MeshFace[] { meshFace });

            var rayHit = ddTree.RayTrace(new Axis(new Vector(0, -36, 0), Vector.UnitY));

            Assert.AreNotEqual(null, rayHit);
        }
    }
}
