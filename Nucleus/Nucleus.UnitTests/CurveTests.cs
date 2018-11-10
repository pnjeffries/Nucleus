using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class CurveTests
    {

        [TestMethod]
        public void AreaCalculationTest()
        {
            var polyCrv = new PolyCurve(new Line(0, 0, 10, 10));
            polyCrv.AddArc(new Vector(10, 0));
            polyCrv.Add(new PolyLine(new Vector(10, 0), new Vector(5, 0), new Vector(5, -5)));
            polyCrv.Close();

            Vector centroid;
            double area = polyCrv.CalculateEnclosedArea(out centroid);
            Assert.AreEqual(205.309725, area, 0.0001);
        }

        [TestMethod]
        public void AreaCalculationTest2()
        {
            // Test a badly-formed polyline (with duplicate points):
            var pline = new PolyLine(
                new Vector(0, 0),
                new Vector(0, 2),
                new Vector(0, 2),
                new Vector(2, 0),
                new Vector(0, 0));
            pline.Close();

            double area = pline.CalculateEnclosedArea();
            Assert.AreEqual(2.0, area);
        }

        [TestMethod]
        public void CurvePlaneTest()
        {
            // Test a badly-formed polyline (with duplicate points):
            var pline = new PolyLine(
                new Vector(-125, -95, -0.00999999977648258),
                new Vector(-125, -95, -0.00999999977648258),
                new Vector(-110, 140, -0.00999999977648258),
                new Vector(115, 130, -0.00999999977648258),
                new Vector(85, -90, -0.00999999977648258));
            pline.Close();

            var plane = pline.Plane();
            Assert.AreEqual(-Vector.UnitZ, plane.Z);
        }

        [TestMethod]
        public void PolyLineCleanTest()
        {
            // Test a badly-formed polyline (with duplicate points):
            var pline = new PolyLine(
                new Vector(0, 0),
                new Vector(0, 2),
                new Vector(0, 2),
                new Vector(2, 0),
                new Vector(0, 0));
            pline.Close();

            bool removedAny = pline.Clean();
            Assert.AreEqual(true, removedAny);
            Assert.AreEqual(3, pline.VertexCount);
        }

        [TestMethod]
        public void PolyLineCleanTest2()
        {
            // Test a badly-formed polyline (with duplicate points):
            var pline = new PolyLine(
                new Vector(0, 0),
                new Vector(0, 2),
                new Vector(0, 2),
                new Vector(2, 0),
                new Vector(0, 0));

            bool removedAny = pline.Clean();
            Assert.AreEqual(true, removedAny);
            Assert.AreEqual(4, pline.VertexCount);
        }

        [TestMethod]
        public void FastDuplicateTest()
        {
            var polyCrv = new PolyCurve(new Line(0, 0, 10, 10));
            polyCrv.AddArc(new Vector(10, 0));
            polyCrv.Add(new PolyLine(new Vector(10, 0), new Vector(5, 0), new Vector(5, -5)));

            for (int i = 0; i < 1000; i++)
            {
                var polyCrv2 = polyCrv.FastDuplicate();
                Assert.AreEqual(polyCrv.Length, polyCrv2.Length);
            }
        }

        [TestMethod]
        public void DuplicateTest()
        {
            var polyCrv = new PolyCurve(new Line(0, 0, 10, 10));
            polyCrv.AddArc(new Vector(10, 0));
            polyCrv.Add(new PolyLine(new Vector(10, 0), new Vector(5, 0), new Vector(5, -5)));

            for (int i = 0; i < 1000; i++)
            {
                var polyCrv2 = polyCrv.Duplicate();
                Assert.AreEqual(polyCrv.Length, polyCrv2.Length);
            }
        }

        [TestMethod]
        public void CurveOverlapTest()
        {
            var crv1 = new Line(0, 0, 0, 10);

            var crv2 = new Line(1, 5, 1, 15);

            Interval intval = crv1.OverlapWith(crv2);
            Assert.AreEqual(0.5, intval.Start);
            Assert.AreEqual(1, intval.End);
        }

        
    }
}
