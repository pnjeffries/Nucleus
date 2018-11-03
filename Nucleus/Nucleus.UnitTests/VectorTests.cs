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
    }
}
