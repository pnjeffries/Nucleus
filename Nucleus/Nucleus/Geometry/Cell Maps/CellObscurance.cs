using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Class to represent the obscurance condition of a map cell
    /// </summary>
    [Serializable]
    public class CellObscurance
    {
        #region Constants

        /// <summary>
        /// The condition that represents full unobscurance
        /// </summary>
        public static readonly CellObscurance Unobscured = new CellObscurance();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CellObscurance() { }

        #endregion

        #region Methods

        /// <summary>
        /// Is the specified position in the cell unobscured from the specified
        /// source position
        /// </summary>
        /// <param name="positionInCell"></param>
        /// <param name="sourcePosition"></param>
        /// <returns></returns>
        public virtual bool IsUnobscured(Vector positionInCell, Vector sourcePosition)
        {
            return true;
        }

        #endregion
    }
}
