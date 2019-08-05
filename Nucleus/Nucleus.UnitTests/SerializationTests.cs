using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void SerializableAttributeTest()
        {
            // Checks for types not marked as serializable
            var unserializable = typeof(ModelObject).Assembly.GetUnserializableTypes();
            if (unserializable.Count > 0)
            {
                Console.WriteLine("The following types are not serializable:");
                foreach (var type in unserializable)
                {
                    Console.WriteLine(" - " + type.FullName);
                }
                Console.WriteLine(
                    "To prevent errors during file serialization, mark these classes with the [Serializable] attribute.");
            }

            Assert.AreEqual(0, unserializable.Count);
        }

        [TestMethod]
        public void SerializeModelDocumentAndDeserialize()
        {
            FilePath path = "C:/TEMP/SerializeModelDocumentAndDeserializeTest.test";

            var model = new Model.Model();
            model.Add(new LinearElement(0, 0, 0, 10, 0, 0));
            var doc = new ModelDocument(model);
            doc.SaveAs(path);

            var doc2 = ModelDocument.Load(path);
            Assert.AreEqual(1, doc2.Model.Elements.Count);
        }
    }
}
