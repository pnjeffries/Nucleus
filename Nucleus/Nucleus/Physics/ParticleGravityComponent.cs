using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Physics
{
    public class ParticleGravityComponent : Unique, IPhysicsEngineComponent
    {
        #region Properties

        /// <summary>
        /// Private backing field for Particles property
        /// </summary>
        private IList<Particle> _Particles = new List<Particle>();

        /// <summary>
        /// The set of particles 
        /// </summary>
        public IList<Particle> Particles
        {
            get { return _Particles; }
        }

        /// <summary>
        /// Private backing field for Gravity property
        /// </summary>
        private Vector _Gravity = new Vector(0, 0, -10);

        /// <summary>
        /// The direction and magnitude of the gravity acceleration, in m/s/s
        /// </summary>
        public Vector Gravity
        {
            get { return _Gravity; }
            set { _Gravity = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a ParticleMotionComponent, generating particles from the
        /// specified nodes.
        /// </summary>
        /// <param name="nodes"></param>
        public ParticleGravityComponent(NodeCollection nodes)
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
            {
                //particle.EndCycle();
                //TODO: Apply other forces
                particle.ApplyForce(Gravity * particle.Mass);
            }
            return true;
        }

        public bool Start(PhysicsEngine engine)
        {
            return true;
        }

        #endregion
    }
}
