using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Interface for tables of objects, which form the top-level storage mechanism
    /// for 
    /// </summary>
    public interface IModelObjectTable
    {
        /// <summary>
        /// Get the next available numeric ID for items in this table
        /// </summary>
        long NextNumericID { get; }
    }
}
