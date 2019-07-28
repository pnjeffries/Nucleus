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
    }
}
