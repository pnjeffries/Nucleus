using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A curve formed of several continuous curves joined together.
    /// </summary>
    [Serializable]
    public class PolyCurve : Curve
    {
        public override bool Closed
        {
            get
            {
                throw new NotImplementedException();
            }
            protected set { }
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
    }
}
