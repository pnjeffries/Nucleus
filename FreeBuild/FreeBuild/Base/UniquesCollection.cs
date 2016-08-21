// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// A collection of objects implementing the IUnique interface
    /// Each item must be unique within this collection - duplicate entries are not allowed.
    /// </summary>
    /// <typeparam name="TItem">The type of uniquely identifiable item</typeparam>
    [Serializable]
    public class UniquesCollection<TItem> : ObservableKeyedCollection<Guid, TItem> where TItem : IUnique
    {
        #region Constructors

        public UniquesCollection() : base() { }

        public UniquesCollection(IEnumerable<IEnumerable<TItem>> toBeCombined) : base(toBeCombined) { }

        #endregion

        protected override Guid GetKeyForItem(TItem item)
        {
            return item.GUID;
        }
    }

    /// <summary>
    /// A collection of unique objects.
    /// </summary>
    [Serializable]
    public class UniquesCollection : UniquesCollection<IUnique>
    {
        public UniquesCollection() : base() { }

        public UniquesCollection(IEnumerable<IEnumerable<IUnique>> toBeCombined) : base(toBeCombined) { }
    }
}
