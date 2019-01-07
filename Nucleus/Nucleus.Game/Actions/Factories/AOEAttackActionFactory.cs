using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;

namespace Nucleus.Game
{
    /// <summary>
    /// An action factory to produce AOEAttackActions
    /// </summary>
    [Serializable]
    public class AOEAttackActionFactory : DirectionalActionFactory
    {
        #region Property

        /// <summary>
        /// Private backing member variable for the Offsets property
        /// </summary>
        private Vector[] _Offsets = new Vector[] { new Vector(1, 0) };

        /// <summary>
        /// The array of cell offsets which is to be used to determine the area of effect 
        /// of the action.  These are the offset vectors of the cell locations to be 
        /// affected relative to the actor position, when the actor is orientated at
        /// 0 degrees (i.e. facing along the X-axis).  These offsets will be automatically 
        /// rotatetd in order to give the pattern of effect in other directions.
        /// </summary>
        public Vector[] Offsets
        {
            get { return _Offsets; }
            set { _Offsets = value; }
        }

        #endregion

        #region Constructors

        public AOEAttackActionFactory() { }

        /// <summary>
        /// Creates an AOEAttackAction factory to create an attack pattern
        /// with the specified list offsets.
        /// </summary>
        public AOEAttackActionFactory(params Vector[] offsets)
        {
            Offsets = offsets;
        }

        /// <summary>
        /// Creates an AOEAttackAction factory to create an attack pattern
        /// with the specified list of alternating X and Y offset components.
        /// </summary>
        /// <param name="offsetComponents"></param>
        public AOEAttackActionFactory(params double[] offsetComponents)
        {
            Offsets = Vector.Create2D(offsetComponents);
        }

        #endregion

        #region Methods

        protected override GameAction ActionForDirection(Vector position, Vector direction, MapCell triggerCell, TurnContext context)
        {
            var pattern = Offsets.Rotate(direction.Angle).Move(position);
            var cells = context.Stage.Map.CellsAt(pattern);
            return new AOEAttackAction(cells, triggerCell, direction);
        }

        #endregion
    }
}
