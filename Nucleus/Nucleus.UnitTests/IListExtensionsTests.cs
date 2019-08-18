using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Extensions;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class IListExtensionsTests
    {
        [TestMethod]
        public void FindNext_ShouldReturnNextHighest()
        {
            var values = new List<Tuple<double>>()
            {
                new Tuple<double>(0.15),
                new Tuple<double>(0.6),
                new Tuple<double>(0.1),
                new Tuple<double>(0.65)
            };
            double nextValue = values.ItemWithNext(i => i.Item1, 0.1).Item1;
            Assert.AreEqual(0.15, nextValue);
        }
    }
}