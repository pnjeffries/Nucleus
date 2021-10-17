using Nucleus.Base;
using Nucleus.Logs;
using Nucleus.Model;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Components
{
    /// <summary>
    /// A data component which marks an element as a door which may be opened or closed
    /// </summary>
    [Serializable]
    public class Door : Unique, IElementDataComponent
    {
        private bool _Locked = false;

        /// <summary>
        /// Is this door currently locked?
        /// </summary>
        public bool Locked
        {
            get { return _Locked; }
            set { ChangeProperty(ref _Locked, value); }
        }

        private string _KeyCode;

        /// <summary>
        /// The keycode of this door.  Keys with a matching keycode
        /// may unlock this door.
        /// </summary>
        public string KeyCode
        {
            get { return _KeyCode; }
            set { ChangeProperty(ref _KeyCode, value); }
        }

        private bool TryUnlock(IActionLog log, EffectContext context)
        {
            Element actor = context.Actor;
            var inventory = actor.GetData<Inventory>();
            var keyItem = inventory.GetKey(KeyCode);
            if (keyItem == null) return false;

            Locked = false;
            if (context.IsPlayerAwareOf(context.Actor) || context.IsPlayerAwareOf(context.Target))
            {
                log.WriteScripted("OpenDoorEffect_Unlocked", context.Actor, context.Target, keyItem);
            }
            return true;
        }

        /// <summary>
        /// Attempt to open this door
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool TryToOpen(IActionLog log, EffectContext context)
        {
            if (Locked)
            {
                if (!TryUnlock(log, context))
                {
                    if (context.IsPlayerAwareOf(context.Actor))
                    {
                        log.WriteScripted("OpenDoorEffect_Locked", context.Actor, context.Target);
                    }
                    return false;
                }
            }

            var owner = context.Target;
            if (owner == null) return false;

            var collider = owner.GetData<MapCellCollider>();
            if (collider == null) return false;

            collider.Solid = false;

            var vision = context.Target.GetData<VisionBlocker>();
            if (vision != null) vision.Opaque = false;

            var ascii = context.Target.GetData<ASCIIStyle>();
            if (ascii != null) ascii.Symbol = "◘";

            return true;
        }
    }
}
