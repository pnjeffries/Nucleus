using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An ActionInputTrigger that is contextual to a particular map cell
    /// </summary>
    public class ActionCellInputTrigger : ActionInputTrigger
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the TargetCell property
        /// </summary>
        private int _TargetCell = -1;

        /// <summary>
        /// The index of the cell that is targeted for this action
        /// </summary>
        public int TargetCell
        {
            get { return _TargetCell; }
            set { _TargetCell = value; }
        }

        #endregion

        #region Constructor

        public ActionCellInputTrigger() { }

        public ActionCellInputTrigger(int targetCell, InputFunction input) : base(input)
        {
            _TargetCell = targetCell;
        }

        #endregion

        #region Methods

        public override bool Matches(ActionInputTrigger other)
        {
            return base.Matches(other) && other is ActionCellInputTrigger 
                && _TargetCell == ((ActionCellInputTrigger)other).TargetCell;
        }

        #endregion
    }
}
