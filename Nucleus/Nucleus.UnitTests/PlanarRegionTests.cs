using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.Extensions;
using Nucleus.Geometry;
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
            var subRegions = region.SplitByLineXY(new Vector(5, 5), new Vector(0, 1), 1);
            Assert.AreEqual(2, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(90, area, 0.0001);
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



        [TestMethod]
        public void ComplexBoundaryWithZeroThicknessBit_ShouldSelectCorrectInside()
        {
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

    }
}
