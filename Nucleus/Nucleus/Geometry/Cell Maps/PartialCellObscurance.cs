using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Class that represents partial obscurance of a map cell
    /// </summary>
    [Serializable]
    public class PartialCellObscurance : CellObscurance
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the StartAngle property
        /// </summary>
        private Angle _StartAngle;

        /// <summary>
        /// The starting angle of the shadow region
        /// </summary>
        public Angle StartAngle
        {
            get { return _StartAngle; }
            set { _StartAngle = value; }
        }

        /// <summary>
        /// Private backing member variable for the EndAngle property
        /// </summary>
        private Angle _EndAngle;

        /// <summary>
        /// The ending angle of the shadow region
        /// </summary>
        public Angle EndAngle
        {
            get { return _EndAngle; }
            set { _EndAngle = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PartialCellObscurance() { }

        /// <summary>
        /// Start and end angle constructor
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public PartialCellObscurance(Angle start, Angle end)
        {
            _StartAngle = start;
            _EndAngle = end;
        }

        #endregion

        #region Methods

        public override bool IsUnobscured(Vector positionInCell, Vector sourcePosition)
        {
            Angle angle = sourcePosition.AngleTo(positionInCell).NormalizeTo2PI();
            if (_StartAngle <= _EndAngle)
            {
                return (angle <= _StartAngle || angle >= _EndAngle);
            }
            else
            {
                return (angle <= _StartAngle && angle >= _EndAngle);
            }
        }

        #endregion
    }
}
