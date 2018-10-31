using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Actions
{
    /// <summary>
    /// An interface for Salamander actions which provide file export functionality.
    /// You can implement this interface yourself, but it's probably better to inherit from ExportActionBase,
    /// which will provide the basic implementation for you.
    /// </summary>
    public interface IExportAction : IAction
    {
        /// <summary>
        /// The filepath that is to be written to
        /// </summary>
        FilePath FilePath { get; set; }

        /// <summary>
        /// The document to be written to a file
        /// </summary>
        ModelDocument Document { get; set; }
    }
}
