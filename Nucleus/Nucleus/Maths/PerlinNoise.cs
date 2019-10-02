using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// A perlin noise generator, based on Ken Perlin's 2002 Improved algorithm
    /// https://mrl.nyu.edu/~perlin/noise/
    /// </summary>
    public static class PerlinNoise
    {
        #region Constants

        private static readonly int[] _Permutation = { 151,160,137,91,90,15,                 // Hash lookup table as defined by Ken Perlin.  This is a randomly
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,    // arranged array of all numbers from 0-255 inclusive.
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180};

        private static readonly int[] _P;

        #endregion

        #region Constructors

        static PerlinNoise()
        {
            _P = new int[512];
            for (int x = 0; x < 512; x++)
            {
                _P[x] = _Permutation[x % 256];
            }
        }

        #endregion

        /// <summary>
        /// Improved Perlin noise function.  Returns a Perlin noise value
        /// for the specified location
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double Noise(Vector v)
        {
            return Noise(v.X, v.Y, v.Z);
        }

        /// <summary>
        /// Improved Perlin noise function.  Returns a Perlin noise value
        /// for the specified coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double Noise(double x, double y, double z)
        {
            // Find unit cube that contains point:
            int iX = (int)Math.Floor(x) & 255;
            int iY = (int)Math.Floor(y) & 255;
            int iZ = (int)Math.Floor(z) & 255;

            // Find relative x,y,z of point in cube
            x -= Math.Floor(x);
            y -= Math.Floor(y);
            z -= Math.Floor(z);

            // Compute fade curves for each of x,y,x
            double u = Fade(x);
            double v = Fade(y);
            double w = Fade(z);

            // Hash coordinates of the 8 cube corners
            int A = _P[iX] + iY, AA = _P[A] + iZ, AB = _P[A + 1] + iZ;
            int B = _P[iX + 1] + iY, BA = _P[B] + iZ, BB = _P[B + 1] + iZ;

            // Add blended results from 8 corners of cube
            return Lerp(w, Lerp(v, Lerp(u, Gradient(_P[AA], x, y, z),
                                           Gradient(_P[BA], x - 1, y, z)),
                                   Lerp(u, Gradient(_P[AB], x, y - 1, z),
                                           Gradient(_P[BB], x - 1, y - 1, z))),
                           Lerp(v, Lerp(u, Gradient(_P[AA + 1], x, y, z - 1),
                                           Gradient(_P[BA + 1], x - 1, y, z - 1)),
                                   Lerp(u, Gradient(_P[AB + 1], x, y - 1, z - 1),
                                           Gradient(_P[BB + 1], x - 1, y - 1, z - 1))));
        }

        /// <summary>
        /// Sum multiple perlin noise functions at decreasing scales to achieve more 'natural' 
        /// looking noise
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="octaves">The number of layers of noise to apply</param>
        /// <param name="persistence">The amplitude factor of each subsequent layer of noise</param>
        /// <param name="lacunarity">The increase in frequency of each subsequent layer of noise</param>
        /// <returns></returns>
        public static double OctaveNoise(Vector pt, int octaves, double persistence = 0.5, double lacunarity = 2)
        {
            return OctaveNoise(pt.X, pt.Y, pt.Z, octaves, persistence, lacunarity);
        }

        /// <summary>
        /// Sum multiple perlin noise functions at decreasing scales to achieve more 'natural' 
        /// looking noise
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="octaves">The number of layers of noise to apply</param>
        /// <param name="persistence">The amplitude factor of each subsequent layer of noise</param>
        /// <param name="lacunarity">The increase in frequency of each subsequent layer of noise</param>
        /// <returns></returns>
        public static double OctaveNoise(double x, double y, double z, int octaves, double persistence = 0.5, double lacunarity = 2)
        {
            double result = 0;
            double frequency = 1;
            double amplitude = 1;
            for (int i = 0; i < octaves; i++)
            {
                result += Noise(x * frequency, y * frequency, z * frequency) * amplitude;
                amplitude *= persistence;
                frequency *= lacunarity;
            }
            return result;
        }

        private static double Fade(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private static double Lerp(double t, double a, double b)
        {
            return a + t * (b - a);
        }

        private static double Gradient(int hash, double x, double y, double z)
        {
            // Convert lo 4 bits of has code into 12 gradient directions
            int h = hash & 15;
            double u = h < 8 ? x : y;
            double v = h < 4 ? y : h == 12 || h == 14 ? x : z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

    }
}
