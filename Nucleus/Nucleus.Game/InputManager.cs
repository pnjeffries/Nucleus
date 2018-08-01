using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Manager class for user input
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// Called when the user presses a key or button
        /// </summary>
        /// <param name="input">The input function pressed</param>
        public void InputPress(InputFunction input)
        {
            GameEngine.Instance.State.InputPress(input, input.DirectionVector());
        }

        /// <summary>
        /// Called when the user releases a key or button
        /// </summary>
        /// <param name="input">The input function pressed</param>
        public void InputRelease(InputFunction input)
        {
            GameEngine.Instance.State.InputRelease(input, input.DirectionVector());
        }
    }
}
