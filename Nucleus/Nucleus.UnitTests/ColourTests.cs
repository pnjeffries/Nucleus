using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Rendering;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class ColourTests
    {
        [TestMethod]
        public void ColourToHex()
        {
            var colour = Colour.Blue;
            Assert.AreEqual("FF0000FF", colour.ToHex());
        }
    }
}
