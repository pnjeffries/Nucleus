using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Physics
{
    [Serializable]
    public class KineticDampingComponent : ParticleBaseComponent, IPhysicsEngineComponent
    {
        public double KE1 { get; set; } = 0;

        public double KE2 { get; set; } = 0;


        public KineticDampingComponent(NodeCollection nodes) : base(nodes) { }

        public bool Cycle(double dt, PhysicsEngine engine)
        {
            double KE3 = 0;
            foreach (var particle in Particles)
            {
                // Calculate new velocity
                particle.Velocity += particle.Residual / (0.5 * particle.LumpedK.LargestComponent());
                particle.Velocity = particle.Velocity.With(particle.Fixity, 0);
                KE3 += particle.KineticEnergy();
            }

            /*if (KE3 < KE2)
            {
                // Peak reached - adjust position to approximate KE peak and stop
                double beta = 3/2 + (4*KE2 - 3*KE1 - KE3) / (KE3 - 2 * KE2 + KE1);
                foreach (var particle in Particles)
                {
                    particle.Position = particle.Position - (1 + beta) * particle.PastVelocity - (particle.Velocity - particle.PastVelocity);
                        //(2 * particle.Residual / particle.LumpedK.LargestComponent()); //.DivideComponents(particle.LumpedK));
                    particle.Velocity = Vector.Zero;
                    particle.EndCycle();
                }
                KE2 = 0;
                KE3 = 0;
            }
            else
            {*/
            // Move normally
            foreach (var particle in Particles)
            {
                particle.Damp(0.99);
                particle.Move(1.0);
                particle.EndCycle();
            }
            //}

            KE1 = KE2;
            KE2 = KE3;

            return true;
        }

        public bool Start(PhysicsEngine engine)
        {
            // Clear stored records
            return true;
        }
    }
}
