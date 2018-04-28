using Nucleus.Model.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Abstract base class for cases which produce results
    /// </summary>
    [Serializable]
    public abstract class ResultsCase : DesignCase
    {

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected ResultsCase() : base() { }

        /// <summary>
        /// Name constructor
        /// </summary>
        /// <param name="name"></param>
        protected ResultsCase(string name) : base(name) { }

        #endregion
    }
}
