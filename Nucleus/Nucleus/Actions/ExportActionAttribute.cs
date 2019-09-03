using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Actions
{
    /// <summary>
    /// Attribute used to tag export action classes.
    /// </summary>
    public class ExportActionAttribute : Attribute
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
        public ExportActionAttribute()
        {
        }

        /// <summary>
        /// Helper function to get the (first) ExportActionAttribute from the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ExportActionAttribute ExtractFrom(Type type)
        {
            object[] exportAtts = type.GetCustomAttributes(typeof(ExportActionAttribute), false);
            if (exportAtts.Count() > 0)
            {
                return (ExportActionAttribute)exportAtts[0];
            }
            return null;
        }
    }
}
