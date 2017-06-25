using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Model;
using Nucleus.Geometry;

namespace Nucleus.Tests
{
    public class Core
    {
        public static void Print(string message)
        {
            Console.WriteLine(message);
        }

        public static ModelDocument GenerateTestModel(int floors = 10)
        {
            Core.Print("Generating test model of " + floors * 12 + " elements...");

            ModelDocument doc = new ModelDocument();

            double floorHeight = 4;

            for (int i = 0; i < floors; i++)
            {
                double z = floorHeight * (i + 1);
                for (int j = 0; j < 10; j += 2)
                {
                    doc.Model.Create.LinearElement(new Line(0, j, z, 10, j, z));
                }
                doc.Model.Create.LinearElement(new Line(0, 0, z, 0, 10, z));
                doc.Model.Create.LinearElement(new Line(10, 0, z, 10, 10, z));
            }

            doc.Model.GenerateNodes(new NodeGenerationParameters());

            Core.Print("Done.");

            return doc;
        }
    }
}
