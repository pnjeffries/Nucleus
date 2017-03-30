using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of user coordinate systems
    /// </summary>
    [Serializable]
    public class UserCoordinateSystemReferenceTable : UserCoordinateSystemReferenceCollection
    {
        #region Methods

        /// <summary>
        /// Get a coordinate system reference by its name.  This performs the same function as
        /// FindByName but with the addition that the keywords 'Global' and 'Local' can be used
        /// to specify the Global and Local axes respectively.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CoordinateSystemReference GetByKeyword(string name)
        {
            if (name.EqualsIgnoreCase("Global")) return CoordinateSystemReference.Global;
            else if (name.EqualsIgnoreCase("Local")) return CoordinateSystemReference.Local;
            else return FindByName(name);
        }

        #endregion
    }
}
