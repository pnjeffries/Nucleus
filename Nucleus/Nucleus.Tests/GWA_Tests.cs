using Nucleus.Geometry;
using Nucleus.Model;
using Nucleus.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Tests
{
    public static class GWA_Tests
    {
        public static TimeSpan SerializeToGWA()
        {
            var sw = new Stopwatch();
            ModelDocument doc = new ModelDocument();
            doc.Model.Create.LinearElement(new Line(0, 0, 10, 0));
            doc.Model.Create.LinearElement(new Line(0, 10, 10, 0));
            doc.Model.Create.LinearElement(new Line(0, 0, -10, 0));
            doc.Model.GenerateNodes(new NodeGenerationParameters());
            doc.Model.Add(new LinearElementSet(doc.Model.Elements));
            doc.Model.Add(new NodeSet(doc.Model.Nodes));

            var format = new GWAFormat();
            Core.Print(format.ToString());
            format.Save("C:\\TEMP\\GWAFormat.txt");
            var context = new GWAContext();
            var serialiser = new ModelDocumentTextSerialiser(format, context);
            sw.Start();
            Core.Print(serialiser.Serialize(doc));
            sw.Stop();

            Core.Print(context.IDMap.ToCSV());

            return sw.Elapsed;
        }
    }
}
