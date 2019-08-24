using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UnitTests
{
    [TestClass]
    public class WidePathTests
    {
        [TestMethod]
        public void KinkyPath_ShouldResolveWithoutSpikes()
        {
            var path1 = new WidePathBasic(new Line(0, 0, 5, 0), 4);
            var path2 = new WidePathBasic(new Line(5, 0, 5, 1), 4);
            var path3 = new WidePathBasic(new Line(5, 1, 10, 1), 4);
            IList<IWidePath> paths = new List<IWidePath>(){path1, path2,path3};
            paths.GenerateNetworkPathNodes(new Model.NodeGenerationParameters());
            paths.GenerateNetworkPathEdges();
            var leftSides = paths.ExtractNetworkPathLeftEdges();
            var rightSides = paths.ExtractNetworkPathRightEdges();
            var edges = paths.ExtractAllEdges();
            Assert.AreEqual(1, path2.RightEdge.Length);
            Assert.AreEqual(1, path2.LeftEdge.Length);
            Assert.AreEqual(30, edges.TotalLength());
        }

        [TestMethod]
        public void KinkyPath_ShouldResolveWithoutSpikes2()
        {
            var path1 = new WidePathBasic(new Line(0, 1, 5, 1), 4);
            var path2 = new WidePathBasic(new Line(5, 1, 5, 0), 4);
            var path3 = new WidePathBasic(new Line(5, 0, 10, 0), 4);
            IList<IWidePath> paths = new List<IWidePath>() { path1, path2, path3 };
            paths.GenerateNetworkPathNodes(new Model.NodeGenerationParameters());
            paths.GenerateNetworkPathEdges();
            var leftSides = paths.ExtractNetworkPathLeftEdges();
            var rightSides = paths.ExtractNetworkPathRightEdges();
            var edges = paths.ExtractAllEdges();
            Assert.AreEqual(1, path2.RightEdge.Length);
            Assert.AreEqual(1, path2.LeftEdge.Length);
            Assert.AreEqual(30, edges.TotalLength());
        }

        [TestMethod]
        public void VariableWidthPath_ShouldHaveStep()
        {
            var path1 = new WidePathBasic(new Line(0, 0, 5, 0), 4);
            var path2 = new WidePathBasic(new Line(5, 0, 10, 0), 6);
            IList<IWidePath> paths = new List<IWidePath>() { path1, path2 };
            paths.GenerateNetworkPathNodes(new Model.NodeGenerationParameters());
            paths.GenerateNetworkPathEdges();
            var leftSides = paths.ExtractNetworkPathLeftEdges();
            var rightSides = paths.ExtractNetworkPathRightEdges();
            var edges = paths.ExtractAllEdges();
            Assert.AreEqual(32, edges.TotalLength());
        }

        [TestMethod]
        public void VariableWidthBent_ShouldFitSmoothly()
        {
            var path1 = new WidePathBasic(new Line(0, 5, 5, 0), 4);
            var path2 = new WidePathBasic(new Line(5, 0, 10, 0), 6);
            IList<IWidePath> paths = new List<IWidePath>() { path1, path2 };
            paths.GenerateNetworkPathNodes(new Model.NodeGenerationParameters());
            paths.GenerateNetworkPathEdges();
            var leftSides = paths.ExtractNetworkPathLeftEdges();
            var rightSides = paths.ExtractNetworkPathRightEdges();
            var edges = paths.ExtractAllEdges();
            Assert.AreEqual(34.142, edges.TotalLength(), 0.001);
        }
    }
}
