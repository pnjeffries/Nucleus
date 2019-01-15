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
    public class IntersectionTests
    {
        [TestMethod]
        public void OffsetExtensionDistanceTest()
        {
            Angle angle = Angle.FromDegrees(30);
            double offsetA = 1;
            double offsetB = 0.5;
            double extension = Intersect.OffsetExtensionDistance(angle, offsetA, offsetB);

            Assert.AreEqual(0.732, extension, 0.001);
        }

        [TestMethod]
        public void OffsetExtensionDistanceTest2()
        {
            Angle angle = Angle.FromDegrees(-30);
            double offsetA = 1;
            double offsetB = 0.5;
            double extension = Intersect.OffsetExtensionDistance(angle, offsetA, offsetB);

            Assert.AreEqual(-0.732, extension, 0.001);
        }

        [TestMethod]
        public void OffsetExtensionDistanceTest3()
        {
            Angle angle = Angle.FromDegrees(30);
            double offsetA = 1;
            double offsetB = -0.5;
            double extension = Intersect.OffsetExtensionDistance(angle, offsetA, offsetB);

            Assert.AreEqual(2.732, extension, 0.001);
        }

        [TestMethod]
        public void OffsetExtensionDistanceRightAngle()
        {
            Angle angle = Angle.FromDegrees(-90);
            double offsetA = 1;
            double offsetB = 0.5;
            double extension = Intersect.OffsetExtensionDistance(angle, offsetA, offsetB);

            Assert.AreEqual(0.5, extension, 0.001);
        }
    }
}
