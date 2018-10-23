using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.Analysis;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class SolarPositioningTests
    {
        [TestMethod]
        public void TestSolarPositioning()
        {
            DateTime date = new DateTime(2003, 10, 17, 12 + 7, 30, 30);

            double jD = SolarPositioning.CalculateJulianDay(date);
            Assert.AreEqual(2452930.312847, jD , 0.001);

            double jM = SolarPositioning.CalculateJulianMillenium(date);

            Angle L = SolarPositioning.CalculateEarthHeliocentricLongitude(jM);
            Assert.AreEqual(24.0182616917, L.Degrees,  0.001);

            Angle B = SolarPositioning.CalculateEarthHeliocentricLatitude(jM);
            Assert.AreEqual(-0.0001011219, B.Degrees, 0.001);
        }
    }
}
