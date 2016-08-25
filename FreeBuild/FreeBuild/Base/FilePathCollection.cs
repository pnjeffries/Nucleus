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

        #region Methods

        protected override string GetKeyForItem(FilePath item)
        {
            return item.Path;
        }

        protected void ShortenToLimit()
        {
            while (Count > MaximumStored) RemoveAt(0);
        }

        protected override void InsertItem(int index, FilePath item)
        {
            if (Contains(item)) Remove(item); //If the item already exists in the collection,
            // remove it and insert it again at the new position.
            base.InsertItem(index, item);
            ShortenToLimit();
        }

        #endregion
    }
}
