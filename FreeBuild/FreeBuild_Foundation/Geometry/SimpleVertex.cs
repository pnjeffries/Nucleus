using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBuild.Base;

namespace FreeBuild.Geometry
{
    public class SimpleVertex : Unique, IVertex
    {

        #region Properties

        /// <summary>
        /// Private backing member variable for the Position property
        /// </summary>
        private Vector _Position = Vector.Unset;

        /// <summary>
        /// The position of this vertex
        /// </summary>
        public Vector Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                NotifyPropertyChanged("Position");
            }
        }

        Vector IVertex.Position
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Position constructor
        /// </summary>
        /// <param name="position"></param>
        public SimpleVertex(Vector position)
        {
            _Position = position;
        }

        #endregion
    }
}
