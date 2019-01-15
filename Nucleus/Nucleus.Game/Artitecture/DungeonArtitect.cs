using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Geometry;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Generator class for dungeons
    /// </summary>
    public class DungeonArtitect
    {
        #region Fields

        /// <summary>
        /// Random number generator
        /// </summary>
        private Random _RNG = new Random();

        #endregion

        #region Properties

        /// <summary>
        /// The blueprint that the generator will work on
        /// </summary>
        private SquareCellMap<BlueprintCell> _Blueprint;

        /// <summary>
        /// The blueprint that the generator will work on
        /// </summary>
        public SquareCellMap<BlueprintCell> Blueprint
        {
            get { return _Blueprint; }
        }

        /// <summary>
        /// Has the level exit yet been placed?
        /// </summary>
        public bool ExitPlaced { get; set; } = false;

        /// <summary>
        /// The collection of rooms created during the generation
        /// </summary>
        public RoomCollection Rooms { get; } = new RoomCollection();

        /// <summary>
        /// The collection of templates to be used to generate rooms
        /// </summary>
        public RoomTemplateCollection Templates { get; } = new RoomTemplateCollection();

        /// <summary>
        /// A collection of 'snapshot' images of the dungeon being generated.
        /// By default this is null and no snapshots will be recorded.  If populated
        /// with a suitable container snapshots will be taken as each room is added
        /// </summary>
        public IList<SquareCellMap<CellGenerationType>> Snapshots { get; set; } = null;

        /// <summary>
        /// The number of rooms typically found between the entry and exit
        /// </summary>
        public int PathToExit { get; set; } = 8;

        /// <summary>
        /// If set to true, parallel corridors will be detected and prevented
        /// </summary>
        public bool PreventParallelCorridors { get; set; } = false;

        #endregion

        #region Constructors

        private DungeonArtitect() { }

        /// <summary>
        /// Creates a new dungeon artitect to work within the specified bounds
        /// </summary>
        /// <param name="iSize"></param>
        /// <param name="jSize"></param>
        public DungeonArtitect(int iSize, int jSize)
        {
            _Blueprint = new SquareCellMap<BlueprintCell>(iSize, jSize);
            for (int i = 0; i < iSize; i++)
                for (int j = 0; j < jSize; j++)
                    _Blueprint[i, j] = new BlueprintCell();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Take a snapshot of the current state of the blueprint and add it to the snapshots collection
        /// </summary>
        public void TakeSnapshot()
        {
            if (Snapshots != null)
            {
                var snap = new SquareCellMap<CellGenerationType>(_Blueprint.SizeX, _Blueprint.SizeY);
                for(int i = 0; i < Blueprint.CellCount; i++)
                {
                    snap[i] = Blueprint[i].GenerationType;
                }
                Snapshots.Add(snap);
            }
        }

        /// <summary>
        /// Generate a dungeon map
        /// </summary>
        /// <param name="iStart"></param>
        /// <param name="jStart"></param>
        /// <param name="startRoom">The room to start with</param>
        /// <param name="startDirection">The initial direction</param>
        /// <returns></returns>
        public bool Generate(int iStart, int jStart, RoomTemplate startRoom, CompassDirection startDirection)
        {
            //TODO: Generate negative space
            return RecursiveGrowth(iStart, jStart, startRoom, startDirection, false, null);
        }

        /// <summary>
        /// Check whether the specified cell is available for assignment
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        protected bool IsCellAvailable(int i, int j, int edgeOffset = 0)
        {
            if (i < edgeOffset || i >= _Blueprint.SizeX - edgeOffset ||
                j < edgeOffset || j >= _Blueprint.SizeY - edgeOffset) return false;
           else return _Blueprint[i, j].GenerationType == CellGenerationType.Untouched;
        }



        /// <summary>
        /// Can a door be placed here?
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="direction"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        protected bool AvailableForDoorway(int i, int j, CompassDirection direction, int width)
        {
            if (direction == CompassDirection.North || direction == CompassDirection.South)
            {
                return AreAllType(i, i + width, j, j, CellGenerationType.Wall);
            }
            else
            {
                return AreAllType(i, i, j, j + width, CellGenerationType.Wall);
            }
        }

        /// <summary>
        /// Are all cells within the specified rectangular region of the specified
        /// type?
        /// </summary>
        protected bool AreAllType(IntRectangle bounds, CellGenerationType type)
        {
            return AreAllType(bounds.XMin, bounds.XMax, bounds.YMin, bounds.YMax, type);
        }

        /// <summary>
        /// Are all cells within the specified rectangular region of the specified
        /// type?
        /// </summary>
        /// <param name="iMin"></param>
        /// <param name="iMax"></param>
        /// <param name="jMin"></param>
        /// <param name="jMax"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected bool AreAllType(int iMin, int iMax, int jMin, int jMax, CellGenerationType type)
        {
            for (int i = iMin; i <= iMax; i++)
            {
                for (int j = jMin; j <= jMax; j++)
                {
                    if (i < 0 || i >= _Blueprint.SizeX ||
                        j < 0 || j >= _Blueprint.SizeY ||
                        _Blueprint[i, j].GenerationType != type) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Are the following range of cells all available?
        /// </summary>
        /// <param name="iMin"></param>
        /// <param name="iMax"></param>
        /// <param name="jMin"></param>
        /// <param name="jMax"></param>
        /// <returns></returns>
        protected bool CheckAvailability(int iMin, int iMax, int jMin, int jMax)
        {
            for (int i = iMin; i <= iMax; i++)
            {
                for (int j = jMin; j <= jMax; j++)
                {
                    if (!IsCellAvailable(i, j, 1)) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Are the selected range of cells all available?
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool CheckAvailability(IntRectangle rect)
        {
            return CheckAvailability(rect.XMin, rect.XMax, rect.YMin, rect.YMax);
        }

        /// <summary>
        /// Count up the number of cells belonging to each room in the specified zone
        /// </summary>
        /// <param name="iMin"></param>
        /// <param name="iMax"></param>
        /// <param name="jMin"></param>
        /// <param name="jMax"></param>
        /// <returns></returns>
        public Dictionary<Room, int> CountRoomCells(int iMin, int iMax, int jMin, int jMax)
        {
            iMin = Math.Max(iMin, 0);
            jMin = Math.Max(jMin, 0);
            iMax = Math.Min(iMax, Blueprint.SizeX - 1);
            jMax = Math.Min(jMax, Blueprint.SizeY - 1);
            var result = new Dictionary<Room, int>();
            for (int i = iMin; i <= iMax; i++)
            {
                for (int j = jMin; j <= jMax; j++)
                {
                    Room room = Blueprint[i, j].Room;
                    if (room != null)
                    {
                        if (result.ContainsKey(room)) result[room] += 1;
                        else result.Add(room, 1);
                    }
                }
            }
            return result;
        }

       /// <summary>
       /// Check whether there is a corridor running parallel to this region
       /// </summary>
       /// <param name="iMin"></param>
       /// <param name="iMax"></param>
       /// <param name="jMin"></param>
       /// <param name="jMax"></param>
       /// <returns></returns>
        public bool IsParallelToCorridor(int iMin, int iMax, int jMin, int jMax)
        {
            var roomCounts = CountRoomCells(iMin - 1, iMax + 1, jMin - 1, jMax + 1);
            foreach (var kvp in roomCounts)
            {
                if (kvp.Key.Template.RoomType == RoomType.Circulation && kvp.Value > 3)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check whether there is a corridor running parallel to this region
        /// </summary>
        public bool IsParallelToCorridor(IntRectangle bounds)
        {
            return IsParallelToCorridor(bounds.XMin, bounds.XMax, bounds.YMin, bounds.YMax);
        }

        /// <summary>
        /// Generate a (possibly multi-tile) doorway
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="direction"></param>
        /// <param name="template"></param>
        /// <param name="template2"></param>
        /// <param name="floorLevel"></param>
        protected void GenerateDoorway(int i, int j, CompassDirection direction, int width)
        {
            for (int n = -1; n < width + 1; n++)
            {
                int iA = i;
                int jA = j;

                if (direction == CompassDirection.North ||
                        direction == CompassDirection.South)
                    iA += n;
                else jA += n;

                if (n < 0 || n > width)
                {
                    SetCell(i, j, CellGenerationType.WallCorner);
                }
                else
                {
                    SetCell(i, j, CellGenerationType.Door);
                }
            }
        }

        /// <summary>
        /// Get the template of the next room to be placed
        /// </summary>
        /// <param name="currentRoom"></param>
        /// <returns></returns>
        public RoomTemplate NextRoom(RoomTemplate currentRoom)
        {
            return Templates.GetRandom(_RNG);
            //TODO: Make work OK!
            RoomType type = RoomType.Room;
            if (Rooms.Count > PathToExit && !ExitPlaced)
            {
                type = RoomType.Exit;
            }
            else if ((!ExitPlaced && _RNG.NextDouble() * _RNG.NextDouble() * _RNG.NextDouble() < currentRoom.CorridorChance) ||
                _RNG.NextDouble() < currentRoom.CorridorChance)
            {
                type = RoomType.Circulation;
            }
            return Templates.GetAllOfType(type).GetRandom(_RNG);
        }

        
        /// <summary>
        /// 'Delete' a cell in the blueprint by returning it to 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="room"></param>
        protected void DeleteCell(int i, int j, Room room)
        {
            if (Blueprint.Exists(i,j))
            {
                BlueprintCell cell = Blueprint[i, j];
                if (cell.Room == room)
                {
                    cell.Room = null;
                    cell.GenerationType = CellGenerationType.Untouched;
                }
            }
        }
     
        /// <summary>
        /// Delete an existing room
        /// </summary>
        /// <param name="room"></param>
        protected void DeleteRoom(Room room)
        {
            Rooms.Remove(room);
            foreach (Room other in Rooms)
            {
                other.Connections.Remove(room);
            }
            for (int i = room.Bounds.XMin - 1; i <= room.Bounds.XMax + 1; i++)
            {
                for (int j = room.Bounds.YMin - 1; j <= room.Bounds.YMax + 1; j++)
                {
                    DeleteCell(i, j, room);
                }
            }
        }
        

        /// <summary>
        /// Generate a rectangular room
        /// </summary>
        /// <param name="iMin"></param>
        /// <param name="iMax"></param>
        /// <param name="jMin"></param>
        /// <param name="jMax"></param>
        /// <param name="template"></param>
        /// <param name="floorLevel"></param>
        /// <param name="slopeDirection"></param>
        /// <returns></returns>
        protected Room GenerateRoom(IntRectangle bounds, RoomTemplate template)
        {
            return GenerateRoom(bounds.XMin, bounds.XMax, bounds.YMin, bounds.YMax, template);
        }

        /// <summary>
        /// Generate a rectangular room
        /// </summary>
        /// <param name="iMin"></param>
        /// <param name="iMax"></param>
        /// <param name="jMin"></param>
        /// <param name="jMax"></param>
        /// <param name="template"></param>
        /// <param name="floorLevel"></param>
        /// <param name="slopeDirection"></param>
        /// <returns></returns>
        protected Room GenerateRoom(int iMin, int iMax, int jMin, int jMax, RoomTemplate template)
        {
            Room newRoom = new Room(template, new IntRectangle(iMin, iMax,jMin, jMax));
            for (int i = iMin - 1; i <= iMax + 1; i++)
            {
                for (int j = jMin - 1; j <= jMax + 1; j++)
                {
                    if (IsCellAvailable(i, j))
                    {
                        //Room interior
                        //MapCellTemplate cellTemplate = template.templateForCell(i, iMin, iMax, j, jMin, jMax);
                        CellGenerationType cellType = template.GenTypeForCell(i, iMin, iMax, j, jMin, jMax);
                        SetCell(i, j, cellType, newRoom);
                        //setCellTemplate(i, j, cellTemplate, cellType, cellFloor);
                        //assignCellRoom(i, j, newRoom);
                    }
                }
            }
            return newRoom;
        }

        /// <summary>
        /// Set a cell of the blueprint to the specified type
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="genType"></param>
        public void SetCell(int i, int j, CellGenerationType genType, Room room = null)
        {
            _Blueprint[i, j].GenerationType = genType;
            _Blueprint[i, j].Room = room;
        }

        
        /// <summary>
        /// Determine an exit direction based on the current placement logic
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="startDir"></param>
        /// <returns></returns>
        public CompassDirection ExitDirection(ExitPlacement logic, CompassDirection startDir)
        {
            if (logic == ExitPlacement.Opposite || logic == ExitPlacement.Opposite_Side) return startDir;
            else if (logic == ExitPlacement.Not_Opposite_Side) return startDir.RandomTurn(_RNG);
            else return startDir.Reverse().RandomOther(_RNG);
        }

        /// <summary>
        /// Generate a dungeon map via recursive growth
        /// </summary>
        /// <param name="iDoor"></param>
        /// <param name="jDoor"></param>
        /// <param name="template"></param>
        /// <param name="direction"></param>
        /// <param name="createDoor"></param>
        /// <param name="floorLevel"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool RecursiveGrowth(int iDoor, int jDoor, RoomTemplate template, CompassDirection direction, bool createDoor, Room parent)
        {
            bool result = false;
            int doorSize = template.EntryWidth;
            if (!createDoor || AvailableForDoorway(iDoor, jDoor, direction, doorSize))
            {
                result = GrowRoom(iDoor, jDoor, template, direction, createDoor, parent);
            }


            return result;
        }

        public bool GrowRoom(int iDoor, int jDoor, RoomTemplate template, CompassDirection direction, bool createDoor, Room parent)
        {
            bool result = false;
            int doorSize = template.EntryWidth;
            var bounds = new IntRectangle(iDoor, jDoor); // Current room bounds
            bool iFrozen = false; // Is growth in the x-direction frozen?
            bool jFrozen = false; // Is growth in the y-direction frozen?
            //Generate target dimensions:
            var dimensions = new IntInterval(
                template.Dimension1.Random(_RNG),template.Dimension2.Random(_RNG));

            //Create starting tiles in front of doorway:
            bounds.Move(direction, 1);
            if (direction == CompassDirection.North || direction == CompassDirection.South)
                bounds.Grow(CompassDirection.East, doorSize - 1);
            else
                bounds.Grow(CompassDirection.North, doorSize - 1);

            if (CheckAvailability(bounds))
            {
                // Grow outwards, checking availability:
                CompassDirection growDirection = direction;
                int persistance = 5; //Failures before abort
                while (persistance > 0)
                {
                    if (!(iFrozen && growDirection.IsHorizontal()) &&
                        !(jFrozen && growDirection.IsVertical()))
                    {
                        if (CheckAvailability(bounds.GrowZone(growDirection, 1)))
                            bounds.Grow(growDirection, 1);
                        else persistance--;
                    }

                    if (bounds.XSize + 1 >= dimensions.Max) iFrozen = true;
                    if (bounds.XSize + 1 > dimensions.Min && bounds.YSize + 1 == dimensions.Min) jFrozen = true;

                    if (bounds.YSize + 1 >= dimensions.Max) jFrozen = true;
                    if (bounds.YSize + 1 > dimensions.Min && bounds.XSize + 1 == dimensions.Min) iFrozen = true;

                    if (jFrozen && iFrozen) persistance = 0;
                    else
                    {
                        growDirection = ExitDirection(template.ExitPlacement, direction); //TODO: ExitPlacement from RoomTemplate?
                    }
                }

                if (template.RoomType == RoomType.Circulation && 
                    (!PreventParallelCorridors || IsParallelToCorridor(bounds)))
                    return false; //Abort parallel corridors

                if ((bounds.XSize + 1 >= template.Dimension1.Min && bounds.YSize + 1 >= template.Dimension2.Min ||
                        (bounds.XSize + 1 >= template.Dimension2.Min && bounds.YSize + 1 >= template.Dimension1.Min)))
                {
                    // Reached target size - create room

                    // Create doorway:
                    if (createDoor) GenerateDoorway(iDoor, jDoor, direction, doorSize);

                    // Create room:
                    Room newRoom = GenerateRoom(bounds, template);

                    //Take snapshot:
                    TakeSnapshot();

                    if (template.RoomType == RoomType.Exit) ExitPlaced = true;

                    //TODO: Store room & connections
                    if (template.RoomType != RoomType.Circulation) result = true; //Circulation rooms cannot validate chains

                    //Sprout new rooms:

                    int tries = template.SproutTries;
                    CompassDirection doorDirection = direction;
                    int iNewDoor = 0;
                    int jNewDoor = 0;
                    bool symmetryLock = true;
                    while (tries > 0 &&
                            (template.MaxConnections < 0 ||
                                    newRoom.Connections.Count < template.MaxConnections))
                    {
                        RoomTemplate nextRoom = NextRoom(template);

                        if (nextRoom != null)
                        {
                            int newDoorWidth = nextRoom.EntryWidth;

                            if (!symmetryLock && _RNG.NextDouble() < template.SymmetryChance)
                            {
                                symmetryLock = true;
                                //Create door opposite last
                                doorDirection = doorDirection.Reverse();
                            }
                            else
                            {
                                symmetryLock = false;
                                //Select random growth direction:
                                doorDirection = ExitDirection(template.ExitPlacement, direction);
                            }

                            //Select door position for this try:
                            bounds.RandomPointOnEdge(doorDirection, _RNG, newDoorWidth - 1, ref iNewDoor, ref jNewDoor);

                            if (RecursiveGrowth(iNewDoor, jNewDoor, nextRoom, doorDirection, true, newRoom)) result = true;
                        }
                        tries -= 1;
                    }

                    if (result == false)
                    {
                        DeleteRoom(newRoom);
                    }
                }   
            }
            else
            {
                if (createDoor && AreAllType(bounds,CellGenerationType.Void) &&
                    AvailableForDoorway(iDoor,jDoor, direction, template.EntryWidth))
                {
                    // TODO: Check room connections
                    GenerateDoorway(iDoor, jDoor, direction, template.EntryWidth);    
                }
            }
                       
            return result;
        }

        #endregion
    }
}
