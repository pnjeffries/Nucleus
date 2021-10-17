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

        /// <summary>
        /// Private backing member variable for the SourceSFX property
        /// </summary>
        private string _SourceSFX = null;

        /// <summary>
        /// The keyword of the special effect to play at the source of the attack
        /// </summary>
        public string SourceSFX
        {
            get { return _SourceSFX; }
            set { _SourceSFX = value; }
        }

        private IList<IEffect> _Effects = null;

        /// <summary>
        /// The blueprint effects which will be applied by actions
        /// </summary>
        public IList<IEffect> Effects
        {
            get { return _Effects; }
            set { _Effects = value; }
        }

        private IList<IEffect> _SelfEffects = null;

        /// <summary>
        /// The blueprint effects which will be applied by actions
        /// </summary>
        public IList<IEffect> SelfEffects
        {
            get { return _SelfEffects; }
            set { _SelfEffects = value; }
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
            _Offsets = offsets;
        }

        /// <summary>
        /// Creates an AOEAttackAction factory to create an attack pattern
        /// with the specified list offsets.
        /// </summary>
        public AOEAttackActionFactory(string sourceSFX, params Vector[] offsets)
            : this(offsets)
        {
            _SourceSFX = sourceSFX;
        }

        public AOEAttackActionFactory(IList<IEffect> effects, string sourceSFX, params Vector[] offsets) : this(sourceSFX, offsets)
        {
            _Effects = effects;
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

        /// <summary>
        /// Creates an AOEAttackAction factory to create an attack pattern
        /// with the specified list of alternating X and Y offset components.
        /// </summary>
        /// <param name="offsetComponents"></param>
        public AOEAttackActionFactory(string sourceSFX, params double[] offsetComponents)
            : this(offsetComponents)
        {
            SourceSFX = sourceSFX;
        }


        /// <summary>
        /// Creates an AOEAttackAction factory to create an attack pattern
        /// with the specified list of alternating X and Y offset components.
        /// </summary>
        /// <param name="offsetComponents"></param>
        public AOEAttackActionFactory(IList<IEffect> effects, string actionName, string sourceSFX, params double[] offsetComponents)
            : this(sourceSFX, offsetComponents)
        {
            ActionName = actionName;
            _Effects = effects;
        }

        /// <summary>
        /// Creates an AOEAttackAction factory to create an attack pattern
        /// with the specified list of alternating X and Y offset components.
        /// </summary>
        /// <param name="offsetComponents"></param>
        public AOEAttackActionFactory(IList<IEffect> effects, IList<IEffect> selfEffects, string actionName, string sourceSFX, params double[] offsetComponents)
            : this(effects, actionName, sourceSFX, offsetComponents)
        {
            ActionName = actionName;
            _Effects = effects;
            _SelfEffects = selfEffects;
        }

        #endregion

        #region Methods

        protected override GameAction ActionForDirection(Vector position, Vector direction, MapCell triggerCell, TurnContext context)
        {
            var cells = TargetableCells(position, direction, context);
            return new AOEAttackAction(cells, triggerCell, direction, Effects, SelfEffects, SourceSFX);
        }

        public override IList<GameMapCell> TargetableCells(Vector position, Vector direction, TurnContext context)
        {
            var pattern = Offsets.Rotate(direction.Angle).Move(position);
            return context.Stage.Map.CellsAt(pattern);
        }

        #endregion
    }
}
