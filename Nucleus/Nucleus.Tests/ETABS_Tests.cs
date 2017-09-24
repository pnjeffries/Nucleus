using Nucleus.ETABS;
using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Tests
{
    public class ETABS_Tests
    {
        public static TimeSpan WriteToETABS()
        {
            var sw = new Stopwatch();

            ModelDocument doc = new ModelDocument();
            doc.Model.Create.LinearElement(new Line(0, 0, 10, 0));
            doc.Model.Create.LinearElement(new Line(0, 10, 10, 0));
            doc.Model.Create.LinearElement(new Line(0, 0, -10, 0));
            doc.Model.GenerateNodes(new NodeGenerationParameters());
            doc.Model.Add(new LinearElementSet(doc.Model.Elements));
            doc.Model.Add(new NodeSet(doc.Model.Nodes));

            var etabs = new ETABSClient();
            var idMap = new ETABSIDMappingTable();
            etabs.WriteModelToEtabs("C:\\Temp\\Test3.edb", doc.Model, ref idMap);
            etabs.Close();
            etabs.Release();

            sw.Stop();

            return sw.Elapsed;
        }
    }
}
