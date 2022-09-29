using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A family which describes a 3D block of geometry which is
    /// positioned according to a point in 3D space
    /// </summary>
    [Serializable]
    public class BlockFamily : Family
    {
        /// <summary>
        /// Private backing field for Geometry property
        /// </summary>
        private VertexGeometry _Geometry;

        /// <summary>
        /// The block base geometry
        /// </summary>
        public VertexGeometry Geometry
        {
            get { return _Geometry; }
            set { _Geometry = value; }
        }
        //TODO!

        public override Material GetPrimaryMaterial()
        {
            //TODO!
            return null;
        }

        public double GetVolume(Material material = null)
        {
            return 0;
        }
    }
}
