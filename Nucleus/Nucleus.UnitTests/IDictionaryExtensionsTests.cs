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
    public class IDictionaryExtensionsTests
    {
        [TestMethod]
        public void GetValueFromPathDictionaryTest()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("Not This", "Bad!");
            dictionary.Add("This", "Good!");

            object value = dictionary.GetFromPath("[This]");

            Assert.AreEqual("Good!", value);

        }
    }
}
