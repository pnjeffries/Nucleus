using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Interface for components, status effects, items etc.
    /// which modify an actor's speed
    /// </summary>
    public interface ISpeedModifier
    {
        /// <summary>
        /// Adjust the specified speed value
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        double ModifySpeed(double speed);
    }
}
