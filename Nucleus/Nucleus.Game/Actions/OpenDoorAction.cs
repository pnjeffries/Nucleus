using Nucleus.Game.Effects;
using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Actions
{
    /// <summary>
    /// An action to open a door
    /// </summary>
    [Serializable]
    public class OpenDoorAction : ElementTargetingAction
    {
        public OpenDoorAction(Element target)
        {
            Trigger = new ActionCellInputTrigger(target.GetData<MapData>().MapCell.Index, InputFunction.Move);
            //Trigger = new ActionInputTrigger(InputFunction.Interact);
            Target = target;
            // Effect
            Effects.Add(new OpenDoorEffect());
        }
    }
}
