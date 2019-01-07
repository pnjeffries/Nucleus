using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Debugging
{
    /// <summary>
    /// A collection debug view to present collection items in 
    /// the debug inspector.
    /// Based on https://www.codeproject.com/Articles/28405/Make-the-debugger-show-the-contents-of-your-custom
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionDebugView<T>
    {
        private ICollection<T> _Collection;

        public CollectionDebugView(ICollection<T> collection)
        {
            _Collection = collection ?? throw new ArgumentNullException("collection");
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                T[] array = new T[_Collection.Count];
                _Collection.CopyTo(array, 0);
                return array;
            }
        }
    }
}
