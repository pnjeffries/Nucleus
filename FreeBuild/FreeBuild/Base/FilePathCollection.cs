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

using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// A collection of file paths intended to store the collection of most recently opened files
    /// for an application
    /// </summary>
    [Serializable]
    public class FilePathCollection : ObservableKeyedCollection<string, FilePath>
    {
        #region Properties

        /// <summary>
        /// Private backing field for MaximumStored property.
        /// </summary>
        [Copy(CopyBehaviour.COPY)]
        private int _MaximumStored = 0;

        /// <summary>
        /// The maximum number of files that will be retained within this collection.
        /// If set to zero the number stored is unlimited.
        /// </summary>
        public int MaximumStored
        {
            get { return _MaximumStored; }
            set { _MaximumStored = value; ShortenToLimit(); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new empty FilePathCollection with unlimited storage
        /// </summary>
        public FilePathCollection() : base() { }

        /// <summary>
        /// Initialise a new empty FilePathCollection 
        /// </summary>
        /// <param name="maximumStored"></param>
        public FilePathCollection(int maximumStored) : this() { }

        #endregion

        #region Methods

        protected override string GetKeyForItem(FilePath item)
        {
            return item.Path;
        }

        protected void ShortenToLimit()
        {
            if (MaximumStored > 0) while (Count > MaximumStored) this.RemoveLast();
        }

        protected override void InsertItem(int index, FilePath item)
        {
            if (item.IsValid)
            {
                if (Contains(item)) Remove(item); //If the item already exists in the collection,
                                                  // remove it and insert it again at the new position.
                base.InsertItem(index, item);
                ShortenToLimit();
            }
        }

        #endregion
    }
}
