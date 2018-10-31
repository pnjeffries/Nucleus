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
            Angle longitude = Angle.FromDegrees(-105.1786);
            Angle latitude = Angle.FromDegrees(39.742476);
            double altitude = 1830.14;
            double pressure = 820; //mbar
            double temperature = 11; //C
            DateTime date = new DateTime(2003, 10, 17, 12 + 7, 30, 30);
            double deltaT = 67;

            double jD = SolarPositioning.CalculateJulianDay(date);
            Assert.AreEqual(2452930.312847, jD , 0.000001);
            double jDE = jD + deltaT / 86400.0; //JDE

            double jC = SolarPositioning.CalculateJulianCentury(jD);
            double jCE = SolarPositioning.CalculateJulianCentury(jDE);
            Assert.AreEqual(0.037928, jC, 0.00001);

            double jM = SolarPositioning.CalculateJulianMillenium(jC);
            double jME = SolarPositioning.CalculateJulianMillenium(jCE);
            Angle L = SolarPositioning.CalculateEarthHeliocentricLongitude(jME);
            Assert.AreEqual(24.0182616917, L.Degrees,  0.00001);

            Angle B = SolarPositioning.CalculateEarthHeliocentricLatitude(jME);
            Assert.AreEqual(-0.0001011219, B.Degrees, 0.00001);

            double R = SolarPositioning.CalculateEarthRadiusVector(jME);
            Assert.AreEqual(0.9965422974, R, 0.00001);

            Angle[] X = SolarPositioning.CalculateNutationCoefficients(jCE);
            Angle deltaR = SolarPositioning.CalculateNutationInLongitude(jCE, X);
            Angle deltaG = SolarPositioning.CalculateNutationInObliquity(jCE, X);

            Assert.AreEqual(-0.00399840, deltaR.Degrees, 0.00001);
            Assert.AreEqual(0.00166657, deltaG.Degrees, 0.00001);

            Angle e = SolarPositioning.CalculateTrueObliquityOfTheEliptic(jME, deltaG);
            Assert.AreEqual(23.440465, e.Degrees, 0.00001);

            Angle gcLat = SolarPositioning.CalculateGeocentricLatitude(B);
            Assert.AreEqual(0.0001011219, gcLat.Degrees, 0.00001);
            Angle gcLong = SolarPositioning.CalculateGeocentricLongitude(L);
            Assert.AreEqual(204.0182616917, gcLong.Degrees, 0.00001);
            Angle deltaTau = SolarPositioning.CalculateAberrationCorrection(R);

            Angle lamda = SolarPositioning.CalculateApparentSunLongitude(gcLong, deltaR, deltaTau);
            Assert.AreEqual(204.0085519281, lamda.Degrees, 0.00001);

            Angle v = SolarPositioning.CalculateApparentSiderealTime(jD, jC, deltaR, e);
            Assert.AreEqual(318.51191, v.Degrees, 0.00001);
            Angle alpha = SolarPositioning.CalculateGeocentricSunRightAscension(lamda, e, gcLat);
            Assert.AreEqual(202.22741, alpha.Degrees, 0.00001);

            Angle delta = SolarPositioning.CalculateGeocentricSunDeclination(lamda, e, gcLat);
            Assert.AreEqual(-9.31434, delta.Degrees, 0.00001);

            Angle H = SolarPositioning.CalculateObserverLocalHourAngle(v, longitude, alpha);
            Assert.AreEqual(11.105900, H.Degrees, 0.00001);

            Angle deltaAlpha;
            Angle alphaDash;
            Angle deltaDash = SolarPositioning.CalculateTopocentricSunDeclanation(
                R, latitude, altitude, H, delta, alpha, out deltaAlpha, out alphaDash);
            Assert.AreEqual(-9.316179, deltaDash.Degrees, 0.00001);
            Assert.AreEqual(-0.000369, deltaAlpha.Degrees, 0.00001);

            Angle Hdash = SolarPositioning.CalculateTopocentricLocalHourAngle(H, deltaAlpha);
            Assert.AreEqual(11.10629, Hdash.Degrees, 0.001);

            Angle theta = SolarPositioning.CalculateTopocentricZenithAngle(latitude, delta, Hdash, pressure, temperature);
            Assert.AreEqual(50.11162, theta.Degrees, 0.01);

            Angle azimuth = SolarPositioning.CalculateTopocentricAzimuthAngle(Hdash, latitude, delta);
            Assert.AreEqual(194.34024, azimuth.Degrees, 0.001);
        }
    }
}
