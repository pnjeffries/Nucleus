using Nucleus.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Tests
{
    public static class Calc_Tests
    {
        public static void WindCalcTest()
        {
            var sb = new StringBuilder();
            var wc = new WindLoadsBox();
            wc.Test(sb);
            Core.Print(sb.ToString());
        }
    }
}
