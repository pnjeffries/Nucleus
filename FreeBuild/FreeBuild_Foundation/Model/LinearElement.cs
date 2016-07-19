using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A linear element is one defined in terms of a set-out curve
    /// and a section that is swept along the curve to define the 3D
    /// solid geometry.
    /// Used to represent objects where one dimension is greater than
    /// the others and the overall geometry can be represented as an
    /// extrusion along a curve, such as Beams, Columns, 
    /// </summary>
    [Serializable]
    public class LinearElement : Element<Curve, SectionProperty>
    {
        #region Constructors

        /// <summary>
        /// Curve geometry constructor
        /// </summary>
        /// <param name="geometry"></param>
        public LinearElement(Curve geometry)
        {
            Geometry = geometry;
        }

        /// <summary>
        /// Curve, property constructor
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="property"></param>
        public LinearElement(Curve geometry, SectionProperty property) : this(geometry)
        {
            Property = property;
        }

        /// <summary>
        /// Start and end point constructor
        /// Creates an element set-out along a straight line between
        /// the start and end points.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public LinearElement(Vector startPoint, Vector endPoint)
        {
            Geometry = new Line(startPoint, endPoint);
        }

        /// <summary>
        /// Start and end point constructor
        /// Creates an element set-out along a straight line between
        /// the start and end points.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="property"></param>
        public LinearElement(Vector startPoint, Vector endPoint, SectionProperty property):this(startPoint, endPoint)
        {
            Property = property;
        }

        #endregion

    }
}
