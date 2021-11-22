using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Game.Components;
using Nucleus.Logs;
using Nucleus.Modals;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Effects
{
    /// <summary>
    /// Effect which displays item information to the user via a modal window
    /// </summary>
    [Serializable]
    public class ShowItemInfoEffect : BasicEffect, IFastDuplicatable
    {
        private Element _Item = null;

        /// <summary>
        /// The item to show info for.  If left null the selected item from the
        /// actor's inventory will be displayed instead.
        /// </summary>
        public Element Item
        {
            get { return _Item; }
            set { _Item = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ShowItemInfoEffect() { }

        /// <summary>
        /// Item constructor
        /// </summary>
        /// <param name="item"></param>
        public ShowItemInfoEffect(Element item)
        {
            _Item = item;
        }

        /// <summary>
        /// Duplication constructor
        /// </summary>
        /// <param name="other"></param>
        public ShowItemInfoEffect(ShowItemInfoEffect other) : this(other.Item) { }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            // Create modal:
            if (!(context.State is ModalState mState)) return false;

            var item = Item;
            if (item == null) item = context.Actor?.GetData<Inventory>()?.Selected?.Item;
            if (item == null) return false;

            var script = GameEngine.Instance.LanguagePack;

            string title = item.Name.CapitaliseFirst();
            var resPickUp = item.GetData<ResourcePickUp>();
            if (resPickUp != null)
            {
                title += " (x" + resPickUp.Resource.Quantity + ")";
            }

            var modal = new ModalWindow(title);

            // Item keywords:
            string keywords = null;
            foreach (var component in item.Data)
            {
                var componentKey = component.GetType().Name + "_Keyword";
                if (script.HasEntryFor(componentKey))
                {
                    var keyword = script.GetText(componentKey);
                    if (keywords == null) keywords = keyword;
                    else keywords += ", " + keyword;
                }
            }
            if (keywords != null) modal.Contents.Add(new Keywords(keywords));

            // Consumable uses:
            if (item.HasData<ConsumableItem>())
            {
                var consumable = item.GetData<ConsumableItem>();
                var usesText = script.GetText("Consumable_Uses", context.RNG, consumable.Uses);
                if (usesText != null) modal.Contents.Add(new Paragraph(usesText));
                modal.Contents.Add(new Paragraph(" "));
            }

            // Item description:
            var descriptionKey = item.Name + "_Description";
            if (script.HasEntryFor(descriptionKey))
            {
                var description = script.GetText(descriptionKey);
                if (description != null) modal.Contents.Add(new Paragraph(description));
            }

            // Quick attack effects
            if (item.HasData<QuickAttack>())
            {
                var headerText = script.GetText("QuickAttack_Header");
                modal.Contents.Add(new Header(headerText));
                var qA = item.GetData<QuickAttack>();
                var effectsDescription = GenerateEffectsDescription(qA.Effects, script, context);
                if (effectsDescription != null) modal.Contents.Add(new Paragraph(effectsDescription));
            }

            // Use item effects
            if (item.HasData<ItemActions>())
            {
                var headerKey = item.Name + "_ActionHeader";
                if (script.HasEntryFor(headerKey))
                {
                    var headerText = script.GetText(headerKey);
                    modal.Contents.Add(new Header(headerText));
                }
                var aDKey = item.Name + "_ActionDescription";
                if (script.HasEntryFor(aDKey))
                {
                    var aDText = script.GetText(aDKey);
                    modal.Contents.Add(new Paragraph(aDText));
                }
                var iA = item.GetData<ItemActions>();
                if (iA.Prototype is WindUpAction wUA)
                {
                    // Dig down to get charged attack effects:
                    var aAE = wUA.SelfEffects.FirstOfType<AddAbilityEffect>();
                    if (aAE.Ability is DirectionalActionAbility dIUA && dIUA.ActionFactory is AOEAttackActionFactory aoeFactory)
                    {
                        var effectsDescription = GenerateEffectsDescription(aoeFactory.Effects, script, context);
                        if (effectsDescription != null) modal.Contents.Add(new Paragraph(effectsDescription));
                    }
                }
            }

            //TODO: Set up modal for item
            mState.OpenModal(modal);
            return true;
        }

        private string GenerateEffectsDescription(IList<IEffect> effects, LogScript script, EffectContext context)
        {
            string effectsDescription = null;
            foreach (var effect in effects)
            {
                var effectKey = effect.GetType().Name + "_Description";
                if (script.HasEntryFor(effectKey))
                {
                    var effectDescription = script.GetText(effectKey, context.RNG, effect);
                    if (effectsDescription == null) effectsDescription = effectDescription;
                    else effectsDescription += "  " + effectDescription;
                }
            }
            return effectsDescription;
        }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new ShowItemInfoEffect(this);
        }
    }
}
