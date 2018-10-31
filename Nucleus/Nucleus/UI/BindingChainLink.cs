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

    /// <summary>
    /// Extension methods for the BindingChainLink class and collections thereof
    /// </summary>
    public static class BindingChainLinkExtensions
    {
        /// <summary>
        /// Subscribe a handler function to the PropertyChanged event of all the source
        /// objects in this chain
        /// </summary>
        /// <param name="chain"></param>
        /// <param name="handler"></param>
        public static void AddPropertyChangedHandler(this IList<BindingChainLink> chain, 
            PropertyChangedEventHandler handler)
        {
            foreach (var link in chain)
            {
                link.Source.PropertyChanged += handler;
            }
        }

        /// <summary>
        /// Unsubscribe a handler function from the PropertyChanged event of all the source
        /// objects in this chain
        /// </summary>
        /// <param name="chain"></param>
        /// <param name="handler"></param>
        public static void RemovePropertyChangedHandler(this IList<BindingChainLink> chain, 
            PropertyChangedEventHandler handler)
        {
            foreach (var link in chain)
            {
                link.Source.PropertyChanged -= handler;
            }
        }

        /// <summary>
        /// Find the index of the item in this list with the specified source
        /// </summary>
        /// <param name="chain"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int IndexOfSource(this IList<BindingChainLink> chain, INotifyPropertyChanged source)
        {
            for (int i = 0; i < chain.Count; i++)
            {
                if (chain[i].Source == source) return i;
            }
            return -1;
        }
    }
}
