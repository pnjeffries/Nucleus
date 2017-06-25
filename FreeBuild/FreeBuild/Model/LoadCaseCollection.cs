using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A collection of load cases
    /// </summary>
    [Serializable]
    public class LoadCaseCollection : ModelObjectCollection<LoadCase>
    {
        #region Constructors

        /// <summary>
        /// Initialise a new blank load case collection
        /// </summary>
        public LoadCaseCollection() : base() { }

        /// <summary>
        /// Initialse a new load case collection owned by the specified model
        /// </summary>
        /// <param name="model"></param>
        protected LoadCaseCollection(Model model) : base(model) { }

        #endregion

        #region Methods

        ///// <summary>
        ///// Retrieve a load contained within one of the constituent cases in this
        ///// collection by its GUID
        ///// </summary>
        ///// <param name="guid"></param>
        ///// <returns>The load with the specified ID if it could be found within
        ///// any of the cases in this collection, else null</returns>
        //public Load GetLoad(Guid guid)
        //{
        //    foreach (LoadCase lC in this)
        //    {
        //        if (lC.Loads.Contains(guid)) return lC.Loads[guid];
        //    }
        //    return null;
        //}

        // TODO: Remove deleted objects from load cases

        #endregion
    }
}
