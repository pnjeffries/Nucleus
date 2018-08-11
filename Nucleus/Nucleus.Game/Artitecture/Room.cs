using Nucleus.Base;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Class to hold information about a room in a game level
    /// </summary>
    public class Room : Unique
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Template property
        /// </summary>
        private RoomTemplate _Template;

        /// <summary>
        /// The template used to generate this Room
        /// </summary>
        public RoomTemplate Template
        {
            get { return _Template; }
            set { _Template = value; }
        }

        /// <summary>
        /// Private backing member variable for the Bounds property
        /// </summary>
        private IntRectangle _Bounds;

        /// <summary>
        /// The rectangular bounding region within which this room lies on the map
        /// </summary>
        public IntRectangle Bounds
        {
            get { return _Bounds; }
            set { _Bounds = value; }
        }

        /// <summary>
        /// Private backing member variable for the Connections property
        /// </summary>
        private IList<Room> _Connections = new List<Room>();

        /// <summary>
        /// The rooms which connect to this one
        /// </summary>
        public IList<Room> Connections
        {
            get { return _Connections; }
            set { _Connections = value; }
        }


        #endregion

        #region Constructors

        public Room() { }

        public Room(RoomTemplate template, IntRectangle bounds)
        {
            Template = template;
            Bounds = bounds;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is this room connected to the other within a specified number of degrees
        /// of separation
        /// </summary>
        /// <param name="other"></param>
        /// <param name="level">The degrees of separation</param>
        /// <returns></returns>
        public bool IsConnectedTo(Room other, int level = 0)
        {
            if (Connections.Contains(other)) return true;
            else if (level > 0)
            {
                foreach (Room connection in Connections)
                {
                    if (connection.IsConnectedTo(other, level - 1)) return true;
                }
            }
            return false;
        }

        #endregion


    }
}
