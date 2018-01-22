using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Abstract base class for cell maps with a regular geometric arrangement
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class RegularCellMap<T> : Unique, ICellMap<T>
    {
        /// <summary>
        /// Get or set the contents of the cell at the specified cell index
        /// </summary>
        /// <param name="cellIndex">The 1-dimensional cell index</param>
        /// <returns></returns>
        public abstract T this[int cellIndex] { get; set; }

        /// <summary>
        /// Does a cell exist at the specified index?
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public abstract bool Exists(int cellIndex);

        /// <summary>
        /// Get the index of the cell at the specified location
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public abstract int IndexAt(Vector location);
    }
}
