using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Base class for game stats which may be modified
    /// by equipment, status effects and so on
    /// </summary>
    [Serializable]
    public abstract class Stat
    {
        private double _Value = 0;

        /// <summary>
        /// The current value
        /// </summary>
        public double Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Stat() { }

        /// <summary>
        /// Value constructor
        /// </summary>
        /// <param name="value"></param>
        public Stat(double value) { _Value = value; }

        #endregion

        #region Operators

        /// <summary>
        /// Implicit conversion to a double
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator double(Stat value) { return value.Value; }

        #endregion
    }
}
