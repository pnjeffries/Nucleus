using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A data component which may be attached to elements and map cells
    /// which will register a collision with solid objects in the same map cell
    /// </summary>
    public class MapCellCollider : Unique, IMapCellDataComponent, IElementDataComponent
    {
        /// <summary>
        /// Private backing member variable for the Solid property
        /// </summary>
        private bool _Solid = true;

        /// <summary>
        /// Is this collider solid?  Solid objects will register collisions with other solid objects.
        /// </summary>
        public bool Solid
        {
            get { return _Solid; }
            set
            {
                _Solid = value;
                NotifyPropertyChanged("Solid");
            }
        }
    }
}
