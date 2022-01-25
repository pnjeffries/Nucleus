using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Ability which allows an element to pick up
    /// </summary>
    [Serializable]
    public class PickUpAbility : Ability
    {
        #region Methods

        public override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            MapData mD = context.Element?.GetData<MapData>();
            if (mD != null && mD.MapCell != null)
            {
                // Are there any pickable-uppable items in this cell?
                Element item = mD.MapCell.Contents.LastWithDataComponent<PickUp>(context.Element);
                if (item != null)
                {
                    addTo.Actions.Add(new PickUpAction(item));
                }
                // TODO: Deal with multiple possible items to pick up?
            }
        }

        #endregion
    }
}
