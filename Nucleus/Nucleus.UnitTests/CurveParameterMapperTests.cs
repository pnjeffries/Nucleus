using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.Geometry;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class CurveParameterMapperTests
    {
        [TestMethod]
        public void MapAToB_Parameter_ShouldMapToEquivalent()
        {
            var c1 = new Line(0, 0, 10, 0);
            var c2 = new Line(0, 0, 5, 0);
            var mapper = new CurveParameterMapper(c1, c2);
            mapper.AddSpanDomain(new Interval(0, 0.5));
            double t1 = 0.25;
            double t2 = mapper.MapAtoB(t1);
            Assert.AreEqual(0.5, t2, 0.0001);
        }

        [TestMethod]
        public void MapBToA_Parameter_ShouldMapToEquivalent()
        {
            var c1 = new Line(0, 0, 10, 0);
            var c2 = new Line(0, 0, 5, 0);
            var mapper = new CurveParameterMapper(c1, c2);
            mapper.AddSpanDomain(new Interval(0, 0.5));
            double t1 = 0.5;
            double t2 = mapper.MapBtoA(t1);
            Assert.AreEqual(0.25, t2, 0.0001);
        }
    }
}
