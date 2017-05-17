using FreeBuild.Geometry;
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
            Console.WriteLine("Select test to run:");
            Console.WriteLine(" a) All");
            Console.WriteLine(" b) Dependency Extraction");
            Console.WriteLine(" c) Matrix Multiplication");
            Console.WriteLine(" d) Matrix Addition");
            Console.WriteLine(" e) Delaunay Triangulation");
            Console.WriteLine(" f) Unserialisable Types");
            Console.WriteLine(" g) GWA Serialisation");
            Console.WriteLine(" h) Uniques Serialisation");

            char option = Console.ReadKey(true).KeyChar;

            if (option == 'a' || option == 'b')
            {
                Type type = typeof(Vertex);
                Reflection_Tests.PrintDependencies(type);
            }

            //Matrix tests:
            if (option == 'a' || option == 'c')
            {
                for (int i = 0; i < 5; i++)
                {
                    Matrix_Tests.MultiplicationTest(800);
                }
            }
            if (option == 'a' || option == 'd')
            {
                for (int i = 0; i < 5; i++)
                {
                    Matrix_Tests.AdditionTest(800);
                }
            }

            //Doc tests:
            //Word_Tests.ReadDocTest();

            //Reflection_Tests.PrintFields(typeof(SymmetricIProfile));
            //Reflection_Tests.PrintProperties(typeof(SymmetricIProfile));

            if (option == 'a' || option == 'e')
            {
                TimeSpan ts1 = new TimeSpan();
                //TimeSpan ts2 = new TimeSpan();

                int runs = 10;

                //Mesh tests:
                for (int i = 0; i < runs; i++)
                {
                    ts1 += Mesh_Tests.DelaunayTest(3000);
                    //ts2 += Mesh_Tests.DelaunayTest(10000);
                }

                //Core.Print("Method 1: " + ts1 + "   Method 2: " + ts2);
                Core.Print("Total: " + ts1 + " Average: " + TimeSpan.FromMilliseconds((ts1.TotalMilliseconds / runs)));//+ "   Method 2: " + ts2);
            }

            if (option == 'a' || option == 'f')
            {
                Reflection_Tests.PrintUnserializableTypes(typeof(ModelDocument));
            }

            if (option == 'a' || option == 'g')
            {
                GWA_Tests.SerializeToGWA();
            }

            if (option == 'a' || option == 'h')
            {
                UniqueFormatter_Tests.SerializeToFormat();
            }

            Console.Read();
        }
    }
}
