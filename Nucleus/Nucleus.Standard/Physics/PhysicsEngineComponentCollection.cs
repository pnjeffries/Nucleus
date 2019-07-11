using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Physics
{
    /// <summary>
    /// A collection of physics engine components
    /// </summary>
    [Serializable]
    public class PhysicsEngineComponentCollection : UniquesCollection<IPhysicsEngineComponent>
    {
    }
}
