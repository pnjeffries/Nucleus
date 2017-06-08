using FreeBuild.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Robot
{
    /// <summary>
    /// A set of options to be used when writing from FreeBuild to Robot 
    /// </summary>
    public class RobotConversionOptions : ConversionOptions
    {
        /// <summary>
        /// Private backing field for UpdateSince property
        /// </summary>
        private DateTime _UpdateSince = DateTime.MinValue;

        /// <summary>
        /// Gets or sets the time of the prior update/read.
        /// If specified, only those objects which have been modified since this time
        /// will be updated here.
        /// </summary>
        public DateTime UpdateSince
        {
            get { return _UpdateSince; }
            set { _UpdateSince = value;  NotifyPropertyChanged("UpdateSince"); }
        }

        /// <summary>
        /// Should this be updated since the last modfications, rather than being a full rewrite of the
        /// file.
        /// </summary>
        public bool Update
        {
            get { return _UpdateSince != DateTime.MinValue; }
        }

        #region Constructors

        /// <summary>
        /// Initialise a new RobotConversionOptions with the default values
        /// </summary>
        public RobotConversionOptions()
        {

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
