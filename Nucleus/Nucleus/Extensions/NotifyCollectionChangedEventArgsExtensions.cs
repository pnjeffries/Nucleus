using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Static extension methods for the NotifyCollectionChangedEventArgs class
    /// </summary>
    public static class NotifyCollectionChangedEventArgsExtensions
    {
        /// <summary>
        /// Invert these arguments to represent the opposite action
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static NotifyCollectionChangedEventArgs Reverse(this NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, args.NewItems, args.NewStartingIndex);
            }
            else if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, args.OldItems, args.OldStartingIndex);
            }
            else if (args.Action == NotifyCollectionChangedAction.Move)
            {
                return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, args.OldItems, args.OldStartingIndex, args.NewStartingIndex);
            }
            else if (args.Action == NotifyCollectionChangedAction.Replace)
            {
                return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, args.OldItems, args.NewItems);
            }
            else
            {
                return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, args.OldItems, args.OldStartingIndex);
            }
        }
    }
}
