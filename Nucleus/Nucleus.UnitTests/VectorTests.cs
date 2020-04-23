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
    public class VectorTests
    {
        [TestMethod]
        public void MagnitudeTest()
        {
            Vector v = new Vector(3, 4);
            Assert.AreEqual(5, v.Magnitude(), 0.0000001);
        }

        [TestMethod]
        public void AngleBetweenTest()
        {
            Vector v1 = new Vector(0.998510540406107, 0.0545591486178354, 0);
            Vector v2 = new Vector(0.998510540406108, 0.0545591486178327, 0);
            Angle a = v1.AngleBetween(v2);

            Assert.AreEqual(0, a);
        }

        [TestMethod]
        public void ParallelTest()
        {
            Vector v1 = new Vector(-7.72034522180095, 2.09672832198529, 0);
            Vector v2 = new Vector(-15.4406904436019, 4.19345664397058, 0);
            Angle a = v1.AngleBetween(v2);

            Assert.AreEqual(0, a);

            bool parallel = v1.IsParallelTo(v2);

            Assert.AreEqual(true, parallel);
        }

        [TestMethod]
        public void ParallelTest2()
        {
            Vector v1 = new Vector(-7.72034522180095, 2.09672832198529, 0);
            Vector v2 = new Vector(15.4406904436019, -4.19345664397058, 0);
            
            bool parallel = v1.IsParallelTo(v2);

            Assert.AreEqual(true, parallel);
        }

        [TestMethod]
        public void ParallelTest3()
        {
            Vector v1 = new Vector(-7.72034522180095, 2.09672832198529, 0);
            Vector v2 = new Vector(15.4406904436019, 4.19345664397058, 0);

            bool parallel = v1.IsParallelTo(v2);

            Assert.AreEqual(false, parallel);
        }

        [TestMethod]
        public void Side_ShouldBeRight()
        {
            Vector v = new Vector(1, 0, 0);
            Vector o = new Vector(0, 0, 0);

            Vector pt = new Vector(0, 1, 0);
            var side = pt.SideOf(o, v);

            Assert.AreEqual(HandSide.Right, side);
        }
    }
}
