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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Interface for objects which may be 'owned' by some specific other object.
    /// Note that while these objects are capable of being owned it does not follow
    /// that they always have an owner.
    /// </summary>
    public interface IOwned<TOwner>
    {
        /// <summary>
        /// This oject that this object 'belongs' to, if any.
        /// </summary>
        TOwner Owner { get; }
    }

    /// <summary>
    /// Extension methods for the IOwned interface and collections thereof
    /// </summary>
    public static class IOwnedExtensions
    {
        /// <summary>
        /// Remove all objects from this list that do not have an owner
        /// </summary>
        /// <param name="list"></param>
        public static void RemoveUnowned<TItem, TOwner>(this IList<TItem> list)
            where TItem : IOwned<TOwner>
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];
                if (item.Owner == null) list.RemoveAt(i);
            }
        }
    }
}
