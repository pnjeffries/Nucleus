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
    public class PlanarRegionTests
    {
        [TestMethod]
        public void SquareRegionOrthogonalSplitByLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(0, 1), 1);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(0, 1));
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(0, 1), 1);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(Angle.FromDegrees(10)), 1);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(Angle.FromDegrees(45)), 1);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(0, 1), 1);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(0, 1), 0);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(0, 1), 1);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(Angle.FromDegrees(45)), 1);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(Angle.FromDegrees(45)), 0);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(0, 1), 1);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(1, 0), 1);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 7.5), new Vector(1, 0), 1);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(0, 1), 1);
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
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(0, 1), 1);
            Assert.AreEqual(4, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(34, area, 0.0001);
        }

    }
}
