using FreeBuild.Geometry;
using FreeBuild.IO;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Tests
{
    public class UniqueFormatter_Tests
    {
        public static TimeSpan SerializeToFormat()
        {
            var sw = new Stopwatch();
            ModelDocument doc = new ModelDocument();
            doc.Model.Create.LinearElement(new Line(0, 0, 10, 0));
            doc.Model.GenerateNodes(new NodeGenerationParameters());

            sw.Start();
            var formatter = new UniqueFormatter();
            formatter.Serialize(null, doc);
            sw.Stop();
            string format = formatter.GenerateFormatDescription();
            Core.Print(format);

            var formatter2 = new UniqueFormatter();
            formatter2.Serialize(null, null);
            formatter2.ReadFormat(new StringReader(format).ReadLine());

            return sw.Elapsed;
        }
    }
}
