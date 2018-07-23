using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// Static helper functions for fresnel integrals and other related algorithms
    /// </summary>
    public static class Fresnel
    {
        /// <summary>
        /// Constant precalculated value of √(PI/2)
        /// </summary>
        private static double _ROOTHALFPI = Math.Sqrt(Math.PI / 2);

        /// <summary>
        /// Constant precalculated value of √(2/PI)
        /// </summary>
        private static double _ROOT2OVERPI = Math.Sqrt(2 / Math.PI);

        /// <summary>
        /// Accuracy
        /// </summary>
        private const double _EPSILON = 0.001;

        /// <summary>
        /// The power series which approximates the solution
        /// to the fresnel sine integral defined as the integral
        /// of sin(t²)dt
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double S(double x)
        {
            double x2 = x * x;
            double x3 = x * x2;
            double x4 = -x2 * x2;
            double xn = 1.0;
            double Sn = 1.0;
            double Sm1 = 0.0;
            double term;
            double factorial = 1.0;
            double sqrt_2_o_pi = 7.978845608028653558798921198687637369517e-1;
            int y = 0;

            if (x == 0.0) return 0.0;
            Sn /= 3.0;
            while (Math.Abs(Sn - Sm1) > _EPSILON * Math.Abs(Sm1))
            {
                Sm1 = Sn;
                y += 1;
                factorial *= (double)(y + y);
                factorial *= (double)(y + y + 1);
                xn *= x4;
                term = xn / factorial;
                term /= (double)(y + y + y + y + 3);
                Sn += term;
            }
            return x3 * sqrt_2_o_pi * Sn;
        }

        /// <summary>
        /// The power series which approximates the solution
        /// to the fresnel sine integral defined as the integral
        /// of sin(π/2 * t²)dt
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double SPiOver2(double x)
        {
            return _ROOT2OVERPI * S(_ROOTHALFPI * x);
        }

        /// <summary>
        /// The power series which approximates the solution
        /// to the fresnel cosine integral defined as the integral
        /// of cos(t²)dt
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double C(double x)
        {
            double x2 = x * x;
            double x3 = x * x2;
            double x4 = -x2 * x2;
            double xn = 1.0;
            double Sn = 1.0;
            double Sm1 = 0.0;
            double term;
            double factorial = 1.0;
            double sqrt_2_o_pi = 7.978845608028653558798921198687637369517e-1;
            int y = 0;

            if (x == 0.0) return 0.0;
            while (Math.Abs(Sn - Sm1) > _EPSILON * Math.Abs(Sm1))
            {
                Sm1 = Sn;
                y += 1;
                factorial *= (double)(y + y);
                factorial *= (double)(y + y - 1);
                xn *= x4;
                term = xn / factorial;
                term /= (double)(y + y + y + y + 1);
                Sn += term;
            }
            return x * sqrt_2_o_pi * Sn;
        }

        /// <summary>
        /// The power series which approximates the solution
        /// to the fresnel cosine integral defined as the integral
        /// of cos(π/2 * t²)dt
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double CPiOver2(double x)
        {
            return _ROOT2OVERPI * C(_ROOTHALFPI * x);
        }

        /// <summary>
        /// The function f(X), defined as
        /// f(X) = (0.5 - S(X))cos(0.5π*X²) - (0.5 - C(X))*sin(0.5π*X²)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double F(double x)
        {
            return (0.5 - SPiOver2(x)) * Math.Cos(Math.PI * 0.5 * x * x) - (0.5 - CPiOver2(x)) * Math.Sin(0.5 * Math.PI * x * x);
        }

        /// <summary>
        /// The function g(X), defined as
        /// g(X) = (0.5 - C(X))cos(0.5π*X²) + (0.5 - C(X))*sin(0.5π*X²)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double G(double x)
        {
            return (0.5 - CPiOver2(x)) * Math.Cos(Math.PI * 0.5 * x * x) + (0.5 - SPiOver2(x)) * Math.Sin(0.5 * Math.PI * x * x);
        }
    }
}
