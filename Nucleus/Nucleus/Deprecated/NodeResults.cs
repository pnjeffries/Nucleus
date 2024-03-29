﻿using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results.Deprecated
{
    /// <summary>
    /// Results storage table for nodes, keyed by result type
    /// </summary>
    [Serializable]
    public class NodeResults : ModelObjectResults<NodeResultTypes, Interval>
    {
        #region Constructors

#if !JS
        /// <summary>
        /// Deserialisation constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected NodeResults(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif

        /// <summary>
        /// Initialises a new empty NodeResults dictionary
        /// </summary>
        public NodeResults() : base() { }

#endregion
    }
}
