using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// A basic set of options used when converting between types.
    /// </summary>
    public class ConversionOptions : NotifyPropertyChangedBase
    {
        #region Properties

        /// <summary>
        /// Private backing field for DeleteMissingObjects property
        /// </summary>
        private bool _DeleteMissingObjects = true;

        /// <summary>
        /// Should objects missing from the source dataset during synchronisation be deleted from the
        /// target model?
        /// </summary>
        public bool DeleteMissingObjects
        {
            get { return _DeleteMissingObjects; }
            set { _DeleteMissingObjects = value;  NotifyPropertyChanged("DeleteMissingObjects"); }
        }

        #endregion
    }
}
