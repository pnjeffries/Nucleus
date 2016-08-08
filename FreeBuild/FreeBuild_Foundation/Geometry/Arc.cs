using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A planar arc between two points
    /// </summary>
    public class Arc : Curve
    {
        #region Properties

        /// <summary>
        /// The full circle that this arc forms part of
        /// </summary>
        public Disk Circle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool Closed
        {
            get
            {
                throw new NotImplementedException();
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsValid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override VertexCollection Vertices
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Constructors

        public Arc()
        {

        }

        #endregion
    }
}
