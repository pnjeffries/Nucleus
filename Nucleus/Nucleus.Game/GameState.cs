using Nucleus.Base;
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

        #endregion

    }
}
