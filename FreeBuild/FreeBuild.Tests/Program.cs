using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            //Reflection_Tests.PrintFields(typeof(SymmetricIProfile));
            //Reflection_Tests.PrintProperties(typeof(SymmetricIProfile));

            GWA_Tests.SerializeToGWA();

            /*TimeSpan ts1 = new TimeSpan();
            TimeSpan ts2 = new TimeSpan();

            TimeSpan ts1 = new TimeSpan();
            //TimeSpan ts2 = new TimeSpan();

            int runs = 10;

            //Mesh tests:
            for (int i = 0; i < 10; i++)
            for (int i = 0; i < runs; i++)
            {
                ts1 += Mesh_Tests.DelaunayTest2(10000);
                ts2 += Mesh_Tests.DelaunayTest(10000);
                ts1 += Mesh_Tests.DelaunayTest(10000);
                //ts2 += Mesh_Tests.DelaunayTest(10000);
            }

            Core.Print("Method 1: " + ts1 + "   Method 2: " + ts2);
            Core.Print("Total: " + ts1 + " Average: " + TimeSpan.FromMilliseconds((ts1.TotalMilliseconds/runs)));//+ "   Method 2: " + ts2);
            */

            Reflection_Tests.PrintUnserializableTypes(typeof(ModelDocument));

            Console.Read();
        }
    }
}
