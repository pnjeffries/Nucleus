using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Assert.AreEqual(0, unserializable.Count);
        }
    }
}
