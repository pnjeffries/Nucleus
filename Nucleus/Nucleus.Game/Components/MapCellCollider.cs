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
    [Serializable]
    public class MapCellCollider : Unique, IMapCellDataComponent, IElementDataComponent
    {
        #region Properties

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

        private Func<Element, bool> _BlockingCheck = el => el.GetData<MapCellCollider>()?.Solid ?? false;

        /// <summary>
        /// Get the condition used to check whether another element blocks this entity's
        /// movement.  (By default this just checks if the object is solid)
        /// </summary>
        public Func<Element, bool> BlockingCheck
        {
            get { return _BlockingCheck; }
            set { _BlockingCheck = value; }
        }

        #endregion

        #region

        /// <summary>
        /// Default constructor
        /// </summary>
        public MapCellCollider()
        {

        }

        public MapCellCollider(bool solid)
        {
            Solid = solid;
        }

        public MapCellCollider(bool solid, Func<Element, bool> blockingCheck) : this(solid)
        {
            _BlockingCheck = blockingCheck;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Can the owner of this collider enter the specified cell?
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool CanEnter(MapCell cell)
        {
            return Blocker(cell) == null;
        }

        /// <summary>
        /// Get the element which is blocking the owner of this collider
        /// from entering the specified cell.  If this returns null, there
        /// is no blocking element in the cell and the owner may enter it.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public Element Blocker(MapCell cell)
        {
            if (!Solid) return null; //Can pass through!
            foreach (Element el in cell.Contents)
            {
                if (BlockingCheck.Invoke(el)) return el; // Blockage!
            }
            return null; // No blockage!
        }

        #endregion

    }
}
