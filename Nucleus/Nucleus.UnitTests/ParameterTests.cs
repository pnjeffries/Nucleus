using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class ParameterTests
    {
        [TestMethod]
        public void GroupingTest()
        {
            var group1 = new ParameterGroup("Group 1");
            var group2 = new ParameterGroup("Group 2");
            var paras = new ParameterCollection();
            paras.Add(new DoubleParameter("Para 1A", group1, 1.0));
            paras.Add(new DoubleParameter("Para 1B", group1, 2.0));
            paras.Add(new DoubleParameter("Para 2A", group2, 3.0));

            var grouped = paras.GetGroupedParameters();
            Assert.AreEqual(2, grouped.Count);
            Assert.AreEqual(2, grouped[group1].Count);
            Assert.AreEqual(1, grouped[group2].Count);
        }
    }
}
