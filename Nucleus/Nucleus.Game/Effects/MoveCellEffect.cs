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
                var oldCell = mD.MapCell;
                if (mD != null && mD.MapCell != null)
                {
                    context.SFX.Trigger(SFXKeywords.Dust, mD.Position);
                    MapCell newCell = context.Map[MoveTo];
                    if (newCell != null && (mover.GetData<MapCellCollider>()?.CanEnter(newCell) ?? true))
                    {
                        newCell.PlaceInCell(mover);
                        if (context.IsPlayerControlled(mover))
                        {
                            if (!WriteFeetItems(log, context, newCell))
                            {
                                if (oldCell != null && oldCell is GameMapCell oC && newCell is GameMapCell nC
                                    && oC.Room != nC.Room && context.RNG.NextDouble() < 0.5)
                                {
                                    WriteRoomAtmosphere(log, context, nC);
                                }
                            }
                        }
                        return true;
                    }
                }

            }
            return false;
        }

        private bool WriteFeetItems(IActionLog log, EffectContext context, MapCell cell)
        {
            var items = cell.Contents.AllWithDataComponent<PickUp>();
            if (items.Count > 0)
            {
                log.WriteLine();
                log.WriteScripted(context, nameof(MoveCellEffect) + "_Item", context.Target, items.Last());
                // TODO: Multiple items
                return true;
            }
            return false;
        }

        private bool WriteRoomAtmosphere(IActionLog log, EffectContext context, GameMapCell cell)
        {
            string key = cell.Room?.Template?.Name + "_Atmosphere";
            if (log.HasScriptFor(key))
            {
                log.WriteLine();
                log.WriteScripted(context, key, context.Target, cell.Room);
                return true;
            }
            return false;
        }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new MoveCellEffect(this);
        }

        #endregion
    }
}
