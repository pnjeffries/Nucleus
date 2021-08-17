using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Base class for objects which can be used to generate actions dynamically
    /// </summary>
    [Serializable]
    public abstract class ActionFactory
    {
        #region Properties

        private string _ActionName = null;

        /// <summary>
        /// The name to be assigned to the generated actions
        /// </summary>
        public string ActionName
        {
            get { return _ActionName; }
            set { _ActionName = value; }
        }

        #endregion

        /// <summary>
        /// Generate actions given the specified context and add them to the available actions
        /// </summary>
        /// <param name="context"></param>
        /// <param name="addTo"></param>
        public abstract void GenerateActions(TurnContext context, AvailableActions addTo);

        /// <summary>
        /// Generate a list of the cells (if any) which may be targetted by the actions this
        /// factory can produce from the specified position and in the specified direction.
        /// </summary>
        /// <returns></returns>
        public abstract IList<MapCell> TargetableCells(Vector position, Vector direction, TurnContext context);

        /// <summary>
        /// Generate a list of the cells (if any) which may be targetted by the actions this
        /// factory can produce.
        /// </summary>
        /// <returns></returns>
        public abstract IList<MapCell> TargetableCells(TurnContext context);
    }
}
