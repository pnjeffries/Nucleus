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
        public void CRegionOrthogonalSplitByLine()
        {
            var pLine = new PolyLine(true,
                Vector.Create2D(0, 0, 10, 0, 10,1, 1,1, 1,9, 10, 9, 10, 10, 0, 10));
            var region = new PlanarRegion(pLine);
            var subRegions = region.SplitByLineXY2(new Vector(5, 5), new Vector(0, 1), 1);
            Assert.AreEqual(3, subRegions.Count);
            double area = subRegions.CalculateTotalArea();
            Assert.AreEqual(34, area, 0.0001);
        }

    }
}
