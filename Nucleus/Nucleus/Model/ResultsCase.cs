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
    public abstract class ResultsCase : ModelObject
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

        #region Methods

        /// <summary>
        /// Does this case contain the specified load?
        /// </summary>
        /// <param name="load"></param>
        /// <returns></returns>
        public abstract bool Contains(Load load);

#endregion
    }
}
