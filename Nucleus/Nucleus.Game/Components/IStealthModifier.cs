using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Components
{
    /// <summary>
    /// Interface for components and items which modify an element's stealth rating
    /// </summary>
    public interface IStealthModifier
    {
        /// <summary>
        /// Apply modifiers to the stealth rating of a given element
        /// </summary>
        /// <param name="stealth"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        double ModifyStealth(double stealth, GameElement element, TurnContext context);
    }
}
