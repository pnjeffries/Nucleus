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
            var rayHit = ddTree.RayTrace(new Axis(new Vector(0, -30), new Vector(0, 1)));
            Assert.AreEqual(20, rayHit.Parameter);
        }
    }
}
