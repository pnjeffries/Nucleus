using FreeBuild.Geometry;
using FreeBuild.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A coordinate system reference that wraps a user-defined coordinate system
    /// </summary>
    [Serializable]
    public class UserCoordinateSystemReference : CoordinateSystemReference
    {
        #region Properties

        /// <summary>
        /// Private backing field for CoordinateSystem property
        /// </summary>
        private ICoordinateSystem _CoordinateSystem;

        /// <summary>
        /// The geometric coordinate system stored within this object
        /// </summary>
        [AutoUI(300)]
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
        public UserCoordinateSystemReference() : base()
        {
            _CoordinateSystem = CartesianCoordinateSystem.Global;
        }

        /// <summary>
        /// Initialise a new user coordinate system
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cSystem"></param>
        public UserCoordinateSystemReference(string name, ICoordinateSystem cSystem) : base()
        {
            Name = name;
            _CoordinateSystem = cSystem;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the coordinate system defined by this object for the specified position along a linear element
        /// </summary>
        /// <param name="element">The linear element the coordinate system relates to</param>
        /// <param name="t">The position along the linear element that the coordinate system relates to</param>
        /// <returns></returns>
        public override ICoordinateSystem GetCoordinateSystem(LinearElement element, double t)
        {
            return CoordinateSystem;
        }

        public Vector GlobalToLocal(Vector vector, bool direction = false)
        {
            return _CoordinateSystem.GlobalToLocal(vector, direction);
        }

        public Vector LocalToGlobal(Vector vector, bool direction = false)
        {
            return _CoordinateSystem.LocalToGlobal(vector, direction);
        }

        public Vector LocalToGlobal(double c0, double c1, double c2 = 0, bool direction = false)
        {
            return _CoordinateSystem.LocalToGlobal(c0, c1, c2, direction);
        }

        #endregion
    }
}
