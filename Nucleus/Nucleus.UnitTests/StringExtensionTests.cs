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
    public class StringExtensionTests
    {
        [TestMethod]
        public void TokeniseBrackets_ShouldOnlySplitOutsideBracket()
        {
            var str = "Hello, Function(arg1,arg2), Goodbye";
            var tokens = str.TokeniseOutsideBrackets();
            Assert.AreEqual(3, tokens.Count);
        }

        [TestMethod]
        public void TokeniseBrackets_ShouldNotSplit()
        {
            var str = "Function(arg1,arg2)";
            var tokens = str.TokeniseOutsideBrackets();
            Assert.AreEqual(1, tokens.Count);
        }
    }
}
