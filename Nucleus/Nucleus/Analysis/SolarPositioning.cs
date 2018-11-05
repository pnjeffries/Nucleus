using Nucleus.Extensions;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Analysis
{
    /// <summary>
    /// Static helper class implementing the Solar Positioning Algorithm.
    /// Used to calculate the position of the sun at a given date and time.
    /// Based on http://www.nrel.gov/docs/fy08osti/34302.pdf
    /// </summary>
    public static class SolarPositioning
    {
        #region Constants

        // Table A4.2:

        /// <summary>
        /// The subset of Table A4.2 used to calculate the L0 component
        /// </summary>
        public static double[,] L0Table = new double[,]
        {
            // A,B,C
            {175347046,0,0},
            {3341656,4.6692568,6283.07585},
            {34894,4.6261,12566.1517},
            {3497,2.7441,5753.3849},
            {3418,2.8289,3.5231},
            {3136,3.6277,77713.7715},
            {2676,4.4181,7860.4194},
            {2343,6.1352,3930.2097},
            {1324,0.7425,11506.7698},
            {1273,2.0371,529.691},
            {1199,1.1096,1577.3435},
            {990,5.233,5884.927},
            {902,2.045,26.298},
            {857,3.508,398.149},
            {780,1.179,5223.694},
            {753,2.533,5507.553},
            {505,4.583,18849.228},
            {492,4.205,775.523},
            {357,2.92,0.067},
            {317,5.849,11790.629},
            {284,1.899,796.298},
            {271,0.315,10977.079},
            {243,0.345,5486.778},
            {206,4.806,2544.314},
            {205,1.869,5573.143},
            {202,2.458,6069.777},
            {156,0.833,213.299},
            {132,3.411,2942.463},
            {126,1.083,20.775},
            {115,0.645,0.98},
            {103,0.636,4694.003},
            {102,0.976,15720.839},
            {102,4.267,7.114},
            {99,6.21,2146.17},
            {98,0.68,155.42},
            {86,5.98,161000.69},
            {85,1.3,6275.96},
            {85,3.67,71430.7},
            {80,1.81,17260.15},
            {79,3.04,12036.46},
            {75,1.76,5088.63},
            {74,3.5,3154.69},
            {74,4.68,801.82},
            {70,0.83,9437.76},
            {62,3.98,8827.39},
            {61,1.82,7084.9},
            {57,2.78,6286.6},
            {56,4.39,14143.5},
            {56,3.47,6279.55},
            {52,0.19,12139.55},
            {52,1.33,1748.02},
            {51,0.28,5856.48},
            {49,0.49,1194.45},
            {41,5.37,8429.24},
            {41,2.4,19651.05},
            {39,6.17,10447.39},
            {37,6.04,10213.29},
            {37,2.57,1059.38},
            {36,1.71,2352.87},
            {36,1.78,6812.77},
            {33,0.59,17789.85},
            {30,0.44,83996.85},
            {30,2.74,1349.87},
            {25,3.16,4690.48}
        };

        /// <summary>
        /// The subset of Table A4.2 used to calculate the L1 component
        /// </summary>
        public static double[,] L1Table = new double[,] 
        {
            // A, B, C
            {628331966747,0,0},
            {206059,2.678235,6283.07585},
            {4303,2.6351,12566.1517},
            {425,1.59,3.523},
            {119,5.796,26.298},
            {109,2.966,1577.344},
            {93,2.59,18849.23},
            {72,1.14,529.69},
            {68,1.87,398.15},
            {67,4.41,5507.55},
            {59,2.89,5223.69},
            {56,2.17,155.42},
            {45,0.4,796.3},
            {36,0.47,775.52},
            {29,2.65,7.11},
            {21,5.34,0.98},
            {19,1.85,5486.78},
            {19,4.97,213.3},
            {17,2.99,6275.96},
            {16,0.03,2544.31},
            {16,1.43,2146.17},
            {15,1.21,10977.08},
            {12,2.83,1748.02},
            {12,3.26,5088.63},
            {12,5.27,1194.45},
            {12,2.08,4694},
            {11,0.77,553.57},
            {10,1.3,6286.6},
            {10,4.24,1349.87},
            {9,2.7,242.73},
            {9,5.64,951.72},
            {8,5.3,2352.87},
            {6,2.65,9437.76},
            {6,4.67,4690.48}
        };

        /// <summary>
        /// The subset of Table A4.2 used to calculate the L2 component
        /// </summary>
        public static double[,] L2Table = new double[,]
        {
            {52919,0,0},
            {8720,1.0721,6283.0758},
            {309,0.867,12566.152},
            {27,0.05,3.52},
            {16,5.19,26.3},
            {16,3.68,155.42},
            {10,0.76,18849.23},
            {9,2.06,77713.77},
            {7,0.83,775.52},
            {5,4.66,1577.34},
            {4,1.03,7.11},
            {4,3.44,5573.14},
            {3,5.14,796.3},
            {3,6.05,5507.55},
            {3,1.19,242.73},
            {3,6.12,529.69},
            {3,0.31,398.15},
            {3,2.28,553.57},
            {2,4.38,5223.69},
            {2,3.75,0.98}
        };

        /// <summary>
        /// The subset of Table A4.2 used to calculate the L3 component
        /// </summary>
        public static double[,] L3Table = new double[,]
        {
            {289,5.844,6283.076},
            {35,0,0},
            {17,5.49,12566.15},
            {3,5.2,155.42},
            {1,4.72,3.52},
            {1,5.3,18849.23},
            {1,5.97,242.73}
        };

        /// <summary>
        /// The subset of table A4.2 used to calculate the L4 component
        /// </summary>
        public static double[,] L4Table = new double[,]
        {
            {114,3.142,0},
            {8,4.13,6283.08},
            {1,3.84,12566.15}
        };

        /// <summary>
        /// The subset of table A4.2 used to calculate the L5 component
        /// </summary>
        public static double[,] L5Table = new double[,]
        {
            { 1, 3.14, 0 }
        };

        /// <summary>
        /// The subset of table A4.2 used to calculate the B0 component
        /// </summary>
        public static double[,] B0Table = new double[,]
        {
            {280,3.199,84334.662},
            {102,5.422,5507.553},
            {80,3.88,5223.69},
            {44,3.7,2352.87},
            {32,4,1577.34}
        };

        /// <summary>
        /// The subset of table A4.2 used to calculate the B1 component
        /// </summary>
        public static double[,] B1Table = new double[,]
        {
            {9,3.9,5507.55},
            {6,1.73,5223.69}
        };

        /// <summary>
        /// The subset of table A4.2 used to calculate the R0 component
        /// </summary>
        public static double[,] R0Table = new double[,]
        {
            {100013989,0,0},
            {1670700,3.0984635,6283.07585},
            {13956,3.05525,12566.1517},
            {3084,5.1985,77713.7715},
            {1628,1.1739,5753.3849},
            {1576,2.8469,7860.4194},
            {925,5.453,11506.77},
            {542,4.564,3930.21},
            {472,3.661,5884.927},
            {346,0.964,5507.553},
            {329,5.9,5223.694},
            {307,0.299,5573.143},
            {243,4.273,11790.629},
            {212,5.847,1577.344},
            {186,5.022,10977.079},
            {175,3.012,18849.228},
            {110,5.055,5486.778},
            {98,0.89,6069.78},
            {86,5.69,15720.84},
            {86,1.27,161000.69},
            {65,0.27,17260.15},
            {63,0.92,529.69},
            {57,2.01,83996.85},
            {56,5.24,71430.7},
            {49,3.25,2544.31},
            {47,2.58,775.52},
            {45,5.54,9437.76},
            {43,6.01,6275.96},
            {39,5.36,4694},
            {38,2.39,8827.39},
            {37,0.83,19651.05},
            {37,4.9,12139.55},
            {36,1.67,12036.46},
            {35,1.84,2942.46},
            {33,0.24,7084.9},
            {32,0.18,5088.63},
            {32,1.78,398.15},
            {28,1.21,6286.6},
            {28,1.9,6279.55},
            {26,4.59,10447.39}
        };

        /// <summary>
        /// The subset of table A4.2 used to calculate the R1 component
        /// </summary>
        public static double[,] R1Table = new double[,]
        {
            {103019,1.10749,6283.07585},
            {1721,1.0644,12566.1517},
            {702,3.142,0},
            {32,1.02,18849.23},
            {31,2.84,5507.55},
            {25,1.32,5223.69},
            {18,1.42,1577.34},
            {10,5.91,10977.08},
            {9,1.42,6275.96},
            {9,0.27,5486.78}
        };

        /// <summary>
        /// The subset of table A4.2 used to calculate the R2 component
        /// </summary>
        public static double[,] R2Table = new double[,]
        {
            {4359,5.7846,6283.0758},
            {124,5.579,12566.152},
            {12,3.14,0},
            {9,3.63,77713.77},
            {6,1.87,5573.14},
            {3,5.47,18849.23}
        };

        /// <summary>
        /// The subset of table A4.2 used to calculate the R3 component
        /// </summary>
        public static double[,] R3Table = new double[,]
        {
            {145,4.273,6283.076},
            {7,3.92,12566.15}
        };

        /// <summary>
        /// The subset of table A4.2 used to calculate the R3 component
        /// </summary>
        public static double[,] R4Table = new double[,]
        {
            {4, 2.56, 6283.08}
        };

        /// <summary>
        /// The Y coefficients for sin terms given in table A4.3
        /// </summary>
        public static int[,] YTable = new int[,]
        {
            {0,0,0,0,1},
            {-2,0,0,2,2},
            {0,0,0,2,2},
            {0,0,0,0,2},
            {0,1,0,0,0},
            {0,0,1,0,0},
            {-2,1,0,2,2},
            {0,0,0,2,1},
            {0,0,1,2,2},
            {-2,-1,0,2,2},
            {-2,0,1,0,0},
            {-2,0,0,2,1},
            {0,0,-1,2,2},
            {2,0,0,0,0},
            {0,0,1,0,1},
            {2,0,-1,2,2},
            {0,0,-1,0,1},
            {0,0,1,2,1},
            {-2,0,2,0,0},
            {0,0,-2,2,1},
            {2,0,0,2,2},
            {0,0,2,2,2},
            {0,0,2,0,0},
            {-2,0,1,2,2},
            {0,0,0,2,0},
            {-2,0,0,2,0},
            {0,0,-1,2,1},
            {0,2,0,0,0},
            {2,0,-1,0,1},
            {-2,2,0,2,2},
            {0,1,0,0,1},
            {-2,0,1,0,1},
            {0,-1,0,0,1},
            {0,0,2,-2,0},
            {2,0,-1,2,1},
            {2,0,1,2,2},
            {0,1,0,2,2},
            {-2,1,1,0,0},
            {0,-1,0,2,2},
            {2,0,0,2,1},
            {2,0,1,0,0},
            {-2,0,2,2,2},
            {-2,0,1,2,1},
            {2,0,-2,0,1},
            {2,0,0,0,1},
            {0,-1,1,0,0},
            {-2,-1,0,2,1},
            {-2,0,0,0,1},
            {0,0,2,2,1},
            {-2,0,2,0,1},
            {-2,1,0,2,1},
            {0,0,1,-2,0},
            {-1,0,1,0,0},
            {-2,1,0,0,0},
            {1,0,0,0,0},
            {0,0,1,2,0},
            {0,0,-2,2,2},
            {-1,-1,1,0,0},
            {0,1,1,0,0},
            {0,-1,1,2,2},
            {2,-1,-1,2,2},
            {0,0,3,2,2},
            {2,-1,0,2,2}
        };

        /// <summary>
        /// The column of 'a' coefficients from table A4.3
        /// </summary>
        public static double[] aTable = new double[]
            {-171996,-13187,-2274,2062,1426,712,-517,-386,-301,217,-158,129,123,63,63,-59,
                -58,-51,48, 46,-38,-31,29,29,26,-22,21,17,16,-16,-15,-13,-12,11,-10,-8,7,
                -7,-7,-7,6,6,6,-6,-6,5,-5,-5,-5,4,4,4,-4,-4,-4,3,-3,-3,-3,-3,-3,-3,-3};

        /// <summary>
        /// The column of 'b' coefficients from table A4.3
        /// </summary>
        public static double[] bTable = new double[]
            {-174.2,1.6,-0.2,0.2,-3.4,0.1, 1.2,-0.4,0,-0.5, 0,0.1,0,0,0.1,
                0,-0.1,0, 0,  0,  0,  0,  0,  0,  0,  0,  0,  -0.1,   0,  0.1,    0,  0,
                0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0};

        /// <summary>
        /// The column of 'c' coefficients from table A4.3
        /// </summary>
        public static double[] cTable = new double[]
            {92025, 5736,   977,    -895,   54, -7, 224,    200,    129,    -95,    0,  -70,
             -53,    0,  -33,    26, 32, 27, 0,  -24,    16, 13, 0,  -12,    0,  0,  -10,    0,
             -8, 7,  9,  7,  6,  0,  5,  3,  -3, 0,  3,  3,  0,  -3, -3, 3,  3,  0,  3,  3,  3,
                0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0};

        /// <summary>
        /// The column of 'd' coefficients from table A4.3
        /// </summary>
        public static double[] dTable = new double[]
            {8.9,   -3.1,   -0.5,   0.5,    -0.1,   0,  -0.6,   0,  -0.1,   0.3,    0,  0,  0,  0,
                0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                0,  0,  0,  0,  0,  0,  0 };

        #endregion

        /// <summary>
        /// Calculate the Julian Day from a date expressed in Universal Time.
        /// The Julian date starts on January 1, in the year -4712 at 12:00:00 UT.
        /// </summary>
        /// <param name="universalTime">The date expressed in Universal Time</param>
        public static double CalculateJulianDay(DateTime universalTime)
        {
            return CalculateJulianDay(universalTime.Year, universalTime.Month, universalTime.Day + universalTime.TimeOfDay.TotalSeconds / 86400.0);
        }

        /// <summary>
        /// Calculate the Julian Day from a date expressed in Universal Time.
        /// The Julian date starts on January 1, in the year -4712 at 12:00:00 UT.
        /// </summary>
        /// <param name="year">The year expressed as an integer (for e.g. 2018)</param>
        /// <param name="month">The month expressed as an integer (for e.g. January = 1)</param>
        /// <param name="day">The  day of the month with decimal time (e.g. for the second day of the
        /// month at 12:30:30 UT, D = 2.521180556).</param>
        /// <returns></returns>
        public static double CalculateJulianDay(int year, int month, double day)
        {
            // If month <= 2, adjust month and year
            if (month <=2)
            {
                year -= 1;
                month += 12;
            }
            double jD = Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) +
                day - 1524.5;
            if (jD > 2299160)
            {
                // Correction for gregorian calendar
                double a = Math.Floor(year / 100.0);
                jD += 2 - a + Math.Floor(a / 4);
            }
            return jD;
        }

        /// <summary>
        /// Calculate the Julian Century from the Julian Day.
        /// (May also be used to calculate the Julian Ephemeris Century (JCE)
        /// from the Julian Ephemeris Day, should one be so inclined).
        /// </summary>
        /// <param name="julianDay"></param>
        /// <returns></returns>
        public static double CalculateJulianCentury(double julianDay)
        {
            return (julianDay - 2451545) / 36525;
        }

        /// <summary>
        /// Calculate the Julian Millennium from the Julian Century.
        /// </summary>
        /// <param name="julianCentury"></param>
        /// <returns></returns>
        public static double CalculateJulianMillenium(double julianCentury)
        {
            return julianCentury / 10;
        }
        
        /// <summary>
        /// Calculate the Julian Millennium from the specified date in Universal Time
        /// </summary>
        /// <param name="universalTime"></param>
        /// <returns></returns>
        public static double CalculateJulianMillenium(DateTime universalTime)
        {
            return CalculateJulianMillenium(CalculateJulianCentury(CalculateJulianDay(universalTime)));
        }

        /// <summary>
        /// Calculate a component of the helocentric latitude or longitude.
        /// Equation (9) in the NREL paper.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="julianMillenium"></param>
        /// <returns></returns>
        private static double CalculateHeliocentricComponent(double[,] table, double julianMillenium)
        {
            double L0 = 0;

            for (int i = 0; i < table.GetLength(0); i++)
            {
                double a = table[i,0];
                double b = table[i,1];
                double c = table[i,2];
                //L0 =	 A *cos ( B + C * JME)
                double L0i = a * Math.Cos(b + c * julianMillenium);
                L0 += L0i;
            }

            return L0;
        }

        /// <summary>
        /// Calculate the Heliocentric longitude of the earth at the specified time.
        /// Steps 3.2.4 to 3.2.5 using Equation (11) in the NREL paper.
        /// </summary>
        /// <param name="julianMillenium"></param>
        /// <returns></returns>
        public static Angle CalculateEarthHeliocentricLongitude(double julianMillenium)
        {
            double L0 = CalculateHeliocentricComponent(L0Table, julianMillenium);
            double L1 = CalculateHeliocentricComponent(L1Table, julianMillenium);
            double L2 = CalculateHeliocentricComponent(L2Table, julianMillenium);
            double L3 = CalculateHeliocentricComponent(L3Table, julianMillenium);
            double L4 = CalculateHeliocentricComponent(L4Table, julianMillenium);
            double L5 = CalculateHeliocentricComponent(L5Table, julianMillenium);

            Angle L = (L0 + 
                L1 * julianMillenium +
                L2 * julianMillenium.Power(2) +
                L3 * julianMillenium.Power(3) +
                L4 * julianMillenium.Power(4) +
                L5 * julianMillenium.Power(5)) / 
                (10.0).Power(8);

            return L.NormalizeTo2PI();
        }

        /// <summary>
        /// Calculate the Heliocentric latitude of the earth at the specified time.
        /// Step 3.2.7 and Equation (11) in the NREL paper.
        /// </summary>
        /// <param name="julianMillenium"></param>
        /// <returns></returns>
        public static Angle CalculateEarthHeliocentricLatitude(double julianMillenium)
        {
            double B0 = CalculateHeliocentricComponent(B0Table, julianMillenium);
            double B1 = CalculateHeliocentricComponent(B1Table, julianMillenium);

            Angle B = (B0 + B1 * julianMillenium) / Math.Pow(10, 8);

            return B;
        }

        /// <summary>
        /// Calculate the Earth Radius Vector, R, in Astronomical Units at the specified time.
        /// Step 3.2.8 and Equation (11) in the NREL paper.
        /// </summary>
        /// <param name="julianMillenium"></param>
        /// <returns></returns>
        public static double CalculateEarthRadiusVector(double julianMillenium)
        {
            double R0 = CalculateHeliocentricComponent(R0Table, julianMillenium);
            double R1 = CalculateHeliocentricComponent(R1Table, julianMillenium);
            double R2 = CalculateHeliocentricComponent(R2Table, julianMillenium);
            double R3 = CalculateHeliocentricComponent(R3Table, julianMillenium);
            double R4 = CalculateHeliocentricComponent(R4Table, julianMillenium);

            double R = (R0 + R1 * julianMillenium +
                R2 * Math.Pow(julianMillenium, 2) +
                R3 * Math.Pow(julianMillenium, 3) +
                R4 * Math.Pow(julianMillenium, 4)) / Math.Pow(10, 8);

            return R;
        }

        /// <summary>
        /// Calculate the geocentric longitude from the heliocentric longitude
        /// </summary>
        /// <param name="hcLong"></param>
        /// <returns></returns>
        public static Angle CalculateGeocentricLongitude(Angle hcLong)
        {
            return (hcLong + Angle.Straight).NormalizeTo2PI();
        }

        /// <summary>
        /// Calculate the geocentric latitude from the heliocentric latitude
        /// </summary>
        /// <param name="hcLat"></param>
        /// <returns></returns>
        public static Angle CalculateGeocentricLatitude(Angle hcLat)
        {
            return -hcLat;
        }

        /// <summary>
        /// Calculate the coefficient values used to calculate the 
        /// nutation in longitude and obliquity.
        /// Steps 3.4.1 to 3.4.5 and equations (15) to (19) in
        /// the NREL paper.
        /// </summary>
        /// <param name="julianCentury"></param>
        /// <returns></returns>
        public static Angle[] CalculateNutationCoefficients(double julianCentury)
        {
            double jC2 = julianCentury * julianCentury;
            double jC3 = jC2 * julianCentury;

            // mean elongation of the moon from the sun:
            Angle X0 = Angle.FromDegrees(
                297.85036 + 445267.111480 * julianCentury -
                0.0019142 * jC2 + jC3 / 189474);

            // mean anomaly of the sun:
            Angle X1 = Angle.FromDegrees(
                357.52772 + 35999.050340 * julianCentury -
                0.0001603 * jC2 - jC3 / 300000);

            // mean anomaly of the moon:
            Angle X2 = Angle.FromDegrees(
                134.96298 + 477198.867398 * julianCentury +
                0.0086972 * jC2 + jC3 / 56250);

            // moon’s argument of latitude:
            Angle X3 = Angle.FromDegrees(
                93.27191 + 483202.017538 * julianCentury -
                0.0036825 * jC2 + jC3 / 327270);

            // longitude of the ascending node of the moon’s mean orbit on the
            // ecliptic, measured from the mean equinox of the date:
            Angle X4 = Angle.FromDegrees(
                125.04452 - 1934.136261 * julianCentury +
                0.0020708 * jC2 + jC3 / 450000);

            return new Angle[] { X0, X1, X2, X3, X4 };
        }

        /// <summary>
        /// Calculate the nutation in longitude for a specified julian century.
        /// Step 3.4.7 and equations (20) and (22) in the NREL paper.
        /// </summary>
        /// <param name="julianCentury"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        public static Angle CalculateNutationInLongitude(double julianCentury, Angle[] X)
        {
            double sum = 0;
            for (int i = 0; i < aTable.Length; i++)
            {
                double sum2 = 0;
                for (int j = 0; j <= 4; j++)
                {
                    sum2 += X[j] * YTable[i, j];
                }
                sum += (aTable[i] + bTable[i] * julianCentury) * Math.Sin(sum2);
            }
            return Angle.FromDegrees(sum / 36000000);
        }

        /// <summary>
        /// Calculate the nutation in longitude for a specified julian century.
        /// Step 3.4.7 and equations (20) and (22) in the NREL paper.
        /// </summary>
        /// <param name="julianCentury"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        public static Angle CalculateNutationInObliquity(double julianCentury, Angle[] X)
        {
            double sum = 0;
            for (int i = 0; i < cTable.Length; i++)
            {
                double sum2 = 0;
                for (int j = 0; j <= 4; j++)
                {
                    sum2 += X[j] * YTable[i, j];
                }
                sum += (cTable[i] + dTable[i] * julianCentury) * Math.Cos(sum2);
            }
            return Angle.FromDegrees(sum / 36000000);
        }

        /// <summary>
        /// Calculate the true obliquity of the eliptic.
        /// Step 3.5 and equations (24) and (25) in the NREL paper
        /// </summary>
        /// <param name="julianMillenium"></param>
        /// <param name="obliquityNutation"></param>
        /// <returns></returns>
        public static Angle CalculateTrueObliquityOfTheEliptic(double julianMillenium, Angle obliquityNutation)
        {
            double U = julianMillenium / 10;
            double e0 = 84381.448 - 4680.93 * U - 1.55 * U * U + 1999.25 * U * U * U -
                51.38 * U.Power(4) - 249.67 * U.Power(5) - 39.05 * U.Power(6) +
                7.12 * U.Power(7) + 27.87 * U.Power(8) + 5.79 * U.Power(9) +
                2.45 * U.Power(10);
            return Angle.FromDegrees(e0 / 3600) + obliquityNutation;
        }

        /// <summary>
        /// Calculate the aberration correction angle.
        /// Step 3.6 and equation (26) in the NREL paper
        /// </summary>
        /// <param name="R"></param>
        /// <returns></returns>
        public static Angle CalculateAberrationCorrection(double R)
        {
            return -Angle.FromDegrees(20.4898 / (3600 * R));
        }

        /// <summary>
        /// Calculate the apparent sun longitude.
        /// Step 3.7 and equation (27) in the NREL paper
        /// </summary>
        /// <param name="geocentricLongitude"></param>
        /// <param name="longitudeNutation"></param>
        /// <param name="aberrationCorrection"></param>
        /// <returns></returns>
        public static Angle CalculateApparentSunLongitude(
            Angle geocentricLongitude, Angle longitudeNutation, Angle aberrationCorrection)
        {
            return geocentricLongitude + longitudeNutation + aberrationCorrection;
        }

        /// <summary>
        /// Calculate the apparent sidereal time at Greenwich.
        /// Step 3.8 and equations (28) and (29) in the NREL paper
        /// </summary>
        public static Angle CalculateApparentSiderealTime(double julianDay, double julianCentury,
            Angle longitudeNutation, Angle eclipticObliquity)
        {
            Angle v0 = Angle.FromDegrees(
                280.46061837
                + 360.98564736629 * (julianDay - 2451545)
                + 0.000387933 * julianCentury * julianCentury
                - (julianCentury * julianCentury * julianCentury) / 38710000.0);
            v0 = v0.NormalizeTo2PI();
            return v0 + longitudeNutation * Math.Cos(eclipticObliquity);
        }

        /// <summary>
        /// Calculate the geocentric sun right ascension.
        /// Step 3.8 and equation (30) in the NREL paper
        /// </summary>
        public static Angle CalculateGeocentricSunRightAscension(Angle apparentSunLong, Angle eclipticObliquity,
            Angle gcLatitude)
        {
            Angle alpha = Math.Atan2(Math.Sin(apparentSunLong) * Math.Cos(eclipticObliquity) -
                Math.Tan(gcLatitude) * Math.Sin(eclipticObliquity),
                Math.Cos(apparentSunLong));
            return alpha.NormalizeTo2PI();
        }

        /// <summary>
        /// Calculate the geocentric sun declination.
        /// Step 3.10 in the NREL paper.
        /// </summary>
        /// <param name="apparentSunLong"></param>
        /// <param name="eclipticObliquity"></param>
        /// <param name="gcLatitude"></param>
        /// <returns></returns>
        public static Angle CalculateGeocentricSunDeclination(Angle apparentSunLong, Angle eclipticObliquity,
            Angle gcLatitude)
        {
            Angle gsd = Math.Asin(Math.Sin(gcLatitude) * Math.Cos(eclipticObliquity)
                + Math.Cos(gcLatitude) * Math.Sin(eclipticObliquity) * Math.Sin(apparentSunLong));
            return gsd;
        }
        
        /// <summary>
        /// Calculate the observer local hour angle, H
        /// Step 3.11 and equation (32) in the NREL paper
        /// </summary>
        /// <param name="siderealTime"></param>
        /// <param name="observerLongitude"></param>
        /// <param name="sunRightAscension"></param>
        /// <returns></returns>
        public static Angle CalculateObserverLocalHourAngle(Angle siderealTime, Angle observerLongitude, 
            Angle sunRightAscension)
        {
            return (siderealTime + observerLongitude - sunRightAscension).NormalizeTo2PI();
        }

        /// <summary>
        /// Calculate the topocentric sun right ascension
        /// Step 3.12 in the NREL paper
        /// </summary>
        /// <param name="earthRadiusVector"></param>
        /// <param name="deltaAlpha">Output.   Parallax in the sun right ascension.</param>
        /// <param name="alphaDash">Output.   Topocentric sun right ascension.</param>
        /// <returns></returns>
        public static Angle CalculateTopocentricSunDeclanation(double earthRadiusVector, 
            Angle observerLatitude, double observerElevation, Angle observerLocalHour,
            Angle gcSunDeclination, Angle sunRightAscension, 
            out Angle deltaAlpha, out Angle alphaDash)
        {
            Angle xi = Angle.FromDegrees(8.794 / (3600.0 * earthRadiusVector));
            Angle u = Math.Atan(0.99664719 * Math.Tan(observerLatitude));
            double x = Math.Cos(u) + (observerElevation / 6378140.0) * Math.Cos(observerLatitude);
            double y = 0.99664719 * Math.Sin(u) + (observerElevation / 6378140.0) * Math.Sin(observerLatitude);
            double denom = Math.Cos(gcSunDeclination) - x * Math.Sin(xi) * Math.Cos(observerLocalHour);
            deltaAlpha = Math.Atan2(-x * Math.Sin(xi) * Math.Sin(observerLocalHour), denom);
            alphaDash = sunRightAscension + deltaAlpha;
            return
                Math.Atan2(
                    (Math.Sin(gcSunDeclination) - y * Math.Sin(xi)) * Math.Cos(deltaAlpha), denom);
        }

        /// <summary>
        /// Calculate the topocentric local hour angle.
        /// Step 3.13 in the NREL paper
        /// </summary>
        /// <param name="localHourAngle"></param>
        /// <param name="deltaAlpha"></param>
        /// <returns></returns>
        public static Angle CalculateTopocentricLocalHourAngle(Angle localHourAngle, Angle deltaAlpha)
        {
            return localHourAngle - deltaAlpha;
        }

        /// <summary>
        /// Calculate the topocentric zenith angle.
        /// Step 3.14 in the NREL paper.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="gcSunDeclination"></param>
        /// <param name="tcLocalHour"></param>
        /// <param name="pressure"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        public static Angle CalculateTopocentricZenithAngle(Angle latitude, Angle gcSunDeclination,
            Angle tcLocalHour, double pressure, double temperature)
        {
            Angle e0 = Math.Asin(Math.Sin(latitude) * Math.Sin(gcSunDeclination) +
                Math.Cos(latitude) * Math.Cos(gcSunDeclination) * Math.Cos(tcLocalHour));
            Angle deltae = Angle.FromDegrees((pressure / 1010.0) * (283.0 / (273.0 + temperature)) *
                1.02 / (60.0 * Math.Tan(Angle.FromDegrees(e0.Degrees + 10.3 / (e0.Degrees + 5.11)))));
            Angle e = e0 + deltae;
            Angle theta = Angle.Right - e;
            return theta;
        }


        /// <summary>
        /// Calculate topocentric azimuth angle.
        /// Step 3.15 in the NREL paper.
        /// </summary>
        /// <returns>An angle measured eastward from north</returns>
        public static Angle CalculateTopocentricAzimuthAngle(Angle tcLocalHour, Angle latitude,
            Angle gcSunDeclination)
        {
            Angle ro = Math.Atan2(Math.Sin(tcLocalHour),
                Math.Cos(tcLocalHour) * Math.Sin(latitude) - Math.Tan(gcSunDeclination) * Math.Cos(latitude));
            return (ro + Angle.Straight).NormalizeTo2PI(); //TODO: Adjust
        }

        /// <summary>
        /// Calculate the incidence angle to an inclined plane.
        /// Step 3.16 and equation (47)
        /// </summary>
        /// <param name="slope">The slope of the surface measured from the horizontal plane</param>
        /// <param name="surfaceAzimuth"> the surface azimuth rotation angle, measured from south 
        /// to the projection of the surface normal on the horizontal plane, positive or negative 
        /// if oriented west or east from south, respectively</param>
        /// <param name="theta">The topocentric zenith angle</param>
        /// <param name="sunAzimuth">The sun azimuth</param>
        /// <param name="sunLongitude">The apparent sun longitude</param>
        /// <returns></returns>
        public static Angle CalculateIncidenceAngle(Angle slope, Angle surfaceAzimuth, Angle theta, 
            Angle sunAzimuth, Angle sunLongitude)
        {
            Angle I = Math.Acos(Math.Cos(theta) * Math.Cos(slope) +
                Math.Sin(slope) * Math.Sin(theta) * Math.Cos(sunAzimuth + sunLongitude));
            return I;
        }

        public static AnglePair CalculateSunPosition(DateTime time, Angle latitude, Angle longitude,
            double elevation)

    }
}
