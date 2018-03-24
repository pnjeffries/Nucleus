using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Tests
{
    public static class Curve_Tests
    {
        public static void SelfIntersectionTest()
        {
            var pline = PolyLine.Rectangle(2, 2);
            bool test = pline.IsSelfIntersectingXY();
            Core.Print(test.ToString());
        }
    }
}
