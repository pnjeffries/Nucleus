using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A class to hold data about a stage or level in a game.
    /// </summary>
    [Serializable]
    public class GameStage : Named
    {
        private StageStyle _Style = null;

        /// <summary>
        /// The style data, if any, associated with this stage
        /// </summary>
        public StageStyle Style
        {
            get { return _Style; }
            set { ChangeProperty(ref _Style, value); }
        }
    }
}
