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
    /// Generic base class for elements - entities posessing geometry defined by a set of vertices
    /// </summary>
    public abstract class Element<TShape, TElementVertex, TProperty> : Unique
        where TShape : Shape<TElementVertex>
        where TElementVertex : IElementVertex
    {

        #region Properties

        /// <summary>
        /// Private backing member variable for the Geometry property
        /// </summary>
        private Shape<TElementVertex> _Geometry;

        /// <summary>
        /// The set-out geometry of the element.
        /// Describes the editable control geometry that primarily defines
        /// the overall geometry of this object.
        /// The set-out curve of 1D Elements, the surface of slabs, etc.
        /// </summary>
        public Shape<TElementVertex> Geometry
        {
            get { return _Geometry; }
            set
            {
                _Geometry = value;
                NotifyPropertyChanged("Geometry");
            }
        }


        #endregion


    }
}
