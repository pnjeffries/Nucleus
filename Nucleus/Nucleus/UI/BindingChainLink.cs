using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UI
{
    /// <summary>
    /// An entry in a binding chain.  This is used
    /// in binding operations to keep track of
    /// the chain of objects which must be traversed
    /// to reach the target.
    /// </summary>
    public class BindingChainLink
    {
        #region Properties

        /// <summary>
        /// The source object to watch for property changes
        /// </summary>
        public INotifyPropertyChanged Source { get; set; }

        /// <summary>
        /// The name of the property on the source object that
        /// is to be watched for changes.
        /// </summary>
        public string PropertyName { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new BindingChainEntry
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        public BindingChainLink(INotifyPropertyChanged source, string propertyName)
        {
            Source = source;
            PropertyName = propertyName;
        }

        #endregion
    }
}
