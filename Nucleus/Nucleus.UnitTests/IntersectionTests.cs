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

        [TestMethod]
        public void LineCircleIntersection()
        {
            var pts = Intersect.LineCircleXY(new Line(0, 0, 10, 0), new Circle(1, new Vector(5, 0)));

            Assert.AreEqual(2, pts.Length);
            Assert.AreEqual(new Vector(4, 0), pts[0]);
            Assert.AreEqual(new Vector(6, 0), pts[1]);
        }

        [TestMethod]
        public void LineCircleIntersection2()
        {
            var pts = Intersect.LineCircleXY(new Line(0, 1, 10, 1), new Circle(1, new Vector(5, 0)));

            Assert.AreEqual(1, pts.Length);
            Assert.AreEqual(new Vector(5, 1), pts[0]);
        }

        [TestMethod]
        public void LineCircleIntersection3()
        {
            var pts = Intersect.LineCircleXY(new Line(0, 1, 10, 1), new Circle(2, new Vector(5, 0)));

            Assert.AreEqual(2, pts.Length);
            //Assert.AreEqual(new Vector(5, 1), pts[0]);
        }

        [TestMethod]
        public void CircleCircleIntersection()
        {
            Vector[] pts = Intersect.CircleCircleXY(new Vector(-1,0), 3, new Vector(1,0), 3);

            Assert.AreEqual(2, pts.Length);
        }

        [TestMethod]
        public void CircleInsideCircleIntersection()
        {
            Vector[] pts = Intersect.CircleCircleXY(new Vector(), 3, new Vector(), 4);

            Assert.AreEqual(0, pts.Length);
        }
    }
}
