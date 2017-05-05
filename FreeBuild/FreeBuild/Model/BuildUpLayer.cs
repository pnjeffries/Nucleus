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
    public class BuildUpLayer : Unique, IOwned<PanelFamily>
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

        /// <summary>
        /// Private backing field for Family property
        /// </summary>
        [Copy(CopyBehaviour.MAP)]
        private PanelFamily _Family;

        /// <summary>
        /// The family that this layer is part of
        /// </summary>
        public PanelFamily Family
        {
            get { return _Family; }
            internal set { _Family = value; }
        }

        PanelFamily IOwned<PanelFamily>.Owner
        {
            get { return _Family; }
        }

        #endregion

        #region Methods

        protected override void ChangeProperty<T>(ref T backingField, T newValue, string propertyName, bool notifyIfSame = false)
        {
            base.ChangeProperty(ref backingField, newValue, propertyName, notifyIfSame);
            if (Family != null) Family.NotifyBuildUpChanged(this);
        }

        #endregion
    }
}
