using FreeBuild.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    public class PointVertex : Vertex
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Point property
        /// </summary>
        private Vector _Point = Vector.Unset;

        /// <summary>
        /// The point that defines the position of the vertex.
        /// </summary>
        [Dimension(DimensionTypes.Distance)]
        public Vector Point
        {
            get { return _Point; }
            set
            {
                _Point = value;
                NotifyPropertyChanged("Point");
                NotifyPropertyChanged("Position");
            }
        }

        /// <summary>
        /// The current position of this vertex.
        /// For Point Vertices, this position is 
        /// identical to and can be set via the 
        /// Point property.
        /// </summary>
        [Dimension(DimensionTypes.Distance)]
        public override Vector Position
        {
            get
            {
                return _Point;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor, creating the vertex at the specified point
        /// </summary>
        /// <param name="point"></param>
        public PointVertex(Vector point)
        {
            _Point = point;
        }

        #endregion

    }
}
