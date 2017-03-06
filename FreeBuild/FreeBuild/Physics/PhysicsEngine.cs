using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Physics
{
    /// <summary>
    /// A customisable physics engine that can be used to manage
    /// iterative simulations of physical and pseudo-physical forces
    /// </summary>
    public class PhysicsEngine : MessageRaiser
    {
        #region Properties

        private PhysicsEngineComponentCollection _Components = new PhysicsEngineComponentCollection();

        /// <summary>
        /// The components that model the physical processes to be simulated by this
        /// engine.  These will be processed in order each cycle.
        /// </summary>
        public PhysicsEngineComponentCollection Components
        {
            get { return _Components; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new blank physics engine
        /// </summary>
        public PhysicsEngine() { }

        #endregion

        #region Methods

        /// <summary>
        /// Start the simulation running
        /// </summary>
        public void Start()
        {
            foreach (IPhysicsEngineComponent component in Components)
            {
                component.Startup(this);
            }
        }

        /// <summary>
        /// Advance the simulation by a certain timestep
        /// </summary>
        /// <param name="dt"></param>
        public void Cycle(double dt)
        {
            foreach (IPhysicsEngineComponent component in Components)
            {
                component.Cycle(dt, this);
            }
        }

        #endregion
    }
}
