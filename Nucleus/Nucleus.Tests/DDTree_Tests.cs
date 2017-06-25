using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Tests
{
    public static class DDTree_Tests
    {
        public static void NodeCreation(int cycles = 5)
        {
            int cycleSize = 200;
            Model.Model model = new Model.Model();
            Random rng = new Random();
            for (int i = 1; i <= cycles; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                for (int j = 0; j < cycleSize; j++)
                {
                    model.Create.Node(new Geometry.Vector(rng.NextDouble() * 100, rng.NextDouble() * 100, rng.NextDouble() * 100), 1);
                }
                sw.Stop();
                Core.Print("Cycle " + i + ": " + model.Nodes.Count + " Nodes: " + sw.Elapsed);
            }
        }
    }
}
