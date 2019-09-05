using Nucleus.Actions;
using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Actions
{
    /// <summary>
    /// Interface for actions which import data from a file
    /// </summary>
    public interface IImportAction : IAction
    {
        /// <summary>
        /// The filepath that is to be read from
        /// </summary>
        FilePath FilePath { get; set; }
    }
}
