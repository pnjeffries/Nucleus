using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class LogTests
    {
        public void LoggingSubjectNameAndGenderTest()
        {
            var log = new StringLog();
            Random rng = new Random();

            var subject0 = new PointElement("Susan");
            subject0.Data.Add(new ElementGender(Base.Gender.Feminine));
            var subject1 = new PointElement("Bob");
            subject1.Data.Add(new ElementGender(Base.Gender.Masculine));
            log.WriteMarkup("{SUBJECT[0|Name]} is {GENDER[0|genderless|male|female]}.  {SUBJECT[1|Name]} is {GENDER[1|genderless|male|female]}.", rng, subject0, subject1);

            Assert.AreEqual("Susan is female.  Bob is male.", log.ToString());
        }
    }
}
