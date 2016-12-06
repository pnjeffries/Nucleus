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

        #endregion
    }
}
