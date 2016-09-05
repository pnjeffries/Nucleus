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
    }
}
