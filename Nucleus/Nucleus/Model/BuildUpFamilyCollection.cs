using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A collection of build-up families
    /// </summary>
    [Serializable]
    public class BuildUpFamilyCollection : ModelObjectCollection<BuildUpFamily>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public BuildUpFamilyCollection() : base() { }

        /// <summary>
        /// Initialise a new BuildUpFamilyCollection containing the specified single item
        /// </summary>
        /// <param name="family"></param>
        public BuildUpFamilyCollection(BuildUpFamily family) : base()
        {
            Add(family);
        }

        /// <summary>
        /// Initialise a new BuildUpFamilyCollection containing the specified set of families
        /// </summary>
        /// <param name="families"></param>
        public BuildUpFamilyCollection(IEnumerable<BuildUpFamily> families)
        {
            if (families != null)
            {
                foreach (BuildUpFamily family in families)
                {
                    Add(family);
                }
            }
        }

        #endregion
    }
}
