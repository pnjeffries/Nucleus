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

using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Nucleus.Conversion
{
    /// <summary>
    /// A table of identifiers mapping from one ID system to another
    /// </summary>
    /// <typeparam name="TFirstID"></typeparam>
    /// <typeparam name="TSecondID"></typeparam>
    [Serializable]
    public class IDMappingTable<TFirstID, TSecondID> : Dictionary<string, BiDirectionary<TFirstID, TSecondID>>, IIDMappingTable
    {
        #region Properties

        /// <summary>
        /// Private backing field for FirstIDName property
        /// </summary>
        private string _FirstIDName;

        /// <summary>
        /// The name of the first set of IDs to be contained within this table
        /// </summary>
        public string FirstIDName
        {
            get { return _FirstIDName; }
        }

        /// <summary>
        /// Private backing field for the SecondIDName property
        /// </summary>
        private string _SecondIDName;

        /// <summary>
        /// The name of the second set of IDs to be contained within this table
        /// </summary>
        public string SecondIDName
        {
            get { return _SecondIDName; }
        }

        private FilePath _FilePath;

        /// <summary>
        /// The path of the last file this ID mapping table applies to.
        /// Use the LinkToFile method to populate this property and also to automatically
        /// set the LastUsed date and time after a read or write operation.
        /// </summary>
        public FilePath FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }

        /// <summary>
        /// Private backing field for LastUsed property
        /// </summary>
        private DateTime _LastUsed = DateTime.UtcNow;

        /// <summary>
        /// The time (in Coordinated Universal Time, UTC) that this mapping table was last used to
        /// convert to a file.  This should be set to DateTime.UtcNow immediately after a read or
        /// write operation using this mapping has been completed.
        /// Using the LinkToFile method will automatically set this property.
        /// </summary>
        public DateTime LastUsed
        {
            get { return _LastUsed; }
            set { _LastUsed = value; }
        }

        /// <summary>
        /// The name of the category used to store objects when no other category is specified.
        /// </summary>
        public virtual string DefaultCategory { get { return ""; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.  Initialises a new IDMappingTable for the two specified ID type names
        /// </summary>
        /// <param name="firstIDName"></param>
        /// <param name="secondIDName"></param>
        public IDMappingTable(string firstIDName, string secondIDName)
        {
            _FirstIDName = firstIDName;
            _SecondIDName = secondIDName;
        }

        protected IDMappingTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Store the specified filepath and the current time
        /// to the relevant properties in this table.
        /// Call this method immediately after a read or write operation
        /// using this mapping table to store a record of the file involved.
        /// </summary>
        /// <param name="filePath"></param>
        public void LinkToFile(FilePath filePath)
        {
            FilePath = filePath;
            LastUsed = DateTime.UtcNow;
        }

        /// <summary>
        /// Determines whether this mapping table contains an entry within the second set stored
        /// under the specified catagory and associated with the given ID from the first set.
        /// </summary>
        /// <param name="category">The mapping category</param>
        /// <param name="firstID">The ID from the first set</param>
        /// <returns></returns>
        public bool HasSecondID(string category, TFirstID firstID)
        {
            return ContainsKey(category) && this[category].ContainsFirst(firstID);
        }

        /// <summary>
        /// Determines whether this mapping table contains an entry within the second set stored
        /// under any catagory and associated with the given ID from the first set.
        /// </summary>
        /// <param name="firstID">The ID from the first set</param>
        /// <returns></returns>
        public bool HasSecondID(TFirstID firstID)
        {
            foreach (var kvp in this)
            {
                if (kvp.Value.ContainsFirst(firstID)) return true;
            }
            return false;
        }

        /// <summary>
        /// Get the ID from the second set within the category and associated with the ID from
        /// the first set specified.
        /// </summary>
        /// <param name="category">The mapping category</param>
        /// <param name="firstID">The ID from the first set</param>
        /// <returns></returns>
        public TSecondID GetSecondID(string category, TFirstID firstID)
        {
            return this[category].GetSecond(firstID);
        }

        /// <summary>
        /// Get the ID from the second set within any category and associated with the ID from
        /// the first set specified.
        /// </summary>
        /// <param name="firstID">The ID from the first set</param>
        /// <returns></returns>
        public TSecondID GetSecondID(TFirstID firstID)
        {
            foreach (var kvp in this)
            {
                if (kvp.Value.ContainsFirst(firstID)) return kvp.Value.GetSecond(firstID);
            }
            return default(TSecondID);
        }

        /// <summary>
        /// Determines whether this mapping tale contains an entry within the first set stored 
        /// under the specified category and associated with the given ID from the second set
        /// </summary>
        /// <param name="category">The mapping category</param>
        /// <param name="secondID">The ID from the second set</param>
        /// <returns></returns>
        public bool HasFirstID(string category, TSecondID secondID)
        {
            return ContainsKey(category) && this[category].ContainsSecond(secondID);
        }

        /// <summary>
        /// Determines whether this mapping tale contains an entry within the first set stored 
        /// under the specified category and associated with the given ID from the second set
        /// </summary>
        /// <param name="secondID">The ID from the second set</param>
        /// <returns></returns>
        public bool HasFirstID(TSecondID secondID)
        {
            return HasFirstID(DefaultCategory, secondID);
        }

        /// <summary>
        /// Get the ID from the first set within the category and associated with the ID from the
        /// second set specified
        /// </summary>
        /// <param name="category">The mapping category</param>
        /// <param name="secondID">The ID from the second set</param>
        /// <returns></returns>
        public TFirstID GetFirstID(string category, TSecondID secondID)
        {
            return this[category].GetFirst(secondID);
        }

        /// <summary>
        /// Get the ID from the first set within the category and associated with the ID from the
        /// second set specified
        /// </summary>
        /// <param name="category">The mapping category</param>
        /// <param name="secondID">The ID from the second set</param>
        /// <returns></returns>
        public TFirstID GetFirstID(TSecondID secondID)
        {
            return GetFirstID(DefaultCategory, secondID);
        }

        /// <summary>
        /// Add a new entry to this mapping table
        /// </summary>
        /// <param name="category">The mapping category</param>
        /// <param name="firstID">The first ID</param>
        /// <param name="secondID">The second ID</param>
        public void Add(string category, TFirstID firstID, TSecondID secondID)
        {
            if (!ContainsKey(category)) Add(category, new BiDirectionary<TFirstID, TSecondID>());
            this[category].Set(firstID, secondID);
        }

        /// <summary>
        /// Add a new entry to this mapping table
        /// </summary>
        /// <param name="firstID"></param>
        /// <param name="secondID"></param>
        public void Add(TFirstID firstID, TSecondID secondID)
        {
            Add(DefaultCategory, firstID, secondID);
        }

        /// <summary>
        /// Remove an entry from this mapping table
        /// </summary>
        /// <param name="category"></param>
        /// <param name="firstID"></param>
        public void Remove(string category, TFirstID firstID)
        {
            if (ContainsKey(category) && this[category].ContainsKey(firstID))
                this[category].Remove(firstID);
        }

        #endregion
    }
}
