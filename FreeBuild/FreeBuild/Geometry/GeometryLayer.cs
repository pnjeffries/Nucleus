using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A collection of vertex geometry that acts equivalently to a layer in a CAD package
    /// </summary>
    public class GeometryLayer : ShapeCollection
    {
        #region Properties

        /// <summary>
        /// Private backing field for the Name property
        /// </summary>
        private string _Name;

        /// <summary>
        /// The name of the layer
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Private backing field for the Visible property
        /// </summary>
        private bool _Visible = true;

        /// <summary>
        /// Whether or not this layer is visible and should be rendered
        /// </summary>
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;
                NotifyPropertyChanged("Visible");
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public GeometryLayer(string name)
        {
            Name = name;
        }

        #endregion


    }
}
