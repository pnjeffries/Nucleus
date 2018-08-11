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
        public SquareCellMap<CellGenerationType> _Blueprint;

        #endregion

        #region Constructors

        public DungeonArtitect() { }

        #endregion

        #region Methods

        /// <summary>
        /// Check whether the specified cell is available for assignment
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        protected bool IsCellAvailable(int i, int j)
        {
            return _Blueprint[i, j] == CellGenerationType.Untouched;
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
                    if (_Blueprint[i, j] != type) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Set a block of cells to the specified type
        /// </summary>
        /// <param name="iMin"></param>
        /// <param name="iMax"></param>
        /// <param name="jMin"></param>
        /// <param name="jMax"></param>
        /// <param name="genType"></param>
        public void SetBlock(int iMin, int iMax, int jMin, int jMax, CellGenerationType genType)
        {
            for (int i = iMin; i <= iMax; i++)
            {
                for (int j = jMin; j <= jMax; j++)
                {
                    _Blueprint[i, j] = genType;
                }
            }
        }

        public bool RecursiveGrowth(int iDoor, int jDoor, RoomTemplate template, CompassDirection direction, bool createDoor, double floorLevel, Room parent)
        {
            bool result = false;
            int doorSize = template.EntryWidth;
            if (!createDoor || AvailableForDoorway(iDoor, jDoor, direction, doorSize))
            {
                GrowRoom(iDoor, jDoor, template, direction, createDoor, floorLevel, parent);
            }


            return result;
        }

        public bool GrowRoom(int iDoor, int jDoor, RoomTemplate template, CompassDirection direction, bool createDoor, double floorLevel, Room parent)
        {
            int iMin = iDoor, iMax = iDoor, jMin = jDoor, jMax = jDoor; // Current room bounds
            bool iFrozen = false; // Is growth in the x-direction frozen?
            bool jFrozen = false; // Is growth in the y-direction frozen?
            //Generate target dimensions:
            var dimensions = new IntInterval(
                template.Dimension1.Random(_RNG),template.Dimension2.Random(_RNG));
        }

        #endregion
    }
}
