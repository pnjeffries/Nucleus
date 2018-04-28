using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Enumerated value representing the different possible types of
    /// a structural load case
    /// </summary>
    public enum LoadCaseType
    {
        /// <summary>
        /// The type of load is undefined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The load is static and is unlikely to change significantly
        /// within the life of the structure
        /// </summary>
        Dead = 10,

        /// <summary>
        /// The load is imposed and may vary significantly during operation
        /// of the structure
        /// </summary>
        Live = 20,

        /// <summary>
        /// The load is due to wind action on the structure
        /// </summary>
        Wind = 30,

        /// <summary>
        /// The load is induced by seismic movements
        /// </summary>
        Seismic = 100,

        /// <summary>
        /// The load is induced by changes in temperature in the structure
        /// </summary>
        Thermal = 200
    }
}
