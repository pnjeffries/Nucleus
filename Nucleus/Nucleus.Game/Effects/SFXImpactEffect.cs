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
    /// A SFX effect that produces an effect at a point on the target
    /// </summary>
    [Serializable]
    public class SFXImpactEffect : BasicEffect, IFastDuplicatable
    {
        public override bool Apply(IActionLog log, EffectContext context)
        {
            Element target = context.Target;

            if (target != null)
            {
                MapData mD = target.GetData<MapData>();
                if (mD != null && mD.MapCell != null)
                {
                    Vector sfxPos = mD.Position;
                    Vector actorPos = context.Actor?.GetData<MapData>()?.Position ?? Vector.Unset;
                    if (actorPos.IsValid()) sfxPos = sfxPos.MoveTowards(actorPos, 0.3);
                    var sfxKey = SFXKeywords.Bash;
                    if (context.Critical) sfxKey = SFXKeywords.CritBash;
                    context.SFX.Trigger(sfxKey, sfxPos);

                    return true;
                }
            }
            return false;
        }

        public IFastDuplicatable FastDuplicate_Internal()
        {
            return new SFXImpactEffect();
        }
    }
}
