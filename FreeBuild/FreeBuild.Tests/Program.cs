using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //Matrix tests:
            /*for (int i = 0; i < 5; i++)
            {
                //Matrix_Tests.MultiplicationTest(800);
            }

            for (int i = 0; i < 5; i++)
            {
                Matrix_Tests.AdditionTest(800);
            }*/

            //Doc tests:
            //Word_Tests.ReadDocTest();

            TimeSpan ts1 = new TimeSpan();
            TimeSpan ts2 = new TimeSpan();

            //Mesh tests:
            for (int i = 0; i < 10; i++)
            {
                ts1 += Mesh_Tests.DelaunayTest2(10000);
                ts2 += Mesh_Tests.DelaunayTest(10000);
            }

            Core.Print("Method 1: " + ts1 + "   Method 2: " + ts2);

            Console.Read();
        }
    }
}
