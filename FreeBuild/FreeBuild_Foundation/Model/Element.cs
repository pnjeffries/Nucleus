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
    /// Generic base class for elements - objects which represent physical model
    /// entities and which are defined by a set-out geometry which describes the
    /// overall abstract form of the element and by a volumetric property which
    /// determines how that design representation converts into a 3D solid object.
    /// </summary>
    [Serializable]
    public abstract class Element<TShape, TProperty> : Unique
        where TShape : Shape
        where TProperty : VolumetricProperty
    {

        #region Properties

        /// <summary>
        /// Private backing member variable for the Geometry property
        /// </summary>
        private TShape _Geometry;

        /// <summary>
        /// The set-out geometry of the element.
        /// Describes the editable control geometry that primarily defines
        /// the overall geometry of this object.
        /// The set-out curve of 1D Elements, the surface of slabs, etc.
        /// </summary>
        public TShape Geometry
        {
            get { return _Geometry; }
            set
            {
                _Geometry = value;
                NotifyPropertyChanged("Geometry");
            }
        }

        /// <summary>
        /// Private backing member variable for the Property property
        /// </summary>
        private TProperty _Property;

        /// <summary>
        /// The volumetric property that describes how the editable set-out 
        /// geometry of this element should be interpreted to produce a 
        /// full 3D solid object
        /// </summary>
        public TProperty Property
        {
            get { return _Property; }
            set
            {
                _Property = value;
                NotifyPropertyChanged("Property");
            }
        }

        #endregion


    }
}
