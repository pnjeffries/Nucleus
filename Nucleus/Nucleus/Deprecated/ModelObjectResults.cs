using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Model;

namespace Nucleus.Results.Deprecated
{
    /// <summary>
    /// Abstract generic base class for object results dictionaries
    /// </summary>
    /// <typeparam name="TResultsType"></typeparam>
    /// <typeparam name="TResults"></typeparam>
    [Serializable]
    public abstract class ModelObjectResults<TResultsType, TResults> : ResultsDictionary<TResultsType, TResults>, IModelObjectResults
    {
        #region Constructors

#if !JS

        /// <summary>
        /// Deserialisation constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ModelObjectResults(SerializationInfo info, StreamingContext context) : base(info, context) { }

#endif

        /// <summary>
        /// Initialises a new empty ResultsDictionary
        /// </summary>
        public ModelObjectResults() : base() { }

#endregion

#region Methods

        public ICaseResults Get(ResultsCase rCase)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ResultsCase rCase)
        {
            throw new NotImplementedException();
        }

#endregion
    }
}
