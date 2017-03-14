using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Physics
{
    /// <summary>
    /// An interface for components of a physics engine that are used to
    /// perform part of the process being simulated
    /// </summary>
    public interface IPhysicsEngineComponent : IUnique
    {
        /// <summary>
        /// Perform any initialisation steps required before this component
        /// can be processed.
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        bool Start(PhysicsEngine engine);

        /// <summary>
        /// Perform the simulation steps governed by this component
        /// </summary>
        /// <param name="dt">The time-step</param>
        /// <param name="engine">The owning physics engine</param>
        /// <returns></returns>
        bool Cycle(double dt, PhysicsEngine engine);
    }
}
