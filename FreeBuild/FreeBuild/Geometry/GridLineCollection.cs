using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A collection of curves that represent parametric gridlines
    /// </summary>
    public class GridLineCollection : CurveCollection
    {
        #region Properties

        /// <summary>
        /// Private backing field for Generator property
        /// </summary>
        private GridLineGenerator _Generator;

        /// <summary>
        /// The object which controls generation of this collection of gridlines.
        /// If null, the grid lines are manually created and will not be replaced on regeneration
        /// </summary>
        public GridLineGenerator Generator
        {
            get { return _Generator; }
            set { _Generator = value;  NotifyPropertyChanged("Generator"); }
        }


        #endregion
    }
}
