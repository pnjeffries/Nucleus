using Nucleus.Game.Components;
using Nucleus.Game.Effects;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Actions
{
    /// <summary>
    /// Use an item
    /// </summary>
    [Serializable]
    public class UseItemAction : ElementTargetingAction
    {
        public UseItemAction():base() { }

        /// <summary>
        /// Constructor for using an item.
        /// If the item is consumable, will automatically add a consume effect.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <param name="selfEffects"></param>
        public UseItemAction(string name, Element item, params IEffect[] selfEffects): base(name, item)
        {
            foreach (var effect in selfEffects) SelfEffects.Add(effect);
            if (item.HasData<ConsumableItem>()) Effects.Add(new ConsumeEffect());
        }

        /// <summary>
        /// Constructor for using an item.
        /// If the item is consumable, will automatically add a consume effect.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <param name="selfEffects"></param>
        public UseItemAction(string name, Element item, IList<IEffect> effects,  IList<IEffect> selfEffects) : base(name, item)
        {
            foreach (var effect in selfEffects) SelfEffects.Add(effect);
            foreach (var effect in effects) Effects.Add(effect);
        }
    }
}
