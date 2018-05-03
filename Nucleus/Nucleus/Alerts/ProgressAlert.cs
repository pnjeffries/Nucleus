using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Alerts
{
    /// <summary>
    /// An alert which reports on the overall progress of an operation
    /// </summary>
    [Serializable]
    public class ProgressAlert : Alert
    {
        #region Properties

        /// <summary>
        /// Private backing field for Progress property
        /// </summary>
        private double _Progress = 0;

        /// <summary>
        /// The progress being reported - typically expressed
        /// from 0-1.
        /// </summary>
        public double Progress
        {
            get { return _Progress; }
            set { ChangeProperty(ref _Progress, value, "Progress"); }
        }

        public override string DisplayText
        {
            get
            {
                return Message + " " + ((int)(Progress * 100)) + "%";
            }
        }

        #endregion

        #region Constructors


        public ProgressAlert(string message, double progress, AlertLevel level = AlertLevel.Information) : base(message, level)
        {
            _Progress = progress;
        }

        public ProgressAlert(string alertID, string message, double progress, AlertLevel level = AlertLevel.Information) : base(alertID, message, level)
        {
            _Progress = progress;
        }

        #endregion

        #region Methods

        public override void Merge(Alert other)
        {
            if (other is ProgressAlert)
            {
                _Progress = ((ProgressAlert)other).Progress;
            }
            base.Merge(other);
        }

        #endregion
    }
}
