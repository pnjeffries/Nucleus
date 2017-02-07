using FreeBuild.Geometry;
using FreeBuild.Model;
using FreeBuild.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Tests
{
    public static class GWA_Tests
    {
        public static TimeSpan SerializeToGWA()
        {
            var sw = new Stopwatch();
            ModelDocument doc = new ModelDocument();
            doc.Model.Create.LinearElement(new Line(0, 0, 10, 0));
            doc.Model.GenerateNodes(new NodeGenerationParameters());

            var serialiser = new ModelDocumentTextSerialiser(new GWAFormat(), new GWAContext());
            sw.Start();
            Core.Print(serialiser.Serialize(doc));
            sw.Stop();

            return sw.Elapsed;
        }
    }
}
