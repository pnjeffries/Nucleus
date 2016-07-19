using FreeBuild.Base;
using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Entities posessing geometry defined by a set of vertices
    /// and potentially a set of attached data defining additional properties.
    /// The IElement interface provides a simple way of interacting with elements without needing to
    /// define specific generic parameters.
    /// </summary>
    public interface IElement : IUnique
    {
        /// <summary>
        /// The geometry of this element.
        /// </summary>
        IShape Geometry { get; }
    }
}
