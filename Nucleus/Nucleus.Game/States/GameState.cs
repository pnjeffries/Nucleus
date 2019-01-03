using Nucleus.Base;
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
    /// Abstract base class for states within a game (levels, menus etc.)
    /// </summary>
    [Serializable]
    public abstract class GameState : Unique
    {
        #region Properties

        /// <summary>
        /// The collection of currently active game elements
        /// </summary>
        public abstract ElementCollection Elements { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Perform state initialisation
        /// </summary>
        public virtual void StartUp()
        {

        }

        /// <summary>
        /// Called every frame update
        /// </summary>
        public virtual void Update(UpdateInfo info)
        {

        }
        
        /// <summary>
        /// Called when the user presses a key or button
        /// </summary>
        /// <param name="input">The input function pressed</param>
        /// <param name="direction">The direction associated with the input,
        /// if applicable.</param>
        public virtual void InputPress(InputFunction input, Vector direction)
        {

        }

        /// <summary>
        /// Called when the user releases a key or button
        /// </summary>
        /// <param name="input">The input function pressed</param>
        /// <param name="direction">The direction associated with the input,
        /// if applicable.</param>
        public virtual void InputRelease(InputFunction input, Vector direction)
        {

        }

        #endregion

    }
}
