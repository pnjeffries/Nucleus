using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBuild.Geometry;

namespace FreeBuild.Model
{
    /// <summary>
    /// A 2D element the geometry of which is determined by a
    /// set-out surface geometry and a face property which describes
    /// the cross-thickness properties.
    /// </summary>
    public class PanelElement : Element<Surface, FaceProperty>
    {
        #region Constructors

        /// <summary>
        /// Initialises a panel element with the specified set-out
        /// geometry.
        /// </summary>
        /// <param name="geometry">The set-out geometry which defines the shape of
        /// the element.</param>
        public PanelElement(Surface geometry)
        {
            Geometry = geometry;
        }

        #endregion
    }
}
