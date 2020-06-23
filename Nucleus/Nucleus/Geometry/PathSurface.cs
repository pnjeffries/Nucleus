using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A planar surface which is defined as a 2D region surrounding a spine curve
    /// which may form part of a network of paths
    /// </summary>
    [Serializable]
    public class PathSurface : Surface, IWidePath
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
        /// Get the overall width of the path.
        /// The combination of LeftOffset and RightOffset
        /// </summary>
        public double Width
        {
            get { return _RightOffset + _LeftOffset; }
        }

        /// <summary>
        /// Private backing member variable for the StartOffset property
        /// </summary>
        private double _StartOffset = 0;

        /// <summary>
        /// The offset of the start edge from the start of the spine curve.  Only used when the path start does not connect into any other path segments.
        /// </summary>
        public double StartOffset
        {
            get { return _StartOffset; }
            set { ChangeProperty(ref _StartOffset, value); }
        }

        /// <summary>
        /// Private backing member variable for the EndOffset property
        /// </summary>
        private double _EndOffset = 0;

        /// <summary>
        /// The offset of the start edge from the end of the spine curve.  Only used when the path end does not connect into any other path segments.
        /// </summary>
        public double EndOffset
        {
            get { return _EndOffset; }
            set { ChangeProperty(ref _EndOffset, value); }
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


        public override VertexCollection Vertices => Spine.Vertices;

        public override bool IsValid => Spine.IsValid;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new path
        /// </summary>
        /// <param name="spine"></param>
        /// <param name="width"></param>
        public PathSurface(Curve spine, double width)
        {
            Spine = spine;
            LeftOffset = width / 2;
            RightOffset = width / 2;
        }
        
        /// <summary>
        /// Create a new path
        /// </summary>
        /// <param name="spine"></param>
        /// <param name="leftOffset"></param>
        /// <param name="rightOffset"></param>
        public PathSurface(Curve spine, double leftOffset, double rightOffset)
        {
            Spine = spine;
            LeftOffset = leftOffset;
            RightOffset = rightOffset;
        }

        #endregion

        #region Methods

        public override double CalculateArea(out Vector centroid)
        {
            // Temp
            double length = Spine.Length;
            centroid = Spine.PointAtLength(length / 2);
            return Spine.Length * Width;
        }

        public override CartesianCoordinateSystem LocalCoordinateSystem(int i, double u, double v, Angle orientation, Angle xLimit)
        {
            return Spine.LocalCoordinateSystem(u,orientation);
        }

        #endregion
    }
}
