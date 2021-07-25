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
    public class CellMapTests
    {
        [TestMethod]
        public void Contour_Square()
        {
            var map = new SquareCellMap<int>(5, 5,
                1, 1, 1, 1, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                1, 1, 1, 1, 1);

            var pLines = map.Contour(i => i == 0);
            Assert.AreEqual(1, pLines.Count);
            var length = pLines[0].Length;
            Assert.AreEqual(12, length);
        }

        [TestMethod]
        public void Contour_Irregular()
        {
            var map = new SquareCellMap<int>(5, 5,
                1, 1, 1, 1, 1,
                1, 0, 1, 0, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 1, 1,
                1, 1, 1, 1, 1);

            var pLines = map.Contour(i => i == 0);
            Assert.AreEqual(1, pLines.Count);
            var length = pLines[0].Length;
            Assert.AreEqual(14, length);
        }

        [TestMethod]
        public void Contour_Island()
        {
            var map = new SquareCellMap<int>(5, 5,
                1, 1, 1, 1, 1,
                1, 0, 0, 0, 1,
                1, 0, 1, 0, 1,
                1, 0, 0, 0, 1,
                1, 1, 1, 1, 1);

            var pLines = map.Contour(i => i == 0);
            Assert.AreEqual(2, pLines.Count);
        }

        [TestMethod]
        public void Contour_Separate()
        {
            var map = new SquareCellMap<int>(5, 5,
                1, 1, 1, 1, 1,
                1, 0, 1, 0, 1,
                1, 0, 1, 0, 1,
                1, 0, 1, 0, 1,
                1, 1, 1, 1, 1);

            var pLines = map.Contour(i => i == 0);
            Assert.AreEqual(2, pLines.Count);
        }
    }
}
