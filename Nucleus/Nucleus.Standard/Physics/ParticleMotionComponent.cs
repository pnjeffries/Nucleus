using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Physics
{
    /// <summary>
    /// A Physics Engine component that deals with resolving the motion of particles
    /// </summary>
    [Serializable]
    public class ParticleMotionComponent : ParticleBaseComponent, IPhysicsEngineComponent
    {
        #region Properties

        /// <summary>
        /// Private backing field for Damping property
        /// </summary>
        private double _Damping = 0.1;

        /// <summary>
        /// The damping factor to be applied to particle
        /// motion.  This is the proportional reduction
        /// in velocity of each particle each cycle.
        /// </summary>
        public double Damping
        {
            get { return _Damping; }
            set { _Damping = value; }
        }

        private double _SpeedLimit = 0.1;

        /// <summary>
        /// The maximum velocity of a particle
        /// </summary>
        public double SpeedLimit
        {
            get { return _SpeedLimit; }
            set { _SpeedLimit = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a ParticleMotionComponent, generating particles from the
        /// specified nodes.
        /// </summary>
        /// <param name="nodes"></param>
        public ParticleMotionComponent(NodeCollection nodes) : base(nodes) { }

        #endregion

        #region Methods

        public bool Cycle(double dt, PhysicsEngine engine)
        {
            double speed = 0;
            foreach (var particle in Particles)
            {
                double vMag = particle.Velocity.MagnitudeSquared();
                if (vMag > speed)
                    speed = vMag;
            }
            if (speed > SpeedLimit.Squared())
            {
                speed = speed.Root();
                double factor = SpeedLimit/speed;
                foreach (var particle in Particles)
                {
                    particle.Velocity *= factor;
                }
            }

            double vFactor = 1.0 - Damping;
            foreach (var particle in Particles)
            {
                particle.Move(dt);
                particle.Damp(vFactor);
            }
            return true;
        }

        public bool Start(PhysicsEngine engine)
        {
            foreach (var particle in Particles) particle.Reset();
            return true;
        }

        #endregion
    }
}
