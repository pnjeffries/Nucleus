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
            for (int i = 0; i < 5; i++)
            {
                //Matrix_Tests.MultiplicationTest(800);
            }

            for (int i = 0; i < 5; i++)
            {
                Matrix_Tests.AdditionTest(800);
            }

            //Doc tests:
            //Word_Tests.ReadDocTest();

            Console.Read();
        }
    }
}
