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
    /// A physics engine component that applies Hooke's law to determine forces
    /// acting on the ends of a set of springs according to the current deformation
    /// </summary>
    [Serializable]
    public class SpringForceComponent : Unique, IPhysicsEngineComponent
    {
        #region Properties

        /// <summary>
        /// Private backing field for Springs property
        /// </summary>
        private IList<Spring> _Springs = new List<Spring>();

        /// <summary>
        /// The collection of springs to be processed
        /// </summary>
        public IList<Spring> Springs
        {
            get { return _Springs; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new spring force physics component including
        /// springs based on the specified set of linear elements
        /// </summary>
        /// <param name="elements"></param>
        public SpringForceComponent(LinearElementCollection elements)
        {
            foreach (var element in elements)
            {
                if (!element.IsDeleted)
                {
                    if (!element.HasData<Spring>())
                    {
                        element.Data.Add(new Spring(element));
                    }
                    Springs.Add(element.GetData<Spring>());
                }
            }
        }

        #endregion

        public bool Cycle(double dt, PhysicsEngine engine)
        {
            foreach (var spring in Springs)
            {
                spring.ApplyForces();
            }
            return true;
        }

        public bool Start(PhysicsEngine engine)
        {
            foreach (Spring spring in Springs) spring.Reset();
            return true;
        }
    }
}
