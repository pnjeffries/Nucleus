using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract base class for objects which represent a load of some kind applied to the model to
    /// be considered during analysis.
    /// </summary>
    [Serializable]
    public abstract class Load : ModelObject
    {

    }

    /// <summary>
    /// Generic abstract base class for objects which represent a load of some kind applied to the model
    /// to be considered during analysis
    /// </summary>
    /// <typeparam name="TAppliedTo">The type of object that this load applies to</typeparam>
    /// <typeparam name="TAppliedToCollection">The type of collection of TAppliedTo to use to store</typeparam>
    [Serializable]
    public abstract class Load<TAppliedTo, TAppliedToCollection> : Load
        where TAppliedTo : ModelObject
        where TAppliedToCollection : ModelObjectCollection<TAppliedTo>, new()
    {
        #region Properties

        /// <summary>
        /// The collection of objects that this load is applied to
        /// </summary>
        public TAppliedToCollection AppliedTo { get; } = new TAppliedToCollection();

        #endregion
    }
}
