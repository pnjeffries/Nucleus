using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A record of an AI target
    /// </summary>
    [Serializable]
    public class TargetAIRecord : NotifyPropertyChangedBase
    {
        #region Properties

        private Element _Target = null;

        /// <summary>
        /// The targetted element
        /// </summary>
        public Element Target
        {
            get { return _Target; }
            set { ChangeProperty(ref _Target, value); }
        }

        private double _Aggro = 0;

        /// <summary>
        /// The level of awareness, or aggro of the specified target
        /// </summary>
        public double Aggro
        {
            get { return _Aggro; }
            set { ChangeProperty(ref _Aggro, value); }
        }

        private Vector _LastKnownPosition;

        /// <summary>
        /// The last known location of this target
        /// </summary>
        public Vector LastKnownPosition
        {
            get { return _LastKnownPosition; }
            set { ChangeProperty(ref _LastKnownPosition, value); }
        }

        #endregion

        #region Constructors

        public TargetAIRecord(Element target)
        {
            _Target = target;
        }

        #endregion
    }
}
