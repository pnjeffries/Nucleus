using Nucleus.Extensions;
using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("NodeSupport".TruncatePascal(6));
            Console.WriteLine("Select test to run:");
            Console.WriteLine(" a) All");
            Console.WriteLine(" b) Dependency Extraction");
            Console.WriteLine(" c) Matrix Multiplication");
            Console.WriteLine(" d) Matrix Addition");
            Console.WriteLine(" e) Delaunay Triangulation");
            Console.WriteLine(" f) Unserialisable Types");
            Console.WriteLine(" g) GWA Serialisation");
            Console.WriteLine(" h) Uniques Serialisation");
            Console.WriteLine(" i) Matrix Inversion");
            Console.WriteLine(" j) Node Creation Tree Speed");
            Console.WriteLine(" k) Word Reading");
            Console.WriteLine(" l) Word Writing");
            Console.WriteLine(" m) Map Reading");
            Console.WriteLine(" n) Address Finding");
            Console.WriteLine(" o) Hyperlink detection");
            Console.WriteLine(" p) Excel Timesheet Unique Descriptions");
            Console.WriteLine(" q) ETABS Writing");
            Console.WriteLine(" r) Wind Calc");
            Console.WriteLine(" s) Self-Intersection");
            Console.WriteLine(" t) Triangle Barycentric Coordinates");
            Console.WriteLine(" u) Log markup");

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
                Reflection_Tests.PrintUnserializableTypes(typeof(ModelDocument).Assembly);
            }

            if (option == 'a' || option == 'g')
            {
                GWA_Tests.SerializeToGWA();
            }

            if (option == 'a' || option == 'h')
            {
                UniqueFormatter_Tests.SerializeToFormat();
            }

            if (option == 'a' || option == 'i')
            {
                Matrix_Tests.InverseTest();
            }

            if (option == 'a' || option == 'j')
            {
                DDTree_Tests.NodeCreation(10);
            }

            if (option == 'a' || option == 'k')
            {
                //Doc tests:
                Word_Tests.ReadDocTest();
            }

            if (option == 'a' || option == 'l')
            {
                Word_Tests.WriteDocTest();
            }

            if (option == 'a' || option == 'm')
            {
                Map_Tests.DownloadMapTest();
            }

            if (option == 'n') //Not automated due to need for user input
            {
                Console.WriteLine("Enter Address:");
                string address = Console.ReadLine();
                Console.WriteLine(Map_Tests.AddressToLat(address));
                Map_Tests.DownloadAddressTest(address);
            }

            if (option == 'a' || option == 'o')
            {
                foreach (string str in "This is a test string (http://www.google.com) with some hyperlinks http://blog.ramboll.com/rcd/ in.".SplitHyperlinks())
                {
                    Console.WriteLine("'" + str + "'");
                }
            }

            if (option == 'p')
            {
                Excel_Tests.ProcessTimeSheet();
            }

            if (option == 'a' || option == 'q')
            {
                ETABS_Tests.WriteToETABS();
            }

            if (option == 'a' || option == 'r')
            {
                Calc_Tests.WindCalcTest();
            }

            if (option == 'a' || option == 's')
            {
                Curve_Tests.SelfIntersectionTest();
            }

            if (option == 'a' || option == 't')
            {
                Triangle_Tests.BarycentricCoordinates_Test();
            }

            if (option == 'a' || option == 'u')
            {
                Log_Tests.LogScriptTest();
            }

            Console.Read();
        }
    }
}
