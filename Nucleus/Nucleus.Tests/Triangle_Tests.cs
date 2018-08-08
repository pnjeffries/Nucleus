using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Tests
{
    public static class Triangle_Tests
    {
        public static void BarycentricCoordinates_Test()
        {
            double x = 0.5, y = 0.5;
            Vector pt1 = new Vector(0, 0, 0);
            Vector pt2 = new Vector(0, 1, 1);
            Vector pt3 = new Vector(1, 1, 1);
            double s, t, u;
            Triangle.BarycentricCoordinates(x, y, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, out s, out t);
            u = 1.0 - s - t;
            Core.Print("s = " + s);
            Core.Print("t = " + t);
            Core.Print("u = " + u);

            Vector rePt = Triangle.PointFromBarycentric(s, t, u, pt1, pt2, pt3);
            Core.Print("pt = " + rePt);
            double z = Triangle.ZCoordinateOfPoint(x, y, pt1, pt2, pt3);
            Core.Print("z = " + z);
        }
    }
}
