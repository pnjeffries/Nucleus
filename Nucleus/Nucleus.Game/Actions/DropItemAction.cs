using Nucleus.Game.Effects;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Drop an item
    /// </summary>
    [Serializable]
    public class DropItemAction : ElementTargetingAction
    {
        public DropItemAction() : base() 
        {
            Effects.Add(new DropItemEffect());
        }

        public DropItemAction(Element item) : base("Drop", item)
        {
            Effects.Add(new DropItemEffect());
        }
    }
}
