using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Stages
{
    /// <summary>
    /// A collection of stages
    /// </summary>
    /// <typeparam name="TStage"></typeparam>
    public class StageCollection<TStage> : UniquesCollection<TStage>
        where TStage : GameStage
    {
    }
}
