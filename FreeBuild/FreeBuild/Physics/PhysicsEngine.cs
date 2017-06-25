using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Physics
{
    /// <summary>
    /// A customisable physics engine that can be used to manage
    /// iterative simulations of physical and pseudo-physical forces
    /// </summary>
    public class PhysicsEngine : MessageRaiser
    {
        #region Properties

        /// <summary>
        /// Private backing field for Components property
        /// </summary>
        private PhysicsEngineComponentCollection _Components = new PhysicsEngineComponentCollection();

        /// <summary>
        /// The components that model the physical processes to be simulated by this
        /// engine.  These will be processed in order each cycle.
        /// </summary>
        public PhysicsEngineComponentCollection Components
        {
            get { return _Components; }
        }

        /// <summary>
        /// Private backing field for MaximumTimeStep property
        /// </summary>
        private double _MaximumTimeStep = 0;

        /// <summary>
        /// The maximum allowable time-step per cycle.  If the time step entered in a cycle is
        /// greater than this then it will be split into several sub-cycles of no more than this
        /// limit.  If set to 0 no limit will be applied.
        /// </summary>
        public double MaximumTimeStep
        {
            get { return _MaximumTimeStep; }
            set { ChangeProperty(ref _MaximumTimeStep, value, "MaximumTimeStep"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new blank physics engine
        /// </summary>
        public PhysicsEngine() { }

        /// <summary>
        /// Initialise a new physics engine to run the specified simulation components
        /// </summary>
        /// <param name="components"></param>
        public PhysicsEngine(PhysicsEngineComponentCollection components) : this()
        {
            Components.AddRange(components);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Start the simulation running
        /// </summary>
        public void Start()
        {
            foreach (IPhysicsEngineComponent component in Components)
            {
                component.Start(this);
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
