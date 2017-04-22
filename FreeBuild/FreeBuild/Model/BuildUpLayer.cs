using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Class representing a layer of a given material and thickness that forms
    /// part of the build-up definition of a Face Family
    /// </summary>
    [Serializable]
    public class BuildUpLayer : Unique
    {
        #region Properties

        /// <summary>
        /// Private backing field for Thickness property
        /// </summary>
        private double _Thickness = 0.1;

        /// <summary>
        /// The thickness of the layer
        /// </summary>
        public double Thickness
        {
            get { return _Thickness; }
            set { ChangeProperty(ref _Thickness, value, "Thickness"); }
        }

        /// <summary>
        /// Private backing field for Material property
        /// </summary>
        private Material _Material = null;

        /// <summary>
        /// The material that this layer is made of
        /// </summary>
        public Material Material
        {
            get { return _Material; }
            set { ChangeProperty(ref _Material, value, "Material"); }
        }

        #endregion
    }
}
