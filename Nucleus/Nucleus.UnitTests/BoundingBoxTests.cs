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
    public class BoundingBoxTests
    {
        [TestMethod]
        public void HorizontalLineBoundingBoxShouldEnclose()
        {
            var line = new Line(0, 0, 10, 0);
            var bBox = line.BoundingBox;
            Assert.AreEqual(10, bBox.SizeX);
            Assert.AreEqual(0, bBox.SizeY);
        }

        [TestMethod]
        public void VerticalLineBoundingBoxShouldEnclose()
        {
            var line = new Line(0, 0, 0, 10);
            var bBox = line.BoundingBox;
            Assert.AreEqual(0, bBox.SizeX);
            Assert.AreEqual(10, bBox.SizeY);
        }

        [TestMethod]
        public void CollectionContainingHorizontalLineBoundingBoxShouldEnclose()
        {
            var line = new Line(0, 0, 10, 0);
            var vGCol = new VertexGeometryCollection(line);
            var bBox = vGCol.BoundingBox;
            Assert.AreEqual(10, bBox.SizeX);
            Assert.AreEqual(0, bBox.SizeY);
        }

        [TestMethod]
        public void XAxisDistance_ShouldBe10()
        {
            var bB1 = new BoundingBox(0, 1, 0, 1);
            var bB2 = new BoundingBox(11, 12, 5, 6);
            var dist = bB1.XAxisDistanceTo(bB2);
            Assert.AreEqual(10, dist);
            var dist2 = bB2.XAxisDistanceTo(bB1);
            Assert.AreEqual(10, dist2);
        }

        [TestMethod]
        public void YAxisDistance_ShouldBe4()
        {
            var bB1 = new BoundingBox(0, 1, 0, 1);
            var bB2 = new BoundingBox(11, 12, 5, 6);
            var dist = bB1.YAxisDistanceTo(bB2);
            Assert.AreEqual(4, dist);
            var dist2 = bB2.YAxisDistanceTo(bB1);
            Assert.AreEqual(4, dist2);
        }

        [TestMethod]
        public void ShortestXYAxisDistance_ShouldBe4()
        {
            var bB1 = new BoundingBox(0, 1, 0, 1);
            var bB2 = new BoundingBox(11, 12, 5, 6);
            var dist = bB1.ShortestXYAxisDistanceTo(bB2);
            Assert.AreEqual(4, dist);
            var dist2 = bB2.ShortestXYAxisDistanceTo(bB1);
            Assert.AreEqual(4, dist2);
        }

        [TestMethod]
        public void XAxisDistance_Overlap_ShouldBe1()
        {
            var bB1 = new BoundingBox(0, 10, 0, 10);
            var bB2 = new BoundingBox(11, 12, 5, 6);
            var dist = bB1.XAxisDistanceTo(bB2);
            Assert.AreEqual(1, dist);
            var dist2 = bB2.XAxisDistanceTo(bB1);
            Assert.AreEqual(1, dist2);
        }
    }
}
