using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Logs;
using Nucleus.Modals;
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
        public ShowItemInfoEffect() { }

        /// <summary>
        /// Duplication constructor
        /// </summary>
        /// <param name="other"></param>
        public ShowItemInfoEffect(ShowItemInfoEffect other) { }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            // Create modal:
            if (!(context.State is ModalState mState)) return false;

            var item = context.Actor?.GetData<Inventory>()?.Selected?.Item;
            if (item == null) return false;

            var script = GameEngine.Instance.LanguagePack;

            var modal = new ModalWindow(item.Name.CapitaliseFirst());

            // Item keywords:
            string keywords = null;
            foreach (var component in item.Data)
            {
                var componentKey = component.GetType().Name + "_Keyword";
                if (script.HasEntryFor(componentKey))
                {
                    var keyword = script.GetText(componentKey);
                    if (keywords == null) keywords = keyword;
                    else keywords += " " + keyword;
                }
            }
            if (keywords != null) modal.Contents.Add(new Keywords(keywords));

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
                    if (aAE.Ability is DirectionalItemUseAbility dIUA && dIUA.ActionFactory is AOEAttackActionFactory aoeFactory)
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
