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
        /// The set-out geometry of the element.
        /// Describes the editable control geometry that primarily defines
        /// the overall geometry of this object.
        /// The set-out curve of 1D Elements, the surface of slabs, etc.
        /// </summary>
        Shape Geometry { get; }

        /// <summary>
        /// The volumetric property that describes how the editable set-out 
        /// geometry of this element should be interpreted to produce a 
        /// full 3D solid object
        /// </summary>
        VolumetricProperty Property { get; }

        /// <summary>
        /// Notify this element that it's geometric representation has been updated
        /// </summary>
        void NotifyGeometryUpdated();
    }
}
