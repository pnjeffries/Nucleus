using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry.Cell_Maps
{
    /// <summary>
    /// A cell in an ICellMap which acts as a container for other objects
    /// </summary>
    public class MapCell<TItem, TCollection>
        where TCollection : IList<TItem>, new()
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Contents property
        /// </summary>
        private TCollection _Contents = new TCollection();

        /// <summary>
        /// The contents of this map cell
        /// </summary>
        public TCollection Contents
        {
            get { return _Contents; }
            set { _Contents = value; }
        }

        #endregion
    }
}
