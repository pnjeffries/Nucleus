using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract base class for Model objects - objects which can be directly added to a model
    /// and form the top-level of data within that model.
    /// </summary>
    [Serializable]
    public abstract class ModelObject : Named, IDeletable
    {

        #region Properties

        /// <summary>
        /// Private backing field for IsDeleted property
        /// </summary>
        private bool _IsDeleted = false;

        /// <summary>
        /// Get a boolean value indicating whether this object has been
        /// marked for deletion.  This flag indicates that the object should be
        /// ignored in any operation that acts only on the current state of the
        /// model and that it should be removed during the next cleanup sweep.
        /// </summary>
        public bool IsDeleted
        {
            get
            {
                return _IsDeleted;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this object.
        /// The object itself will not be immediately removed from the model
        /// but will instead be flagged for future removal and ignored wherever
        /// appropriate.
        /// </summary>
        public void Delete()
        {
            if (!_IsDeleted)
            {
                _IsDeleted = true;
                NotifyPropertyChanged("IsDeleted");
            }
        }

        /// <summary>
        /// Undelete this object.
        /// If the deletion flag on this object is set it will be unset and
        /// the object restored.
        /// </summary>
        public void Undelete()
        {
            if (_IsDeleted)
            {
                _IsDeleted = false;
                NotifyPropertyChanged("IsDeleted");
            }
        }

        #endregion
    }
}
