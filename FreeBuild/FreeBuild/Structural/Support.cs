using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Structural
{
    /// <summary>
    /// Represents a point of nodal support in a structural analysis model
    /// </summary>
    [Serializable]
    public class Support : Unique
    {
        #region properties

        private Bool6D _Directions = new Bool6D(false);

        /// <summary>
        /// The directions in which this support acts
        /// </summary>
        public Bool6D Directions
        {
            get { return _Directions; }
            set { _Directions = value;  NotifyPropertyChanged("Directions"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new support with no properties set.
        /// </summary>
        public Support()
        {
        }

        /// <summary>
        /// Initialises a new support with the specified fixed directions
        /// </summary>
        /// <param name="directions"></param>
        public Support(Bool6D directions)
        {
            Directions = directions;
        }

        #endregion
    }
}
