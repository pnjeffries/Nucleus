using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A set of loading which is applied to the model under a particular condition
    /// </summary>
    public class LoadCase
    {
        #region Properties

        /// <summary>
        /// The set of loads which form this load case
        /// </summary>
        public LoadCollection Loads { get; } = new LoadCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new blank load case.
        /// </summary>
        public LoadCase() : base() { }

        #endregion
    }
}
