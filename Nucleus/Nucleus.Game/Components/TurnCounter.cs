using Nucleus.Extensions;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Element data component which indicates that an element is able to take
    /// a turn, either directed by the player or by AI, and keeps track of the
    /// order in which these turns are taken.
    /// </summary>
    [Serializable]
    public class TurnCounter : IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the CountDown property
        /// </summary>
        private int _CountDown = 0;

        /// <summary>
        /// The count down until the element can take its next turn
        /// </summary>
        public int CountDown
        {
            get { return _CountDown; }
            set { _CountDown = value; }
        }

        /// <summary>
        /// Private backer for speed
        /// </summary>
        private double _Speed = 1;

        /// <summary>
        /// The relative base speed of action of the element
        /// </summary>
        public double Speed
        {
            get { return _Speed; }
            set { _Speed = value; }
        }

        #endregion

        #region Constructors

        public TurnCounter() { }

        public TurnCounter(int countDown)
        {
            _CountDown = countDown;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reset the turn countdown to the specified value, adjusted for
        /// the given element's speed.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public void ResetCountdown(Element element, int value)
        {
            var speed = AdjustedSpeed(element);
            if (speed == 0) CountDown = int.MaxValue; //No int infinity!
            else CountDown = (int)(value / speed);
        }

        private double AdjustedSpeed(Element element)
        {
            double result = Speed;
            foreach (var data in element.Data)
            {
                if (data is ISpeedModifier speedMod)
                {
                    result = speedMod.ModifySpeed(result);
                }
            }
            return result;
        }

        #endregion
    }
}
