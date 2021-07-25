using Nucleus.Base;
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
        /// <summary>
        /// Attempt to open this door
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool TryToOpen(EffectContext context)
        {
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
