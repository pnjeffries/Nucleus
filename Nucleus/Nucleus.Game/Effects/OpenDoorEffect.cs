using Nucleus.Game.Components;
using Nucleus.Logs;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Effects
{
    /// <summary>
    /// An effect which attempts to open a door
    /// </summary>
    [Serializable]
    public class OpenDoorEffect : BasicEffect
    {
        public override bool Apply(IActionLog log, EffectContext context)
        {
            Door door = context.Target?.GetData<Door>();

            if (door == null) return false;

            if (door.TryToOpen(context))
            {
                context.SFX.Trigger(SFXKeywords.Open, context.Target.GetNominalPosition());
            }

            return true;
        }
    }
}
