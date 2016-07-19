using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Core
{
    /// <summary>
    /// The core manager class.
    /// Deals with general file and data handline and overall top-level
    /// applicaton management.
    /// </summary>
    public class Core
    {
        #region Properties

        /// <summary>
        /// Private internal singleton instance
        /// </summary>
        private static Core _Instance = null;

        /// <summary>
        /// Get the singleton instance of the core object.
        /// </summary>
        public static Core Instance { get { return _Instance; } }

        /// <summary>
        /// Get the host application interface
        /// </summary>
        public IHost Host { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="host"></param>
        private Core(IHost host)
        {
            Host = host;
        }

        #endregion
    }
}
