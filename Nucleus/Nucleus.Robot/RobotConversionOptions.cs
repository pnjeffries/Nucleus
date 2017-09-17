using Nucleus.Conversion;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Robot
{
    /// <summary>
    /// A set of options to be used when writing from Nucleus to Robot 
    /// </summary>
    public class RobotConversionOptions : ModelConversionOptions, IAutoUIHostable
    {
       #region Constructors

        /// <summary>
        /// Initialise a new RobotConversionOptions with the default values
        /// </summary>
        public RobotConversionOptions()
        {

        }

        /// <summary>
        /// Initialise a new RobotConversionOptions, specifying whether or not to update
        /// </summary>
        /// <param name="update"></param>
        public RobotConversionOptions(bool update)
        {
            Update = update;
        }

        /// <summary>
        /// Initialise a new RobotConversionOptions set to update objects
        /// modified since the specified date
        /// </summary>
        /// <param name="updateSince"></param>
        public RobotConversionOptions(DateTime updateSince)
        {
            UpdateSince = updateSince;
        }

        #endregion
    }
}
