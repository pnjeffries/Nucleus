using Nucleus.Base;
using Nucleus.Game.Components;
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
    /// Data component to store element field of view within a Cell Map
    /// </summary>
    public class MapAwareness : Unique, IElementDataComponent, IEndOfTurn, IOutOfTurnMove
    {
        #region Constants

        /// <summary>
        /// The value used to indicate that a cell is directly visible
        /// </summary>
        public const int Visible = 10;

        /// <summary>
        /// The value used to indicate that a cell is remembered
        /// </summary>
        public const int Remembered = 3;

        #endregion

        #region Properties

        private ICellMap<int> _FieldOfView;

        /// <summary>
        /// The element field of view map
        /// </summary>
        public ICellMap<int> FieldOfView
        {
            get { return _FieldOfView; }
            set
            {
                ChangeProperty(ref _FieldOfView, value, "FieldOfView", true);
            }
        }

        private double _VisualRange = 10;

        /// <summary>
        /// The visual range of the element
        /// </summary>
        public double VisualRange
        {
            get { return _VisualRange; }
            set
            {
                ChangeProperty(ref _VisualRange, value);
            }
        }

        #endregion

        #region Constructor

        public MapAwareness() { }

        public MapAwareness(double visualRange)
        {
            _VisualRange = visualRange;
        }

        #endregion

        #region Methods

        private void UpdateFOV(TurnContext context)
        {
            if (context.Element != null)
            {
                var map = context?.Stage?.Map;
                var fov = map.SpawnNewGrid<int>();
                if (FieldOfView != null)
                {
                    for (int i = 0; i < FieldOfView.CellCount; i++)
                    {
                        if (FieldOfView[i] > 0) fov[i] = Remembered;
                    }
                }
                var mD = context.Element.GetData<MapData>();
                if (mD != null)
                {
                    map.FieldOfView<int, MapCell>(mD.Position, VisualRange,
                        x => IsTransparent(x, context.Element), fov, Visible);
                    FieldOfView = fov;
                }
            }
        }

        public void EndOfTurn(TurnContext context)
        {
            UpdateFOV(context);
        }

        private bool IsTransparent(MapCell cell, Element element)
        {
            foreach (var el in cell.Contents)
            {
                if (el.HasData<VisionBlocker>())
                {
                    var vB = el.GetData<VisionBlocker>();
                    return vB.IsTransparent(element);
                }
            }
            return true;
        }

        /// <summary>
        /// Get the awareness of the specified cell
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public int AwarenessOfCell(int cellIndex)
        {
            return FieldOfView?[cellIndex] ?? 0;
        }

        public void OutOfTurnMove(TurnContext context)
        {
            UpdateFOV(context);
        }

        /// <summary>
        /// Called when the stage is changed
        /// </summary>
        /// <param name="stage"></param>
        public void StageChanged(GameStage stage)
        {
            FieldOfView = null;
            //TODO: Store previous fovs in case we allow returning to previous levels?
        }

        #endregion
    }
}
