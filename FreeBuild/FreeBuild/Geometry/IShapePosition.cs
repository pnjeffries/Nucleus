using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Interface for objects which describe a position on a shape
    /// </summary>
    public interface IShapePosition
    {
    }

    /// <summary>
    /// Generic version of the IShapePosition interface.
    /// Allows the type of shape the position is applicable to
    /// to be defined.
    /// </summary>
    /// <typeparam name="TShape"></typeparam>
    public interface IShapePosition<TShape> : IShapePosition
        where TShape : Shape
    {
        /// <summary>
        /// Get the point on the shape that this object
        /// describes as a Vector
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        Vector PointOn(TShape shape);
    }
}
