using Nucleus.Base;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    [Serializable]
    public class RoomTemplate : Unique
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Dimension1 property
        /// </summary>
        private IntInterval _Dimension1 = new IntInterval(1, 6);

        /// <summary>
        /// The domain of possible values for the first dimension of this room.
        /// </summary>
        public IntInterval Dimension1
        {
            get { return _Dimension1; }
            set
            {
                _Dimension1 = value;
                NotifyPropertyChanged("Dimension1");
            }
        }

        /// <summary>
        /// Private backing member variable for the Dimension2 property
        /// </summary>
        private IntInterval _Dimension2 = new IntInterval(2, 6);

        /// <summary>
        /// The domain of possible values for the second dimension of this room.
        /// </summary>
        public IntInterval Dimension2
        {
            get { return _Dimension2; }
            set
            {
                _Dimension2 = value;
                NotifyPropertyChanged("Dimension2");
            }
        }

        /// <summary>
        /// Private backing member variable for the EntryWidth property
        /// </summary>
        private int _EntryWidth = 1;

        /// <summary>
        /// The width of the entryway into this type of room
        /// </summary>
        public int EntryWidth
        {
            get { return _EntryWidth; }
            set
            {
                _EntryWidth = value;
                NotifyPropertyChanged("EntryWidth");
            }
        }

        /// <summary>
        /// Private backing member variable for the SproutTries property
        /// </summary>
        private int _SproutTries = 4;

        /// <summary>
        /// The number of attempts that will be made to generate rooms coming off of one of this type
        /// </summary>
        public int SproutTries
        {
            get { return _SproutTries; }
            set
            {
                _SproutTries = value;
                NotifyPropertyChanged("SproutTries");
            }
        }

        /// <summary>
        /// Private backing member variable for the MaxConnections property
        /// </summary>
        private int _MaxConnections = -1;

        /// <summary>
        /// The maximum number of allowed connections to this room type.  When <0, connections are unlimited.
        /// </summary>
        public int MaxConnections
        {
            get { return _MaxConnections; }
            set
            {
                _MaxConnections = value;
                NotifyPropertyChanged("MaxConnections");
            }
        }


        /// <summary>
        /// Private backing member variable for the SymmetryChance property
        /// </summary>
        private double _SymmetryChance = 0.8;

        /// <summary>
        /// The chance of sprouting doorways symmetrically
        /// </summary>
        public double SymmetryChance
        {
            get { return _SymmetryChance; }
            set
            {
                _SymmetryChance = value;
                NotifyPropertyChanged("SymmetryChance");
            }
        }

        /// <summary>
        /// Private backing member variable for the CorridorChance property
        /// </summary>
        private double _CorridorChance = 0.5;

        /// <summary>
        /// The chance of sprouting a corridor (or other circulation space)
        /// </summary>
        public double CorridorChance
        {
            get { return _CorridorChance; }
            set
            {
                _CorridorChance = value;
                NotifyPropertyChanged("CorridorChance");
            }
        }

        /// <summary>
        /// Private backing member variable for the DoorChance property
        /// </summary>
        private double _DoorChance = 0.9;

        /// <summary>
        /// The chance of creating a door between this and connected rooms.  The maximum chance of the two room templates will be used.
        /// </summary>
        public double DoorChance
        {
            get { return _DoorChance; }
            set
            {
                _DoorChance = value;
                NotifyPropertyChanged("DoorChance");
            }
        }

        // TODO:
        // SpawnChance
        // Features
        // ExitPlacement
        // ExtremeExit
        // StandardCellTemplate
        // WallTemplate
        // Slope
        // EntryTemplate
        // SlopeTemplate


        #endregion
    }
}
