using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Tests
{
    public static class Log_Tests
    {
        public static void LogScriptTest()
        {
            var log = new StringLog();
            Random rng = new Random();
            log.WriteMarkup("This is a <B>Test</B>, which could read {RANDOM[option one|option two|flibble]}!", rng);

            var subject0 = new PointElement("Susan");
            subject0.Data.Add(new ElementGender(Base.Gender.Feminine));
            var subject1 = new PointElement("Bob");
            subject1.Data.Add(new ElementGender(Base.Gender.Masculine));
            log.WriteLine();
            log.WriteMarkup("{SUBJECT[0|Name]} is {GENDER[0|genderless|male|female]}. {SUBJECT[1|Name]} is {GENDER[1|genderless|male|female]}.", rng, subject0, subject1);
            Core.Print(log.ToString());
        }
    }
}
