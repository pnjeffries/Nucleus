using Nucleus.Base;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A template which is used to generate a room
    /// </summary>
    [Serializable]
    public class RoomTemplate : Unique
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the RoomType property
        /// </summary>
        private RoomType _RoomType = RoomType.Room;

        /// <summary>
        /// The type of the room
        /// </summary>
        public RoomType RoomType
        {
            get { return _RoomType; }
            set
            {
                _RoomType = value;
                NotifyPropertyChanged("RoomType");
            }
        }


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
        /// Private backing member variable for the ExitPlacement property
        /// </summary>
        private ExitPlacement _ExitPlacement = ExitPlacement.Any;

        /// <summary>
        /// The logic for the placement of the primary exit from this room
        /// </summary>
        public ExitPlacement ExitPlacement
        {
            get { return _ExitPlacement; }
            set
            {
                _ExitPlacement = value;
                NotifyPropertyChanged("ExitPlacement");
            }
        }


        /// <summary>
        /// Private backing member variable for the SproutTries property
        /// </summary>
        private int _SproutTries = 6;

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

        #region Constructors

        public RoomTemplate() { }

        public RoomTemplate(RoomType roomType, int dim1Min, int dim1Max, int dim2Min, int dim2Max)
        {
            RoomType = roomType;
            Dimension1 = new IntInterval(dim1Min, dim1Max);
            Dimension2 = new IntInterval(dim2Min, dim2Max);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determine the generation type for a cell within a room
        /// with this template
        /// </summary>
        /// <param name="i"></param>
        /// <param name="iMin"></param>
        /// <param name="iMax"></param>
        /// <param name="j"></param>
        /// <param name="jMin"></param>
        /// <param name="jMax"></param>
        /// <returns></returns>
        public virtual CellGenerationType GenTypeForCell(int i, int iMin, int iMax, int j, int jMin, int jMax)
        {
            if ((i == iMin - 1 && (j == jMin - 1 || j == jMax + 1)) ||
                    (i == iMax + 1 && (j == jMin - 1 || j == jMax + 1)))
            {
                return CellGenerationType.WallCorner;
            }
            else if (i < iMin || i > iMax || j < jMin || j > jMax)
            {
                return CellGenerationType.Wall;
            }
            else
            {
                return CellGenerationType.Void;
            }
        }

        #endregion
    }
}
