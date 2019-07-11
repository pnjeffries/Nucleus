using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UI
{
    /// <summary>
    /// Interface for objects which have a data context property
    /// </summary>
    public interface IDataContext
    {
        /// <summary>
        /// The data context of the object
        /// </summary>
        object DataContext { get; set; }
    }
}
