using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// A named object which implements the IDeletable interface to allow it to be
    /// tagged for deletion
    /// </summary>
    [Serializable]
    public abstract class Deletable : Named, IDeletable
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

        #region Constructors

        protected Deletable()
        {
        }

        protected Deletable(Deletable other) : base(other)
        {
            _IsDeleted = other.IsDeleted;
        }

        protected Deletable(string name) : base(name)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this object.
        /// The object itself will not be immediately removed from the model
        /// but will instead be flagged for future removal and ignored in most
        /// operations.  Check the IsDeleted property to see whether this
        /// object is marked for deletion.
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
        public virtual void Undelete()
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
