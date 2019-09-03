using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Actions
{
    /// <summary>
    /// Attribute used to tag file importer actions
    /// </summary>
    public class ImportActionAttribute : Attribute
    {
        /// <summary>
        /// The list of file extensions that will be supported.
        /// The preceding '.' should be included (i.e. '.txt', '.xml' etc.)
        /// </summary>
        public string[] Extensions { get; set; }

        /// <summary>
        /// The list of filter strings for each file type
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ImportActionAttribute()
        {
        }

        /// <summary>
        /// Helper function to get the (first) ExportActionAttribute from the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ImportActionAttribute ExtractFrom(Type type)
        {
            object[] importAtts = type.GetCustomAttributes(typeof(ImportActionAttribute), false);
            if (importAtts.Count() > 0)
            {
                return (ImportActionAttribute)importAtts[0];
            }
            return null;
        }
    }
}
