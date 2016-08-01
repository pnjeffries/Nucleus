using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    public class PolyLine : Curve
    {
        #region Properties

        /// <summary>
        /// Whether this curve is closed.
        /// If true, the end of the curve is treated as being the same as the start point.
        /// Default is false.
        /// </summary>
        public override bool Closed { get; protected set; }

        /// <summary>
        /// Is this polyline valid?
        /// </summary>
        public override bool IsValid
        {
            get
            {
                if (Vertices.Count > 1) return true;
                else return false;
            }
        }

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this polyline.
        /// The polyline will be defined as straight lines in between the vertices
        /// in this collection.
        /// </summary>
        public override VertexCollection Vertices { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Points constructor.
        /// Creates a polyline between the specified set of points
        /// </summary>
        /// <param name="points"></param>
        /// <param name="close"></param>
        public PolyLine(IEnumerable<Vector> points, bool close = false)
        {
            foreach(Vector pt in points)
            {
                Vertices.Add(new Vertex(pt));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Close this polyline, so that a line segment joins the last vertex and the first one.
        /// </summary>
        /// <param name="close">If true, polyline will be made closed.  If false, will be made unclosed.</param>
        public void Close(bool close = true)
        {
            Closed = close;
        }

        #endregion
    }
}
