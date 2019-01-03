using Nucleus.Geometry;
using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An effect which moves the target in a particular direction (typically
    /// involuntarily).  Resistance to knockback is determined by the Inertia component.
    /// </summary>
    [Serializable]
    public class KnockbackEffect : BasicEffect
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Direction property
        /// </summary>
        private Vector _Direction = Vector.Zero;

        /// <summary>
        /// The direction in which the element is to be knocked
        /// </summary>
        public Vector Direction
        {
            get { return _Direction; }
            set { _Direction = value; }
        }

        /// <summary>
        /// Private backing member variable for the Power property
        /// </summary>
        private double _Power = 1;

        /// <summary>
        /// The power of the knockback effect (translates to the number of spaces an average-sized target will move)
        /// </summary>
        public double Power
        {
            get { return _Power; }
            set { _Power = value; }
        }

        #endregion

        #region Constructor

        public KnockbackEffect(Vector direction, double power = 2)
        {
            Direction = direction;
            Power = power;
        }

        #endregion

        #region Methods

        public override bool Apply(IActionLog log, EffectContext context)
        {
            int n = (int)Power; //TODO: Account for target mass?

            Element mover = context.Target;
            if (mover.IsDeleted || (mover.GetData<Inertia>()?.Fixed ?? false))
                return false; // Target is immovable or deleted

            if (mover != null)
            {
                bool moved = false;
                for (int i = 0; i < n; i++)
                {
                    //Move element:
                    MapData mD = mover.GetData<MapData>();
                    if (mD != null && mD.MapCell != null)
                    {
                        //SFX - Temp?  Move elsewhere?
                        /*Vector sfxPos = mD.Position;
                        Vector actorPos = context.Actor?.GetData<MapData>()?.Position ?? Vector.Unset;
                        if (actorPos.IsValid()) sfxPos = sfxPos.MoveTowards(actorPos, 0.3);
                        context.SFX.Trigger(SFXKeywords.Bash, sfxPos);*/

                        // Dust trail:
                        context.SFX.Trigger(SFXKeywords.Dust, mD.Position);

                        MapCell newCell = mD.MapCell.AdjacentCellInDirection(Direction);
                        if (newCell != null && (mover.GetData<MapCellCollider>()?.CanEnter(newCell) ?? true))
                        {
                            newCell.PlaceInCell(mover);
                            moved = true;
                        }
                    }
                }
                return moved;

            }
            return false;
        }

        #endregion

    }
}
