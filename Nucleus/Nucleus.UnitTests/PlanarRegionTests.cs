using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class PlanarRegionTests
    {
        [TestMethod]
        public void SquareRegionOrthogonalSplitByLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var perimeterMappers = new List<CurveParameterMapper>();
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(0, 1), 1, perimeterMappers);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(2, subRegions.Count);
            Assert.AreEqual(90, area, 0.0001);
            Assert.AreEqual(2, perimeterMappers.Count);
            Assert.AreEqual(subRegions[0].Perimeter.SegmentCount, perimeterMappers[0].SpanDomains.Count);
            Assert.AreEqual(subRegions[1].Perimeter.SegmentCount, perimeterMappers[1].SpanDomains.Count);
            double tA = 0.9;
            Vector ptA = region.Perimeter.PointAt(tA);
            double tB = perimeterMappers[0].MapAtoB(tA);
            Vector ptB = subRegions[0].Perimeter.PointAt(tB);
            Assert.AreEqual(ptA, ptB);
            var intervalsB = perimeterMappers[0].MapAtoB(new Interval(0, 1));
            Assert.AreEqual(2, intervalsB.Count);
        }

        [TestMethod]
        public void SquareRegionOrthogonalSplitByZeroThicknessLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(0, 1));
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(100, area, 0.0001);
        }

        [TestMethod]
        public void SquareRegionReversedOrthogonalSplitByLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 0, 10, 10, 10, 10, 0));
            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(0, 1), 1);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(90, area, 0.0001);
        }

        [TestMethod]
        public void SquareRegionObliqueSplitByLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine.ToPolyCurve());
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(Angle.FromDegrees(10)), 1);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(89.845, area, 0.001);
        }

        [TestMethod]
        public void SquareRegionDiagonalSplitByLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine.ToPolyCurve());
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(Angle.FromDegrees(45)), 1);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(86.357, area, 0.001);
        }

        [TestMethod]
        public void ORegionOrthogonalSplitByLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var vLine = new PolyLine(true,
                Vector.Create2D(1, 1, 9, 1, 9, 9, 1, 9));
            region.Voids.Add(vLine);
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(0, 1), 1);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(34, area, 0.0001);
        }

        [TestMethod]
        public void ORegionOrthogonalSplitByZeroThicknessLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var vLine = new PolyLine(true,
                Vector.Create2D(1, 1, 9, 1, 9, 9, 1, 9));
            region.Voids.Add(vLine);
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(0, 1), 0);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(36, area, 0.0001);
        }

        [TestMethod]
        public void ORegionOrthogonalSplitByLineReversedVoid()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var vLine = new PolyLine(true,
                Vector.Create2D(1, 1, 9, 1, 9, 9, 1, 9));
            vLine.Reverse();
            region.Voids.Add(vLine);
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(0, 1), 1);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(34, area, 0.0001);
        }

        [TestMethod]
        public void ORegionDiagonalSplitByLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var vLine = new PolyLine(true,
                Vector.Create2D(1, 1, 9, 1, 9, 9, 1, 9));
            region.Voids.Add(vLine);
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(Angle.FromDegrees(45)), 1);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(33.1715, area, 0.0001);
        }

        [TestMethod]
        public void ORegionDiagonalSplitByZeroThicknessLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var vLine = new PolyLine(true,
                Vector.Create2D(1, 1, 9, 1, 9, 9, 1, 9));
            region.Voids.Add(vLine);
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(Angle.FromDegrees(45)), 0);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(36, area, 0.0001);
        }

        [TestMethod]
        public void Figure8RegionOrthogonalSplitByLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var vLine = new PolyLine(true,
                Vector.Create2D(1, 1, 9, 1, 9, 4, 1, 4));
            var vLine2 = new PolyLine(true,
                Vector.Create2D(1, 6, 9, 6, 9, 9, 1, 9));
            region.Voids.Add(vLine);
            region.Voids.Add(vLine2);
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(0, 1), 1);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(48, area, 0.0001);
        }

        [TestMethod]
        public void Figure8RegionOrthogonalSplitByHorizontalLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var vLine = new PolyLine(true,
                Vector.Create2D(1, 1, 9, 1, 9, 4, 1, 4));
            var vLine2 = new PolyLine(true,
                Vector.Create2D(1, 6, 9, 6, 9, 9, 1, 9));
            region.Voids.Add(vLine);
            region.Voids.Add(vLine2);
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(1, 0), 1);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(42, area, 0.0001);
        }

        [TestMethod]
        public void Figure8RegionOrthogonalSplitByHorizontalLine2()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var vLine = new PolyLine(true,
                Vector.Create2D(1, 1, 9, 1, 9, 4, 1, 4));
            var vLine2 = new PolyLine(true,
                Vector.Create2D(1, 6, 9, 6, 9, 9, 1, 9));
            region.Voids.Add(vLine);
            region.Voids.Add(vLine2);
            var subRegions = region.SplitByLineXY(new Vector(5, 7.5), new Vector(1, 0), 1);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(50, area, 0.0001);
        }

        [TestMethod]
        public void CRegionOrthogonalSplitByLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10,1, 1,1, 1,9, 10, 9, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(0, 1), 1);
            Assert.AreEqual(3, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(26, area, 0.0001);
        }

        [TestMethod]
        public void SRegionOrthogonalSplitByLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0,0, 0,1, 9,1, 9,4.5, 0,4.5, 0,10, 10,10, 10,9, 1,9, 1,5.5, 10,5.5, 10,0));
            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(0, 1), 1);
            Assert.AreEqual(4, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(34, area, 0.0001);
        }

        [TestMethod]
        public void ComplexBoundary_ShouldSplitIn2()
        {
            var pLine = new PolyLine(true,
                new Vector(8.4500001053093, 30.1999999962864, 0),
                new Vector(15.5000001061708, 33.5999999966007, 0),
                new Vector(18.3500001057982, 34.9499999948894, 0),
                new Vector(19.650000105612, 35.5999999962805, 0),
                new Vector(36.2000001054257, 43.1999999960826, 0),
                new Vector(51.2000001055421, 49.7499999945867, 0),
                new Vector(58.3000001061009, 52.8999999939988, 0),
                new Vector(63.5000001062872, 55.2499999945867, 0),
                new Vector(72.1500001058448, 59.1499999944936, 0),
                new Vector(78.950000105775, 62.2499999939755, 0),
                new Vector(79.6000001060311, 62.5499999930908, 0),
                new Vector(89.1000001061475, 67.1999999945983, 0),
                new Vector(89.8000001063338, 67.549999994575, 0),
                new Vector(91.3500001061475, 68.2999999937019, 0),
                new Vector(116.200000105891, 79.6499999935913, 0),
                new Vector(116.700000105891, 78.5999999940977, 0),
                new Vector(133.900000106078, 85.6499999933003, 0),
                new Vector(139.200000105891, 87.8499999953783, 0),
                new Vector(141.800000106567, 88.9499999942782, 0),
                new Vector(142.150000106078, 89.0999999940977, 0),
                new Vector(144.35000010638, 90.0499999937019, 0),
                new Vector(146.950000106008, 91.1999999939871, 0),
                new Vector(148.900000106194, 92.0999999946798, 0),
                new Vector(150.700000106008, 92.9499999943946, 0),
                new Vector(152.85000010638, 93.9999999938882, 0),
                new Vector(157.60000010638, 96.2499999940919, 0),
                new Vector(158.900000106194, 96.8999999941734, 0),
                new Vector(160.700000106008, 97.7999999946915, 0),
                new Vector(162.250000106753, 98.6499999933003, 0),
                new Vector(182.40000010631, 110.249999994296, 0),
                new Vector(190.65000010631, 115.6499999933, 0),
                new Vector(209.750000106986, 127.399999993999, 0),
                new Vector(212.750000106869, 129.2999999944, 0),
                new Vector(214.400000106427, 130.349999993894, 0),
                new Vector(217.700000106241, 132.499999992287, 0),
                new Vector(222.600000106613, 135.599999995495, 0),
                new Vector(226.700000106241, 137.649999993475, 0),
                new Vector(240.700000106357, 144.649999992776, 0),
                new Vector(278.450000106473, 161.699999993376, 0),
                new Vector(281.40000010666, 162.94999999358, 0),
                new Vector(285.500000107335, 164.4499999939, 0),
                new Vector(311.300000107149, 172.049999991985, 0),
                new Vector(318.750000107451, 174.349999993981, 0),
                new Vector(333.900000106893, 178.699999993201, 0),
                new Vector(341.700000106706, 180.749999991589, 0),
                new Vector(366.500000107568, 186.34999999369, 0),
                new Vector(424.050000107614, 192.199999993376, 0),
                new Vector(445.400000107358, 191.899999991991, 0),
                new Vector(454.600000107544, 190.799999992392, 0),
                new Vector(473.000000108033, 188.849999994476, 0),
                new Vector(500.75000010815, 184.29999999149, 0),
                new Vector(509.80000010808, 182.849999992002, 0),
                new Vector(518.950000107405, 181.449999993376, 0),
                new Vector(528.850000107894, 179.949999991775, 0),
                new Vector(537.800000108196, 178.949999991601, 0),
                new Vector(547.150000107824, 178.049999991694, 0),
                new Vector(556.85000010801, 177.549999992974, 0),
                new Vector(571.90000010794, 176.999999991094, 0),
                new Vector(580.100000108127, 176.949999991688, 0),
                new Vector(580.15000010794, 176.949999992095, 0),
                new Vector(596.550000108313, 177.249999990396, 0),
                new Vector(602.150000108057, 177.499999991182, 0),
                new Vector(605.45000010787, 177.699999991892, 0),
                new Vector(614.250000108616, 178.299999990995, 0),
                new Vector(624.000000108732, 179.149999990477, 0),
                new Vector(636.400000108173, 180.699999991193, 0),
                new Vector(651.450000107987, 183.299999990792, 0),
                new Vector(665.000000108848, 185.999999989988, 0),
                new Vector(686.40000010829, 191.399999990274, 0),
                new Vector(697.100000108592, 194.199999990175, 0),
                new Vector(708.150000108522, 197.499999989377, 0),
                new Vector(719.750000109081, 201.349999991391, 0),
                new Vector(731.150000108639, 205.999999989086, 0),
                new Vector(731.666901817889, 206.221902238052, 0),
                new Vector(737.582641279906, 200.141269437363, 0),
                new Vector(728, 196.150000461494, 0),
                new Vector(694.79999999993, 184.950000461482, 0),
                new Vector(685.349999999977, 182.750000461499, 0),
                new Vector(641.650000000023, 173.400000461494, 0),
                new Vector(634.349999999977, 172.500000461499, 0),
                new Vector(634.650000000023, 169.800000461488, 0),
                new Vector(635.400000000023, 163.400000461494, 0),
                new Vector(640.900000000023, 164.050000461488, 0),
                new Vector(652.29999999993, 165.900000461494, 0),
                new Vector(662.599999999977, 167.900000461494, 0),
                new Vector(671.349999999977, 169.750000461499, 0),
                new Vector(680, 171.900000461494, 0),
                new Vector(700.099999999977, 177.050000461488, 0),
                new Vector(704.650000000023, 178.300000461488, 0),
                new Vector(708.75, 179.450000461482, 0),
                new Vector(720.099999999977, 183.500000461499, 0),
                new Vector(736.5, 189.600000461476, 0),
                new Vector(744.631066502227, 192.896378773189, 0),
                new Vector(749.525982221959, 187.865023813573, 0),
                new Vector(749.150000108639, 187.699999990087, 0),
                new Vector(719.900000108522, 175.249999989494, 0),
                new Vector(717.650000108522, 174.349999991275, 0),
                new Vector(696.900000108406, 167.14999999039, 0),
                new Vector(678.100000108476, 162.499999989988, 0),
                new Vector(636.100000108359, 157.349999991478, 0),
                new Vector(626.600000108243, 157.399999992602, 0),
                new Vector(622.850000108243, 157.399999990885, 0),
                new Vector(596.500000108499, 157.349999992002, 0),
                new Vector(596.450000107754, 150.799999991374, 0),
                new Vector(564.450000107638, 153.249999991094, 0),
                new Vector(537.05000010808, 158.049999991694, 0),
                new Vector(533.350000107894, 158.549999990879, 0),
                new Vector(524.750000108266, 159.499999992491, 0),
                new Vector(518.850000107777, 160.049999991781, 0),
                new Vector(490.300000107964, 162.649999991001, 0),
                new Vector(483.150000107475, 163.099999993196, 0),
                new Vector(481.850000107777, 163.149999991583, 0),
                new Vector(478.300000107847, 162.999999990192, 0),
                new Vector(457.000000108033, 161.949999992881, 0),
                new Vector(429.850000107544, 160.499999993393, 0),
                new Vector(409.950000107056, 159.099999992584, 0),
                new Vector(394.300000107498, 157.34999999369, 0),
                new Vector(383.900000107125, 155.749999991094, 0),
                new Vector(372.350000107312, 153.449999993289, 0),
                new Vector(351.200000106823, 148.599999992497, 0),
                new Vector(329.450000106706, 142.649999992689, 0),
                new Vector(307.650000106776, 136.299999993382, 0),
                new Vector(289.100000106962, 130.099999992992, 0),
                new Vector(285.700000106473, 128.949999993289, 0),
                new Vector(280.15000010666, 127.099999994185, 0),
                new Vector(278.250000107219, 126.399999993475, 0),
                new Vector(275.950000106473, 125.549999993091, 0),
                new Vector(271.40000010666, 123.749999993481, 0),
                new Vector(262.400000106543, 120.149999992398, 0),
                new Vector(259.550000106916, 118.999999992899, 0),
                new Vector(247.650000106543, 114.299999993702, 0),
                new Vector(244.900000106543, 113.1499999933, 0),
                new Vector(242.200000106357, 111.999999992579, 0),
                new Vector(235.60000010673, 108.949999994598, 0),
                new Vector(231.750000106986, 107.099999994301, 0),
                new Vector(223.750000106986, 103.249999993277, 0),
                new Vector(209.950000106241, 96.6499999939988, 0),
                new Vector(201.500000106869, 92.5499999932945, 0),
                new Vector(199.050000106799, 91.3499999953783, 0),
                new Vector(197.238000106765, 90.4169999947771, 0),
                new Vector(192.40000010631, 87.999999994383, 0),
                new Vector(185.600000106497, 84.4499999938998, 0),
                new Vector(183.300000106683, 83.2499999933934, 0),
                new Vector(179.950000106124, 81.4499999948021, 0),
                new Vector(176.65000010631, 79.6499999933003, 0),
                new Vector(173.300000106567, 77.749999992986, 0),
                new Vector(168.550000106683, 74.9999999935972, 0),
                new Vector(163.750000106753, 72.1999999949767, 0),
                new Vector(159.050000106567, 69.3999999930966, 0),
                new Vector(148.700000106008, 63.1999999945983, 0),
                new Vector(117.750000106636, 44.5999999946798, 0),
                new Vector(113.950000105775, 50.6999999952968, 0),
                new Vector(109.75000010652, 48.549999994284, 0),
                new Vector(95.9000001059612, 41.2499999944994, 0),
                new Vector(85.2000001056585, 35.5999999966007, 0),
                new Vector(51.1000001059147, 19.2999999951862, 0),
                new Vector(44.1000001059147, 16.4499999959953, 0),
                new Vector(35.4500001055421, 12.9999999947904, 0),
                new Vector(29.0000001062872, 10.4999999948777, 0),
                new Vector(22.6000001057982, 8.04999999457505, 0),
                new Vector(4.05000010598451, 1.39999999367865, 0),
                new Vector(0, 4.614994396E-07, 0),
                new Vector(3, 1.00000046149944, 0),
                new Vector(4.04999999993015, 1.40000046149362, 0),
                new Vector(9.15000000002328, 3.35000046147616, 0),
                new Vector(10.8499999999767, 4.00000046149944, 0),
                new Vector(9.75, 6.90000046149362, 0),
                new Vector(17.5, 9.65000046149362, 0),
                new Vector(21.1500000000233, 10.950000461482, 0),
                new Vector(24.75, 12.3000004614878, 0),
                new Vector(28.4000000000233, 13.700000461482, 0),
                new Vector(30.5499999999302, 14.5500004614878, 0),
                new Vector(32.75, 15.450000461482, 0),
                new Vector(38.3499999999767, 17.7500004614994, 0),
                new Vector(41.8499999999767, 19.200000461482, 0),
                new Vector(51.5, 23.2500004614994, 0),
                new Vector(54.5499999999302, 24.6000004614762, 0),
                new Vector(57.25, 25.8500004614762, 0),
                new Vector(59.9000000000233, 27.1000004614762, 0),
                new Vector(62.5999999999767, 28.4000004614936, 0),
                new Vector(70.3499999999767, 32.1500004614936, 0),
                new Vector(72.3499999999767, 33.1500004614936, 0),
                new Vector(77.9000000000233, 35.9000004614936, 0),
                new Vector(84.5499999999302, 39.200000461482, 0),
                new Vector(87.5499999999302, 40.6500004614936, 0),
                new Vector(89.0999999999767, 41.4000004614936, 0),
                new Vector(100.699999999953, 47.4000004614936, 0),
                new Vector(106.449999999953, 50.450000461482, 0),
                new Vector(111.199999999953, 53.0000004614994, 0),
                new Vector(137.599999999977, 67.5000004614994, 0),
                new Vector(166.349999999977, 83.2500004614994, 0),
                new Vector(211.29999999993, 106.600000461476, 0),
                new Vector(241.75, 120.650000461494, 0),
                new Vector(283.79999999993, 136.300000461488, 0),
                new Vector(325.29999999993, 148.200000461482, 0),
                new Vector(365, 156.850000461476, 0),
                new Vector(404.199999999953, 162.350000461476, 0),
                new Vector(446.04999999993, 165.800000461488, 0),
                new Vector(457.75, 166.100000461476, 0),
                new Vector(494.449999999953, 165.100000461476, 0),
                new Vector(502.79999999993, 164.600000461476, 0),
                new Vector(537.349999999977, 162.600000461476, 0),
                new Vector(545.25, 162.150000461494, 0),
                new Vector(551.449999999953, 161.850000461476, 0),
                new Vector(566.54999999993, 161.100000461476, 0),
                new Vector(572.849999999977, 160.900000461494, 0),
                new Vector(578.54999999993, 160.850000461476, 0),
                new Vector(597.849999999977, 161.000000461499, 0),
                new Vector(610.900000000023, 161.500000461499, 0),
                new Vector(626, 162.200000461482, 0),
                new Vector(624.849999999977, 171.700000461482, 0),
                new Vector(597.25, 169.800000461488, 0),
                new Vector(544.29999999993, 171.250000461499, 0),
                new Vector(505.29999999993, 173.900000461494, 0),
                new Vector(495.349999999977, 174.500000461499, 0),
                new Vector(469.5, 175.450000461482, 0),
                new Vector(461.79999999993, 175.450000461482, 0),
                new Vector(451.949999999953, 175.350000461476, 0),
                new Vector(444.099999999977, 175.100000461476, 0),
                new Vector(436.199999999953, 174.750000461499, 0),
                new Vector(430.29999999993, 174.400000461494, 0),
                new Vector(422.849999999977, 173.850000461476, 0),
                new Vector(416.400000000023, 173.300000461488, 0),
                new Vector(400.699999999953, 171.600000461476, 0),
                new Vector(394.099999999977, 170.800000461488, 0),
                new Vector(384.25, 169.450000461482, 0),
                new Vector(378.099999999977, 168.550000461488, 0),
                new Vector(367.349999999977, 166.550000461488, 0),
                new Vector(345.5, 162.450000461482, 0),
                new Vector(345.025999999954, 162.345000461501, 0),
                new Vector(331.54999999993, 159.350000461476, 0),
                new Vector(320.650000000023, 156.650000461494, 0),
                new Vector(306.29999999993, 152.900000461494, 0),
                new Vector(301.599999999977, 151.450000461482, 0),
                new Vector(289.099999999977, 147.450000461482, 0),
                new Vector(274.949999999953, 142.750000461499, 0),
                new Vector(269.949999999953, 141.000000461499, 0),
                new Vector(257, 136.300000461488, 0),
                new Vector(253, 134.750000461499, 0),
                new Vector(248.449999999953, 133.050000461488, 0),
                new Vector(245, 131.700000461482, 0),
                new Vector(239.04999999993, 129.400000461494, 0),
                new Vector(236.949999999953, 128.550000461488, 0),
                new Vector(234.75, 127.600000461476, 0),
                new Vector(232.599999999977, 126.650000461494, 0),
                new Vector(224.099999999977, 122.800000461488, 0),
                new Vector(220.900000000023, 121.350000461476, 0),
                new Vector(211.5, 116.950000461482, 0),
                new Vector(206.79999999993, 114.700000461482, 0),
                new Vector(200.849999999977, 111.800000461488, 0),
                new Vector(195, 108.850000461476, 0),
                new Vector(189.099999999977, 105.850000461476, 0),
                new Vector(186.599999999977, 104.500000461499, 0),
                new Vector(113.650000000023, 69.0500004614878, 0),
                new Vector(89.0999999999767, 58.0500004614878, 0),
                new Vector(85.5499999999302, 56.450000461482, 0),
                new Vector(49.5, 40.1500004614936, 0),
                new Vector(45.2999999999302, 38.2500004614994, 0),
                new Vector(11.7999999999302, 24.0000004614994, 0),
                new Vector(6.84999999997672, 21.950000461482, 0),
                new Vector(4.40000000002328, 20.9000004614936, 0),
                new Vector(3.44999999995343, 23.3200004614773, 0),
                new Vector(2.01000000000931, 27.0000004614994, 0));

            var region = new PlanarRegion(pLine.ToPolyCurve(true));
            var subRegions = region.SplitByLineXY(
                new Vector(-123.37641004297012, 263.609708579005), 
                new Vector(0.465240060345707, -0.88518454926050483), 18);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(12945.3, area, 0.1);
        }

        [TestMethod]
        public void ComplexBoundary_ShouldTrimEndsOnly()
        {
            var pLine = new PolyLine(true,
                new Vector(0, 139.307499328599, 0),
                new Vector(7.4499998484971, 154.057499021292, 0),
                new Vector(18.8499996801838, 143.15749960969, 0),
                new Vector(47.7499996802071, 115.40749960969, 0),
                new Vector(77.1999996801605, 87.7074996097072, 0),
                new Vector(128.549999680137, 41.5074996096955, 0),
                new Vector(150.199999680161, 22.0074996096955, 0),
                new Vector(156.099999680184, 28.5574996096839, 0),
                new Vector(156.049999680137, 29.1074996097013, 0),
                new Vector(147.449999680161, 36.8574996097013, 0),
                new Vector(131.199999680161, 51.3074996096839, 0),
                new Vector(117.749999680207, 63.1574996096897, 0),
                new Vector(116.39999968023, 63.6074996097013, 0),
                new Vector(44.9999996802071, 129.007499609696, 0),
                new Vector(11.2540615192847, 161.565228386782, 0),
                new Vector(14.849999848986, 168.407499021181, 0),
                new Vector(28.9499996801605, 155.757499609696, 0),
                new Vector(72.9499996801605, 116.40749960969, 0),
                new Vector(124.299999680137, 70.9074996096897, 0),
                new Vector(143.70199984964, 54.3904990181036, 0),
                new Vector(169.199999849428, 29.6574990189984, 0),
                new Vector(160.711999849533, 20.414499019389, 0),
                new Vector(157.399999849615, 16.8074990198947, 0),
                new Vector(156.186999680474, 15.7564996095025, 0),
                new Vector(146.941950025503, 5.45014939660905, 0),
                new Vector(140.788524305681, 0, 0),
                new Vector(119.209999849903, 24.4044990199036, 0),
                new Vector(115.899999849382, 28.0574990207097, 0),
                new Vector(92.5499998495216, 53.3574990197958, 0),
                new Vector(68.7999998494051, 79.0074990209832, 0),
                new Vector(62.4499998487299, 85.8574990209891, 0),
                new Vector(55.749999849475, 91.6574990217923, 0),
                new Vector(40.8499998491025, 104.407499023189, 0),
                new Vector(23.099999848986, 119.607499021688, 0),
                new Vector(11.1999998484971, 129.757499022293, 0));

            var region = new PlanarRegion(pLine.ToPolyCurve(true));
            var subRegions = region.SplitByLineXY(
                new Vector(55.0030483159521, 193.04815785657905),
                new Vector(-0.7829015687268509, -0.6221455888174694), 18);
            Assert.AreEqual(1, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(4980.7, area, 0.1);
        }


        /*
        [TestMethod]
        public void ComplexBoundaryWithZeroThicknessBit_ShouldSelectCorrectInside()
        {
            // This is probably impossible - the zero thickness bit leads to degeneracy.
            // Approach should probably instead be to tidy up boundary first to remove such
            // areas somehow.
            var pLine = new PolyLine(true,
                new Vector(1342.95000029169, 535.849999316008, 0),
                new Vector(1342.80000021821, 535.799999287497, 0),
                new Vector(1342.40000021784, 535.649999287503, 0),
                new Vector(1342.20000021765, 535.449999286706, 0),
                new Vector(1342.05000021809, 535.199999287317, 0),
                new Vector(1342.05000021809, 534.849999288504, 0),
                new Vector(1342.25000021828, 534.249999288324, 0),
                new Vector(1342.40000021772, 533.849999288417, 0),
                new Vector(1344.45887594169, 529.279295180721, 0),
                new Vector(1048.35000021593, 395.349999294325, 0),
                new Vector(860.700000214274, 312.249999298219, 0),
                new Vector(625.258704324369, 212.441947530722, 0),
                new Vector(628.27200021327, 184.995999300503, 0),
                new Vector(592.046000213246, 169.436999302125, 0),
                new Vector(581.829000212834, 192.253999300214, 0),
                new Vector(517.619000212522, 165.478999302519, 0),
                new Vector(517.276904204628, 165.570747579302, 0),
                new Vector(497.600000212085, 156.599999304308, 0),
                new Vector(491.037104174262, 153.580647589406, 0),
                new Vector(477.950000211596, 147.049999304319, 0),
                new Vector(432.150000211434, 124.19999930411, 0),
                new Vector(405.700000211014, 113.29999930592, 0),
                new Vector(373.900000210968, 100.449999306409, 0),
                new Vector(348.650000210851, 92.6499993050238, 0),
                new Vector(334.600000210921, 88.2499993074161, 0),
                new Vector(303.150000210502, 78.4999993094243, 0),
                new Vector(272.600000210456, 69.5499993078993, 0),
                new Vector(265.300000210642, 67.8499993078003, 0),
                new Vector(260.950000210083, 66.7499993085221, 0),
                new Vector(219.068000210566, 56.689999309514, 0),
                new Vector(197.200000209617, 51.5499993088015, 0),
                new Vector(159.800000209943, 43.7499993109086, 0),
                new Vector(152.140000209678, 42.1499993098259, 0),
                new Vector(143.650000209454, 40.4499993099016, 0),
                new Vector(134.500000210013, 38.5999993104197, 0),
                new Vector(124.500000209897, 36.6999993119971, 0),
                new Vector(108.600000209408, 33.7499993118108, 0),
                new Vector(93.7000002089189, 31.0999993113219, 0),
                new Vector(89.750000209664, 30.3499993120204, 0),
                new Vector(70.8500002090586, 26.6999993116187, 0),
                new Vector(37.5500002091285, 20.8499993128062, 0),
                new Vector(33.9500002084533, 20.1999993123172, 0),
                new Vector(10.9000002085231, 15.9499993126083, 0),
                new Vector(11.2000002083369, 13.999999313819, 0),
                new Vector(3.75000020908192, 12.7499993131205, 0),
                new Vector(2.089655027E-07, 12.0999993136211, 0),
                new Vector(1.94999999995343, 0, 0),
                new Vector(1.19999999995343, 4.75, 0),
                new Vector(0, 12.1000000000058, 0),
                new Vector(11.1999999999534, 14, 0),
                new Vector(10.9000000000233, 15.9500000000116, 0),
                new Vector(10.1999999999534, 15.8000000000175, 0),
                new Vector(8.94999999995343, 23.8500000000058, 0),
                new Vector(10.1999999999534, 24.0500000000175, 0),
                new Vector(9.79999999993015, 27.1500000000233, 0),
                new Vector(18.7999999999302, 28.7000000000116, 0),
                new Vector(18.9499999999534, 28, 0),
                new Vector(49.1500000000233, 33.2000000000116, 0),
                new Vector(51.0999999999767, 33.5500000000175, 0),
                new Vector(52.0499999999302, 35.5, 0),
                new Vector(73.9499999999534, 39.6000000000058, 0),
                new Vector(75.2999999999302, 41.3500000000058, 0),
                new Vector(85.8499999999767, 43.25, 0),
                new Vector(104.900000000023, 46.9500000000116, 0),
                new Vector(109.5, 47.9500000000116, 0),
                new Vector(127.650000000023, 51.8000000000175, 0),
                new Vector(131.75, 52.75, 0),
                new Vector(131.54999999993, 53.8000000000175, 0),
                new Vector(134.349999999977, 54.3000000000175, 0),
                new Vector(136.650000000023, 54.8000000000175, 0),
                new Vector(138.900000000023, 55.4000000000233, 0),
                new Vector(139.900000000023, 50.9500000000116, 0),
                new Vector(147, 52.4500000000116, 0),
                new Vector(145.949999999953, 57.1000000000058, 0),
                new Vector(150.349999999977, 58.1000000000058, 0),
                new Vector(170.039999999921, 62.7900000000081, 0),
                new Vector(173.449999999953, 63.6000000000058, 0),
                new Vector(191.5, 67.9000000000233, 0),
                new Vector(214, 73.5500000000175, 0),
                new Vector(232.400000000023, 78.3500000000058, 0),
                new Vector(249.25, 82.8500000000058, 0),
                new Vector(273.349999999977, 89.5500000000175, 0),
                new Vector(277.660000000033, 73.3099999999977, 0),
                new Vector(281.79999999993, 74.25, 0),
                new Vector(277.04999999993, 90.6500000000233, 0),
                new Vector(289.75, 94.3000000000175, 0),
                new Vector(323.900000000023, 104.450000000012, 0),
                new Vector(341.949999999953, 110.100000000006, 0),
                new Vector(366.25, 117.950000000012, 0),
                new Vector(382.150000000023, 123.150000000023, 0),
                new Vector(417.150000000023, 135.25, 0),
                new Vector(439.75, 143.150000000023, 0),
                new Vector(467.900000000023, 153.600000000006, 0),
                new Vector(492.79999999993, 163, 0),
                new Vector(521.25, 174.550000000017, 0),
                new Vector(561.650000000023, 191.200000000012, 0),
                new Vector(578, 198, 0),
                new Vector(581.54999999993, 198.700000000012, 0),
                new Vector(582.25, 198.850000000006, 0),
                new Vector(582.5, 198.950000000012, 0),
                new Vector(588.04999999993, 201.300000000017, 0),
                new Vector(605.150000000023, 208.700000000012, 0),
                new Vector(604.5, 210.050000000017, 0),
                new Vector(606.849999999977, 211.050000000017, 0),
                new Vector(669.25, 238.150000000023, 0),
                new Vector(684.699999999953, 245, 0),
                new Vector(738.54999999993, 268.650000000023, 0),
                new Vector(782.599999999977, 288.25, 0),
                new Vector(991.949999999953, 380.850000000006, 0),
                new Vector(1098.29999999993, 427.25, 0),
                new Vector(1149.29999999993, 449.950000000012, 0),
                new Vector(1227.09999999998, 484.300000000017, 0),
                new Vector(1342.04999999993, 534.850000000006, 0));
            //var crv = pLine.SelfIntersectionXYLoops().ItemWithMax(i => i.Length);
            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY(
                new Vector(2.005957934999123, 33.496535005768457),
                new Vector(-0.17125088863668844, -0.98522745249061372), 18);
            Assert.AreEqual(1, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(4980.7, area, 0.1);
        }
        */

        [TestMethod]
        public void ComplexBoundaryWithLake_ShouldKeepLakeOutsideBoundary()
        {
            var pLine = new PolyLine(true,
                new Vector(145.059000000008, 5.34200000000419, 0),
                new Vector(143.459999999963, 14.8730000000214, 0),
                new Vector(140.418999999994, 23.0570000000007, 0),
                new Vector(138.71100000001, 25.9159999999974, 0),
                new Vector(134.574999999953, 29.6750000000175, 0),
                new Vector(126.179999999935, 35.3330000000133, 0),
                new Vector(124.233000000007, 37.6380000000063, 0),
                new Vector(123.247999999905, 40.8120000000054, 0),
                new Vector(123.623999999953, 45.6270000000077, 0),
                new Vector(125.917999999947, 49.1510000000126, 0),
                new Vector(128.768999999971, 50.6020000000135, 0),
                new Vector(127.358000000007, 53.9500000000116, 0),
                new Vector(124.897999999928, 53.1860000000161, 0),
                new Vector(121.733000000007, 52.1630000000005, 0),
                new Vector(116.684000000008, 49.9710000000196, 0),
                new Vector(86.3329999999842, 43.4940000000061, 0),
                new Vector(82.8379999999888, 42.2960000000021, 0),
                new Vector(80.5009999999311, 37.8920000000217, 0),
                new Vector(77.734999999986, 31.6200000000244, 0),
                new Vector(76.8199999999488, 28.1560000000172, 0),
                new Vector(77.3249999999534, 26.073000000004, 0),
                new Vector(78.7579999999143, 25.1469999999972, 0),
                new Vector(89.6339999999618, 23.7229999999981, 0),
                new Vector(93.6059999999125, 23.8179999999993, 0),
                new Vector(97.9149999999208, 25.7410000000091, 0),
                new Vector(99.7609999999404, 23.2280000000028, 0),
                new Vector(100.660999999964, 22.9220000000205, 0),
                new Vector(102.076999999932, 23.0330000000249, 0),
                new Vector(107.693999999901, 26.070000000007, 0),
                new Vector(114.473999999929, 31.4630000000179, 0),
                new Vector(120.826000000001, 32.707000000024, 0),
                new Vector(125.354999999981, 31.8540000000212, 0),
                new Vector(135.626999999979, 24.0299999999988, 0),
                new Vector(138.484999999986, 18.9800000000105, 0),
                new Vector(141.344999999972, 4.46700000000419, 0),
                new Vector(115.149999999907, 0.5, 0),
                new Vector(90.9499999999534, 0.5, 0),
                new Vector(76.3999999999069, 2, 0),
                new Vector(51.8999999999069, 9.60000000000582, 0),
                new Vector(34.3499999999767, 18.8500000000058, 0),
                new Vector(21.6499999999069, 27.8500000000058, 0),
                new Vector(0, 46.7000000000116, 0),
                new Vector(68.7999999999302, 58.7000000000116, 0),
                new Vector(129.54999999993, 69.4500000000116, 0),
                new Vector(174.54999999993, 77.8500000000058, 0),
                new Vector(176.25, 78.2000000000116, 0),
                new Vector(211.099999999977, 85.0500000000175, 0),
                new Vector(212.25, 85.3000000000175, 0),
                new Vector(213.75, 85.6500000000233, 0),
                new Vector(214.149999999907, 85.75, 0),
                new Vector(214.649999999907, 85.9000000000233, 0),
                new Vector(215, 86.0500000000175, 0),
                new Vector(216.099999999977, 86.6000000000058, 0),
                new Vector(231.949999999953, 88.8000000000175, 0),
                new Vector(234.949999999953, 89.3500000000058, 0),
                new Vector(242.349999999977, 91.1500000000233, 0),
                new Vector(246.75, 92.25, 0),
                new Vector(249.54999999993, 93.1000000000058, 0),
                new Vector(252.449999999953, 94.1000000000058, 0),
                new Vector(259.04999999993, 96.3500000000058, 0),
                new Vector(262.75, 97.7000000000116, 0),
                new Vector(268.199999999953, 99.9500000000116, 0),
                new Vector(273.5, 102.400000000023, 0),
                new Vector(277.79999999993, 104.450000000012, 0),
                new Vector(282.099999999977, 106.550000000017, 0),
                new Vector(286.199999999953, 108.700000000012, 0),
                new Vector(291.54999999993, 111.550000000017, 0),
                new Vector(292.79999999993, 112.300000000017, 0),
                new Vector(294.849999999977, 113.550000000017, 0),
                new Vector(304.25, 119.700000000012, 0),
                new Vector(307, 121.5, 0),
                new Vector(308.149999999907, 122.350000000006, 0),
                new Vector(309.75, 122.950000000012, 0),
                new Vector(289.949999999953, 105, 0),
                new Vector(279.949999999953, 95.8500000000058, 0),
                new Vector(276.5, 92.5, 0),
                new Vector(272.199999999953, 88.1500000000233, 0),
                new Vector(242.04999999993, 57.6500000000233, 0),
                new Vector(240.899999999907, 58.5500000000175, 0),
                new Vector(238.75, 56.1000000000058, 0),
                new Vector(239.79999999993, 54.9500000000116, 0),
                new Vector(232.5, 48.4000000000233, 0),
                new Vector(223.949999999953, 40.75, 0),
                new Vector(202.75, 26.3000000000175, 0),
                new Vector(184.54999999993, 17.6500000000233, 0),
                new Vector(156.04999999993, 8.20000000001164, 0));
            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY(
                new Vector(125.6969755805265, 59.612600404736696),
                new Vector(-0.33332211131568723, 0.94281300908931709), 18);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(11326.0122, area, 0.01);
        }

        [TestMethod]
        public void ComplexBoundary_ShouldSplitIn3()
        {
            var pLine = new PolyLine(true,
                new Vector(231.921691365074, 10.2502823234245, 0),
                new Vector(224.570241658366, 18.7503450158983, 0),
                new Vector(201.520923045697, 45.5995304487005, 0),
                new Vector(152.120290430845, 0.0002926206216216, 0),
                new Vector(144.52023702662, 8.90035038851784, 0),
                new Vector(141.720242438721, 12.1503441054083, 0),
                new Vector(130.670236064936, 25.1503515269142, 0),
                new Vector(115.120237381081, 43.4503499767161, 0),
                new Vector(99.8702360175084, 61.4503513450036, 0),
                new Vector(96.2702315043425, 65.7503568811226, 0),
                new Vector(75.3202350912616, 90.4503526734188, 0),
                new Vector(70.4202339590993, 96.2503540063044, 0),
                new Vector(64.3202349547064, 103.500352778006, 0),
                new Vector(57.8702330791857, 111.100355004019, 0),
                new Vector(53.5202331149485, 116.250354985008, 0),
                new Vector(51.7702376433881, 118.300349679019, 0),
                new Vector(49.6705376030877, 120.749999728025, 0),
                new Vector(48.8211278181989, 120.749999728723, 0),
                new Vector(40.5212622237159, 118.550035352499, 0),
                new Vector(24.6712777876528, 114.050039772614, 0),
                new Vector(17.7652607387863, 112.021034752019, 0),
                new Vector(13.5212748969207, 110.800038961112, 0),
                new Vector(6.62127737619448, 108.800039679103, 0),
                new Vector(3.72011432389263, 107.950531022216, 0),
                new Vector(0.320116349845193, 114.35052720891, 0),
                new Vector(0.0006431702058762, 114.941935320821, 0),
                new Vector(8.79061157105025, 118.271923349821, 0),
                new Vector(13.4605702250265, 120.4119044033, 0),
                new Vector(51.2408579711337, 135.311990403919, 0),
                new Vector(65.8810812399024, 134.63199990851, 0),
                new Vector(92.7698283432983, 134.544000736205, 0),
                new Vector(109.088665238232, 140.281943386013, 0),
                new Vector(113.020665383199, 141.651943444303, 0),
                new Vector(123.420661156182, 145.351941941801, 0),
                new Vector(181.321944089606, 165.751322441211, 0),
                new Vector(186.121381482459, 151.650076862221, 0),
                new Vector(161.039357044967, 141.217482636712, 0),
                new Vector(165.792817934067, 131.356601002422, 0),
                new Vector(172.867725074408, 124.686314212624, 0),
                new Vector(144.290722417645, 94.4813113463169, 0),
                new Vector(117.530315195443, 65.8042690530128, 0),
                new Vector(111.203200042131, 71.7083967229992, 0),
                new Vector(108.552277725656, 75.2126944480988, 0),
                new Vector(111.880676728324, 78.6630714256025, 0),
                new Vector(111.052870681509, 79.7305318165163, 0),
                new Vector(106.031258107512, 75.1563268364989, 0),
                new Vector(104.276929461397, 77.0825896861206, 0),
                new Vector(103.062762789428, 75.9833441396186, 0),
                new Vector(101.922256872174, 74.2510945737013, 0),
                new Vector(123.921821169322, 47.7015671601985, 0),
                new Vector(126.571933668922, 43.8513515673112, 0),
                new Vector(129.921884177835, 34.8515022403153, 0),
                new Vector(130.471739590983, 34.2016716457147, 0),
                new Vector(135.821705146343, 28.7517067342997, 0),
                new Vector(145.721679759095, 19.0017317367019, 0),
                new Vector(149.571577997645, 15.6018216041266, 0),
                new Vector(150.021401838167, 15.3519194705004, 0),
                new Vector(150.471159717301, 15.2020001774072, 0),
                new Vector(150.920935181901, 15.2020001774072, 0),
                new Vector(151.320756566594, 15.2519778505084, 0),
                new Vector(151.720552450861, 15.4019013071083, 0),
                new Vector(152.120421872009, 15.6518196952238, 0),
                new Vector(153.520346195437, 16.7517602351145, 0),
                new Vector(155.020299285185, 18.1517164522083, 0),
                new Vector(167.920376931783, 29.9517843775975, 0),
                new Vector(178.220375136123, 38.1017829568009, 0),
                new Vector(196.670559078106, 52.7518989761011, 0),
                new Vector(203.170530905481, 55.3219418403169, 0),
                new Vector(213.408293897519, 65.2987108743982, 0),
                new Vector(230.284400307108, 81.3598023200175, 0),
                new Vector(232.749396286672, 83.1947993270005, 0),
                new Vector(236.330707910121, 85.9169573465188, 0),
                new Vector(239.38927178178, 86.8419618096086, 0),
                new Vector(244.276978339651, 84.7131946901209, 0),
                new Vector(244.272043262376, 80.6516044525197, 0),
                new Vector(249.221644670353, 78.2502378833015, 0),
                new Vector(246.570228150696, 76.0003612764122, 0),
                new Vector(244.120870976127, 78.9495872620028, 0),
                new Vector(239.722293600556, 75.250783559808, 0),
                new Vector(240.021810190985, 74.751589242398, 0),
                new Vector(257.52108262456, 54.5524290791072, 0),
                new Vector(273.271694883471, 68.1517168169084, 0),
                new Vector(278.871992032859, 62.7008963303233, 0),
                new Vector(277.69102525292, 51.3902144942258, 0),
                new Vector(279.280489988974, 48.5791298929253, 0),
                new Vector(277.241932113422, 47.4253801193263, 0),
                new Vector(277.071572452085, 45.9701820327027, 0),
                new Vector(268.221596945776, 39.7501992476173, 0),
                new Vector(263.691259700805, 36.1443247477, 0),
                new Vector(257.507449233555, 42.9040244349162, 0),
                new Vector(256.704642515979, 40.2272355245077, 0),
                new Vector(255.000445853453, 38.7889135314035, 0),
                new Vector(260.121615074342, 33.3002137122094, 0),
                new Vector(254.520282257348, 28.900300889014, 0),
                new Vector(246.807949692942, 36.7876183098997, 0),
                new Vector(237.728034443804, 28.705000407499, 0),
                new Vector(235.995385999093, 28.6410133893078, 0),
                new Vector(229.599385547452, 23.1048785633175, 0),
                new Vector(228.007244480657, 21.0453417716199, 0),
                new Vector(227.66889935045, 21.4325922460121, 0),
                new Vector(226.072414108552, 20.0509032026166, 0),
                new Vector(233.418012812966, 11.6967241952079, 0));

            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY(
                new Vector(130.38896919418625, 126.20025374796298),
                new Vector(0.64622962731436839, -0.76314301987316413), 18);
            Assert.AreEqual(3, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(9807.8, area, 0.1);
        }


        [TestMethod]
        public void ComplexBoundary_ShouldClipEnds()
        {
            var pLine = new PolyLine(true,
                new Vector(24.7290336200967, 78.8801340963983, 0),
                new Vector(30.8683787853806, 77.895829197194, 0),
                new Vector(33.7527785858838, 90.3764215354749, 0),
                new Vector(35.6093509134371, 99.2301093977003, 0),
                new Vector(37.464023957029, 99.2765375713934, 0),
                new Vector(42.7223053296329, 120.493917656597, 0),
                new Vector(57.9019800415263, 175.133630697499, 0),
                new Vector(65.0951099477243, 173.020579179894, 0),
                new Vector(54.8581253110897, 136.152021875198, 0),
                new Vector(52.7624376197928, 127.968419032288, 0),
                new Vector(51.0118708640221, 121.165392806375, 0),
                new Vector(48.6652229411993, 111.978120872285, 0),
                new Vector(44.4635411322815, 94.769859446591, 0),
                new Vector(39.8701226601843, 74.8968561618822, 0),
                new Vector(38.7072827247321, 69.3288777011912, 0),
                new Vector(43.0670962400036, 68.4183619508985, 0),
                new Vector(57.5392317097867, 128.558723448397, 0),
                new Vector(63.4655335008283, 127.937342812889, 0),
                new Vector(63.9795022227336, 126.739527560974, 0),
                new Vector(63.1555217928253, 124.139920417598, 0),
                new Vector(62.6624686680152, 122.217571009678, 0),
                new Vector(61.8182131795911, 118.840307322098, 0),
                new Vector(61.6987082734122, 118.266637653287, 0),
                new Vector(60.8345777661307, 113.800555166701, 0),
                new Vector(52.7857903999975, 74.3721858502831, 0),
                new Vector(52.4544232840999, 74.3167403848784, 0),
                new Vector(51.5757997208857, 69.9895657219749, 0),
                new Vector(55.0200317295967, 68.6780353010981, 0),
                new Vector(53.8668466148083, 62.470202887489, 0),
                new Vector(54.772953652835, 61.258099351573, 0),
                new Vector(53.4689525985159, 57.8368585938006, 0),
                new Vector(52.7379066505819, 57.8772591767774, 0),
                new Vector(51.4835441593314, 54.7467741138826, 0),
                new Vector(48.1730376338237, 55.3389823917823, 0),
                new Vector(47.4855656917207, 51.8968034611898, 0),
                new Vector(47.6311357569066, 49.5462745525001, 0),
                new Vector(46.9758032542886, 45.5986704659881, 0),
                new Vector(51.3697808611905, 44.1278263603745, 0),
                new Vector(50.894825359981, 41.8741047291842, 0),
                new Vector(48.8296968446812, 42.1722361338907, 0),
                new Vector(48.6202226823079, 41.2438890315825, 0),
                new Vector(47.5064391272026, 41.2294376022764, 0),
                new Vector(45.4264391271863, 32.3594376022811, 0),
                new Vector(43.1764391271863, 32.8094376022927, 0),
                new Vector(43.0764391272096, 32.6594376022986, 0),
                new Vector(43.0264391272212, 32.4094376022986, 0),
                new Vector(42.9764391272329, 32.1094376022811, 0),
                new Vector(43.0264391272212, 31.8094376022927, 0),
                new Vector(43.3264391272096, 31.0094376022753, 0),
                new Vector(41.2465979250846, 20.830824504781, 0),
                new Vector(39.0819275947288, 10.2513520612847, 0),
                new Vector(35.7256555557251, 0, 0),
                new Vector(34.265486885095, 0.329595504677854, 0),
                new Vector(29.815276803798, 1.80416797427461, 0),
                new Vector(37.9748938002158, 44.0883466652886, 0),
                new Vector(42.2264391272329, 43.1594376022986, 0),
                new Vector(47.6764391271863, 41.9594376022869, 0),
                new Vector(48.126439127198, 43.8594376022811, 0),
                new Vector(38.3690074870246, 46.0613358927949, 0),
                new Vector(42.1210380158154, 63.9197086240747, 0),
                new Vector(37.8170455162181, 65.094062919874, 0),
                new Vector(33.941945755214, 47.0275433248898, 0),
                new Vector(25.0621644306812, 49.0174001223932, 0),
                new Vector(25.4011074309237, 52.248900696286, 0),
                new Vector(18.6030689537292, 54.0555178678769, 0),
                new Vector(19.0737244160264, 58.7283864360943, 0),
                new Vector(12.3240466319257, 57.9702807645954, 0),
                new Vector(0.440159650635906, 60.7799754089792, 0),
                new Vector(0, 62.0329833556898, 0),
                new Vector(5.3264391272096, 60.9094376022986, 0),
                new Vector(9.97643912723288, 59.9594376022869, 0),
                new Vector(13.4264391271863, 60.9094376022986, 0),
                new Vector(17.876439127198, 60.8594376022811, 0),
                new Vector(18.5264391272212, 63.9594376022869, 0),
                new Vector(15.9764391272329, 64.3094376022927, 0),
                new Vector(18.3264391272096, 74.5594376022927, 0),
                new Vector(20.8264391272096, 74.2594376022753, 0),
                new Vector(21.9764391272329, 79.3594376022811, 0),
                new Vector(22.5884391271975, 79.2194376022962, 0));

            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY(
                new Vector(20.266361816641236, 108.3441130743964),
                new Vector(-0.39469272911688447, -0.91881317447142952), 18);
            Assert.AreEqual(1, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(2423.3, area, 0.1);
        }

        [TestMethod]
        public void ComplexBoundary_ShouldNotInfill()
        {
            var pLine = new PolyLine(true,
                new Vector(73.0649956400739, 117.107829217392, 0),
                new Vector(72.0176987358136, 114.715159266314, 0),
                new Vector(56.7639427115209, 82.7074833234947, 0),
                new Vector(55.8763405224308, 81.1760932636098, 0),
                new Vector(17.9130761069246, 8.25415542870178, 0),
                new Vector(22.1247583744116, 7.13296999430167, 0),
                new Vector(63.7464320108993, 87.5593983888975, 0),
                new Vector(71.9428474670276, 104.19259623051, 0),
                new Vector(80.0970376548357, 122.458147991594, 0),
                new Vector(87.5625597827602, 141.013045604399, 0),
                new Vector(94.3194011552259, 159.847268298006, 0),
                new Vector(100.316963587771, 178.779084661714, 0),
                new Vector(101.960864371387, 184.846973618201, 0),
                new Vector(103.078713766532, 186.0995466733, 0),
                new Vector(107.042321637738, 184.137161334103, 0),
                new Vector(98.6566670208704, 157.328400163387, 0),
                new Vector(92.424302456202, 133.882754235296, 0),
                new Vector(93.9940173212672, 132.654532809713, 0),
                new Vector(91.8167680876795, 124.064634305192, 0),
                new Vector(88.903945997241, 115.036379614292, 0),
                new Vector(86.5120252410416, 109.753947599995, 0),
                new Vector(81.1137108216062, 97.4071874496876, 0),
                new Vector(73.9550439415034, 81.2363103695097, 0),
                new Vector(69.4635887971381, 71.5499999999884, 0),
                new Vector(68.0135887970682, 70.1000000000058, 0),
                new Vector(67.3135887971148, 69.2999999999884, 0),
                new Vector(60.9135887970915, 60.5499999999884, 0),
                new Vector(38.0635887971148, 7.79999999998836, 0),
                new Vector(35.0635887971148, 0.5, 0),
                new Vector(33.1635887970915, 0, 0),
                new Vector(2.96358879713807, 8.14999999999418, 0),
                new Vector(0.0807065503904596, 9.31486425281037, 0),
                new Vector(49.9897565164138, 94.3730581082928, 0),
                new Vector(51.1258087233873, 96.3488719387969, 0),
                new Vector(52.2165086213499, 98.3320394030889, 0),
                new Vector(54.1499171174364, 102.000792399311, 0),
                new Vector(61.4966298551299, 117.193813846592, 0),
                new Vector(66.9453027101699, 128.391616853187, 0),
                new Vector(69.345603421214, 133.192945645191, 0),
                new Vector(70.8320108964108, 136.46314814451, 0),
                new Vector(72.1542315145489, 139.532698326511, 0),
                new Vector(73.7635887970682, 138.950000000012, 0),
                new Vector(75.9135887970915, 144.799999999988, 0),
                new Vector(77.2024045947474, 148.122106777999, 0),
                new Vector(78.748944352963, 147.400039937493, 0),
                new Vector(81.0175638347864, 153.487776051712, 0),
                new Vector(79.4811353558907, 154.410416599305, 0),
                new Vector(79.6635887970915, 154.899999999994, 0),
                new Vector(87.3883655490354, 177.998868271912, 0),
                new Vector(87.7259694598615, 179.252912111406, 0),
                new Vector(90.4909933427116, 188.812898220902, 0),
                new Vector(97.7642074814066, 186.059030818986, 0));

            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY(
                new Vector(84.946409084741987, 189.4919184620864, 0),
                new Vector(0.976225817702759, 0.21675597535149876, 0), 18);
            Assert.AreEqual(1, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(4166.25, area, 0.1);
        }


        [TestMethod]
        public void ComplexBoundary_ShouldSplitIn4()
        {
            var pLine = new PolyLine(true,
                    new Vector(8.4505576096708, 30.2018942827999, 0),
                    new Vector(15.5005675972207, 33.6018990925804, 0),
                    new Vector(18.3505876010749, 34.9519092768023, 0),
                    new Vector(19.650536849862, 35.6018839026801, 0),
                    new Vector(36.2005661226576, 43.2018985390023, 0),
                    new Vector(51.2005951907486, 49.751911725878, 0),
                    new Vector(58.3005863830913, 52.9019078089041, 0),
                    new Vector(63.5005849945592, 55.2519071798015, 0),
                    new Vector(72.1505931942957, 59.15191089359, 0),
                    new Vector(78.9505806750385, 62.2519051856943, 0),
                    new Vector(79.6005663272226, 62.5518985626986, 0),
                    new Vector(89.1005766820163, 67.2019038167782, 0),
                    new Vector(89.8005386326695, 67.5518847919884, 0),
                    new Vector(91.3505733170314, 68.3019021335058, 0),
                    new Vector(116.20190047205, 79.6514261801785, 0),
                    new Vector(116.701488322695, 78.6022916942893, 0),
                    new Vector(133.90063069982, 85.6519268874836, 0),
                    new Vector(139.200614373549, 87.8519199242874, 0),
                    new Vector(141.800605830038, 88.9519163082878, 0),
                    new Vector(142.150602444774, 89.1019148574851, 0),
                    new Vector(144.350597131182, 90.0519125624851, 0),
                    new Vector(146.950585803599, 91.2019075526914, 0),
                    new Vector(148.900574570987, 92.1019023690023, 0),
                    new Vector(150.700560445432, 92.9518956983811, 0),
                    new Vector(152.850558865932, 94.0018948584911, 0),
                    new Vector(157.60056555341, 96.2518982518814, 0),
                    new Vector(158.900550398161, 96.9018906744896, 0),
                    new Vector(160.700528596179, 97.8018797740806, 0),
                    new Vector(162.250507415622, 98.6518681414018, 0),
                    new Vector(182.400470661349, 110.251846797386, 0),
                    new Vector(190.650448146509, 115.651831786497, 0),
                    new Vector(209.750468702987, 127.401845010696, 0),
                    new Vector(212.750463089207, 129.301841405802, 0),
                    new Vector(214.400451762951, 130.351834198082, 0),
                    new Vector(217.700434546336, 132.501823240484, 0),
                    new Vector(222.60047704482, 135.601850287901, 0),
                    new Vector(226.700497762533, 137.651864864078, 0),
                    new Vector(240.700547116343, 144.651889057801, 0),
                    new Vector(278.450611576322, 161.701918741106, 0),
                    new Vector(281.4005973069, 162.951912694902, 0),
                    new Vector(285.500655881362, 164.451937758189, 0),
                    new Vector(311.30070376629, 172.051952388778, 0),
                    new Vector(318.75070236763, 174.35195166929, 0),
                    new Vector(333.900710772839, 178.701954725606, 0),
                    new Vector(366.500866768183, 186.351987649599, 0),
                    new Vector(424.050971306511, 192.201995900396, 0),
                    new Vector(445.401116332621, 191.901989172882, 0),
                    new Vector(454.601104789646, 190.801990553504, 0),
                    new Vector(473.0010550156, 188.851996247482, 0),
                    new Vector(500.751158063067, 184.301983291283, 0),
                    new Vector(509.801152339904, 182.851984208799, 0),
                    new Vector(518.951148142805, 181.45198485229, 0),
                    new Vector(528.851128086448, 179.951987889595, 0),
                    new Vector(537.801053306437, 178.951996244694, 0),
                    new Vector(547.151071551838, 178.051993761183, 0),
                    new Vector(556.851041616406, 177.551995305577, 0),
                    new Vector(571.901018926874, 177.001996132778, 0),
                    new Vector(580.101000666036, 176.951996244781, 0),
                    new Vector(580.150987873087, 176.951996245189, 0),
                    new Vector(596.550966690178, 177.251995858882, 0),
                    new Vector(602.150945063797, 177.501994894294, 0),
                    new Vector(605.450933350367, 177.701994185103, 0),
                    new Vector(614.250919948681, 178.301993270405, 0),
                    new Vector(624.000934042386, 179.151996243483, 0),
                    new Vector(636.400854448322, 180.701986324188, 0),
                    new Vector(651.450814755517, 183.301979466603, 0),
                    new Vector(665.000776852714, 186.001971913094, 0),
                    new Vector(686.40074923879, 191.401964919904, 0),
                    new Vector(697.10072787886, 194.201959330181, 0),
                    new Vector(708.15069696866, 197.501950098289, 0),
                    new Vector(719.750650885166, 201.351934805192, 0),
                    new Vector(731.150611500721, 206.001918738388, 0),
                    new Vector(741.05059546954, 210.251911857195, 0),
                    new Vector(750.400581002468, 214.451905359194, 0),
                    new Vector(759.600566307548, 218.751898489601, 0),
                    new Vector(768.500543028116, 223.101887112687, 0),
                    new Vector(777.050521652214, 227.651875735784, 0),
                    new Vector(786.501845158171, 232.851527266495, 0),
                    new Vector(790.751433779136, 226.000096900505, 0),
                    new Vector(789.101451331633, 225.200105410797, 0),
                    new Vector(783.951474754722, 222.450117918284, 0),
                    new Vector(780.601483406615, 220.600122696196, 0),
                    new Vector(754.651410857216, 207.250086143089, 0),
                    new Vector(728.00134812342, 196.150060196, 0),
                    new Vector(694.801224246388, 184.950022759498, 0),
                    new Vector(685.351235850016, 182.750025460904, 0),
                    new Vector(641.651119866874, 173.400004228984, 0),
                    new Vector(634.352100502234, 172.500125129183, 0),
                    new Vector(634.65199105721, 169.801110135479, 0),
                    new Vector(635.401874203933, 163.402107283706, 0),
                    new Vector(640.900858666166, 164.051987265586, 0),
                    new Vector(652.300822063116, 165.901981325587, 0),
                    new Vector(662.600798756233, 167.9019768, 0),
                    new Vector(671.350773400743, 169.75197143908, 0),
                    new Vector(680.000751952408, 171.90196610798, 0),
                    new Vector(700.100743748364, 177.051964051701, 0),
                    new Vector(704.650730017805, 178.301960279583, 0),
                    new Vector(708.750686345273, 179.451948029891, 0),
                    new Vector(720.100657579722, 183.501937375491, 0),
                    new Vector(736.500637616147, 189.601929865603, 0),
                    new Vector(745.750602581888, 193.351915662497, 0),
                    new Vector(762.000574603328, 200.651903065504, 0),
                    new Vector(770.700554418261, 204.90189320498, 0),
                    new Vector(777.400538743241, 208.251885367383, 0),
                    new Vector(782.750515749911, 211.101873118605, 0),
                    new Vector(792.000504337251, 216.201867477677, 0),
                    new Vector(795.401849581627, 218.301520132693, 0),
                    new Vector(797.551825318369, 214.801559630898, 0),
                    new Vector(798.001769190538, 214.201634467987, 0),
                    new Vector(798.501041329699, 213.652435114986, 0),
                    new Vector(800.399209502502, 215.500651493785, 0),
                    new Vector(797.400449846522, 219.401833416079, 0),
                    new Vector(800.150449178182, 221.201832978695, 0),
                    new Vector(806.400457621552, 225.151838890684, 0),
                    new Vector(810.300442133332, 227.751828332606, 0),
                    new Vector(814.150418625679, 230.3518124572, 0),
                    new Vector(819.250398285571, 234.151797301805, 0),
                    new Vector(832.750387738808, 244.301789372286, 0),
                    new Vector(841.200357328402, 250.951765439793, 0),
                    new Vector(857.550320783746, 265.351733253803, 0),
                    new Vector(867.15030044585, 274.551713763183, 0),
                    new Vector(900.05034307274, 305.651752842183, 0),
                    new Vector(926.500433676061, 326.801822605805, 0),
                    new Vector(931.700431583566, 330.35182117729, 0),
                    new Vector(944.600457156659, 338.751838225988, 0),
                    new Vector(947.17199625785, 340.440946777002, 0),
                    new Vector(945.391996447346, 325.580926057795, 0),
                    new Vector(945.377734804759, 324.771615074278, 0),
                    new Vector(937.60151814064, 320.200142247195, 0),
                    new Vector(930.001538663288, 315.400155209005, 0),
                    new Vector(927.051539190346, 313.500155552581, 0),
                    new Vector(925.601548980339, 312.550161968684, 0),
                    new Vector(923.251548107946, 311.000161385979, 0),
                    new Vector(922.051570934709, 310.200176604791, 0),
                    new Vector(920.201594169135, 308.850193449587, 0),
                    new Vector(917.601604461903, 306.900201190379, 0),
                    new Vector(912.501619752496, 302.85021333539, 0),
                    new Vector(901.651624317979, 294.250216871995, 0),
                    new Vector(899.351622459828, 292.400215417292, 0),
                    new Vector(898.251641159295, 291.500230716803, 0),
                    new Vector(895.951636394253, 289.600226509385, 0),
                    new Vector(893.651680085342, 287.600264499779, 0),
                    new Vector(892.301691652043, 286.350274054392, 0),
                    new Vector(880.401680051815, 275.550265042286, 0),
                    new Vector(862.20169964456, 257.950284017978, 0),
                    new Vector(838.251697788481, 234.250282202178, 0),
                    new Vector(816.801513224957, 216.850114020985, 0),
                    new Vector(812.551096142968, 213.000001107983, 0),
                    new Vector(800.550550401211, 212.300101816305, 0),
                    new Vector(799.05095753714, 213.199760553689, 0),
                    new Vector(771.25141872617, 197.400089233997, 0),
                    new Vector(749.151396367932, 187.700079167, 0),
                    new Vector(719.901380325784, 175.250072294293, 0),
                    new Vector(717.651358914562, 174.350063731603, 0),
                    new Vector(696.901251326664, 167.150028963806, 0),
                    new Vector(678.101129777846, 162.500005015201, 0),
                    new Vector(636.100992350839, 157.349996258505, 0),
                    new Vector(626.600994982873, 157.399996245687, 0),
                    new Vector(622.850998575799, 157.399996243999, 0),
                    new Vector(596.501990024, 157.349998126389, 0),
                    new Vector(596.450933913235, 150.799998275383, 0),
                    new Vector(564.450825788663, 153.250011116703, 0),
                    new Vector(537.050843991921, 158.050007915095, 0),
                    new Vector(533.35087573796, 158.550003624288, 0),
                    new Vector(524.750896298792, 159.50000135458, 0),
                    new Vector(518.850907975808, 160.050000265299, 0),
                    new Vector(490.300920549664, 162.649999115703, 0),
                    new Vector(483.150946983253, 163.099997454177, 0),
                    new Vector(481.850999507587, 163.149995432381, 0),
                    new Vector(478.301043341053, 162.999997283187, 0),
                    new Vector(457.001048899605, 161.949997559888, 0),
                    new Vector(429.851059371256, 160.499998119602, 0),
                    new Vector(409.951089242822, 159.10000022038, 0),
                    new Vector(394.301125169382, 157.350004105683, 0),
                    new Vector(383.901171375415, 155.750011211785, 0),
                    new Vector(372.351207062486, 153.450018320495, 0),
                    new Vector(351.201241408126, 148.600026195694, 0),
                    new Vector(329.451269394485, 142.650033851998, 0),
                    new Vector(307.651296055643, 136.300041618699, 0),
                    new Vector(289.10132254113, 130.100050470501, 0),
                    new Vector(285.701322498498, 128.950050323299, 0),
                    new Vector(280.151317745098, 127.10004848268, 0),
                    new Vector(278.25134381582, 126.400058086787, 0),
                    new Vector(275.951356695616, 125.550062846683, 0),
                    new Vector(271.401365512633, 123.750066373788, 0),
                    new Vector(262.401370396139, 120.150068326097, 0),
                    new Vector(259.551378759323, 119.000071701099, 0),
                    new Vector(247.651372340159, 114.300069034187, 0),
                    new Vector(244.901386456215, 113.150074937003, 0),
                    new Vector(242.201403701794, 112.00008228168, 0),
                    new Vector(235.601423722808, 108.9500915432, 0),
                    new Vector(231.751437804196, 107.100098309194, 0),
                    new Vector(223.751425260911, 103.250092272501, 0),
                    new Vector(209.951431177557, 96.6500951190828, 0),
                    new Vector(201.5014358185, 92.55009736988, 0),
                    new Vector(199.051446479629, 91.3501025938021, 0),
                    new Vector(197.239455578732, 90.4171072782774, 0),
                    new Vector(192.401453949977, 88.0001062944939, 0),
                    new Vector(185.60146400868, 84.450111008191, 0),
                    new Vector(183.301462471369, 83.2501108097786, 0),
                    new Vector(179.951473697554, 81.4501168434799, 0),
                    new Vector(176.651483742869, 79.6501223211817, 0),
                    new Vector(173.30149481236, 77.7501285990002, 0),
                    new Vector(168.551500059781, 75.0001316373819, 0),
                    new Vector(163.751505461638, 72.200134789804, 0),
                    new Vector(159.051510770223, 69.4001379505789, 0),
                    new Vector(148.701512483996, 63.200138974702, 0),
                    new Vector(117.750148833147, 44.6004675001022, 0),
                    new Vector(113.950634955079, 50.6996871458832, 0),
                    new Vector(109.751459018677, 48.5501090319012, 0),
                    new Vector(95.9014631815953, 41.2501112300961, 0),
                    new Vector(85.2014647817705, 35.600112021697, 0),
                    new Vector(51.1013799633365, 19.3000722315046, 0),
                    new Vector(44.1013726248639, 16.450069221406, 0),
                    new Vector(35.4513660010416, 13.0000665708794, 0),
                    new Vector(29.0013378689764, 10.5000555744919, 0),
                    new Vector(22.6013577227714, 8.05006328900345, 0),
                    new Vector(4.0513253735844, 1.40005100358394, 0),
                    new Vector(0.0006812778301537, 0.0019453979039099, 0),
                    new Vector(3.00066016416531, 1.00193835998653, 0),
                    new Vector(4.05064311192837, 1.40193181749783, 0),
                    new Vector(9.15064115461428, 3.3519310670963, 0),
                    new Vector(10.8497087949654, 4.00157457668683, 0),
                    new Vector(9.75065431266557, 6.9019359794911, 0),
                    new Vector(17.5006633402081, 9.65193923009792, 0),
                    new Vector(21.150654159952, 10.9519359603873, 0),
                    new Vector(24.7506428783527, 12.3019317298022, 0),
                    new Vector(28.4006346045062, 13.7019285562856, 0),
                    new Vector(30.5506243490381, 14.5519245017786, 0),
                    new Vector(32.7506151107373, 15.4519207224948, 0),
                    new Vector(38.3506181265693, 17.7519219630922, 0),
                    new Vector(41.8506126402644, 19.201919690182, 0),
                    new Vector(51.5006015992258, 23.251915056404, 0),
                    new Vector(54.5505850401241, 24.6019077269884, 0),
                    new Vector(57.2505741275381, 25.8519026747963, 0),
                    new Vector(59.9005672824569, 27.1018994459009, 0),
                    new Vector(62.600560514722, 28.4018961874826, 0),
                    new Vector(70.3505572252907, 32.1518946084834, 0),
                    new Vector(72.3505459517473, 33.1518889716826, 0),
                    new Vector(77.9005372102838, 35.9018854397873, 0),
                    new Vector(84.5505593252601, 39.2018956584798, 0),
                    new Vector(87.5505682606017, 40.6518999300024, 0),
                    new Vector(89.1005499333842, 41.4018910619779, 0),
                    new Vector(100.70053348795, 47.401882555685, 0),
                    new Vector(106.450526701054, 50.4518789557042, 0),
                    new Vector(111.200518959085, 53.0018747994909, 0),
                    new Vector(137.600517076906, 67.501873753703, 0),
                    new Vector(166.350516627892, 83.2518735039048, 0),
                    new Vector(211.300556776929, 106.601894355379, 0),
                    new Vector(241.750619050348, 120.651922334189, 0),
                    new Vector(283.800700256485, 136.301951514586, 0),
                    new Vector(325.300769092399, 148.201970278897, 0),
                    new Vector(365.000824073679, 156.85198156038, 0),
                    new Vector(404.200893676607, 162.35199132908, 0),
                    new Vector(446.050971872988, 165.801996385999, 0),
                    new Vector(457.75096869655, 166.101996304584, 0),
                    new Vector(494.451057278784, 165.101994926605, 0),
                    new Vector(502.801053001662, 164.601995182689, 0),
                    new Vector(537.351058927597, 162.601994836878, 0),
                    new Vector(545.251050109626, 162.151995339198, 0),
                    new Vector(551.451043938752, 161.851995637786, 0),
                    new Vector(566.55104092136, 161.101995840087, 0),
                    new Vector(572.851017758017, 160.901996575383, 0),
                    new Vector(578.550998005783, 160.851996748679, 0),
                    new Vector(597.850974472007, 161.001996565785, 0),
                    new Vector(610.900955205201, 161.501995827595, 0),
                    new Vector(625.999873990892, 162.201945705077, 0),
                    new Vector(624.850120636518, 171.699891479802, 0),
                    new Vector(597.250990991131, 169.79999673579, 0),
                    new Vector(544.300942793139, 171.249998212501, 0),
                    new Vector(505.300927531207, 173.899999117682, 0),
                    new Vector(495.350949783693, 174.499997775798, 0),
                    new Vector(469.500978000578, 175.449996714597, 0),
                    new Vector(461.801002583234, 175.449996714597, 0),
                    new Vector(451.951018498978, 175.349996876204, 0),
                    new Vector(444.101035553496, 175.099997419282, 0),
                    new Vector(436.20104924764, 174.749998026004, 0),
                    new Vector(430.301063930849, 174.399998897105, 0),
                    new Vector(422.851076800143, 173.849999847094, 0),
                    new Vector(416.401093825465, 173.300001298892, 0),
                    new Vector(400.701111502247, 171.600003212981, 0),
                    new Vector(394.101125572342, 170.80000491839, 0),
                    new Vector(384.251137798768, 169.450006594096, 0),
                    new Vector(378.101161420462, 168.550010050996, 0),
                    new Vector(367.351181572769, 166.550013800181, 0),
                    new Vector(345.501197601086, 162.450016797986, 0),
                    new Vector(345.027214106754, 162.345020454406, 0),
                    new Vector(331.551226236974, 159.350023150182, 0),
                    new Vector(320.651244150475, 156.650027587602, 0),
                    new Vector(306.301271452801, 152.90003472229, 0),
                    new Vector(301.601297304034, 151.450042697688, 0),
                    new Vector(289.101311919396, 147.450047374587, 0),
                    new Vector(274.951315627084, 142.750048576097, 0),
                    new Vector(269.951337761246, 141.000056323101, 0),
                    new Vector(257.00134723098, 136.30005919718, 0),
                    new Vector(253.001362706651, 134.750065762084, 0),
                    new Vector(248.451355403638, 133.050062840892, 0),
                    new Vector(245.001366509707, 131.700067199999, 0),
                    new Vector(239.051363137318, 129.400065897906, 0),
                    new Vector(236.95138337044, 128.550074087398, 0),
                    new Vector(234.751397814369, 127.600080324599, 0),
                    new Vector(232.601410492673, 126.65008592658, 0),
                    new Vector(224.101406943169, 122.800084369286, 0),
                    new Vector(220.90141585737, 121.350088408479, 0),
                    new Vector(211.501425381401, 116.95009286658, 0),
                    new Vector(206.801432469045, 114.700096259592, 0),
                    new Vector(200.851441720501, 111.800100768683, 0),
                    new Vector(195.001449262956, 108.850104572193, 0),
                    new Vector(189.101468619891, 105.850114414701, 0),
                    new Vector(186.60146635666, 104.500113396905, 0),
                    new Vector(113.651410187827, 69.0500858394953, 0),
                    new Vector(89.10140551324, 58.0500837306026, 0),
                    new Vector(85.5514133293182, 56.4500872532954, 0),
                    new Vector(49.5014051523758, 40.1500835613988, 0),
                    new Vector(45.3014121042797, 38.2500867062772, 0),
                    new Vector(11.8013876185287, 24.0000759382965, 0),
                    new Vector(6.85138021723833, 21.9500727662817, 0),
                    new Vector(4.40006723266561, 20.9006298463792, 0),
                    new Vector(3.4500670642592, 23.3206302585022, 0),
                    new Vector(2.01006584416609, 27.0006333734782, 0));

            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY(
                new Vector(795.08618254810506, 259.69802829180884, 0),
                new Vector(-0.858378546072934, -0.51301683368259543, 0), 18);
            Assert.AreEqual(4, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(15067.5, area, 0.1);
        }

        [TestMethod]
        public void ComplexBoundary_StartpointInSplit_ShouldSplitIn4()
        {
            var pLine = new PolyLine(true,
                    new Vector(149.801733316854, 775.094187676994, 0),
                    new Vector(141.85230316536, 766.644787734811, 0),
                    new Vector(141.85230316536, 766.644787734811, 0),
                    new Vector(143.701759682153, 764.145522171311, 0),
                    new Vector(143.701759682153, 764.145522171311, 0),
                    new Vector(144.251690775971, 763.595591079589, 0),
                    new Vector(144.251690775971, 763.595591079589, 0),
                    new Vector(153.851724594948, 754.844178390107, 0),
                    new Vector(153.851724594948, 754.844178390107, 0),
                    new Vector(151.901726685697, 752.794180587283, 0),
                    new Vector(151.901726685697, 752.794180587283, 0),
                    new Vector(130.951727199717, 730.594181138586, 0),
                    new Vector(130.951727199717, 730.594181138586, 0),
                    new Vector(123.651718611596, 722.994172142295, 0),
                    new Vector(123.651718611596, 722.994172142295, 0),
                    new Vector(121.251729219686, 720.494183192292, 0),
                    new Vector(121.251729219686, 720.494183192292, 0),
                    new Vector(117.351742447936, 716.24419760771, 0),
                    new Vector(117.351742447936, 716.24419760771, 0),
                    new Vector(113.801758677, 712.244215894898, 0),
                    new Vector(113.801758677, 712.244215894898, 0),
                    new Vector(112.169768995955, 710.281228305685, 0),
                    new Vector(112.169768995955, 710.281228305685, 0),
                    new Vector(106.401761474553, 703.794218945986, 0),
                    new Vector(106.401761474553, 703.794218945986, 0),
                    new Vector(105.751776569057, 702.994237526989, 0),
                    new Vector(105.751776569057, 702.994237526989, 0),
                    new Vector(99.5017682446633, 695.544227375503, 0),
                    new Vector(99.5017682446633, 695.544227375503, 0),
                    new Vector(92.551761637209, 687.344219431106, 0),
                    new Vector(92.551761637209, 687.344219431106, 0),
                    new Vector(88.9017720583361, 682.944231996109, 0),
                    new Vector(88.9017720583361, 682.944231996109, 0),
                    new Vector(85.5517881236738, 678.844251660688, 0),
                    new Vector(85.5517881236738, 678.844251660688, 0),
                    new Vector(78.6017849058844, 670.044247349404, 0),
                    new Vector(78.6017849058844, 670.044247349404, 0),
                    new Vector(74.4017785155447, 664.594239059399, 0),
                    new Vector(74.4017785155447, 664.594239059399, 0),
                    new Vector(72.1018023475772, 661.544270663784, 0),
                    new Vector(72.1018023475772, 661.544270663784, 0),
                    new Vector(68.6518211052753, 656.844296216703, 0),
                    new Vector(68.6518211052753, 656.844296216703, 0),
                    new Vector(62.6018551373854, 647.844346428697, 0),
                    new Vector(62.6018551373854, 647.844346428697, 0),
                    new Vector(49.7018661937909, 625.894366919296, 0),
                    new Vector(49.7018661937909, 625.894366919296, 0),
                    new Vector(49.0518772887299, 624.744386041508, 0),
                    new Vector(49.0518772887299, 624.744386041508, 0),
                    new Vector(48.6019052336924, 623.844441930909, 0),
                    new Vector(48.6019052336924, 623.844441930909, 0),
                    new Vector(46.9019073159434, 620.194446649286, 0),
                    new Vector(46.9019073159434, 620.194446649286, 0),
                    new Vector(46.1519338876242, 618.494506879011, 0),
                    new Vector(46.1519338876242, 618.494506879011, 0),
                    new Vector(46.0019555917243, 618.044571992301, 0),
                    new Vector(46.0019555917243, 618.044571992301, 0),
                    new Vector(40.8519443297992, 606.494490053097, 0),
                    new Vector(40.8519443297992, 606.494490053097, 0),
                    new Vector(40.7019787086174, 605.844659185997, 0),
                    new Vector(40.7019787086174, 605.844659185997, 0),
                    new Vector(40.1520000371384, 603.944683579204, 0),
                    new Vector(40.1520000371384, 603.944683579204, 0),
                    new Vector(40.052000036696, 602.994812221907, 0),
                    new Vector(40.052000036696, 602.994812221907, 0),
                    new Vector(39.8019975417992, 599.69479141591, 0),
                    new Vector(39.8019975417992, 599.69479141591, 0),
                    new Vector(39.2519982105587, 592.094805460802, 0),
                    new Vector(39.2519982105587, 592.094805460802, 0),
                    new Vector(38.9520000370685, 589.04481511761, 0),
                    new Vector(38.9520000370685, 589.04481511761, 0),
                    new Vector(38.4019929626957, 585.044746426109, 0),
                    new Vector(38.4019929626957, 585.044746426109, 0),
                    new Vector(38.0019975244068, 580.944793188508, 0),
                    new Vector(38.0019975244068, 580.944793188508, 0),
                    new Vector(36.6519333827309, 571.494508626201, 0),
                    new Vector(36.6519333827309, 571.494508626201, 0),
                    new Vector(36.4019366852008, 570.844517213409, 0),
                    new Vector(36.4019366852008, 570.844517213409, 0),
                    new Vector(35.8019456421025, 569.194541845209, 0),
                    new Vector(35.8019456421025, 569.194541845209, 0),
                    new Vector(35.1019557013642, 567.044572741608, 0),
                    new Vector(35.1019557013642, 567.044572741608, 0),
                    new Vector(34.7519766666228, 565.844644620287, 0),
                    new Vector(34.7519766666228, 565.844644620287, 0),
                    new Vector(34.6519915343961, 565.244733826985, 0),
                    new Vector(34.6519915343961, 565.244733826985, 0),
                    new Vector(34.5019950701389, 563.79476800689, 0),
                    new Vector(34.5019950701389, 563.79476800689, 0),
                    new Vector(34.3519980277633, 562.244798575208, 0),
                    new Vector(34.3519980277633, 562.244798575208, 0),
                    new Vector(34.0019991692388, 553.8948259592, 0),
                    new Vector(34.0019991692388, 553.8948259592, 0),
                    new Vector(33.1519963667961, 543.994781738089, 0),
                    new Vector(33.1519963667961, 543.994781738089, 0),
                    new Vector(32.9019974017283, 540.8447947776, 0),
                    new Vector(32.9019974017283, 540.8447947776, 0),
                    new Vector(31.5020000367658, 522.99481910991, 0),
                    new Vector(31.5020000367658, 522.99481910991, 0),
                    new Vector(30.7019988733809, 510.944816224306, 0),
                    new Vector(30.7019988733809, 510.944816224306, 0),
                    new Vector(30.5459994776174, 506.312834176904, 0),
                    new Vector(30.5459994776174, 506.312834176904, 0),
                    new Vector(30.1519998565782, 496.444842736702, 0),
                    new Vector(30.1519998565782, 496.444842736702, 0),
                    new Vector(29.5519806790398, 477.294670723408, 0),
                    new Vector(29.5519806790398, 477.294670723408, 0),
                    new Vector(28.001985916635, 468.844699586189, 0),
                    new Vector(28.001985916635, 468.844699586189, 0),
                    new Vector(27.5849880411988, 466.176713183697, 0),
                    new Vector(27.5849880411988, 466.176713183697, 0),
                    new Vector(24.9019858373795, 451.094698485103, 0),
                    new Vector(24.9019858373795, 451.094698485103, 0),
                    new Vector(24.5019891741686, 448.444720594591, 0),
                    new Vector(24.5019891741686, 448.444720594591, 0),
                    new Vector(23.8019935725024, 443.844748442294, 0),
                    new Vector(23.8019935725024, 443.844748442294, 0),
                    new Vector(23.6520018462325, 442.044847719691, 0),
                    new Vector(23.6520018462325, 442.044847719691, 0),
                    new Vector(23.701998577104, 440.894922909589, 0),
                    new Vector(23.701998577104, 440.894922909589, 0),
                    new Vector(24.0519977563526, 435.694935094594, 0),
                    new Vector(24.0519977563526, 435.694935094594, 0),
                    new Vector(24.4019977112766, 430.544935765007, 0),
                    new Vector(24.4019977112766, 430.544935765007, 0),
                    new Vector(24.7519967852859, 425.444949255587, 0),
                    new Vector(24.7519967852859, 425.444949255587, 0),
                    new Vector(24.8512263325974, 424.393893545493, 0),
                    new Vector(24.8512263325974, 424.393893545493, 0),
                    new Vector(19.9010859254049, 423.243871299084, 0),
                    new Vector(19.9010859254049, 423.243871299084, 0),
                    new Vector(8.30192450201139, 422.243943592795, 0),
                    new Vector(8.30192450201139, 422.243943592795, 0),
                    new Vector(8.15200003702194, 420.494824826397, 0),
                    new Vector(8.15200003702194, 420.494824826397, 0),
                    new Vector(8.15200003702194, 419.194911000406, 0),
                    new Vector(8.15200003702194, 419.194911000406, 0),
                    new Vector(9.05199567344971, 408.844961173803, 0),
                    new Vector(9.05199567344971, 408.844961173803, 0),
                    new Vector(9.55199220997747, 403.894995461393, 0),
                    new Vector(9.55199220997747, 403.894995461393, 0),
                    new Vector(10.3519850398879, 398.795041182311, 0),
                    new Vector(10.3519850398879, 398.795041182311, 0),
                    new Vector(12.251981548965, 389.045059006283, 0),
                    new Vector(12.251981548965, 389.045059006283, 0),
                    new Vector(14.1519817991648, 379.145057812682, 0),
                    new Vector(14.1519817991648, 379.145057812682, 0),
                    new Vector(14.9019785317359, 375.395074149797, 0),
                    new Vector(14.9019785317359, 375.395074149797, 0),
                    new Vector(15.1019733692519, 374.495097379695, 0),
                    new Vector(15.1019733692519, 374.495097379695, 0),
                    new Vector(15.4019657176686, 373.295127986494, 0),
                    new Vector(15.4019657176686, 373.295127986494, 0),
                    new Vector(17.851961024222, 364.745144217508, 0),
                    new Vector(17.851961024222, 364.745144217508, 0),
                    new Vector(20.7019590614364, 354.845151119807, 0),
                    new Vector(20.7019590614364, 354.845151119807, 0),
                    new Vector(24.5519561374094, 342.145160703687, 0),
                    new Vector(24.5519561374094, 342.145160703687, 0),
                    new Vector(29.0519558952656, 327.395161458611, 0),
                    new Vector(29.0519558952656, 327.395161458611, 0),
                    new Vector(33.5019555935869, 312.945162490389, 0),
                    new Vector(33.5019555935869, 312.945162490389, 0),
                    new Vector(33.9809534784872, 311.430169181986, 0),
                    new Vector(33.9809534784872, 311.430169181986, 0),
                    new Vector(38.5519663459854, 293.845126767788, 0),
                    new Vector(38.5519663459854, 293.845126767788, 0),
                    new Vector(42.9519582536304, 279.045153649698, 0),
                    new Vector(42.9519582536304, 279.045153649698, 0),
                    new Vector(45.3519575876417, 271.04515586971, 0),
                    new Vector(45.3519575876417, 271.04515586971, 0),
                    new Vector(46.8019563608104, 266.245159932208, 0),
                    new Vector(46.8019563608104, 266.245159932208, 0),
                    new Vector(49.7019582861103, 256.645153557096, 0),
                    new Vector(49.7019582861103, 256.645153557096, 0),
                    new Vector(51.151957185124, 251.845157205506, 0),
                    new Vector(51.151957185124, 251.845157205506, 0),
                    new Vector(52.651953989407, 246.895167749404, 0),
                    new Vector(52.651953989407, 246.895167749404, 0),
                    new Vector(54.2019468310755, 242.145189684496, 0),
                    new Vector(54.2019468310755, 242.145189684496, 0),
                    new Vector(63.1019766433164, 211.395082636707, 0),
                    new Vector(63.1019766433164, 211.395082636707, 0),
                    new Vector(64.7019805614837, 203.795064516307, 0),
                    new Vector(64.7019805614837, 203.795064516307, 0),
                    new Vector(66.3019774127752, 196.2950791537, 0),
                    new Vector(66.3019774127752, 196.2950791537, 0),
                    new Vector(67.1517799412832, 192.445973585011, 0),
                    new Vector(67.1517799412832, 192.445973585011, 0),
                    new Vector(98.9500649531838, 196.445757863286, 0),
                    new Vector(98.9500649531838, 196.445757863286, 0),
                    new Vector(100.90082396986, 222.045851980889, 0),
                    new Vector(100.90082396986, 222.045851980889, 0),
                    new Vector(113.200820857077, 224.245851425687, 0),
                    new Vector(113.200820857077, 224.245851425687, 0),
                    new Vector(158.744959911797, 232.125148033083, 0),
                    new Vector(158.744959911797, 232.125148033083, 0),
                    new Vector(176.411802441697, 171.65227082331, 0),
                    new Vector(176.411802441697, 171.65227082331, 0),
                    new Vector(175.40180244192, 170.294270820683, 0),
                    new Vector(175.40180244192, 170.294270820683, 0),
                    new Vector(173.951806104276, 168.34427574859, 0),
                    new Vector(173.951806104276, 168.34427574859, 0),
                    new Vector(160.001644908218, 153.094103311887, 0),
                    new Vector(160.001644908218, 153.094103311887, 0),
                    new Vector(158.401665645419, 151.744120810297, 0),
                    new Vector(158.401665645419, 151.744120810297, 0),
                    new Vector(138.001677207765, 133.394131632085, 0),
                    new Vector(138.001677207765, 133.394131632085, 0),
                    new Vector(131.301690317923, 127.244143671007, 0),
                    new Vector(131.301690317923, 127.244143671007, 0),
                    new Vector(129.901734300889, 125.844187654089, 0),
                    new Vector(129.901734300889, 125.844187654089, 0),
                    new Vector(128.701774076791, 124.444234059396, 0),
                    new Vector(128.701774076791, 124.444234059396, 0),
                    new Vector(124.251787463436, 118.894250971905, 0),
                    new Vector(124.251787463436, 118.894250971905, 0),
                    new Vector(116.551778799854, 109.294240275107, 0),
                    new Vector(116.551778799854, 109.294240275107, 0),
                    new Vector(115.951786854421, 108.54425034381, 0),
                    new Vector(115.951786854421, 108.54425034381, 0),
                    new Vector(110.951798510039, 102.044265496108, 0),
                    new Vector(110.951798510039, 102.044265496108, 0),
                    new Vector(110.101813054178, 100.894285173097, 0),
                    new Vector(110.101813054178, 100.894285173097, 0),
                    new Vector(104.20182544156, 92.4943028728012, 0),
                    new Vector(104.20182544156, 92.4943028728012, 0),
                    new Vector(100.051840938162, 86.2443261813896, 0),
                    new Vector(100.051840938162, 86.2443261813896, 0),
                    new Vector(98.6708493637852, 84.0223397362861, 0),
                    new Vector(98.6708493637852, 84.0223397362861, 0),
                    new Vector(96.8518527714768, 81.0943452233914, 0),
                    new Vector(96.8518527714768, 81.0943452233914, 0),
                    new Vector(92.351857957663, 73.6443538063031, 0),
                    new Vector(92.351857957663, 73.6443538063031, 0),
                    new Vector(90.4518673698185, 70.4443696590897, 0),
                    new Vector(90.4518673698185, 70.4443696590897, 0),
                    new Vector(86.4018754994031, 63.144384311192, 0),
                    new Vector(86.4018754994031, 63.144384311192, 0),
                    new Vector(80.5518814049428, 52.4943950650049, 0),
                    new Vector(80.5518814049428, 52.4943950650049, 0),
                    new Vector(78.8518910818966, 49.2444135659898, 0),
                    new Vector(78.8518910818966, 49.2444135659898, 0),
                    new Vector(75.6019008058356, 42.6944331600098, 0),
                    new Vector(75.6019008058356, 42.6944331600098, 0),
                    new Vector(71.5019080081256, 33.9444485312852, 0),
                    new Vector(71.5019080081256, 33.9444485312852, 0),
                    new Vector(68.0019123617094, 26.2444581096934, 0),
                    new Vector(68.0019123617094, 26.2444581096934, 0),
                    new Vector(55.3046289304039, 9.86763916444E-05, 0),
                    new Vector(55.3046289304039, 9.86763916444E-05, 0),
                    new Vector(54.7005802817876, 0.293960181094008, 0),
                    new Vector(54.7005802817876, 0.293960181094008, 0),
                    new Vector(46.9005867985543, 3.84395731420955, 0),
                    new Vector(46.9005867985543, 3.84395731420955, 0),
                    new Vector(45.6001302319346, 4.4453616956016, 0),
                    new Vector(45.6001302319346, 4.4453616956016, 0),
                    new Vector(46.6513588969829, 6.29580143568455, 0),
                    new Vector(46.6513588969829, 6.29580143568455, 0),
                    new Vector(47.3005521203158, 6.04611173440935, 0),
                    new Vector(47.3005521203158, 6.04611173440935, 0),
                    new Vector(49.9996413824847, 10.8444926448865, 0),
                    new Vector(49.9996413824847, 10.8444926448865, 0),
                    new Vector(49.3001332954736, 11.2453670545074, 0),
                    new Vector(49.3001332954736, 11.2453670545074, 0),
                    new Vector(50.2515367933083, 12.8957117515965, 0),
                    new Vector(50.2515367933083, 12.8957117515965, 0),
                    new Vector(50.8006412618561, 12.5462816352956, 0),
                    new Vector(50.8006412618561, 12.5462816352956, 0),
                    new Vector(64.2001265031286, 36.195355133299, 0),
                    new Vector(64.2001265031286, 36.195355133299, 0),
                    new Vector(65.6501214453019, 38.8453458893928, 0),
                    new Vector(65.6501214453019, 38.8453458893928, 0),
                    new Vector(71.3001198687125, 49.2953429734043, 0),
                    new Vector(71.3001198687125, 49.2953429734043, 0),
                    new Vector(75.1501155137084, 56.4453348855895, 0),
                    new Vector(75.1501155137084, 56.4453348855895, 0),
                    new Vector(78.1501074436819, 62.2453192835092, 0),
                    new Vector(78.1501074436819, 62.2453192835092, 0),
                    new Vector(82.2500975835137, 70.54529932281, 0),
                    new Vector(82.2500975835137, 70.54529932281, 0),
                    new Vector(89.4000965941232, 85.995297510497, 0),
                    new Vector(89.4000965941232, 85.995297510497, 0),
                    new Vector(98.1000885940157, 105.195279799111, 0),
                    new Vector(98.1000885940157, 105.195279799111, 0),
                    new Vector(100.000081493752, 109.445263917005, 0),
                    new Vector(100.000081493752, 109.445263917005, 0),
                    new Vector(103.300076144515, 117.395251029986, 0),
                    new Vector(103.300076144515, 117.395251029986, 0),
                    new Vector(132.151309938519, 187.095818819886, 0),
                    new Vector(132.151309938519, 187.095818819886, 0),
                    new Vector(134.451286659576, 186.345826410892, 0),
                    new Vector(134.451286659576, 186.345826410892, 0),
                    new Vector(139.701883705682, 182.795335977804, 0),
                    new Vector(139.701883705682, 182.795335977804, 0),
                    new Vector(140.451980498503, 179.144671974005, 0),
                    new Vector(140.451980498503, 179.144671974005, 0),
                    new Vector(139.002300980967, 174.045573600684, 0),
                    new Vector(139.002300980967, 174.045573600684, 0),
                    new Vector(139.400387372472, 173.946052002808, 0),
                    new Vector(139.400387372472, 173.946052002808, 0),
                    new Vector(140.800048932899, 177.545181729685, 0),
                    new Vector(140.800048932899, 177.545181729685, 0),
                    new Vector(141.100017161691, 178.645065235585, 0),
                    new Vector(141.100017161691, 178.645065235585, 0),
                    new Vector(141.249999917811, 179.79493303239, 0),
                    new Vector(141.249999917811, 179.79493303239, 0),
                    new Vector(141.249999917811, 180.044845376688, 0),
                    new Vector(141.249999917811, 180.044845376688, 0),
                    new Vector(141.200004006037, 181.144755435205, 0),
                    new Vector(141.200004006037, 181.144755435205, 0),
                    new Vector(141.000028760871, 182.244619283389, 0),
                    new Vector(141.000028760871, 182.244619283389, 0),
                    new Vector(140.750060527702, 182.994523983187, 0),
                    new Vector(140.750060527702, 182.994523983187, 0),
                    new Vector(140.350110111176, 183.994400024385, 0),
                    new Vector(140.350110111176, 183.994400024385, 0),
                    new Vector(139.750214804313, 184.894242984708, 0),
                    new Vector(139.750214804313, 184.894242984708, 0),
                    new Vector(138.900341553963, 185.794108779286, 0),
                    new Vector(138.900341553963, 185.794108779286, 0),
                    new Vector(137.200473204139, 186.99401584969, 0),
                    new Vector(137.200473204139, 186.99401584969, 0),
                    new Vector(135.151605934487, 188.093408043205, 0),
                    new Vector(135.151605934487, 188.093408043205, 0),
                    new Vector(135.000641570776, 187.593934501696, 0),
                    new Vector(135.000641570776, 187.593934501696, 0),
                    new Vector(130.050650974852, 189.493930892, 0),
                    new Vector(130.050650974852, 189.493930892, 0),
                    new Vector(120.800076841027, 192.995252705499, 0),
                    new Vector(120.800076841027, 192.995252705499, 0),
                    new Vector(121.049626757274, 193.594172504585, 0),
                    new Vector(121.049626757274, 193.594172504585, 0),
                    new Vector(117.550880228751, 194.443868090108, 0),
                    new Vector(117.550880228751, 194.443868090108, 0),
                    new Vector(116.050999917905, 194.493867145386, 0),
                    new Vector(116.050999917905, 194.493867145386, 0),
                    new Vector(114.901086467085, 194.443870908406, 0),
                    new Vector(114.901086467085, 194.443870908406, 0),
                    new Vector(113.75118647411, 194.293883952807, 0),
                    new Vector(113.75118647411, 194.293883952807, 0),
                    new Vector(113.551300245686, 194.243912395585, 0),
                    new Vector(113.551300245686, 194.243912395585, 0),
                    new Vector(112.50139256171, 193.843947563699, 0),
                    new Vector(112.50139256171, 193.843947563699, 0),
                    new Vector(111.551506049233, 193.394001320907, 0),
                    new Vector(111.551506049233, 193.394001320907, 0),
                    new Vector(110.70162239077, 192.794083444402, 0),
                    new Vector(110.70162239077, 192.794083444402, 0),
                    new Vector(110.251737115555, 192.394185421901, 0),
                    new Vector(110.251737115555, 192.394185421901, 0),
                    new Vector(109.301835686201, 191.144315120211, 0),
                    new Vector(109.301835686201, 191.144315120211, 0),
                    new Vector(108.701881931745, 190.094396050001, 0),
                    new Vector(108.701881931745, 190.094396050001, 0),
                    new Vector(108.001921349904, 188.694474886288, 0),
                    new Vector(108.001921349904, 188.694474886288, 0),
                    new Vector(107.751956080785, 187.994572132593, 0),
                    new Vector(107.751956080785, 187.994572132593, 0),
                    new Vector(107.252173734247, 186.095399215905, 0),
                    new Vector(107.252173734247, 186.095399215905, 0),
                    new Vector(107.550536142313, 185.946218011901, 0),
                    new Vector(107.550536142313, 185.946218011901, 0),
                    new Vector(108.600083477446, 188.145269571483, 0),
                    new Vector(108.600083477446, 188.145269571483, 0),
                    new Vector(112.50070875627, 192.345824763994, 0),
                    new Vector(112.50070875627, 192.345824763994, 0),
                    new Vector(117.451248958823, 192.695836583007, 0),
                    new Vector(117.451248958823, 192.695836583007, 0),
                    new Vector(122.151921804529, 191.044480630488, 0),
                    new Vector(122.151921804529, 191.044480630488, 0),
                    new Vector(117.801924306666, 180.694486583903, 0),
                    new Vector(117.801924306666, 180.694486583903, 0),
                    new Vector(115.451928750379, 174.894497551286, 0),
                    new Vector(115.451928750379, 174.894497551286, 0),
                    new Vector(82.3519059319515, 93.7444448425958, 0),
                    new Vector(82.3519059319515, 93.7444448425958, 0),
                    new Vector(79.1519106973428, 86.894455043599, 0),
                    new Vector(79.1519106973428, 86.894455043599, 0),
                    new Vector(73.3517370719928, 76.3941923654929, 0),
                    new Vector(73.3517370719928, 76.3941923654929, 0),
                    new Vector(72.2500255266204, 75.1946432305849, 0),
                    new Vector(72.2500255266204, 75.1946432305849, 0),
                    new Vector(72.7499999178108, 88.4949186840968, 0),
                    new Vector(72.7499999178108, 88.4949186840968, 0),
                    new Vector(73.5500021821354, 100.694938358094, 0),
                    new Vector(73.5500021821354, 100.694938358094, 0),
                    new Vector(73.7000006311573, 103.694907339901, 0),
                    new Vector(73.7000006311573, 103.694907339901, 0),
                    new Vector(73.7499996483093, 105.444872941298, 0),
                    new Vector(73.7499996483093, 105.444872941298, 0),
                    new Vector(73.6999999177642, 108.444810381683, 0),
                    new Vector(73.6999999177642, 108.444810381683, 0),
                    new Vector(71.8000039870385, 128.494775942585, 0),
                    new Vector(71.8000039870385, 128.494775942585, 0),
                    new Vector(71.0500073513249, 135.194745888904, 0),
                    new Vector(71.0500073513249, 135.194745888904, 0),
                    new Vector(68.4860087297857, 162.552724073699, 0),
                    new Vector(68.4860087297857, 162.552724073699, 0),
                    new Vector(63.5960029385751, 186.58379042201, 0),
                    new Vector(63.5960029385751, 186.58379042201, 0),
                    new Vector(62.6180070840055, 199.137737209589, 0),
                    new Vector(62.6180070840055, 199.137737209589, 0),
                    new Vector(60.6850253350567, 209.501639354392, 0),
                    new Vector(60.6850253350567, 209.501639354392, 0),
                    new Vector(55.1500412584282, 228.994583275984, 0),
                    new Vector(55.1500412584282, 228.994583275984, 0),
                    new Vector(52.6000251739752, 238.894641715509, 0),
                    new Vector(52.6000251739752, 238.894641715509, 0),
                    new Vector(51.5000368331093, 242.944598788687, 0),
                    new Vector(51.5000368331093, 242.944598788687, 0),
                    new Vector(50.4500438502291, 246.594574395509, 0),
                    new Vector(50.4500438502291, 246.594574395509, 0),
                    new Vector(48.7500521765323, 251.794548926991, 0),
                    new Vector(48.7500521765323, 251.794548926991, 0),
                    new Vector(43.3000531659927, 267.894546105497, 0),
                    new Vector(43.3000531659927, 267.894546105497, 0),
                    new Vector(35.7500556951854, 289.844538727688, 0),
                    new Vector(35.7500556951854, 289.844538727688, 0),
                    new Vector(20.1000297752908, 339.344625554484, 0),
                    new Vector(20.1000297752908, 339.344625554484, 0),
                    new Vector(18.9000310669653, 344.144620387786, 0),
                    new Vector(18.9000310669653, 344.144620387786, 0),
                    new Vector(17.7500375893433, 348.544595432584, 0),
                    new Vector(17.7500375893433, 348.544595432584, 0),
                    new Vector(16.6500472491607, 352.144563818787, 0),
                    new Vector(16.6500472491607, 352.144563818787, 0),
                    new Vector(15.2000299496576, 357.844624619, 0),
                    new Vector(15.2000299496576, 357.844624619, 0),
                    new Vector(12.4680246328935, 370.864639850799, 0),
                    new Vector(12.4680246328935, 370.864639850799, 0),
                    new Vector(8.50001493457239, 384.306695440609, 0),
                    new Vector(8.50001493457239, 384.306695440609, 0),
                    new Vector(2.19601683621295, 422.721681492811, 0),
                    new Vector(2.19601683621295, 422.721681492811, 0),
                    new Vector(1.95000262721442, 425.54479031291, 0),
                    new Vector(1.95000262721442, 425.54479031291, 0),
                    new Vector(1.65000293427147, 429.244789547694, 0),
                    new Vector(1.65000293427147, 429.244789547694, 0),
                    new Vector(1.25000276230276, 434.894792017585, 0),
                    new Vector(1.25000276230276, 434.894792017585, 0),
                    new Vector(0.0499999650055543, 458.344877798401, 0),
                    new Vector(0.0499999650055543, 458.344877798401, 0),
                    new Vector(1.2500085006468, 504.444998826395, 0),
                    new Vector(1.2500085006468, 504.444998826395, 0),
                    new Vector(8.90002730418928, 543.445100528887, 0),
                    new Vector(8.90002730418928, 543.445100528887, 0),
                    new Vector(10.0000243355753, 548.095087979804, 0),
                    new Vector(10.0000243355753, 548.095087979804, 0),
                    new Vector(11.0500208955491, 552.9950719263, 0),
                    new Vector(11.0500208955491, 552.9950719263, 0),
                    new Vector(13.1500244617928, 562.495089083503, 0),
                    new Vector(13.1500244617928, 562.495089083503, 0),
                    new Vector(19.0001519195503, 585.245398088999, 0),
                    new Vector(19.0001519195503, 585.245398088999, 0),
                    new Vector(20.6001114401734, 587.795334017486, 0),
                    new Vector(20.6001114401734, 587.795334017486, 0),
                    new Vector(21.7500788064208, 590.495257399103, 0),
                    new Vector(21.7500788064208, 590.495257399103, 0),
                    new Vector(22.4000631507952, 592.045220066502, 0),
                    new Vector(22.4000631507952, 592.045220066502, 0),
                    new Vector(22.900035462575, 593.545137001609, 0),
                    new Vector(22.900035462575, 593.545137001609, 0),
                    new Vector(30.1000752202235, 611.095249215985, 0),
                    new Vector(30.1000752202235, 611.095249215985, 0),
                    new Vector(33.6500748861581, 619.845248093101, 0),
                    new Vector(33.6500748861581, 619.845248093101, 0),
                    new Vector(38.2001011713874, 629.295306665183, 0),
                    new Vector(38.2001011713874, 629.295306665183, 0),
                    new Vector(39.0060935351066, 630.988290624984, 0),
                    new Vector(39.0060935351066, 630.988290624984, 0),
                    new Vector(47.6811504903017, 649.997395795304, 0),
                    new Vector(47.6811504903017, 649.997395795304, 0),
                    new Vector(57.8851226116531, 666.422350920009, 0),
                    new Vector(57.8851226116531, 666.422350920009, 0),
                    new Vector(72.1602099273587, 689.005482326611, 0),
                    new Vector(72.1602099273587, 689.005482326611, 0),
                    new Vector(79.2502078374382, 698.355478608399, 0),
                    new Vector(79.2502078374382, 698.355478608399, 0),
                    new Vector(102.420249558869, 727.915529120684, 0),
                    new Vector(102.420249558869, 727.915529120684, 0),
                    new Vector(159.950256809825, 790.69553747421, 0),
                    new Vector(159.950256809825, 790.69553747421, 0),
                    new Vector(173.145656185874, 805.558225577467, 0),
                    new Vector(173.145656185874, 805.558225577467, 0),
                    new Vector(177.045461713618, 804.290488270637, 0),
                    new Vector(177.045461713618, 804.290488270637, 0),
                    new Vector(172.751727973809, 799.744181958784, 0),
                    new Vector(172.751727973809, 799.744181958784, 0),
                    new Vector(165.001731199096, 791.494185391988, 0),
                    new Vector(165.001731199096, 791.494185391988, 0),
                    new Vector(149.801733316854, 775.094187676994, 0));

            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY(
                new Vector(98.537333252060279, 427.81177494282196, 0),
                new Vector(0.12683818126225263, 0.99192342233364161, 0), 18);
            Assert.AreEqual(4, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(17269.087637230135, area, 0.1);
        }

        [TestMethod]
        public void ComplexBoundary_ContainsZeroLengthSegment_ShouldSplitIn2()
        {
            var pLine = new PolyLine(true,
                    new Vector(66.1983257398064, 149.463197875494, 0),
                    new Vector(67.7451785737811, 147.976612121509, 0),
                    new Vector(68.4392654657979, 147.340365802266, 0),
                    new Vector(69.3219739359485, 146.561505387996, 0),
                    new Vector(73.4136604146799, 142.918906575574, 0),
                    new Vector(75.4223066250444, 141.133231529007, 0),
                    new Vector(75.9843263582267, 140.586403170057, 0),
                    new Vector(79.5746232056749, 137.30384605156, 0),
                    new Vector(81.0295383582795, 135.999439450494, 0),
                    new Vector(84.9176769480659, 132.460236233287, 0),
                    new Vector(86.561284603575, 130.970716794623, 0),
                    new Vector(87.7521183119515, 129.928737252831, 0),
                    new Vector(94.6711579106424, 123.607025083701, 0),
                    new Vector(95.2190574367459, 123.113915510897, 0),
                    new Vector(96.537106732345, 121.960622375666, 0),
                    new Vector(98.7518121715401, 120.167765191202, 0),
                    new Vector(99.9773894540138, 119.197516509878, 0),
                    new Vector(101.219195891445, 118.214419895587, 0),
                    new Vector(105.091321656795, 115.259376546664, 0),
                    new Vector(107.071392795129, 113.787016002039, 0),
                    new Vector(110.97137815853, 110.887026691369, 0),
                    new Vector(112.924805079269, 109.43447846931, 0),
                    new Vector(115.82661765035, 107.283134663498, 0),
                    new Vector(119.157774564658, 104.81000307911, 0),
                    new Vector(127.083422142401, 99.0914977799363, 0),
                    new Vector(131.295026662995, 96.0330708493376, 0),
                    new Vector(132.009128572672, 95.5229980540088, 0),
                    new Vector(134.34328906022, 93.8484917896898, 0),
                    new Vector(138.148143035451, 91.2104596228908, 0),
                    new Vector(140.085112399338, 89.8851647958433, 0),
                    new Vector(143.631637054751, 87.5039265485159, 0),
                    new Vector(145.439279151192, 86.3160474584867, 0),
                    new Vector(149.658644088141, 83.6217539455787, 0),
                    new Vector(154.595091885137, 80.5682812107545, 0),
                    new Vector(157.128009340829, 79.069207851417, 0),
                    new Vector(159.151485333352, 77.905708776938, 0),
                    new Vector(162.596678294243, 75.9084953862569, 0),
                    new Vector(169.735334504217, 71.7861728699312, 0),
                    new Vector(170.988656572191, 71.0843125148156, 0),
                    new Vector(172.63748364464, 70.1349883226235, 0),
                    new Vector(176.022377016549, 68.2373965867453, 0),
                    new Vector(179.83742971306, 66.2027017589256, 0),
                    new Vector(184.243455808291, 63.8730788036793, 0),
                    new Vector(188.138334338292, 61.9003480740249, 0),
                    new Vector(191.502195676503, 60.1933139630635, 0),
                    new Vector(194.777766632806, 58.5555284866404, 0),
                    new Vector(199.388693429143, 56.3767389165987, 0),
                    new Vector(201.201777361772, 55.5205605247137, 0),
                    new Vector(204.968999308253, 53.7625236321327, 0),
                    new Vector(223.382814230855, 45.2096587357362, 0),
                    new Vector(224.101550713428, 44.910185201517, 0),
                    new Vector(228.022204628956, 43.2896485099336, 0),
                    new Vector(229.562934660123, 42.7563182177688, 0),
                    new Vector(231.657081633018, 42.0923201620825, 0),
                    new Vector(235.559952471838, 40.8084841941559, 0),
                    new Vector(236.448523469021, 40.5616554678693, 0),
                    new Vector(237.448804657496, 40.2282300375226, 0),
                    new Vector(242.094138221116, 38.84994425418, 0),
                    new Vector(245.224941236404, 37.9204872132116, 0),
                    new Vector(248.399162865104, 36.8949697193714, 0),
                    new Vector(288.040493211407, 23.9712764869728, 0),
                    new Vector(290.169863645586, 23.2926864932038, 0),
                    new Vector(290.769109639374, 23.1668448290098, 0),
                    new Vector(292.074966063236, 22.8930845679023, 0),
                    new Vector(299.721189887266, 21.2842684498881, 0),
                    new Vector(299.721189887266, 21.2842684498881, 0),
                    new Vector(299.721189887266, 21.2842684498881, 0),
                    new Vector(303.087115313176, 20.5760554545767, 0),
                    new Vector(303.603693058177, 20.4712729506797, 0),
                    new Vector(307.850780010263, 19.6131736113663, 0),
                    new Vector(315.79893197657, 18.1135221858497, 0),
                    new Vector(319.493831035147, 17.3645561134507, 0),
                    new Vector(321.619776948008, 16.9596140369489, 0),
                    new Vector(326.308757041102, 16.052069563827, 0),
                    new Vector(335.411740850512, 14.4427022593495, 0),
                    new Vector(343.440810294982, 13.037615106319, 0),
                    new Vector(346.195626608702, 12.5784790525573, 0),
                    new Vector(351.724201945466, 11.716224215642, 0),
                    new Vector(353.931203381332, 11.4082705276809, 0),
                    new Vector(356.17866178677, 11.1017989252311, 0),
                    new Vector(367.208662777232, 9.74193588204331, 0),
                    new Vector(370.011704771046, 9.43614944561543, 0),
                    new Vector(373.860415866066, 9.03102196167171, 0),
                    new Vector(379.321864412203, 8.52533222617298, 0),
                    new Vector(386.456009207172, 7.92244675096021, 0),
                    new Vector(388.396617273575, 7.76510015054028, 0),
                    new Vector(392.185478382412, 7.56029684769094, 0),
                    new Vector(394.162679333615, 7.45890192811974, 0),
                    new Vector(398.696563141162, 7.25513173965407, 0),
                    new Vector(400.913095310845, 7.20358448246177, 0),
                    new Vector(403.123451629139, 7.15334916118121, 0),
                    new Vector(408.141667758836, 7.05397860985769, 0),
                    new Vector(409.701043541316, 7.00199948590001, 0),
                    new Vector(412.001828978635, 7.00199948630495, 0),
                    new Vector(415.003535273362, 7.05287597651965, 0),
                    new Vector(417.914539000597, 7.10306567727676, 0),
                    new Vector(422.603271141264, 7.19974050319386, 0),
                    new Vector(433.348830042895, 6.8072110361265, 0),
                    new Vector(434.508269692188, 6.75199948676106, 0),
                    new Vector(437.25875566777, 6.75199948565017, 0),
                    new Vector(439.303601595657, 6.85962295641269, 0),
                    new Vector(442.393761350558, 7.00914712529686, 0),
                    new Vector(453.552391728293, 7.5595726124343, 0),
                    new Vector(455.50823551975, 7.659872285758, 0),
                    new Vector(455.508235544968, 7.65987228705122, 0),
                    new Vector(457.524618601654, 7.76327654633061, 0),
                    new Vector(460.996669080572, 8.01857437546531, 0),
                    new Vector(466.278684413644, 8.41721710852989, 0),
                    new Vector(474.447577227835, 8.96849215101577, 0),
                    new Vector(479.994695562711, 9.42234735049271, 0),
                    new Vector(483.032266094238, 9.67547817804318, 0),
                    new Vector(489.440313882128, 10.2809629752886, 0),
                    new Vector(492.961306230029, 10.643419450424, 0),
                    new Vector(495.996775585672, 11.0550057310003, 0),
                    new Vector(498.683378535019, 11.4098400196341, 0),
                    new Vector(500.229447369399, 11.6388872527261, 0),
                    new Vector(501.135227916166, 11.8200435590446, 0),
                    new Vector(504.841485307995, 12.5713108117974, 0),
                    new Vector(509.28384753383, 13.4697660960837, 0),
                    new Vector(515.003553334181, 14.6137072897653, 0),
                    new Vector(532.735273120648, 18.0102057643616, 0),
                    new Vector(534.257561133312, 18.3048421549257, 0),
                    new Vector(537.004050001744, 18.8042039773757, 0),
                    new Vector(538.424733466148, 19.0774123349484, 0),
                    new Vector(541.06556624322, 19.6702523878609, 0),
                    new Vector(543.658552070611, 20.3466834759065, 0),
                    new Vector(545.978420758545, 21.1199730361505, 0),
                    new Vector(550.010623144298, 22.5120429925979, 0),
                    new Vector(559.049246230003, 24.9417839947273, 0),
                    new Vector(576.321810269495, 29.2091226424898, 0),
                    new Vector(580.133221262438, 30.6267516928606, 0),
                    new Vector(578.041558247543, 36.2503578163479, 0),
                    new Vector(613.40064293216, 49.4019349451992, 0),
                    new Vector(618.200610861764, 51.2519225852157, 0),
                    new Vector(634.300563673838, 58.4519014694088, 0),
                    new Vector(641.254537679022, 62.0408880538016, 0),
                    new Vector(645.000579572399, 63.7519085796084, 0),
                    new Vector(654.550540477037, 68.1518906950078, 0),
                    new Vector(657.500500517315, 69.8018679880188, 0),
                    new Vector(661.790490508312, 72.3218620693951, 0),
                    new Vector(668.900494204951, 76.4518642306211, 0),
                    new Vector(672.150491239619, 78.3518624970166, 0),
                    new Vector(675.050489706337, 80.0518615989131, 0),
                    new Vector(681.050491794944, 83.5518628357095, 0),
                    new Vector(683.250461841002, 84.8518451354175, 0),
                    new Vector(690.200460495427, 89.3018438318977, 0),
                    new Vector(693.500447311206, 91.4518352411978, 0),
                    new Vector(697.050437483122, 93.8018287384184, 0),
                    new Vector(701.200415454223, 96.651813608798, 0),
                    new Vector(707.350401218864, 101.151803080313, 0),
                    new Vector(709.030396423652, 102.411799483903, 0),
                    new Vector(710.550394005259, 103.551797670894, 0),
                    new Vector(726.900390924769, 115.801795412001, 0),
                    new Vector(748.837282257344, 132.65753590568, 0),
                    new Vector(755.008119717615, 131.944672861556, 0),
                    new Vector(748.931609304855, 126.680209903105, 0),
                    new Vector(743.271614906145, 122.230214306997, 0),
                    new Vector(687.861549692228, 80.6401670862979, 0),
                    new Vector(654.371476912755, 60.0601230729953, 0),
                    new Vector(620.291394899949, 43.1700828798057, 0),
                    new Vector(580.761314036325, 27.7200518244936, 0),
                    new Vector(557.55920730345, 20.0280225170136, 0),
                    new Vector(491.999146732618, 7.00201138140983, 0),
                    new Vector(442.001056279172, 0.900001773319673, 0),
                    new Vector(402.150962600484, 0.150000567606185, 0),
                    new Vector(359.200909152743, 2.6500028645969, 0),
                    new Vector(319.750818823813, 7.05001587301376, 0),
                    new Vector(316.700820211438, 7.60001562270918, 0),
                    new Vector(313.600825778674, 8.15001463500084, 0),
                    new Vector(309.450830925489, 8.85001376690343, 0),
                    new Vector(278.050693085534, 15.7000470742059, 0),
                    new Vector(275.850704976474, 16.4000432908069, 0),
                    new Vector(270.35070894903, 18.0500421351171, 0),
                    new Vector(262.000698023941, 20.6000454823952, 0),
                    new Vector(251.750679356628, 24.0000514826097, 0),
                    new Vector(210.600575828226, 40.0500926894019, 0),
                    new Vector(206.550580672803, 41.9500903383014, 0),
                    new Vector(202.450589012704, 43.8000865795184, 0),
                    new Vector(165.400496766553, 62.6501335873036, 0),
                    new Vector(163.250528758857, 63.850115731213, 0),
                    new Vector(161.950562035898, 64.5000990926928, 0),
                    new Vector(133.050438390463, 82.350170090911, 0),
                    new Vector(96.2503823486622, 108.600210647797, 0),
                    new Vector(93.5503904215293, 110.700204368797, 0),
                    new Vector(61.1502802891191, 138.150301899906, 0),
                    new Vector(56.5502794529311, 142.950302622805, 0),
                    new Vector(52.27752189595, 147.430567319807, 0));

            var region = new PlanarRegion(pLine.ToPolyCurve());
            var subRegions = region.SplitByLineXY(
                new Vector(570.512749406731, 101.03504420188818, 0),
                new Vector(0.11543589479571154, -0.9933149320294713, 0), 18);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(5727.41290942685, area, 0.1);
        }

        [TestMethod]
        public void ComplexBoundary_StartBetweenCutters_ShouldSplitIn2()
        {
            var pLine = new PolyLine(true,
                    new Vector(8.71763598057441, 398.501768934977, 0),
                    new Vector(13.851996373618, 384.681054223096, 0),
                    new Vector(16.5510169707704, 335.529999052989, 0),
                    new Vector(11.3017032489879, 335.430012127501, 0),
                    new Vector(10.901966419071, 334.330735845986, 0),
                    new Vector(10.4519851412624, 331.880837775301, 0),
                    new Vector(10.201990161906, 330.13087291838, 0),
                    new Vector(9.95199484587647, 327.88091507589, 0),
                    new Vector(9.85199678933714, 326.130949095183, 0),
                    new Vector(9.75199800310656, 323.780977608578, 0),
                    new Vector(9.75199800310656, 322.0310274199, 0),
                    new Vector(9.8519958788529, 320.281064588082, 0),
                    new Vector(10.5329952662578, 311.144072850497, 0),
                    new Vector(16.035995235783, 237.328073217475, 0),
                    new Vector(16.2519247075543, 234.432019145199, 0),
                    new Vector(25.5001824944047, 234.631981478597, 0),
                    new Vector(36.450029437081, 285.181248095294, 0),
                    new Vector(47.7000469694613, 327.0313079599, 0),
                    new Vector(59.3000715703238, 361.881375334982, 0),
                    new Vector(71.449657735182, 391.780356924282, 0),
                    new Vector(59.4040163194295, 395.40518939108, 0),
                    new Vector(60.2420162452618, 399.7231890127, 0),
                    new Vector(69.4104326738743, 398.672089165077, 0),
                    new Vector(75.2501063141972, 410.081451536389, 0),
                    new Vector(76.700104572461, 412.961448074697, 0),
                    new Vector(85.55010177847, 430.58144251458, 0),
                    new Vector(97.2001422576141, 454.181516270095, 0),
                    new Vector(106.300127669354, 469.2314921527, 0),
                    new Vector(108.849111559568, 474.041462055873, 0),
                    new Vector(118.800178160542, 490.081571463495, 0),
                    new Vector(120.5601776205, 492.601570689993, 0),
                    new Vector(124.650172593654, 498.4815634634, 0),
                    new Vector(136.250168759958, 515.581557800499, 0),
                    new Vector(140.650165042258, 522.181552210794, 0),
                    new Vector(149.300171386451, 535.081561997882, 0),
                    new Vector(156.300153870136, 545.731535295374, 0),
                    new Vector(159.650149518973, 551.131528112601, 0),
                    new Vector(169.600153661217, 566.981534723076, 0),
                    new Vector(174.200155764702, 574.231537992775, 0),
                    new Vector(174.790153472801, 575.161534380488, 0),
                    new Vector(176.800156503683, 578.281539158896, 0),
                    new Vector(189.300173516735, 596.531564762583, 0),
                    new Vector(193.200175032718, 602.181566970801, 0),
                    new Vector(198.945173713495, 610.320565213973, 0),
                    new Vector(198.953152754111, 610.332533776178, 0),
                    new Vector(198.966193227563, 610.350592786097, 0),
                    new Vector(200.291367403581, 612.221928145096, 0),
                    new Vector(201.900731622241, 611.582180876285, 0),
                    new Vector(213.317264142213, 623.858678179677, 0),
                    new Vector(213.441267418559, 623.991681700485, 0),
                    new Vector(228.149144792464, 635.980807445798, 0),
                    new Vector(236.351361346547, 642.181931386673, 0),
                    new Vector(239.401311495691, 641.131948549475, 0),
                    new Vector(252.721297175623, 636.921953075973, 0),
                    new Vector(261.900757823838, 633.964128073276, 0),
                    new Vector(261.92036203912, 633.981765542791, 0),
                    new Vector(273.621300165192, 630.221952138498, 0),
                    new Vector(277.607683791779, 628.974271070474, 0),
                    new Vector(257.651717341621, 610.170302684099, 0),
                    new Vector(255.30166987027, 607.980257681396, 0),
                    new Vector(254.101697588456, 606.830284244497, 0),
                    new Vector(252.901712630526, 605.630299287091, 0),
                    new Vector(250.651713332045, 603.330299988389, 0),
                    new Vector(217.051614885451, 574.230211818183, 0),
                    new Vector(215.201620140346, 572.780215935491, 0),
                    new Vector(213.401633330388, 571.330226562801, 0),
                    new Vector(207.351642435184, 566.380233378877, 0),
                    new Vector(204.201682749088, 563.530269852286, 0),
                    new Vector(202.20169610437, 561.580282873882, 0),
                    new Vector(200.151700746967, 559.580287403689, 0),
                    new Vector(195.701693016337, 555.230279854179, 0),
                    new Vector(193.451697909855, 553.030284637382, 0),
                    new Vector(190.651698711328, 550.280285424698, 0),
                    new Vector(182.101698000217, 541.930285279494, 0),
                    new Vector(172.201698181918, 532.380285044695, 0),
                    new Vector(170.391727611306, 530.510315449676, 0),
                    new Vector(167.20173978305, 527.130327882973, 0),
                    new Vector(164.017755362787, 523.4783420194, 0),
                    new Vector(162.701748783235, 521.980338320282, 0),
                    new Vector(161.120753506315, 520.178343702486, 0),
                    new Vector(161.114768286701, 520.171360946086, 0),
                    new Vector(160.201756460126, 519.130347069498, 0),
                    new Vector(158.814764330862, 517.483356417099, 0),
                    new Vector(158.795751911239, 517.461341700895, 0),
                    new Vector(155.991774950875, 514.140368984576, 0),
                    new Vector(155.976735187462, 514.123321843974, 0),
                    new Vector(152.851765025407, 510.430357191683, 0),
                    new Vector(152.340773329604, 509.808367298974, 0),
                    new Vector(152.306766580557, 509.767358815501, 0),
                    new Vector(150.151778998668, 507.130374017783, 0),
                    new Vector(149.151785568451, 505.880382092786, 0),
                    new Vector(148.001813886571, 504.330420259095, 0),
                    new Vector(145.176827392192, 500.294439099584, 0),
                    new Vector(144.701817561989, 499.630425772775, 0),
                    new Vector(143.932821110822, 498.505430760182, 0),
                    new Vector(142.451828768244, 496.380441955873, 0),
                    new Vector(141.206853022915, 494.448479592393, 0),
                    new Vector(140.551842061104, 493.430462588789, 0),
                    new Vector(138.701850428479, 490.480475931196, 0),
                    new Vector(134.701856623986, 483.930486159399, 0),
                    new Vector(133.651869021123, 482.130507414375, 0),
                    new Vector(132.451884446898, 479.930535691994, 0),
                    new Vector(131.901897187694, 478.8305611744, 0),
                    new Vector(131.451911129639, 477.880590607499, 0),
                    new Vector(130.601906909375, 475.980581699085, 0),
                    new Vector(129.801934653195, 474.180644121196, 0),
                    new Vector(129.001956358436, 471.730711661599, 0),
                    new Vector(128.801973185269, 470.980774762487, 0),
                    new Vector(128.501985757728, 469.43083971928, 0),
                    new Vector(128.301993999747, 467.880903595593, 0),
                    new Vector(128.101996407611, 465.030938994692, 0),
                    new Vector(128.000672923867, 462.5300531869, 0),
                    new Vector(121.601472130395, 464.729778460198, 0),
                    new Vector(120.071876917384, 461.920521892782, 0),
                    new Vector(117.421876792447, 457.04052166178, 0),
                    new Vector(114.771878081025, 452.160524025385, 0),
                    new Vector(114.543878879398, 451.735525518277, 0),
                    new Vector(106.301878847298, 436.480525460094, 0),
                    new Vector(103.151884018211, 430.58053514568, 0),
                    new Vector(101.171888217214, 426.720543331699, 0),
                    new Vector(93.5218883475754, 411.790543586976, 0),
                    new Vector(92.7518897680566, 410.280546370574, 0),
                    new Vector(91.2118906675605, 407.230548153282, 0),
                    new Vector(85.8188899343368, 396.593546699674, 0),
                    new Vector(73.3518681795103, 398.179866924387, 0),
                    new Vector(73.0521085279761, 395.7018538056, 0),
                    new Vector(84.3519660799066, 394.120747641078, 0),
                    new Vector(81.0519661195576, 381.430748370592, 0),
                    new Vector(78.9219660771778, 373.230748195085, 0),
                    new Vector(78.6019830532605, 371.980814508075, 0),
                    new Vector(78.4519909552764, 370.730880355201, 0),
                    new Vector(77.9519907837966, 366.580878923472, 0),
                    new Vector(77.3019943205873, 362.130889667897, 0),
                    new Vector(77.2519980034558, 360.930978050194, 0),
                    new Vector(77.2519980033394, 359.081008548674, 0),
                    new Vector(77.2120008043712, 357.080939567881, 0),
                    new Vector(78.0519976278301, 324.2810267967, 0),
                    new Vector(78.101997428108, 322.781032785686, 0),
                    new Vector(78.2519947547698, 318.431110319274, 0),
                    new Vector(78.6519799680682, 316.331187946198, 0),
                    new Vector(79.011979272007, 314.481191525876, 0),
                    new Vector(80.0319787702756, 309.33119406199, 0),
                    new Vector(81.4019795244094, 302.081190338184, 0),
                    new Vector(82.6719780846033, 295.7811974746, 0),
                    new Vector(83.2019770237384, 293.18120268028, 0),
                    new Vector(83.5419760737568, 291.58120715039, 0),
                    new Vector(84.641975882696, 286.431208043185, 0),
                    new Vector(89.7209838645067, 258.482166806381, 0),
                    new Vector(89.723989060265, 258.460142609896, 0),
                    new Vector(90.3719855959062, 254.381155914889, 0),
                    new Vector(92.5519828217803, 240.681173346587, 0),
                    new Vector(92.712960163258, 239.264589849804, 0),
                    new Vector(13.8243152517424, 223.950132038221, 0),
                    new Vector(12.3500005892711, 243.430925422785, 0),
                    new Vector(7.80000080936588, 301.530922588398, 0),
                    new Vector(7.16000142158009, 309.180915270001, 0),
                    new Vector(6.68108215520624, 314.821995814273, 0),
                    new Vector(8.68995472544339, 314.65209116909, 0),
                    new Vector(8.64999958698172, 315.680935983401, 0),
                    new Vector(7.60000200441573, 327.780908512475, 0),
                    new Vector(6.20000180369243, 342.980904340395, 0),
                    new Vector(3.75000208895653, 369.08090739639, 0),
                    new Vector(2.80000331974588, 378.580895089573, 0),
                    new Vector(0.350083564175293, 400.930162430974, 0),
                    new Vector(0.0011677265865728, 401.791984367883, 0));

            var region = new PlanarRegion(pLine.ToPolyCurve());
            var subRegions = region.SplitByLineXY(
                new Vector(163.67820600348261, 391.02656774606533, 0),
                new Vector(-0.99997617294925267, -0.0069031538999340114, 0), 18);
            Assert.AreEqual(4, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(14047.9084451208, area, 0.1);
        }


        [TestMethod]
        public void ComplexBoundary_StartBetweenCutters2_ShouldSplitIn2()
        {
            var pLine = new PolyLine(true,
                    new Vector(265.258961987198, 31.6172281807558, 0),
                    new Vector(264.856446098769, 33.3007668571954, 0),
                    new Vector(264.85644586198, 33.300767849898, 0),
                    new Vector(263.516445862129, 38.9307678478071, 0),
                    new Vector(263.096490117139, 40.7005813434953, 0),
                    new Vector(258.066601079074, 47.8504236158042, 0),
                    new Vector(256.536601406406, 50.0204231526004, 0),
                    new Vector(256.536600353895, 50.0204246482172, 0),
                    new Vector(252.926600459148, 55.1604244959017, 0),
                    new Vector(249.306600575917, 60.3104243311973, 0),
                    new Vector(248.456814003177, 61.5201205100166, 0),
                    new Vector(238.497174319462, 64.0300297070935, 0),
                    new Vector(237.627173530404, 64.2500299068925, 0),
                    new Vector(237.62707517948, 64.2500602405053, 0),
                    new Vector(223.95708283456, 69.2500574415026, 0),
                    new Vector(211.867090530228, 73.4500547671923, 0),
                    new Vector(211.867089106236, 73.4500552630925, 0),
                    new Vector(205.307089105598, 75.7400552642939, 0),
                    new Vector(205.307087516761, 75.7400558204972, 0),
                    new Vector(201.71708751691, 77.0000558200991, 0),
                    new Vector(201.7170818788, 77.0000578179024, 0),
                    new Vector(176.41708187887, 86.0500578189967, 0),
                    new Vector(176.417078782688, 86.0500589323055, 0),
                    new Vector(149.717078782385, 95.7000589317176, 0),
                    new Vector(141.567081059911, 98.65005810812, 0),
                    new Vector(137.919490826846, 99.946490780936, 0),
                    new Vector(139.456677256188, 106.80051088078, 0),
                    new Vector(171.517857043073, 90.7018986823095, 0),
                    new Vector(172.467837843345, 90.2519077768957, 0),
                    new Vector(173.467817847966, 89.8019167747989, 0),
                    new Vector(174.417783702258, 89.4019311518932, 0),
                    new Vector(175.517741184449, 89.0019466130179, 0),
                    new Vector(177.717695426429, 88.3019611723139, 0),
                    new Vector(185.266718560364, 86.352213475504, 0),
                    new Vector(190.266185672372, 104.300300407602, 0),
                    new Vector(141.865622686347, 117.541537526982, 0),
                    new Vector(146.109786042644, 136.465449679641, 0),
                    new Vector(382.917677135789, 71.0019658913079, 0),
                    new Vector(384.153669146006, 70.6819679599139, 0),
                    new Vector(391.769668881781, 68.7129680282087, 0),
                    new Vector(391.769859257969, 68.712897524907, 0),
                    new Vector(391.770031472086, 68.7127900255146, 0),
                    new Vector(391.770178426406, 68.7126499604201, 0),
                    new Vector(391.770294064423, 68.7124831021938, 0),
                    new Vector(393.018068123376, 66.4518924393051, 0),
                    new Vector(401.516761381761, 64.8021461009048, 0),
                    new Vector(402.166475116857, 66.6513313470932, 0),
                    new Vector(402.166553318617, 66.651501187298, 0),
                    new Vector(402.166661770898, 66.651653500594, 0),
                    new Vector(402.166796682053, 66.6517829617951, 0),
                    new Vector(402.1669533354, 66.6518850448192, 0),
                    new Vector(402.167126254179, 66.6519561808091, 0),
                    new Vector(402.167309392942, 66.6519938826095, 0),
                    new Vector(402.167496348848, 66.6519968323119, 0),
                    new Vector(402.167680585757, 66.6519649265974, 0),
                    new Vector(413.217680585803, 63.6519649265974, 0),
                    new Vector(413.217863322468, 63.6518955180945, 0),
                    new Vector(413.218029065407, 63.6517918863974, 0),
                    new Vector(413.218171481392, 63.6516579911113, 0),
                    new Vector(413.218285128707, 63.6514989486022, 0),
                    new Vector(413.218365664827, 63.6513208358083, 0),
                    new Vector(413.218410012545, 63.6511304585147, 0),
                    new Vector(413.218416477321, 63.6509350910201, 0),
                    new Vector(413.218384812004, 63.6507421985152, 0),
                    new Vector(413.018396997824, 62.9007878949051, 0),
                    new Vector(412.728573544999, 61.1718410910107, 0),
                    new Vector(414.566686980193, 60.9021179238043, 0),
                    new Vector(415.166453813203, 63.1012629785109, 0),
                    new Vector(415.166524394415, 63.101447563502, 0),
                    new Vector(415.166629896383, 63.1016146641923, 0),
                    new Vector(415.166766198934, 63.1017577548046, 0),
                    new Vector(415.166927978978, 63.1018712471123, 0),
                    new Vector(415.16710891854, 63.1019507088931, 0),
                    new Vector(415.167301951326, 63.1019930370094, 0),
                    new Vector(415.167499538627, 63.1019965783053, 0),
                    new Vector(415.167693964206, 63.1019611945085, 0),
                    new Vector(424.697693964234, 60.371961194498, 0),
                    new Vector(424.697694942006, 60.371960913908, 0),
                    new Vector(432.417694941978, 58.1519609139068, 0),
                    new Vector(432.417699343525, 58.1519596372091, 0),
                    new Vector(440.734699343564, 55.7189596372191, 0),
                    new Vector(443.867699482711, 54.8019595965161, 0),
                    new Vector(443.867718198686, 54.8019539192028, 0),
                    new Vector(449.917718198616, 52.9019539192086, 0),
                    new Vector(449.917761531426, 52.9019392132177, 0),
                    new Vector(455.395761531428, 50.9019392132177, 0),
                    new Vector(472.566761624301, 44.6309391792165, 0),
                    new Vector(474.069761674502, 44.0819391609111, 0),
                    new Vector(521.859719644766, 26.6309545084951, 0),
                    new Vector(533.767676507356, 23.4519660246151, 0),
                    new Vector(533.767684420105, 23.4519638775091, 0),
                    new Vector(551.717679203022, 18.5019653160998, 0),
                    new Vector(572.917673968128, 12.9019666989043, 0),
                    new Vector(572.91768169438, 12.9019646249071, 0),
                    new Vector(593.267681694473, 7.35196462491876, 0),
                    new Vector(593.267871998949, 7.35189115721732, 0),
                    new Vector(593.268043434946, 7.35178059959435, 0),
                    new Vector(593.268188868649, 7.35163755301619, 0),
                    new Vector(593.268302247976, 7.35146796991467, 0),
                    new Vector(593.268378854846, 7.35127890721196, 0),
                    new Vector(593.268415501341, 7.35107823251747, 0),
                    new Vector(593.268410662538, 7.35087429659325, 0),
                    new Vector(593.268364539836, 7.35067558570881, 0),
                    new Vector(593.268279052456, 7.35049036910641, 0),
                    new Vector(591.018279052456, 3.55049036911805, 0),
                    new Vector(591.018168719253, 3.55033858449315, 0),
                    new Vector(591.018031972111, 3.55021008479525, 0),
                    new Vector(591.017873626086, 3.55010939470958, 0),
                    new Vector(591.015595742851, 3.54894534361665, 0),
                    new Vector(584.074547787895, 0.0035179707047064, 0),
                    new Vector(584.067873406457, 0.0001092824968509, 0),
                    new Vector(584.067660573171, 2.95838981401E-05, 0),
                    new Vector(584.067435240955, 0, 0),
                    new Vector(584.067209047964, 2.20586953219E-05, 0),
                    new Vector(564.467209047987, 4.20002205870696, 0),
                    new Vector(564.467139082146, 4.20003971390543, 0),
                    new Vector(549.867139082169, 8.45003971390543, 0),
                    new Vector(549.8671240689, 8.45004421219346, 0),
                    new Vector(533.967127649114, 13.3500431088032, 0),
                    new Vector(510.467136048595, 20.400040589011, 0),
                    new Vector(497.667160511366, 24.1000335176941, 0),
                    new Vector(487.06718271703, 26.7000280710054, 0),
                    new Vector(456.667185083032, 34.0000275029161, 0),
                    new Vector(455.417187373037, 34.3000269532204, 0),
                    new Vector(454.721439566234, 34.463732319574, 0));

            var region = new PlanarRegion(pLine.ToPolyCurve());
            var subRegions = region.SplitByLineXY(
                new Vector(262.530482240542, 30.840508528449426, 0),
                new Vector(0.96178849935535515, 0.27379350340680852, 0), 18);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(12522.667926643599, area, 0.1);
        }

        [TestMethod]
        public void Not_OverlappingSquares_ShouldCreateL()
        {
            var plineA = new PolyLine(true, Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var plineB = new PolyLine(true, Vector.Create2D(5, 5, 15, 5, 15, 15, 5, 15));
            var regionA = new PlanarRegion(plineA);
            var regionB = new PlanarRegion(plineB);
            var result = regionA.Not(regionB);
            Assert.AreEqual(75, result.CalculateTotalArea());
        }

        [TestMethod]
        public void Not_OverlappingSquaresWithVoid_ShouldCreateL()
        {
            var plineA = new PolyLine(true, Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var plineB = new PolyLine(true, Vector.Create2D(5, 5, 15, 5, 15, 15, 5, 15));
            var plineV = new PolyLine(true, Vector.Create2D(1, 1, 9, 1, 9, 9, 1, 9));
            var regionA = new PlanarRegion(plineA, plineV);
            var regionB = new PlanarRegion(plineB);
            var result = regionA.Not(regionB);
            Assert.AreEqual(27, result.CalculateTotalArea());
        }

        [TestMethod]
        public void Not_ContainingSquare_ShouldCreateL()
        {
            var plineA = new PolyLine(true, Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            //var plineB = new PolyLine(true, Vector.Create2D(5, 5, 15, 5, 15, 15, 5, 15));
            var plineV = new PolyLine(true, Vector.Create2D(1, 1, 9, 1, 9, 9, 1, 9));
            var regionA = new PlanarRegion(plineA);
            var regionB = new PlanarRegion(plineV);
            var result = regionA.Not(regionB);
            Assert.AreEqual(36, result.CalculateTotalArea());
        }

        [TestMethod]
        public void Not_ContainingSquareOverlappingVoid_ShouldCreateO()
        {
            var plineA = new PolyLine(true, Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var plineB = new PolyLine(true, Vector.Create2D(4, 4, 9, 4, 9, 9, 4, 9));
            var plineV = new PolyLine(true, Vector.Create2D(1, 1, 6, 1, 6, 6, 1, 6));
            var regionA = new PlanarRegion(plineA, plineV);
            var regionB = new PlanarRegion(plineB);
            var result = regionA.Not(regionB);
            Assert.AreEqual(54, result.CalculateTotalArea());
        }

        [TestMethod]
        public void Not_OverlappingSquareWithVoidL_ShouldCreate2Ls()
        {
            var plineA = new PolyLine(true, Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var plineB = new PolyLine(true, Vector.Create2D(5, 5, 15, 5, 15, 6, 6, 6, 6,15, 5,15));
            var plineV = new PolyLine(true, Vector.Create2D(1, 1, 9, 1, 9, 9, 1, 9));
            var regionA = new PlanarRegion(plineA, plineV);
            var regionB = new PlanarRegion(plineB);
            var result = regionA.Not(regionB);
            Assert.AreEqual(34, result.CalculateTotalArea());
        }

        [TestMethod]
        public void Not_OverlappingSquaresInnerVoid_ShouldCreateLWithVoid()
        {
            var plineA = new PolyLine(true, Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var plineB = new PolyLine(true, Vector.Create2D(5, 5, 15, 5, 15, 15, 5, 15));
            var plineV = new PolyLine(true, Vector.Create2D(1, 1, 4, 1, 4, 4, 1, 4));
            var regionA = new PlanarRegion(plineA, plineV);
            var regionB = new PlanarRegion(plineB);
            var result = regionA.Not(regionB);
            Assert.AreEqual(66, result.CalculateTotalArea());
        }

        [TestMethod]
        public void Not_OverlappingSquaresStartInside_ShouldCreateInverseL()
        {
            var plineA = new PolyLine(true, Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var plineB = new PolyLine(true, Vector.Create2D(-5, -5, 5, -5, 5, 5, -5, 5));
            var regionA = new PlanarRegion(plineA);
            var regionB = new PlanarRegion(plineB);
            var result = regionA.Not(regionB);
            Assert.AreEqual(75, result.CalculateTotalArea());
        }

        [TestMethod]
        public void Not_OverlappingCircles_ShouldTakeBite()
        {
            var circleA = new Arc(new Circle(5, new Vector(-2, 0)));
            var circleB = new Arc(new Circle(5, new Vector(2, 0)));
            var regionA = new PlanarRegion(circleA);
            var regionB = new PlanarRegion(circleB);
            var result = regionA.Not(regionB);
            double area = result.CalculateTotalArea();
            Assert.AreEqual(38.906, area, 0.001);
        }

        [TestMethod]
        public void Not_SliceThroughSquare_ShouldCutIn2()
        {
            var square = PolyLine.Rectangle(100, 100);
            var pSquare = square.ToPolyCurve();
            var line = new Line(-60, 5, 60, -5);
            double width = 16;
            var perimeter = new PolyCurve(line.Offset(width / 2), true);
            perimeter.Add(line.Offset(-width / 2).Reversed(), true, true);
            perimeter.Close();
            var cutter = new PlanarRegion(perimeter);
            var region = new PlanarRegion(pSquare);
            var result = region.Not(cutter);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void ContainsXY_PointInside_ShouldBeTrue()
        {
            var pLine = new PolyLine(true,
                new Vector(-58.0235137939453, 60.5225067138672, 0),
                new Vector(58.9555130004883, 63.0466690063477, 0),
                new Vector(54.3530426025391, -57.5168228149414, 0),
                new Vector(-54.6571044921875, -53.6499099731445, 0)
                );
            var region = new PlanarRegion(pLine);
            var testPt = new Vector(-42, -50, 0);
            var result = region.ContainsXY(testPt);
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Test to check a doughnut shape void is cutting correctly
        /// </summary>
        [TestMethod]
        public void SquareRegionCentralAreaRemoval()
        {
            var regionPLine = new PolyLine(true,
                Vector.Create2D(0, 0, 20, 0, 20, 20, 0, 20));
            var region = new PlanarRegion(regionPLine);

            var voidPLine =  new PolyLine(true,
                Vector.Create2D(5, 5, 15, 5, 15, 15, 5, 15));
            var voidRegion = new PlanarRegion(voidPLine);
            var perimeterMappers = new List<CurveParameterMapper>();

            var subRegions = region.Not(voidRegion);

            var numberOfRegions = subRegions.Count;
            Assert.AreEqual(1, numberOfRegions);

            var hasVoids = subRegions[0].HasVoids;
            Assert.AreEqual(true, hasVoids);

            var numberOfVoids = subRegions[0].Voids.Count;
            Assert.AreEqual(1, numberOfVoids);

            var areaOfRegion = subRegions[0].CalculateArea().Abs();
            Assert.AreEqual(300, areaOfRegion);

            var areaOfVoid = subRegions[0].Voids.TotalEnclosedArea();
            Assert.AreEqual(100, areaOfVoid);
        }

    }
}
