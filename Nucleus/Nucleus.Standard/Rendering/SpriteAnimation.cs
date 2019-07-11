using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// Datastructure to hold the information of an animation in a sprite
    /// </summary>
    [Serializable]
    public class SpriteAnimation
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Directions property
        /// </summary>
        private SortedList<Angle, SpriteAnimationDirection> _Directions = new SortedList<Angle, SpriteAnimationDirection>();

        /// <summary>
        /// The animation directions
        /// </summary>
        public SortedList<Angle, SpriteAnimationDirection> Directions
        {
            get { return _Directions; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new SpriteAnimation
        /// </summary>
        public SpriteAnimation() { }

        #endregion

        #region Methods

        /// <summary>
        /// Get the 
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public SpriteAnimationDirection GetDirection(Angle angle)
        {   
            if (_Directions.ContainsKey(angle))
            {
                return _Directions[angle];
            }
            else
            {
                Angle mindA = Angle.Complete;
                SpriteAnimationDirection closest = null;

                foreach (var entry in Directions)
                {
                    Angle dA = (angle - entry.Key).Normalize();
                    if (dA.Abs() < mindA.Abs())
                    {
                        closest = entry.Value;
                        mindA = dA;
                    }
                }

                return closest;
            }

        }

        #endregion
    }
}
