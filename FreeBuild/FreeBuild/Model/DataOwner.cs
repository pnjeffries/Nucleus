using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract base class for model objects which own data components.
    /// These objects contain a data store which can be used to hold data of various kinds
    /// attached to this object in an easily extensible way.
    /// </summary>
    [Serializable]
    public abstract class DataOwner<TDataStore, TData> : ModelObject
        where TDataStore : DataStore<TData>, new()
    {
        #region Properties

        /// <summary>
        /// Private backing field for Data property
        /// </summary>
        private TDataStore _Data;

        /// <summary>
        /// The store of data objects attached to this model object.
        /// This container can be used to add and retrieve data objects of specific
        /// types.
        /// </summary>
        public TDataStore Data
        {
            get
            {
                if (_Data == null) _Data = new TDataStore();
                return _Data;
            }
        }

        /// <summary>
        /// Gets whether or not this object has any attached data components
        /// </summary>
        public bool HasData
        {
            get
            {
                return _Data != null && _Data.Count > 0;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the data component of the specified type attached to this object (if one exists)
        /// </summary>
        /// <typeparam name="T">The type of the attached data component to be retrieved</typeparam>
        /// <returns></returns>
        public T GetData<T>()where T : class, TData
        {
            return Data.GetData<T>();
        }

        /// <summary>
        /// Get data of the specified generic type (or the closest available sub-type) attached to
        /// this object.  If no data component of the specified type is found then optionally a
        /// new one will be created.
        /// </summary>
        /// <typeparam name="T">The type of data component to be retrieved.</typeparam>
        /// <param name="create">If true, a new data component of the specified type will
        /// be created and returned should one not already exist.</param>
        /// <returns></returns>
        public T GetData<T>(bool create) where T : class, TData, new()
        {
            return Data.GetData<T>(create);
        }

        /// <summary>
        /// Get all data within this store that is of the specified generic type or which
        /// is assignable to that type.
        /// </summary>
        /// <typeparam name="T">The type of data component to be retrieved.</typeparam>
        /// <returns></returns>
        public IList<T> GetAllData<T>() where T : TData
        {
            return Data.GetAllData<T>();
        }

        #endregion

    }
}
