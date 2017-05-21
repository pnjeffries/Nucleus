using FreeBuild.Maths;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Tests
{
    /// <summary>
    /// Test functions for matrices
    /// </summary>
    public static class Matrix_Tests
    {
        public static TimeSpan MultiplicationTest(int size)
        {
            Core.Print("Multiplication Speed Test:");
            Stopwatch sw = new Stopwatch();
            Random rng = new Random(1);
            Matrix A = new ArrayMatrix(size, size, rng);
            Matrix B = new ArrayMatrix(size, size, rng);

            sw.Start();
            Matrix C = A * B;
            sw.Stop();
            Core.Print(sw.Elapsed.ToString());
            return sw.Elapsed;
        }

        public static TimeSpan AdditionTest(int size)
        {
            Core.Print("Addition Speed Test:");
            Stopwatch sw = new Stopwatch();
            Random rng = new Random(1);
            Matrix A = new ArrayMatrix(size, size, rng);
            Matrix B = new ArrayMatrix(size, size, rng);

            sw.Start();
            Matrix C = A + B;
            sw.Stop();
            Core.Print(sw.Elapsed.ToString());
            return sw.Elapsed;
        }

        public static TimeSpan InverseTest()
        {
            Core.Print("Inverse Test:");
            Stopwatch sw = new Stopwatch();
            Matrix A = new ArrayMatrix(
                1, 0, 0, 2,
                0, 1, 0, -3,
                0, 0, 1, 5,
                0, 0, 0, 1);
            Core.Print(A.ToString());
            Core.Print("Determinant: " + A.Determinant().ToString());
            //Core.Print(A.Adjugate().ToString());
            sw.Start();
            Matrix A1 = A.Inverse();
            sw.Stop();
            Core.Print(A1.ToString());
            return sw.Elapsed;
        }
    }
}
