using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Abstract base class for cases which represent a design situation.
    /// This encompasses load cases which define a particular set of physical
    /// loads to be applied to a model and results cases which represent the
    /// input data for a particular analysis run which produces results
    /// </summary>
    [Serializable]
    public abstract class DesignCase : ModelObject
    {
        protected DesignCase()
        {
        }

        protected DesignCase(string name) : base(name)
        {
        }
    }
}
