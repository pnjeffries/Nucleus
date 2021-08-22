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
    /// Effect which changes the actor's orientation
    /// </summary>
    [Serializable]
    public class ActorOrientationEffect : BasicEffect, IFastDuplicatable
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the OrientTo property
        /// </summary>
        private Angle _OrientTo;

        /// <summary>
        /// The orientation angle (anticlockwise from +X) to set
        /// </summary>
        public Angle OrientTo
        {
            get { return _OrientTo; }
            set { _OrientTo = value; }
        }

        #endregion

        #region Constructors

        public ActorOrientationEffect(Angle orientTo)
        {
            OrientTo = orientTo;
        }

        /// <summary>
        /// Creates an orientation effect such that the specified element would
        /// face towards the specified cell
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="cell"></param>
        public ActorOrientationEffect(Element actor, MapCell cell)
        {
            OrientTo = actor?.GetData<MapData>()?.Position.AngleTo(cell.Position) ?? 0;
        }

        /// <summary>
        /// Creates an orientation effect to face in the specified direction
        /// </summary>
        /// <param name="direction"></param>
        public ActorOrientationEffect(Vector direction)
        {
            OrientTo = direction.Angle;
        }

        /// <summary>
        /// Duplication constructor
        /// </summary>
        /// <param name="other"></param>
        public ActorOrientationEffect(ActorOrientationEffect other)
        {
            OrientTo = other.OrientTo;
        }

        #endregion

        #region Methods

        public override bool Apply(IActionLog log, EffectContext context)
        {
            context.Actor.Orientation = OrientTo;
            //TODO: Make optional whether is applied to the actor or target?
            return true;
        }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new ActorOrientationEffect(this);
        }

        #endregion
    }
}
