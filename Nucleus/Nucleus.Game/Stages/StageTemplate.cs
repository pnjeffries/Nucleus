using Nucleus.Base;
using Nucleus.Game.Artitecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Stages
{
    /// <summary>
    /// A template used to generate a stage
    /// </summary>
    [Serializable]
    public class StageTemplate : Named
    {
        /// <summary>
        /// The table of creatures which may be present in this stage
        /// </summary>
        public WeightedTable<Func<GameElement>> CreatureTable { get; set; }
    }
}
