using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// Abstract base class for dictionaries that store analysis results,
    /// either directly or within further sub-dictionaries.
    /// </summary>
    [Serializable]
    public abstract class ResultsDictionary<TKey, TValue> : Dictionary<TKey,TValue>
    {
        #region Properties

       /// <summary>
       /// Indexer - hides the underlying dictionary implementation
       /// in order to return the default of the value type when no record matching the
       /// key is stored.
       /// </summary>
       /// <param name="key"></param>
       /// <returns></returns>
        public new TValue this[TKey key]
        {
            get { return SafeGet(key); }
            set { base[key] = value; }
        }

        #endregion

        #region Constructors

#if !JS
        /// <summary>
        /// Deserialisation constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ResultsDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif

        /// <summary>
        /// Initialises a new empty ResultsDictionary
        /// </summary>
        public ResultsDictionary() : base() {}

        #endregion

        /// <summary>
        /// Returns either the value corresponding to the given key, if the key exists within the dictionary
        /// or the value type default value if it does not.  Essentially wraps TryGetValue in a form which can be used
        /// on a single line.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue SafeGet(TKey key)
        {
            TValue result;
            TryGetValue(key, out result);
            return result;
        }
    }
}
