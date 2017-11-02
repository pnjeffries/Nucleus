using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Physics
{
    /// <summary>
    /// A data component used during physical simulations to model
    /// linear elements as strings
    /// </summary>
    [Serializable]
    public class Spring : IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// The element this spring represents and is attached to
        /// </summary>
        public LinearElement Element { get; set; }

        /// <summary>
        /// The length of the spring at which it is at rest and
        /// is exerting no force, in m
        /// </summary>
        public double RestLength { get; set; }

        /// <summary>
        /// The stiffness of the spring in N/m
        /// </summary>
        public double Stiffness { get; set; } = 1.0;

        /// <summary>
        /// Does this spring resist compression?
        /// </summary>
        public bool Compression { get; set; } = true;

        /// <summary>
        /// Does this spring resist tension?
        /// </summary>
        public bool Tension { get; set; } = true;

        /// <summary>
        /// The particle at the start of the spring
        /// </summary>
        public Particle StartParticle { get; set; } = null;

        /// <summary>
        /// The particle at the end of the spring
        /// </summary>
        public Particle EndParticle { get; set; } = null;

        #endregion

        #region Constructors

        public Spring(LinearElement element)
        {
            Element = element;
            Reset();
        }

        #endregion

        #region Methods

        // Reset this spring to its initial state
        public void Reset()
        {
            if (Element?.Geometry != null)
            {
                if (Element.Family != null)
                {
                    Stiffness = Element.Family.GetAxialStiffness();
                }
                else Stiffness = 0;
            }
            if (Element.StartNode != null)
            {
                StartParticle = Element.StartNode.GetData<Particle>();
            }
            if (Element.EndNode != null)
            {
                EndParticle = Element.EndNode.GetData<Particle>();
            }
            RestLength = Element.EndNode.Position.DistanceTo(Element.StartNode.Position);

        }

        /// <summary>
        /// Calculate and apply any internal forces due to deformation
        /// to the particles connected to this spring
        /// </summary>
        public void ApplyForces()
        {
            if (StartParticle != null && EndParticle != null)
            {
                Vector dir = EndParticle.Position - StartParticle.Position;
                double length = dir.Magnitude();
                dir /= length;
                double extension = length - RestLength;
                if ((Compression && extension < 0) 
                    || (Tension && extension > 0))
                {
                    Vector force = dir * (extension * Stiffness);
                    StartParticle.ApplyForce(force);
                    EndParticle.ApplyForce(-force);
                }
            }
        }

        /// <summary>
        /// Calculate and return the axial force due to compression/tension in this spring
        /// </summary>
        /// <returns></returns>
        public double AxialForce()
        {
            Vector dir = EndParticle.Position - StartParticle.Position;
            double length = dir.Magnitude();
            dir /= length;
            double extension = length - RestLength;
            return extension * Stiffness;
        }

        #endregion
    }
}
