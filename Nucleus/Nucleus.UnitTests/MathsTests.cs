using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class MathsTests
    {
        [TestMethod]
        public void PowerTest()
        {
            Assert.AreEqual(Math.Pow(2.67, 10), (2.67).Power(10),0.0000000001);
        }

        [TestMethod]
        public void IntPowerTest()
        {
            Assert.AreEqual(Math.Pow(3, 6), 3.Power(6));
        }
    }
}
