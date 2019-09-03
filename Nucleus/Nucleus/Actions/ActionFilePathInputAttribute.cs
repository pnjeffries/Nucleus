using Nucleus.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Actions
{
    /// <summary>
    /// Attributes of an action input parameter property that is a FilePath.
    /// Used to specify information about the input and how it should be obtained/used,
    /// including information specifically relevant to file paths
    /// </summary>
    public class ActionFilePathInputAttribute : ActionInputAttribute
    {
        /// <summary>
        /// The file type filter
        /// </summary>
        public string Filter { get; set; } = "All Files (*.*)|*.*";

        /// <summary>
        /// Open an existing file?  If true, an Open file dialog will be used, 
        /// if false a Save file dialog will be used instead.
        /// </summary>
        public bool Open { get; set; } = false;
    }
}
