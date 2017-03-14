using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of load cases
    /// </summary>
    [Serializable]
    public class LoadCaseCollection : ModelObjectCollection<LoadCase>
    {
        /// <summary>
        /// Initialise a new blank load case collection
        /// </summary>
        public LoadCaseCollection() : base() { }

        /// <summary>
        /// Initialse a new load case collection owned by the specified model
        /// </summary>
        /// <param name="model"></param>
        protected LoadCaseCollection(Model model) : base(model) { }
    }
}
