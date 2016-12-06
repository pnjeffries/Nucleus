using FreeBuild.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// An object which holds additional non-geometric attributes about how a
    /// piece of geometry should be displayed or organised.
    /// </summary>
    [Serializable]
    public class GeometryAttributes
    {
        #region Properties

        /// <summary>
        /// The name of the layer (if any) on which this object should be displayed
        /// </summary>
        public string LayerName { get; set; } = null;

        /// <summary>
        /// The ID of the source object from which this geometry was generated or
        /// to which it is otherwise linked.
        /// </summary>
        public string SourceID { get; set; } = null;

        /// <summary>
        /// The brush which determines how this geometry should be drawn.
        /// </summary>
        public DisplayBrush Brush { get; set; } = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public GeometryAttributes() { }

        /// <summary>
        /// Initialise an attributes object with the specified data
        /// </summary>
        /// <param name="sourceID"></param>
        /// <param name="layerName"></param>
        public GeometryAttributes(string sourceID, string layerName = null, DisplayBrush brush = null)
        {
            SourceID = sourceID;
            LayerName = layerName;
            Brush = brush;
        }

        #endregion
    }
}
