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
        public void LineLineIntersection()
        {
            var pt = Intersect.LineLineXY(new Line(0, 0, 1, 0), new Line(0, 0, 1, 0), true);

            Assert.AreEqual(false, pt.IsValid());
        }

        [TestMethod]
        public void LineLineIntersection2()
        {
            var pt = Intersect.LineLineXY(new Line(0, 0, 1, 0), new Line(1, 0, 2, 0), true);

            Assert.AreEqual(true, pt.IsValid());
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
        public void LineCircleIntersection4()
        {
            var pts = Intersect.LineCircleXY(new Line(-42, -42, -42, -29.7061), new Circle(10, new Vector(-42, -42)));

            Assert.AreEqual(2, pts.Length);
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

        [TestMethod]
        public void LineInPolygon()
        {
            var line = new Line(new Vector(50, 34, 0), new Vector(21.1496, 34, 0));
            var vertices = new List<Vertex>()
            {
                new Vertex( 17.5636795786842 , -50 , 0),
                new Vertex( 50 , -50 , 0),
                new Vertex( 50 , -50 , 0),
                new Vertex( 50 , 50 , 0),
                new Vertex( 50 , 50 , 0),
                new Vertex( 2.57214422843186 , 50 , 0),
                new Vertex( 2.57214422843186 , 50 , 0),
                new Vertex( 17.5636795786842 , -50 , 0)
            };
            var curves = Intersect.LineInPolygonXY(line, vertices);

            Assert.AreEqual(1, curves.Count);
            Assert.AreEqual(line.Length, curves.TotalLength());
        }

        [TestMethod]
        public void PolyCurveDomainInPolygon()
        {
            var pline = new PolyLine(
               new Vector(0, 0),
               new Vector(0, 10),
               new Vector(-20, 10),
               new Vector(-20, 0));
            pline.Close();
            var polyCrv = pline.ToPolyCurve();

            var polygon = new Vertex[]
            {
                new Vertex(-10,0.001),
                new Vertex(-0.0001,0.001),
                new Vertex(-0.0001,-10),
                new Vertex(-10,-10)
            };

            var ints = Intersect.CurveDomainInPolygonXY(polyCrv, polygon);

            Assert.AreEqual(1, ints.Count);
            Assert.AreEqual(0.875, ints[0].Start);
            Assert.AreEqual(0.99999875, ints[0].End);
        }

        [TestMethod]
        public void PolyCurveDomainInPolygon2()
        {
            var pline = new PolyLine(
               new Vector(0, 0),
               new Vector(0, 10),
               new Vector(-20, 10),
               new Vector(-20, 0));
            pline.Close();
            var polyCrv = pline.ToPolyCurve();

            var polygon = new Vertex[]
            {
                new Vertex(-10,0),
                new Vertex(-0.0001,0),
                new Vertex(-0.0001,-10),
                new Vertex(-10,-10)
            };

            var ints = Intersect.CurveDomainInPolygonXY(polyCrv, polygon);

            Assert.AreEqual(1, ints.Count);
            Assert.AreEqual(0.875, ints[0].Start);
            Assert.AreEqual(0.99999875, ints[0].End);
        }

        [TestMethod]
        public void PolygonContainmentTest()
        {
            var polygon = new Vertex[]
            {
                new Vertex(-10,0),
                new Vertex(-0.0001,0),
                new Vertex(-0.0001,-10),
                new Vertex(-10,-10)
            };

            bool inside = polygon.PolygonContainmentXY(new Vector(-20, 0, 0));

            Assert.AreEqual(false, inside);
        }
    }
}
