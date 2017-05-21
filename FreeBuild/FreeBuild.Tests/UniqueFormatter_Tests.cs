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
            ModelDocument doc = Core.GenerateTestModel(1000);

            /*sw.Start();
            var formatter = new UniqueFormatter();
            formatter.Serialize(null, doc);
            sw.Stop();
            string format = formatter.GenerateFormatDescription();
            Core.Print(format);

            var formatter2 = new UniqueFormatter();
            formatter2.Serialize(null, null);
            formatter2.ReadFormat(new StringReader(format).ReadLine());*/

            sw.Start();
            doc.SaveAs("C:/TEMP/SerializationControl.bin", DocumentSaveFileType.Binary);
            sw.Stop();
            Core.Print("Binary: " + sw.Elapsed);

            sw.Reset();
            sw.Start();
            doc.SaveAs("C:/TEMP/SerializationTest.ass", DocumentSaveFileType.ASS);
            sw.Stop();
            Core.Print("ASS: " + sw.Elapsed);

            return sw.Elapsed;
        }
    }
}
