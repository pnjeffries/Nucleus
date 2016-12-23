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

namespace FreeBuild.Model
{
    /// <summary>
    /// A table of Levels, automatically ordered by Z-coordinate.
    /// </summary>
    /// <remarks>Currently, the case where the z-coordinate of a level is modified after being added
    /// is not dealt with.  TODO?</remarks>
    [Serializable]
    public class LevelTable : LevelCollection
    {
        #region Properties

        /// <summary>
        /// Private backing field for NextNumericID property
        /// </summary>
        private long _NextNumericID = 1;

        /// <summary>
        /// The numeric ID that will be assigned to the next element to be added to this table
        /// </summary>
        public long NextNumericID
        {
            get { return _NextNumericID; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new empty LevelCollection.
        /// </summary>
        public LevelTable() : base() { }

        /// <summary>
        /// Owner constructor.  Initialises a new LevelCollection owned by the specified model.
        /// </summary>
        /// <param name="model">The model that owns the items in this collection</param>
        public LevelTable(Model model) : base(model) { }

        #endregion

        #region Methods

        // Overrides InsertItem to automatically sort this table by level
        protected override void InsertItem(int index, Level item)
        {
            int insertIndex = index;

            for (int i = 0; i < Count; i++)
            {
                Level retrievedItem = this[i];
                if (Comparer<double>.Default.Compare(item.Z, retrievedItem.Z) < 0)
                {
                    insertIndex = i;
                    break;
                }
            }

            base.InsertItem(insertIndex, item);
        }

        /// <summary>
        /// Sort the levels in this table by their z-values.
        /// This should rarely be necessary, as added items are automatically
        /// inserted in a position sorted by z-coordinate.  However, modification
        /// of the 'Z' coordinate of levels after insertion may mean that Levels can
        /// become out of order.
        /// </summary>
        public void Resort()
        {
            List<Level> list = base.Items as List<Level>;
            if (list != null)
            {
                list.Sort((x, y) => x.Z.CompareTo(y.Z));
            }
        }

        protected override void SetNumericID(Level item)
        {
            item.NumericID = NextNumericID;
            _NextNumericID++;
        }

        #endregion
    }
}
