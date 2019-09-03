using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class PerlinNoiseTests
    {
        [TestMethod]
        public void PerlinNoise_ShouldBeZero()
        {
            double value = PerlinNoise.Noise(0, 0, 0);
            Assert.AreEqual(0, value, 0.0001);
        }

        [TestMethod]
        public void PerlinNoise_ShouldNotBeZero()
        {
            double value = PerlinNoise.Noise(0.5, 0.5, 0);
            Assert.AreEqual(-0.25, value, 0.0001);
        }
    }
}
