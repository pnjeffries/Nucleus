using FreeBuild.Base;
using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// A dictionary of IDMappingTables keyed by the filepath they were last written to.
    /// </summary>
    [Serializable]
    public class IDMappingsDictionary : Dictionary<FilePath, IIDMappingTable>
    {
        #region Methods

        /// <summary>
        /// Add a table to this collection,
        /// automatically extracting the key from the stored FilePath.
        /// The FilePath property of the table must have been set for the
        /// table to be successfully added.
        /// </summary>
        /// <param name="table"></param>
        /// <returns>True if successful, false if not.</returns>
        public bool Add(IIDMappingTable table)
        {
            if (table.FilePath.IsSet)
            {
                this[table.FilePath] = table;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the most recently stored ID table of the specified type
        /// </summary>
        /// <typeparam name="TTable">The type of table to find</typeparam>
        /// <returns>The table of that type with the most recent LastUsed value,
        /// or null if no tables of the specified type have been stored.</returns>
        public TTable GetLatest<TTable>() where TTable : class, IIDMappingTable
        {
            TTable result = null;
            DateTime mostRecent = DateTime.MinValue;
            foreach (IIDMappingTable table in Values)
            {
                if (table is TTable && table.LastUsed > mostRecent)
                {
                    mostRecent = table.LastUsed;
                    result = (TTable)table;
                }
            }
            return result;
        }

        /// <summary>
        /// Get the most recently stored ID table for a file with
        /// the specified extension.
        /// </summary>
        /// <param name="extension">The file extension to find</param>
        /// <returns>The table of that extension with the most recent LastUsed value,
        /// or null if no tables of the specified type have been stored.</returns>
        public IIDMappingTable GetLatest(string extension)
        {
            IIDMappingTable result = null;
            DateTime mostRecent = DateTime.MinValue;
            foreach (IIDMappingTable table in Values)
            {
                if (table.FilePath.Extension.EqualsIgnoreCase(extension) && table.LastUsed > mostRecent)
                {
                    mostRecent = table.LastUsed;
                    result = table;
                }
            }
            return result;
        }

        #endregion
    }
}
