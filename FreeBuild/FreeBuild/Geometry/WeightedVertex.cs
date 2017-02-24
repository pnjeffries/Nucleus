using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A subclass of Vertex which in addition to the normal
    /// vertex functionality allows for this vertex to be assigned a 'weight'
    /// value to be used during certain geometric calculations (e.g. Voronoi cell generation)
    /// </summary>
    [Serializable]
    public class WeightedVertex : Vertex
    {
        #region Properties

        /// <summary>
        /// Private backing field for Weighting property
        /// </summary>
        private double _Weighting = 1.0;

        /// <summary>
        /// The weighting of this vertex
        /// </summary>
        public double Weighting
        {
            get { return _Weighting; }
            set { _Weighting = value; }
        }

        #endregion

        #region Constructors

        public WeightedVertex() : base() { }

        public WeightedVertex(Vector position, double weighting = 1.0) : base(position)
        {
            _Weighting = weighting;
        }

        #endregion
    }
}
