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
    public class IntervalTests
    {
        [TestMethod]
        public void IntervalBooleanTest()
        {
            var subtractors = new List<Interval>()
            {
                new Interval(0.939584287478505, 0.144604415745046)
            };
            var test = new Interval(0.877977251819598, 0.216248693902337);
            var result = test.BooleanDifference(subtractors);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(new Interval(0.877977251819598, 0.939584287478505), result[0]);
            Assert.AreEqual(new Interval(0.144604415745046, 0.216248693902337), result[1]);
        }

        [TestMethod]
        public void IntervalBooleanTest2()
        {
            var subtractors = new List<Interval>()
            {
                new Interval(0.290137140140111, 0.461615654656265)
            };
            var test = new Interval(0.784400005286198, 0.347037049103518);
            var result = test.BooleanDifference(subtractors);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(new Interval(0.784400005286198, 0.290137140140111), result[0]);
        }
    }
}
