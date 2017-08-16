using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;
using RC = Rhino.Geometry;

namespace Nucleus.Rhino
{
    public class RhinoCurve : Geometry.WrappedCurve
    {
        #region Properties

        /// <summary>
        /// Private backing field for the Curve property
        /// </summary>
        private RC.Curve _Curve;

        /// <summary>
        /// The wrapped Rhino curve
        /// </summary>
        public RC.Curve Curve
        {
            get { return _Curve; }
        }

        public override bool Closed
        {
            get
            {
                return _Curve.IsClosed;
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsValid
        {
            get
            {
                return _Curve != null && _Curve.IsValid;
            }
        }

        public override VertexCollection Vertices
        {
            get
            {
                return new VertexCollection();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new RhinoCurve wrapping the specified
        /// Rhino curve object.
        /// </summary>
        /// <param name="curve"></param>
        public RhinoCurve(RC.Curve curve)
        {
            _Curve = curve;
        }

        #endregion

        #region Methods

        public override double CalculateLength()
        {
            return _Curve.GetLength();
        }

        public override double CalculateEnclosedArea(out Vector centroid, Plane onPlane = null)
        {
            RC.Plane rcp = FBtoRC.Convert(onPlane);
            RC.AreaMassProperties.Compute(_Curve);
            return base.CalculateEnclosedArea(out centroid, onPlane);
        }

        #endregion
    }
}
