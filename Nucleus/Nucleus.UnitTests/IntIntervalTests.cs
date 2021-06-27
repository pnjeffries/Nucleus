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
    public class IntIntervalTests
    {
        [TestMethod]
        public void Enumerator_ShouldIncrement()
        {
            var interval = new IntInterval(0, 5);
            int count = 0;
            foreach (var value in interval)
            {
                Assert.AreEqual(count, value);
                count++;
            }
        }

        [TestMethod]
        public void Enumerator_ShouldDecrement()
        {
            var interval = new IntInterval(5, 0);
            int count = 5;
            foreach (var value in interval)
            {
                Assert.AreEqual(count, value);
                count--;
            }
        }
    }
}
