using Nucleus.Base;
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
    public class ParticleMotionComponent : Unique, IPhysicsEngineComponent
    {
        #region Properties

        private IList<Particle> _Particles = new List<Particle>();

        /// <summary>
        /// The set of particles 
        /// </summary>
        public IList<Particle> Particles
        {
            get { return _Particles; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a ParticleMotionComponent, generating particles from the
        /// specified nodes.
        /// </summary>
        /// <param name="nodes"></param>
        public ParticleMotionComponent(NodeCollection nodes)
        {
            foreach (Node node in nodes)
            {
                if (!node.HasData<Particle>())
                {
                    node.Data.Add(new Particle(node));
                }
                Particles.Add(node.GetData<Particle>());
            }
        }

        #endregion

        #region Methods

        public bool Cycle(double dt, PhysicsEngine engine)
        {
            foreach (var particle in Particles)
                particle.Move(dt);
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
