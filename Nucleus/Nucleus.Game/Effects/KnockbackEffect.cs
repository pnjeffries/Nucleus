using Nucleus.Base;
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
    public class KnockbackEffect : BasicEffect, IDirectionalEffect, IFastDuplicatable
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

        private IEffect _ImpactEffect = new DamageEffect(1);

        /// <summary>
        /// The effect (if any) which will be applied should the knockback be stopped by the target
        /// hitting a blocking object.  This effect will be applied to both the target and the object
        /// they have collided with.
        /// </summary>
        public IEffect ImpactEffect
        {
            get { return _ImpactEffect; }
            set { _ImpactEffect = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Directionless constructor.
        /// Should be used for templates only.
        /// </summary>
        /// <param name="power"></param>
        public KnockbackEffect(double power = 1) : this (Vector.UnitX, power)
        {

        }

        /// <summary>
        /// Directional constructor
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="power"></param>
        public KnockbackEffect(Vector direction, double power = 1)
        {
            Direction = direction;
            Power = power;
        }

        /// <summary>
        /// Duplication constructor
        /// </summary>
        /// <param name="other"></param>
        public KnockbackEffect(KnockbackEffect other)
        {
            Direction = other.Direction;
            Power = other.Power;
            ImpactEffect = other.ImpactEffect; //Duplicate?
        }

        #endregion

        #region Methods

        public override bool Apply(IActionLog log, EffectContext context)
        {
            int n = (int)Power;

            if (context.Critical) n += 1; // Critical hit

            n = context.Actor.Data.ApplyModifiers<int, IKnockbackModifier>(n, (value, iModifier) => iModifier.ModifyKnockback(value, log, context));

            Element mover = context.Target;
            if (mover.IsDeleted || (mover.GetData<Inertia>()?.Fixed ?? false))
                return false; // Target is immovable or deleted

            if (mover != null)
            {
                var weight = mover.GetData<ElementWeight>();
                if (weight != null)
                {
                    // Modify according to mass
                    n += weight.KnockbackModifier;
                }
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
                        if (newCell != null && (mover.GetData<MapCellCollider>()?.CanEnter(newCell) ?? false))
                        {
                            newCell.PlaceInCell(mover);
                            moved = true;
                        }
                        else
                        {
                            context.SFX.Trigger(SFXKeywords.Knock, mD.Position + Direction * 0.5, Direction);
                            if (ImpactEffect != null && newCell != null)
                            {
                                var blocker = mover.GetData<MapCellCollider>()?.Blocker(newCell);

                                if (blocker != null)
                                {
                                    WriteLog(log, context, blocker);

                                    // Apply to target:
                                    ImpactEffect.Apply(log, context);
                                    // Apply to blocker:
                                    ImpactEffect.Apply(log, context.CloneWithTarget(blocker));
                                }
                            }
                            i = n; //Finish
                        }
                    }
                }

                if (moved)
                {
                    context.ElementMovedOutOfTurn(mover);
                }

                if (moved && context.State is RLState)
                {
                    var rlState = (RLState)context.State;
                    rlState.DelayAITurn(); // Allow for a slight pause to register the movement
                }
                return moved;

            }
            return false;
        }

        protected virtual void WriteLog(IActionLog log, EffectContext context, Element blocker)
        {
            if (log == null) return;
            string key = "Knockback_Impact";
            if (log.HasScriptFor(key))
            {
                if (context.IsPlayerAwareOf(blocker) || context.IsPlayerAwareOf(context.Target))
                {
                    log.WriteScripted(context, key, context.Actor, context.Target, blocker);
                }
            }
        }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new KnockbackEffect(this);
        }

        #endregion

    }
}
