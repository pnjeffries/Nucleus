using Nucleus.Geometry;
using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A base class for actions which target a particular cell
    /// </summary>
    [Serializable]
    public abstract class CellTargetingAction : TargetedAction<MapCell>
    {
        #region Constructor

        public CellTargetingAction()
        {
        }

        public CellTargetingAction(string name) : base(name)
        {
        }

        public CellTargetingAction(string name, IEffect effect) : base(name, effect)
        {
        }

        public CellTargetingAction(string name, params IEffect[] effects) : base(name, effects)
        {
        }

        public CellTargetingAction(string name, MapCell target, params IEffect[] effects) : base(name, target, effects)
        {
        }

        #endregion

        #region Methods

        protected override void ApplyEffects(IActionLog log, EffectContext context, bool allowCrit = true)
        {
            if (Target != null)
            {
                var elements = Target.Contents.ToList();

                foreach (var element in elements)
                {
                    if (CanTarget(element))
                    {
                        context.Target = element;
                        base.ApplyEffects(log, context, allowCrit);
                    }
                }
            }
        }

        #endregion
    }
}
