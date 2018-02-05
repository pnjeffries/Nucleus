using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A game entity family description
    /// </summary>
    public class GameEntityFamily : Family
    {
        public override Material GetPrimaryMaterial()
        {
            return null; //TODO?
        }
    }
}
