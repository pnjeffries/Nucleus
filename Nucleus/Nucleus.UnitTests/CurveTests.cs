using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.Base;
using Nucleus.Extensions;
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
        public void SubdomainByCentreTest()
        {
            var polyCrv = new PolyCurve(new Line(0, 0, 10, 10));
            polyCrv.AddArc(new Vector(10, 0));
            polyCrv.Add(new PolyLine(new Vector(10, 0), new Vector(5, 0), new Vector(5, -5)));

            var domain = polyCrv.SubdomainByCentre(0.5, 20);
            Curve subCrv = polyCrv.Extract(domain);

            Assert.AreEqual(20, subCrv.Length, 0.00001);

        }

        [TestMethod]
        public void SubdomainByCentreTest2()
        {
            var polyCrv = new PolyCurve(new Line(0, 0, 10, 10));
            polyCrv.AddArc(new Vector(10, 0));
            polyCrv.Add(new PolyLine(new Vector(10, 0), new Vector(5, 0), new Vector(5, -5)));
            polyCrv.Close();

            var domain = polyCrv.SubdomainByCentre(0.01, 20);
            Curve subCrv = polyCrv.Extract(domain);

            Assert.AreEqual(20, subCrv.Length, 0.00001);

        }

        [TestMethod]
        public void SubdomainByCentreTest3()
        {
            var pline = new PolyLine(
                new Vector(0, 0),
                new Vector(0, 2),
                new Vector(0, 2),
                new Vector(2, 0),
                new Vector(0, 0));
            pline.Close();
            var polyCrv = pline.ToPolyCurve();

            var domain = polyCrv.SubdomainByCentre(0.1, 20);
            Curve subCrv = polyCrv.Extract(domain);

            Assert.AreEqual(pline.Length, subCrv.Length, 0.00001);
        }

        [TestMethod]
        public void SubdomainByCentreNoShuntTest()
        {
            var pline = new PolyLine(
                new Vector(0, 0),
                new Vector(0, 2),
                new Vector(0, 2),
                new Vector(2, 0));

            var polyCrv = pline.ToPolyCurve();

            double length = 3.0;
            var domain = polyCrv.SubdomainByCentre(0, length);
            Curve subCrv = polyCrv.Extract(domain);

            Assert.AreEqual(length/2, subCrv.Length, 0.00001);
        }

        [TestMethod]
        public void SubdomainByCentreShuntTest()
        {
            var pline = new PolyLine(
                new Vector(0, 0),
                new Vector(0, 2),
                new Vector(0, 2),
                new Vector(2, 0));

            var polyCrv = pline.ToPolyCurve();

            double length = 3.0;
            var domain = polyCrv.SubdomainByCentre(0, length, true);
            Curve subCrv = polyCrv.Extract(domain);

            Assert.AreEqual(length, subCrv.Length, 0.00001);
        }

        [TestMethod]
        public void CurveOverlapTest()
        {
            var crv1 = new Line(0, 0, 0, 10);

            var crv2 = new Line(1, 5, 1, 15);

            Interval intval = crv1.ProjectionOf(crv2);
            Assert.AreEqual(0.5, intval.Start);
            Assert.AreEqual(1, intval.End);
        }

        [TestMethod]
        public void PolyCurveTangentTest()
        {
            var pCrv = new PolyCurve(
                new Line(-14.670025365972 , 50 , 0, -50, 50, 0),
                new Line(-50, 50, 0, -50, -50, 0),
                new Line(-50, -50, 0 , -14.670025365972, -50, 0),
                new Line(-14.670025365972, -50, 0, -14.670025365972, 50, 0)
                );

            double t = 0.342527307783402;
            Vector tangent = pCrv.TangentAt(t);
            Assert.AreEqual(new Vector(0, -1, 0), tangent);
        }

        [TestMethod]
        public void PolyCurveOffsetWithChinkFailsIfNotNull()
        {
            var pLine = new PolyLine(true,
                new Vector(3, 52),
                new Vector(57, 58),
                new Vector(66, 4),
                new Vector(21, 24),
                new Vector(31, 34),
                new Vector(15, 30)
            );
            var pCrv = pLine.ToPolyCurve();
            var offsetCrv = pCrv.Offset(16.0);
            var polygon = pLine.Vertices.ExtractPoints();
                Assert.AreEqual(null, offsetCrv);
            
        }

        [TestMethod]
        public void PolyCurveOffsetWithChink_ShouldBeLargestLoopInsideInitial()
        {
            var pLine = new PolyLine(true,
                new Vector(7, 63),
                new Vector(45, 74),
                new Vector(83, 12),
                new Vector(31, 19),
                new Vector(35, 35),
                new Vector(21, 30),
                new Vector(7, 63)
            );
            var pCrv = pLine.ToPolyCurve();
            var offsetCrv = pCrv.Offset(16.0);
            var polygon = pLine.Vertices.ExtractPoints();
            double area = offsetCrv.CalculateEnclosedArea();
            Assert.AreEqual(22.858, area, 0.01);
            foreach (var v in offsetCrv.Vertices)
            {
                bool inside = polygon.PolygonContainmentXY(v.Position);
                Assert.AreEqual(true, inside);
            }
        }

        [TestMethod]
        public void PolyCurveOffsetsWithChink_ShouldBeInsideInitial()
        {
            var pLine = new PolyLine(true,
                new Vector(7, 63),
                new Vector(45, 74),
                new Vector(83, 12),
                new Vector(31, 19),
                new Vector(35, 35),
                new Vector(21, 30),
                new Vector(7, 63)
            );
            var pCrv = pLine.ToPolyCurve();
            var offsetCrv = pCrv.Offset(16.0, false);
            var offsetCrvs = offsetCrv.SelfIntersectionXYLoopsAlignedWith(pCrv);
            var polygon = pLine.Vertices.ExtractPoints();
            double area = offsetCrvs.Sum(i => i.CalculateEnclosedArea().Abs());
            Assert.AreEqual(23.305, area, 0.01);
            foreach (var subCrv in offsetCrvs)
            {
                foreach (var v in subCrv.Vertices)
                {
                    bool inside = polygon.PolygonContainmentXY(v.Position);
                    Assert.AreEqual(true, inside);
                }
            }
        }

        [TestMethod]
        public void PolyCurveOffsetWithChinkFailsIfOutsideInitial2()
        {
            var pLine = new PolyLine(true,
                new Vector(15, 56),
                new Vector(72, 36),
                new Vector(72, 29),
                new Vector(15, 12),
                new Vector(11, 31),
                new Vector(2, 13),
                new Vector(-5, 24),
                new Vector(15, 56)
            );
            var pCrv = pLine.ToPolyCurve();
            PolyCurve offsetCrv = pCrv.Offset(16.0, true) as PolyCurve;
           
            var polygon = pLine.Vertices.ExtractPoints();
            foreach (var v in offsetCrv.Vertices)
            {
                bool inside = polygon.PolygonContainmentXY(v.Position);
                Assert.AreEqual(true, inside);
            }
        }

        [TestMethod]
        public void PolyCurveOffsetHourglassFailsIfNotNull()
        {
            var pLine = new PolyLine(true,
                new Vector(16.0990763671286, 15.9585221874058),
                new Vector(34, 16),
                new Vector(59.4083740969327, 14.9302153041428),
                new Vector(62.8591637874557, 3),
                new Vector(36, 8),
                new Vector(13, 2)
            );
            var pCrv = pLine.ToPolyCurve();
            var offsetCrv = pCrv.Offset(16.0);
            Assert.AreEqual(null, offsetCrv);

        }

        [TestMethod]
        public void PolyCurveTriangleDoubleInversion()
        {
            var pLine = new PolyLine(true,
                new Vector(0, 40),
                new Vector(0, 0),
                new Vector(15, 0));
            var pCrv = pLine.ToPolyCurve();
            var offsetCrv = pCrv.Offset(-16.0);
            Assert.AreEqual(null, offsetCrv);
        }

        [TestMethod]
        public void PolyCurveLongBigOffsetFailsIfNonNull()
        {
            var pLine = new PolyLine(true,
                new Vector(1.7499999521533, 507.454619162483),
                new Vector(5.15000009874348, 508.154619199398),
                new Vector(7.40000009885989, 508.354619197809),
                new Vector(8.75000009848736, 508.154619199689),
                new Vector(14.2000000986736, 507.604619200109),
                new Vector(14.3500000990462, 502.104619199003),
                new Vector(14.3500000990462, 501.154619200301),
                new Vector(14.3000000992324, 500.254619198502),
                new Vector(14.1500000988599, 499.304619199102),
                new Vector(14.0000000984874, 498.704619200405),
                new Vector(13.8000000992324, 498.154619198496),
                new Vector(13.5000000984874, 497.604619197606),
                new Vector(12.3000000992324, 489.404619198292),
                new Vector(12.6000000990462, 475.054619199684),
                new Vector(13.1500000988599, 460.50461919891),
                new Vector(13.4000000988599, 453.304619198694),
                new Vector(14.6500000988599, 423.754619198386),
                new Vector(14.9500000986736, 417.054619200004),
                new Vector(15.1000000990462, 412.654619198496),
                new Vector(15.3750000984874, 406.006619198393),
                new Vector(15.6000000990462, 400.554619200004),
                new Vector(15.9000000988599, 395.654619198496),
                new Vector(16.5500000992324, 381.404619199195),
                new Vector(16.7550000993069, 377.306619199808),
                new Vector(16.8500000990462, 375.4046191987),
                new Vector(17.5850000984501, 360.088619200105),
                new Vector(17.7500000984874, 356.654619199107),
                new Vector(18.1500000988599, 349.754619199288),
                new Vector(18.4500000986736, 343.154619198292),
                new Vector(19.5500000992324, 324.754619198007),
                new Vector(22.8500000990462, 324.654619198409),
                new Vector(24.7500000986038, 304.154619198787),
                new Vector(25.2500000986038, 298.804619199509),
                new Vector(25.7500000986038, 293.504619198997),
                new Vector(28.2500000986038, 268.254619196698),
                new Vector(32.5500000992324, 269.154619198205),
                new Vector(32.5506826576311, 269.15151518269),
                new Vector(32.5470000000205, 269.15061964991),
                new Vector(35.4490000000224, 255.804619649891),
                new Vector(41.6500000000233, 227.152619649889),
                new Vector(45.7970000000205, 208.808619649906),
                new Vector(45.9085290530929, 208.32683423601),
                new Vector(46.7270000985591, 204.7236191993),
                new Vector(49.0230000986485, 194.620619198598),
                new Vector(52.6219974710839, 178.778347043088),
                new Vector(52.8960000000661, 177.556619649898),
                new Vector(59.8887317285407, 153.716291950899),
                new Vector(60.5500000994653, 151.354619197402),
                new Vector(61.6156187858433, 148.344246409601),
                new Vector(63.8675508400192, 141.516690057586),
                new Vector(63.9500000989065, 141.254619198589),
                new Vector(67.9000000990927, 129.004619197804),
                new Vector(68.6500000990927, 126.604619198886),
                new Vector(69.4500000989065, 124.40461919771),
                new Vector(71.9000000990927, 117.254619197687),
                new Vector(72.2229622092564, 116.362273020408),
                new Vector(79.6020000000717, 95.9106196498906),
                new Vector(79.9167521734489, 95.1042868777004),
                new Vector(80.5500000994653, 93.3546191978094),
                new Vector(84.4836397521431, 83.404824782192),
                new Vector(84.6944128863979, 82.8648657286831),
                new Vector(96.9891140619293, 49.818273304787),
                new Vector(97.0069999999832, 49.7536196498841),
                new Vector(97.044548136997, 49.6692735640099),
                new Vector(97.0499997407896, 49.6546203467005),
                new Vector(100.700000099023, 41.4546191981062),
                new Vector(101.799292669981, 39.1432861002977),
                new Vector(106.545999999973, 29.149619649892),
                new Vector(110.414809201262, 21.3492204244831),
                new Vector(114.200000099023, 13.7046191999107),
                new Vector(118.98800009978, 3.70161919848761),
                new Vector(119.800000099698, 2.00461919829831),
                new Vector(120.08437491348, 1.4149596579955),
                new Vector(120.126000000047, 1.32761964990641),
                new Vector(109.522355277557, 0),
                new Vector(101.072000000044, 17.4476196499018),
                new Vector(93.737000000081, 32.585619649908),
                new Vector(85.4450000000652, 50.9566196498927),
                new Vector(81.0189999999711, 62.3516196498822),
                new Vector(75.8310000000056, 75.4816196498869),
                new Vector(68.5219999999972, 93.9276196498831),
                new Vector(60.2760000000708, 116.049619649886),
                new Vector(56.737000000081, 126.780619649886),
                new Vector(57.6720000000205, 127.107619649905),
                new Vector(57.7330000000075, 127.128619649884),
                new Vector(57.3080000000773, 128.405619649886),
                new Vector(56.314000000013, 128.062619649892),
                new Vector(55.2560000000522, 131.271619649895),
                new Vector(59.6040000000503, 132.528619649907),
                new Vector(58.1500000000233, 138.654619649897),
                new Vector(54.0500000000466, 156.304619649891),
                new Vector(53.4729999999981, 158.687619649892),
                new Vector(46.0690000000177, 157.299619649886),
                new Vector(43.4570000000531, 166.754619649902),
                new Vector(37.3570000000764, 189.088619649905),
                new Vector(31.4440000000177, 210.952619649906),
                new Vector(24.8379999999888, 236.739619649888),
                new Vector(20.7920000000158, 254.703619649896),
                new Vector(17.6970000000438, 269.519619649887),
                new Vector(13.5019999999786, 290.664619649906),
                new Vector(10.9000000000233, 309.754619649902),
                new Vector(10.5999999999767, 312.054619649891),
                new Vector(10.3000000000466, 314.404619649897),
                new Vector(10, 316.704619649885),
                new Vector(9.34999999997672, 322.454619649885),
                new Vector(8.75, 328.154619649897),
                new Vector(8.09999999997672, 333.904619649897),
                new Vector(7.5, 339.104619649908),
                new Vector(6.90000000002328, 344.354619649908),
                new Vector(5.84999999997672, 353.154619649897),
                new Vector(5.15000000002328, 360.154619649897),
                new Vector(4.55000000004657, 365.904619649897),
                new Vector(4.05000000004657, 371.654619649897),
                new Vector(3.5, 377.404619649897),
                new Vector(3.0899999999674, 382.0646196499),
                new Vector(2.74600000004284, 385.933619649906),
                new Vector(2.5, 388.704619649885),
                new Vector(2.36100000003353, 390.663619649888),
                new Vector(2.09999999997672, 394.354619649908),
                new Vector(1.65000000002328, 400.004619649902),
                new Vector(1.30000000004657, 405.654619649897),
                new Vector(1.09999999997672, 409.904619649897),
                new Vector(1, 411.304619649891),
                new Vector(0.800000000046566, 416.704619649885),
                new Vector(0.75, 419.404619649897),
                new Vector(0.599999999976717, 423.704619649885),
                new Vector(0.550000000046566, 427.954619649885),
                new Vector(0.450000000069849, 432.204619649885),
                new Vector(0.349999999976717, 438.004619649902),
                new Vector(0.300000000046566, 443.754619649902),
                new Vector(0.131999999983236, 457.605619649898),
                new Vector(0, 468.504619649902),
                new Vector(0.450000000069849, 494.204619649885),
                new Vector(1.7499999521533, 507.454619162483));
            var pCrv = pLine.ToPolyCurve();
            var offsetCrv = pCrv.Offset(16.0);

            Assert.AreEqual(null, offsetCrv);
        }

        [TestMethod]
        public void PolyCurveReduceSquareRemoveOneSegment()
        {
            var pLine = new PolyLine(true,
                new Vector(0, 0),
                new Vector(10, 0),
                new Vector(11, 5),
                new Vector(10, 10),
                new Vector(0, 10));
            var pCrv = pLine.ToPolyCurve();
            int reduced = pCrv.Reduce(2);

            Assert.AreEqual(1, reduced);
        }

        [TestMethod]
        public void PolyCurveReduceSquareSidedDoNotRemove()
        {
            var pLine = new PolyLine(true,
                new Vector(0, 0),
                new Vector(10, 0),
                new Vector(11, 5),
                new Vector(10, 10),
                new Vector(0, 10));
            var pCrv = pLine.ToPolyCurve();
            int reduced = pCrv.Reduce(new Interval(-2,0));

            Assert.AreEqual(0, reduced);
        }

        [TestMethod]
        public void PolyCurveReduceComplexShape()
        {
            var pLine = new PolyLine(true,
                new Vector(1.7499999521533, 507.454619162483),
                new Vector(5.15000009874348, 508.154619199398),
                new Vector(7.40000009885989, 508.354619197809),
                new Vector(8.75000009848736, 508.154619199689),
                new Vector(14.2000000986736, 507.604619200109),
                new Vector(14.3500000990462, 502.104619199003),
                new Vector(14.3500000990462, 501.154619200301),
                new Vector(14.3000000992324, 500.254619198502),
                new Vector(14.1500000988599, 499.304619199102),
                new Vector(14.0000000984874, 498.704619200405),
                new Vector(13.8000000992324, 498.154619198496),
                new Vector(13.5000000984874, 497.604619197606),
                new Vector(12.3000000992324, 489.404619198292),
                new Vector(12.6000000990462, 475.054619199684),
                new Vector(13.1500000988599, 460.50461919891),
                new Vector(13.4000000988599, 453.304619198694),
                new Vector(14.6500000988599, 423.754619198386),
                new Vector(14.9500000986736, 417.054619200004),
                new Vector(15.1000000990462, 412.654619198496),
                new Vector(15.3750000984874, 406.006619198393),
                new Vector(15.6000000990462, 400.554619200004),
                new Vector(15.9000000988599, 395.654619198496),
                new Vector(16.5500000992324, 381.404619199195),
                new Vector(16.7550000993069, 377.306619199808),
                new Vector(16.8500000990462, 375.4046191987),
                new Vector(17.5850000984501, 360.088619200105),
                new Vector(17.7500000984874, 356.654619199107),
                new Vector(18.1500000988599, 349.754619199288),
                new Vector(18.4500000986736, 343.154619198292),
                new Vector(19.5500000992324, 324.754619198007),
                new Vector(22.8500000990462, 324.654619198409),
                new Vector(24.7500000986038, 304.154619198787),
                new Vector(25.2500000986038, 298.804619199509),
                new Vector(25.7500000986038, 293.504619198997),
                new Vector(28.2500000986038, 268.254619196698),
                new Vector(32.5500000992324, 269.154619198205),
                new Vector(32.5506826576311, 269.15151518269),
                new Vector(32.5470000000205, 269.15061964991),
                new Vector(35.4490000000224, 255.804619649891),
                new Vector(41.6500000000233, 227.152619649889),
                new Vector(45.7970000000205, 208.808619649906),
                new Vector(45.9085290530929, 208.32683423601),
                new Vector(46.7270000985591, 204.7236191993),
                new Vector(49.0230000986485, 194.620619198598),
                new Vector(52.6219974710839, 178.778347043088),
                new Vector(52.8960000000661, 177.556619649898),
                new Vector(59.8887317285407, 153.716291950899),
                new Vector(60.5500000994653, 151.354619197402),
                new Vector(61.6156187858433, 148.344246409601),
                new Vector(63.8675508400192, 141.516690057586),
                new Vector(63.9500000989065, 141.254619198589),
                new Vector(67.9000000990927, 129.004619197804),
                new Vector(68.6500000990927, 126.604619198886),
                new Vector(69.4500000989065, 124.40461919771),
                new Vector(71.9000000990927, 117.254619197687),
                new Vector(72.2229622092564, 116.362273020408),
                new Vector(79.6020000000717, 95.9106196498906),
                new Vector(79.9167521734489, 95.1042868777004),
                new Vector(80.5500000994653, 93.3546191978094),
                new Vector(84.4836397521431, 83.404824782192),
                new Vector(84.6944128863979, 82.8648657286831),
                new Vector(96.9891140619293, 49.818273304787),
                new Vector(97.0069999999832, 49.7536196498841),
                new Vector(97.044548136997, 49.6692735640099),
                new Vector(97.0499997407896, 49.6546203467005),
                new Vector(100.700000099023, 41.4546191981062),
                new Vector(101.799292669981, 39.1432861002977),
                new Vector(106.545999999973, 29.149619649892),
                new Vector(110.414809201262, 21.3492204244831),
                new Vector(114.200000099023, 13.7046191999107),
                new Vector(118.98800009978, 3.70161919848761),
                new Vector(119.800000099698, 2.00461919829831),
                new Vector(120.08437491348, 1.4149596579955),
                new Vector(120.126000000047, 1.32761964990641),
                new Vector(109.522355277557, 0),
                new Vector(101.072000000044, 17.4476196499018),
                new Vector(93.737000000081, 32.585619649908),
                new Vector(85.4450000000652, 50.9566196498927),
                new Vector(81.0189999999711, 62.3516196498822),
                new Vector(75.8310000000056, 75.4816196498869),
                new Vector(68.5219999999972, 93.9276196498831),
                new Vector(60.2760000000708, 116.049619649886),
                new Vector(56.737000000081, 126.780619649886),
                new Vector(57.6720000000205, 127.107619649905),
                new Vector(57.7330000000075, 127.128619649884),
                new Vector(57.3080000000773, 128.405619649886),
                new Vector(56.314000000013, 128.062619649892),
                new Vector(55.2560000000522, 131.271619649895),
                new Vector(59.6040000000503, 132.528619649907),
                new Vector(58.1500000000233, 138.654619649897),
                new Vector(54.0500000000466, 156.304619649891),
                new Vector(53.4729999999981, 158.687619649892),
                new Vector(46.0690000000177, 157.299619649886),
                new Vector(43.4570000000531, 166.754619649902),
                new Vector(37.3570000000764, 189.088619649905),
                new Vector(31.4440000000177, 210.952619649906),
                new Vector(24.8379999999888, 236.739619649888),
                new Vector(20.7920000000158, 254.703619649896),
                new Vector(17.6970000000438, 269.519619649887),
                new Vector(13.5019999999786, 290.664619649906),
                new Vector(10.9000000000233, 309.754619649902),
                new Vector(10.5999999999767, 312.054619649891),
                new Vector(10.3000000000466, 314.404619649897),
                new Vector(10, 316.704619649885),
                new Vector(9.34999999997672, 322.454619649885),
                new Vector(8.75, 328.154619649897),
                new Vector(8.09999999997672, 333.904619649897),
                new Vector(7.5, 339.104619649908),
                new Vector(6.90000000002328, 344.354619649908),
                new Vector(5.84999999997672, 353.154619649897),
                new Vector(5.15000000002328, 360.154619649897),
                new Vector(4.55000000004657, 365.904619649897),
                new Vector(4.05000000004657, 371.654619649897),
                new Vector(3.5, 377.404619649897),
                new Vector(3.0899999999674, 382.0646196499),
                new Vector(2.74600000004284, 385.933619649906),
                new Vector(2.5, 388.704619649885),
                new Vector(2.36100000003353, 390.663619649888),
                new Vector(2.09999999997672, 394.354619649908),
                new Vector(1.65000000002328, 400.004619649902),
                new Vector(1.30000000004657, 405.654619649897),
                new Vector(1.09999999997672, 409.904619649897),
                new Vector(1, 411.304619649891),
                new Vector(0.800000000046566, 416.704619649885),
                new Vector(0.75, 419.404619649897),
                new Vector(0.599999999976717, 423.704619649885),
                new Vector(0.550000000046566, 427.954619649885),
                new Vector(0.450000000069849, 432.204619649885),
                new Vector(0.349999999976717, 438.004619649902),
                new Vector(0.300000000046566, 443.754619649902),
                new Vector(0.131999999983236, 457.605619649898),
                new Vector(0, 468.504619649902),
                new Vector(0.450000000069849, 494.204619649885),
                new Vector(1.7499999521533, 507.454619162483));
            var pCrv = pLine.ToPolyCurve();
            int reduced = pCrv.ReduceInside(2);

            Assert.AreEqual(91, reduced);
        }

        [TestMethod]
        public void PolyLineReduceComplexShape()
        {
            var pLine = new PolyLine(true,
                new Vector(1.7499999521533, 507.454619162483),
                new Vector(5.15000009874348, 508.154619199398),
                new Vector(7.40000009885989, 508.354619197809),
                new Vector(8.75000009848736, 508.154619199689),
                new Vector(14.2000000986736, 507.604619200109),
                new Vector(14.3500000990462, 502.104619199003),
                new Vector(14.3500000990462, 501.154619200301),
                new Vector(14.3000000992324, 500.254619198502),
                new Vector(14.1500000988599, 499.304619199102),
                new Vector(14.0000000984874, 498.704619200405),
                new Vector(13.8000000992324, 498.154619198496),
                new Vector(13.5000000984874, 497.604619197606),
                new Vector(12.3000000992324, 489.404619198292),
                new Vector(12.6000000990462, 475.054619199684),
                new Vector(13.1500000988599, 460.50461919891),
                new Vector(13.4000000988599, 453.304619198694),
                new Vector(14.6500000988599, 423.754619198386),
                new Vector(14.9500000986736, 417.054619200004),
                new Vector(15.1000000990462, 412.654619198496),
                new Vector(15.3750000984874, 406.006619198393),
                new Vector(15.6000000990462, 400.554619200004),
                new Vector(15.9000000988599, 395.654619198496),
                new Vector(16.5500000992324, 381.404619199195),
                new Vector(16.7550000993069, 377.306619199808),
                new Vector(16.8500000990462, 375.4046191987),
                new Vector(17.5850000984501, 360.088619200105),
                new Vector(17.7500000984874, 356.654619199107),
                new Vector(18.1500000988599, 349.754619199288),
                new Vector(18.4500000986736, 343.154619198292),
                new Vector(19.5500000992324, 324.754619198007),
                new Vector(22.8500000990462, 324.654619198409),
                new Vector(24.7500000986038, 304.154619198787),
                new Vector(25.2500000986038, 298.804619199509),
                new Vector(25.7500000986038, 293.504619198997),
                new Vector(28.2500000986038, 268.254619196698),
                new Vector(32.5500000992324, 269.154619198205),
                new Vector(32.5506826576311, 269.15151518269),
                new Vector(32.5470000000205, 269.15061964991),
                new Vector(35.4490000000224, 255.804619649891),
                new Vector(41.6500000000233, 227.152619649889),
                new Vector(45.7970000000205, 208.808619649906),
                new Vector(45.9085290530929, 208.32683423601),
                new Vector(46.7270000985591, 204.7236191993),
                new Vector(49.0230000986485, 194.620619198598),
                new Vector(52.6219974710839, 178.778347043088),
                new Vector(52.8960000000661, 177.556619649898),
                new Vector(59.8887317285407, 153.716291950899),
                new Vector(60.5500000994653, 151.354619197402),
                new Vector(61.6156187858433, 148.344246409601),
                new Vector(63.8675508400192, 141.516690057586),
                new Vector(63.9500000989065, 141.254619198589),
                new Vector(67.9000000990927, 129.004619197804),
                new Vector(68.6500000990927, 126.604619198886),
                new Vector(69.4500000989065, 124.40461919771),
                new Vector(71.9000000990927, 117.254619197687),
                new Vector(72.2229622092564, 116.362273020408),
                new Vector(79.6020000000717, 95.9106196498906),
                new Vector(79.9167521734489, 95.1042868777004),
                new Vector(80.5500000994653, 93.3546191978094),
                new Vector(84.4836397521431, 83.404824782192),
                new Vector(84.6944128863979, 82.8648657286831),
                new Vector(96.9891140619293, 49.818273304787),
                new Vector(97.0069999999832, 49.7536196498841),
                new Vector(97.044548136997, 49.6692735640099),
                new Vector(97.0499997407896, 49.6546203467005),
                new Vector(100.700000099023, 41.4546191981062),
                new Vector(101.799292669981, 39.1432861002977),
                new Vector(106.545999999973, 29.149619649892),
                new Vector(110.414809201262, 21.3492204244831),
                new Vector(114.200000099023, 13.7046191999107),
                new Vector(118.98800009978, 3.70161919848761),
                new Vector(119.800000099698, 2.00461919829831),
                new Vector(120.08437491348, 1.4149596579955),
                new Vector(120.126000000047, 1.32761964990641),
                new Vector(109.522355277557, 0),
                new Vector(101.072000000044, 17.4476196499018),
                new Vector(93.737000000081, 32.585619649908),
                new Vector(85.4450000000652, 50.9566196498927),
                new Vector(81.0189999999711, 62.3516196498822),
                new Vector(75.8310000000056, 75.4816196498869),
                new Vector(68.5219999999972, 93.9276196498831),
                new Vector(60.2760000000708, 116.049619649886),
                new Vector(56.737000000081, 126.780619649886),
                new Vector(57.6720000000205, 127.107619649905),
                new Vector(57.7330000000075, 127.128619649884),
                new Vector(57.3080000000773, 128.405619649886),
                new Vector(56.314000000013, 128.062619649892),
                new Vector(55.2560000000522, 131.271619649895),
                new Vector(59.6040000000503, 132.528619649907),
                new Vector(58.1500000000233, 138.654619649897),
                new Vector(54.0500000000466, 156.304619649891),
                new Vector(53.4729999999981, 158.687619649892),
                new Vector(46.0690000000177, 157.299619649886),
                new Vector(43.4570000000531, 166.754619649902),
                new Vector(37.3570000000764, 189.088619649905),
                new Vector(31.4440000000177, 210.952619649906),
                new Vector(24.8379999999888, 236.739619649888),
                new Vector(20.7920000000158, 254.703619649896),
                new Vector(17.6970000000438, 269.519619649887),
                new Vector(13.5019999999786, 290.664619649906),
                new Vector(10.9000000000233, 309.754619649902),
                new Vector(10.5999999999767, 312.054619649891),
                new Vector(10.3000000000466, 314.404619649897),
                new Vector(10, 316.704619649885),
                new Vector(9.34999999997672, 322.454619649885),
                new Vector(8.75, 328.154619649897),
                new Vector(8.09999999997672, 333.904619649897),
                new Vector(7.5, 339.104619649908),
                new Vector(6.90000000002328, 344.354619649908),
                new Vector(5.84999999997672, 353.154619649897),
                new Vector(5.15000000002328, 360.154619649897),
                new Vector(4.55000000004657, 365.904619649897),
                new Vector(4.05000000004657, 371.654619649897),
                new Vector(3.5, 377.404619649897),
                new Vector(3.0899999999674, 382.0646196499),
                new Vector(2.74600000004284, 385.933619649906),
                new Vector(2.5, 388.704619649885),
                new Vector(2.36100000003353, 390.663619649888),
                new Vector(2.09999999997672, 394.354619649908),
                new Vector(1.65000000002328, 400.004619649902),
                new Vector(1.30000000004657, 405.654619649897),
                new Vector(1.09999999997672, 409.904619649897),
                new Vector(1, 411.304619649891),
                new Vector(0.800000000046566, 416.704619649885),
                new Vector(0.75, 419.404619649897),
                new Vector(0.599999999976717, 423.704619649885),
                new Vector(0.550000000046566, 427.954619649885),
                new Vector(0.450000000069849, 432.204619649885),
                new Vector(0.349999999976717, 438.004619649902),
                new Vector(0.300000000046566, 443.754619649902),
                new Vector(0.131999999983236, 457.605619649898),
                new Vector(0, 468.504619649902),
                new Vector(0.450000000069849, 494.204619649885),
                new Vector(1.7499999521533, 507.454619162483));

            int reduced = pLine.ReduceInside(2);

            Assert.AreEqual(91, reduced);
        }

        [TestMethod]
        public void ShortestPath_Line()
        {
            var line = new Line(0, 0, 10, 0);
            var path = line.ShortestPath(0.25, 0.75);
            Assert.AreEqual(new Interval(0.25, 0.75), path);
        }

        [TestMethod]
        public void ShortestPath_CircleWrap()
        {
            var circ = new Arc(new Circle(10));
            var path = circ.ShortestPath(0.1, 0.9);
            Assert.AreEqual(new Interval(0.9, 0.1), path);
        }

        [TestMethod]
        public void ShortestPath_CircleIntervalWrap()
        {
            var circ = new Arc(new Circle(10));
            var path = circ.ShortestPath(0.1, new Interval(0.5, 0.9));
            Assert.AreEqual(new Interval(0.9, 0.1), path);
        }

        [TestMethod]
        public void ShortestPath_CircleIntervalNotWrap()
        {
            var circ = new Arc(new Circle(10));
            var path = circ.ShortestPath(0.1, new Interval(0.2, 0.9));
            Assert.AreEqual(new Interval(0.1, 0.2), path);
        }

        [TestMethod]
        public void Reduce_PolylineStartEndFlat_ShouldNotCrash()
        {
            var pline = new PolyLine(Vector.Create2D(
                5, 0,
                10, 0,
                10, 10,
                0, 10,
                0, 0), true);
            int reduced = pline.Reduce(1);
            Assert.AreEqual(1, reduced);
        }

        [TestMethod]
        public void ContiguousEdges_PolylineSquareWithExtraVerts_ShouldReturn4Sides()
        {
            var pline = new PolyLine(Vector.Create2D(
                5, 0,
                10, 0,
                10, 10,
                0, 10,
                0, 0), true);
            var edges = pline.ContinuousSubDomains(Angle.FromDegrees(1));
            Assert.AreEqual(4, edges.Count);
        }

        [TestMethod]
        public void ContiguousEdges_PolylineSquareWithExtraVertsHighTolerance_ShouldReturn1Side()
        {
            var pline = new PolyLine(Vector.Create2D(
                5, 0,
                10, 0,
                10, 10,
                0, 10,
                0, 0), true);
            var edges = pline.ContinuousSubDomains(Angle.FromDegrees(91));
            Assert.AreEqual(1, edges.Count);
        }

        [TestMethod]
        public void ContiguousEdges_CircleArc_ShouldReturn1Side()
        {
            var circ = new Arc(new Circle(5));
            var edges = circ.ContinuousSubDomains(Angle.FromDegrees(1));
            Assert.AreEqual(1, edges.Count);
            Assert.AreEqual(new Interval(0, 1), edges[0]);
        }

        [TestMethod]
        public void Offset_PolylineWithKinkAndShortSegment()
        {
            var pline = new PolyLine(Vector.Create2D(
                0, 0,
                5, 0,
                6, 0,
                10, 4));
            var offs1 = pline.Offset(3);
            var offs2 = pline.Offset(-3);
            //TODO: Check lengths
        }

        [TestMethod]
        public void Offset_PolyCurveWithKinkAndShortSegment()
        {
            var pline = new PolyLine(Vector.Create2D(
                0, 0,
                5, 0,
                6, 0,
                10, 4)).ToPolyCurve(true);
            var offs1 = pline.Offset(3);
            var offs2 = pline.Offset(-3);
            double length1 = offs1.Length;
            double length2 = offs2.Length;
            Assert.AreEqual(14.1421, length1, 0.0001);
            Assert.AreEqual(9.17157, length2, 0.0001);
        }

        [TestMethod]
        public void Offset_PolyCurveWithKinkAndShortSegment2()
        {
            var pline = new PolyLine(
                new Vector(58.201900000684, 83.1562000009872, 0),
                new Vector(57.6990000000224, 65.5513000003994, 0),
                new Vector(54.854400000535, 65.6013000010862, 0),
                new Vector(54.1088444438218, 36.5608046584474, 0)
                ).ToPolyCurve(true);
            var offs1 = pline.Offset(16);
            
            double length1 = offs1.Length;
            
            Assert.AreEqual(49.59881, length1, 0.0001);
            
        }

        [TestMethod]
        public void Offset_PolyCurveWithKinkAndShortSegment3()
        {
            var pline = new PolyLine(
                new Vector(18.434077003462, 24.6821823006518, 0),
                new Vector(20.2657449943471, 39.8535286418336, 0),
                new Vector(19.9433512320896, 40.934691052402, 0)
                ).ToPolyCurve(true);
            var offs1 = pline.Offset(-8, new CurveOffsetParameters(true,true,false));

            double length1 = offs1.Length;

            Assert.AreEqual(13.6283689, length1, 0.0001);

        }

        [TestMethod]
        public void VariableOffset_PolylineWithKinkAndShortSegment()
        {
            var pline = new PolyLine(Vector.Create2D(
                0, 0,
                5, 0,
                6, 0,
                10, 4));
            var offs1 = pline.Offset(new double[] { 3, 0, 3 });
            var offs2 = pline.Offset(-3);
            // TODO: Check lengths
        }

        [TestMethod]
        public void Line_CachedDataUpdatedOnChange()
        {
            var line = new Line(0, 0, 10, 5);
            var bBox = line.BoundingBox;
            Assert.AreEqual(10, bBox.SizeX);
            Assert.AreEqual(5, bBox.SizeY);
            Assert.AreEqual(11.18033, line.Length, 0.0001);

            line.End.Position = new Vector(8, 10);
            var bBox2 = line.BoundingBox;
            Assert.AreEqual(8, bBox2.SizeX);
            Assert.AreEqual(10, bBox2.SizeY);
            Assert.AreEqual(12.80624, line.Length, 0.0001);
        }

        [TestMethod]
        public void PolyCurve_CachedDataUpdatedOnChange()
        {
            var pCrv = new PolyCurve(new Line(0, 0, 10, 5));
            var bBox = pCrv.BoundingBox;
            Assert.AreEqual(10, bBox.SizeX);
            Assert.AreEqual(5, bBox.SizeY);
            Assert.AreEqual(11.18033, pCrv.Length, 0.0001);

            pCrv.AddLine(new Vector(10, 10));
            var bBox2 = pCrv.BoundingBox;
            Assert.AreEqual(10, bBox2.SizeX);
            Assert.AreEqual(10, bBox2.SizeY);
            Assert.AreEqual(16.18033, pCrv.Length, 0.0001);
        }

        [TestMethod]
        public void SmallTriangle_OffsetShouldBeNull()
        {
            var pLine = new PolyLine(true,
                new Vector(134.151583327481, 3.97679246051294, 0),
                new Vector(135.867899311823, 2.08059785037767, 0),
                new Vector(132.551152617239, 2.18588798217887, 0));
            var pCrv = pLine.ToPolyCurve(true);
            var cOP = new CurveOffsetParameters()
            {
                //CollapseInvertedSegments = true
            };
            var offset = pCrv.Offset(16, cOP);
            Assert.AreEqual(null, offset);
        }

        [TestMethod]
        public void Projection_2Lines_ShouldMapToEquivalentDomain()
        {
            var l1 = new Line(0, 0, 10, 0);
            var l2 = new Line(1, 1, 4, 1);
            Interval projection = l1.ProjectionOf(l2);
            Assert.AreEqual(new Interval(0.1, 0.4), projection);
        }

        [TestMethod]
        public void Projection_LineOntoPolyLine_ShouldMapToEquivalentDomain()
        {
            var l1 = new PolyLine(Vector.Create2D(0, 0, 10, 0, 10, 1));
            var l2 = new Line(1, 1, 4, 1);
            Interval projection = l1.ProjectionOf(l2);
            Assert.AreEqual(new Interval(0.05, 0.2), projection);
        }

        [TestMethod]
        public void Projection_LineOntoPolyLineClosed_ShouldMapToWrappingDomain()
        {
            var l1 = new PolyLine(true, Vector.Create2D(5, 0, 10, 0, 10, 10, 0, 10, 0,0));
            var l2 = new Line(1, 0, 9, 0);
            Interval projection = l1.ProjectionOf(l2);
            Assert.AreEqual(0.84, projection.Start, 0.000001);
            Assert.AreEqual(0.16, projection.End, 0.0000001);
        }

        [TestMethod]
        public void TrimShortEndCurves_Recursive_ShouldRunWithoutNullReferenceException()
        {
            var poly = new PolyLine(false, Vector.Create2D(
                211.49032676342119, 268.80234289863893,
                218.46615310393312, 256.00881070963936,
                222.87921107027509, 256.36447725350456,
                222.87085974340667, 256.34361844231978));
            var pCrv = poly.ToPolyCurve();
            while (pCrv.TrimShortEndCurves(16, Angle.FromDegrees(45), true, true, 16 / 3)) { }
        }
    }
}
