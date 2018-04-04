using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Information about an update cycle
    /// </summary>
    public class UpdateInfo
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the TimeStep property
        /// </summary>
        private double _TimeStep = 0;

        /// <summary>
        /// The timestep between the previous update and this one, in seconds.
        /// </summary>
        public double TimeStep
        {
            get { return _TimeStep; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialise a new UpdateInfo
        /// </summary>
        /// <param name="timeStep"></param>
        public UpdateInfo(double timeStep)
        {
            _TimeStep = timeStep;
        }

        #endregion
    }
}
