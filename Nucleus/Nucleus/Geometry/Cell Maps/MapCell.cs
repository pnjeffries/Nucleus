using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A cell in an ICellMap which acts as a container for other objects
    /// </summary>
    /// <remarks>Hmmm... this is getting a bit overly complicated...</remarks>
    public class MapCell : DataOwner<MapCellDataStore, IMapCellDataComponent, MapCell>, IMapCell
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Contents property
        /// </summary>
        private ElementCollection _Contents = new ElementCollection();

        /// <summary>
        /// The contents of this map cell
        /// </summary>
        public ElementCollection Contents
        {
            get { return _Contents; }
            set { _Contents = value; }
        }

        #endregion

        #region Methods

        protected override MapCellDataStore NewDataStore()
        {
            return new MapCellDataStore();
        }

        #endregion
    }
}
