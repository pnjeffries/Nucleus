using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    public class UserCoordinateSystem : ModelCoordinateSystem, ICoordinateSystem
    {
        #region Properties

        /// <summary>
        /// Private backing field for CoordinateSystem property
        /// </summary>
        private ICoordinateSystem _CoordinateSystem;

        /// <summary>
        /// The geometric coordinate system stored within this object
        /// </summary>
        public ICoordinateSystem CoordinateSystem
        {
            get { return _CoordinateSystem; }
            set { ChangeProperty(ref _CoordinateSystem, value, "CoordinateSystem"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new user coordinate system aligned to the
        /// global coordinate system
        /// </summary>
        public UserCoordinateSystem() : base()
        {
            _CoordinateSystem = CartesianCoordinateSystem.Global;
        }

        /// <summary>
        /// Initialise a new user coordinate system
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cSystem"></param>
        public UserCoordinateSystem(string name, ICoordinateSystem cSystem) : base()
        {
            Name = name;
            _CoordinateSystem = cSystem;
        }

        #endregion

        #region Methods

        public Vector GlobalToLocal(Vector vector, bool direction = false)
        {
            if (_CoordinateSystem != null) return _CoordinateSystem.GlobalToLocal(vector, direction);
            else return Vector.Unset;
        }

        public Vector LocalToGlobal(Vector vector, bool direction = false)
        {
            if (_CoordinateSystem != null) return _CoordinateSystem.LocalToGlobal(vector, direction);
            else return Vector.Unset;
        }

        public Vector LocalToGlobal(double c0, double c1, double c2 = 0, bool direction = false)
        {
            if (_CoordinateSystem != null) return _CoordinateSystem.LocalToGlobal(c0, c1, c2, direction);
            else return Vector.Unset;
        }

        #endregion
    }
}
