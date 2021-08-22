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
    /// An effect which will move an element to a specific cell
    /// </summary>
    public class MoveCellEffect : BasicEffect, IFastDuplicatable
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the MoveTo property
        /// </summary>
        private int _MoveTo;

        /// <summary>
        /// The index of the mapcell to move to
        /// </summary>
        public int MoveTo
        {
            get { return _MoveTo; }
            set { _MoveTo = value; }
        }

        #endregion

        #region Constructors

        public MoveCellEffect(int moveTo)
        {
            _MoveTo = moveTo;
        }

        public MoveCellEffect(MoveCellEffect other) : this(other.MoveTo) { }

        #endregion

        #region Methods

        public override bool Apply(IActionLog log, EffectContext context)
        {
            Element mover = context.Target;
            if (mover != null)
            {
                //Move element:
                MapData mD = mover.GetData<MapData>();
                if (mD != null && mD.MapCell != null)
                {
                    context.SFX.Trigger(SFXKeywords.Dust, mD.Position);
                    MapCell newCell = context.Map[MoveTo];
                    if (newCell != null && (mover.GetData<MapCellCollider>()?.CanEnter(newCell) ?? true))
                    {
                        newCell.PlaceInCell(mover);
                        if (context.IsPlayerControlled(mover))
                        {
                            WriteFeetItems(log, context, newCell);
                        }
                        return true;
                    }
                }

            }
            return false;
        }

        private void WriteFeetItems(IActionLog log, EffectContext context, MapCell cell)
        {
            var items = cell.Contents.AllWithDataComponent<PickUp>();
            if (items.Count > 0)
            {
                log.WriteLine();
                log.WriteScripted(context, nameof(MoveCellEffect) + "_Item", context.Target, items.First());
                // TODO: Multiple items
            }
        }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new MoveCellEffect(this);
        }

        #endregion
    }
}
