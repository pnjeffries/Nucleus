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
    }
}
