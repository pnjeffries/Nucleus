using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Interface for objects which can be deleted.
    /// Deleted objects will remain within the object model but be marked for
    /// deletion and removed at some future point.
    /// </summary>
    public interface IDeletable
    {
        #region Properties

        /// <summary>
        /// Get a boolean value indicating whether this object has been
        /// marked for deletion.
        /// </summary>
        bool IsDeleted { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this object.
        /// The object itself will not be immediately removed from the model
        /// but will instead be flagged for future removal and ignored wherever
        /// appropriate.
        /// </summary>
        void Delete();

        /// <summary>
        /// Undelete this object.
        /// If the deletion flag on this object is set it will be unset and
        /// the object restored.
        /// </summary>
        void Undelete();

        #endregion
    }
}
