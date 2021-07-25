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
        /// The relative speed of action of the element
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

        public void ResetCountdown(int value)
        {
            CountDown = (int)(value / Speed);
        }

        #endregion
    }
}
