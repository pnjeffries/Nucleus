using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A basic wide path object that can be used to simply generate edge curves
    /// of a path segment.
    /// </summary>
    [Serializable]
    public class WidePathBasic : Unique, IWidePath
    {
        #region Properties

        /// <summary>
        /// Private backing field for Spine Property
        /// </summary>
        private Curve _Spine;

        /// <summary>
        /// The spine curve from which the outer curves are to be derived
        /// </summary>
        public Curve Spine
        {
            get { return _Spine; }
            set { ChangeProperty(ref _Spine, value, "Spine"); }
        }

        /// <summary>
        /// Private backing field for LeftEdge property
        /// </summary>
        private Curve _LeftEdge = null;

        /// <summary>
        /// The left edge curve
        /// </summary>
        public Curve LeftEdge
        {
            get { return _LeftEdge; }
            set { ChangeProperty(ref _LeftEdge, value, "LeftEdge"); }
        }

        /// <summary>
        /// Private backing field for RightEdge property
        /// </summary>
        private Curve _RightEdge = null;

        /// <summary>
        /// The right edge curve
        /// </summary>
        public Curve RightEdge
        {
            get { return _RightEdge; }
            set { ChangeProperty(ref _RightEdge, value, "RightEdge"); }
        }

        /// <summary>
        /// Private backing field for LeftOffset property
        /// </summary>
        private double _LeftOffset = 0;

        /// <summary>
        /// The offset distance of the left edge from the spine
        /// </summary>
        public double LeftOffset
        {
            get { return _LeftOffset; }
            set { ChangeProperty(ref _LeftOffset, value, "LeftOffset"); }
        }

        /// <summary>
        /// Private backing field for RightOffset property
        /// </summary>
        private double _RightOffset = 0;

        /// <summary>
        /// The offset distance of the right edge from the spine
        /// </summary>
        public double RightOffset
        {
            get { return _RightOffset; }
            set { ChangeProperty(ref _RightOffset, value, "RightOffset"); }
        }

        /// <summary>
        /// Private backing field for StartCapLeft property
        /// </summary>
        private Curve _StartCapLeft = null;

        /// <summary>
        /// The curve capping the left side of the starting end of the path.
        /// Will usually be null.
        /// </summary>
        public Curve StartCapLeft
        {
            get { return _StartCapLeft; }
            set { ChangeProperty(ref _StartCapLeft, value, "StartCapLeft"); }
        }

        /// <summary>
        /// Private backing field for StartCapRight property
        /// </summary>
        private Curve _StartCapRight = null;

        /// <summary>
        /// The curve capping the right side of the starting end of the path.
        /// Will usually be null.
        /// </summary>
        public Curve StartCapRight
        {
            get { return _StartCapRight; }
            set { ChangeProperty(ref _StartCapRight, value, "StartCapRight"); }
        }

        /// <summary>
        /// Private backing field for EndCapLeft property
        /// </summary>
        private Curve _EndCapLeft = null;

        /// <summary>
        /// The curve capping the left side of the end of the path.
        /// Will usually be null.
        /// </summary>
        public Curve EndCapLeft
        {
            get { return _EndCapLeft; }
            set { ChangeProperty(ref _EndCapLeft, value, "EndCapLeft"); }
        }

        /// <summary>
        /// Private backing field for EndCapRight property
        /// </summary>
        private Curve _EndCapRight = null;

        /// <summary>
        /// The curve capping the right side of the end of the path.
        /// Will usually be null.
        /// </summary>
        public Curve EndCapRight
        {
            get { return _EndCapRight; }
            set { ChangeProperty(ref _EndCapRight, value, "EndCapRight"); }
        }

        

        double IWidePath.LeftEndPinch
        {
            get
            {
                return 0;
            }
        }

        double IWidePath.RightEndPinch
        {
            get
            {
                return 0;
            }
        }


        #endregion

        #region Constructors

        public WidePathBasic(Curve spine, double width)
        {
            Spine = spine;
            LeftOffset = width / 2;
            RightOffset = width / 2;
        }

        public WidePathBasic(Curve spine, double leftOffset, double rightOffset)
        {
            Spine = spine;
            LeftOffset = leftOffset;
            RightOffset = rightOffset;
        }

        #endregion

        #region Static Functions

        /// <summary>
        /// Generate a set of WidePathBasic objects for the specified network of curves and with the specified width.
        /// Edges may also be optionally generated.
        /// </summary>
        /// <param name="curves"></param>
        /// <param name="width"></param>
        /// <param name="generateEdges"></param>
        /// <returns></returns>
        public static IList<WidePathBasic> GenerateForCurves(IList<Curve> curves, double width, bool generateEdges = true)
        {
            var result = new List<WidePathBasic>();
            foreach (Curve curve in curves)
            {
                result.Add(new Geometry.WidePathBasic(curve.Duplicate(), width));
            }
            if (generateEdges)
            {
                result.GenerateNetworkPathNodes(new Model.NodeGenerationParameters());
                result.GenerateNetworkPathEdges();
            }
            return result;
        }

        /// <summary>
        /// Generate a set of WidePathBasic objects for the specified network of curves and with the specified width.
        /// Edges may also be optionally generated.
        /// </summary>
        /// <param name="curves"></param>
        /// <param name="width"></param>
        /// <param name="generateEdges"></param>
        /// <returns></returns>
        public static IList<WidePathBasic> GenerateForCurves(IList<Curve> curves, double leftOffset, double rightOffset, bool generateEdges = true)
        {
            var result = new List<WidePathBasic>();
            foreach (Curve curve in curves)
            {
                result.Add(new WidePathBasic(curve.Duplicate(), leftOffset, rightOffset));
            }
            if (generateEdges)
            {
                result.GenerateNetworkPathNodes(new Model.NodeGenerationParameters());
                result.GenerateNetworkPathEdges();
            }
            return result;
        }

        #endregion
    }
}
