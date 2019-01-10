using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Physics
{
    [Serializable]
    public abstract class ParticleBaseComponent : Unique
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

        #endregion

        #region Constructors

        public ParticleBaseComponent(NodeCollection nodes)
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
    }
}
