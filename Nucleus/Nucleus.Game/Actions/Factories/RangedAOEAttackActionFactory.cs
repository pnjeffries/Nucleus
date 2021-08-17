using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Action factory for ranged area-of-effect attack actions
    /// </summary>
    [Serializable]
    public class RangedAOEAttackActionFactory : AOEAttackActionFactory
    {
        #region Properties

        private double _Range = 10;

        /// <summary>
        /// The maximum range of the attack
        /// </summary>
        public double Range
        {
            get { return _Range; }
            set { _Range = value; }
        }

        /// <summary>
        /// The collider used to check for projectile collision
        /// </summary>
        private MapCellCollider _ProjectileCollider = new MapCellCollider(true);

        #endregion

        #region Constructors

        public RangedAOEAttackActionFactory(IList<IEffect> effects, string actionName, string sourceSFX, params double[] offsetComponents) : base(effects, actionName, sourceSFX, offsetComponents)
        {

        }

        public RangedAOEAttackActionFactory(double range, IList<IEffect> effects, string actionName, string sourceSFX, params double[] offsetComponents) : base(effects, actionName, sourceSFX, offsetComponents)
        {
            _Range = range;
        }

        public RangedAOEAttackActionFactory(double range, IList<IEffect> effects, IList<IEffect> selfEffects, string actionName, string sourceSFX, params double[] offsetComponents) : base(effects, selfEffects, actionName, sourceSFX, offsetComponents)
        {
            _Range = range;
        }


        #endregion

        #region Methods

        private bool ProjectileHit(MapCell cell)
        {
            return !_ProjectileCollider.CanEnter(cell);
        }

        public override IList<MapCell> TargetableCells(Vector position, Vector direction, TurnContext context)
        {
            var cell = context.Stage.Map.CellAt(position)?.FirstCellInDirectionWhere(direction, ProjectileHit, (int)_Range, true);
            if (cell != null) position = cell.Position; //Update position to impact point
            return base.TargetableCells(position, direction, context);
        }

        #endregion

    }
}
