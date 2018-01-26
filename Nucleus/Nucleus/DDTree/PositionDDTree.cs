using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.DDTree
{
    /// <summary>
    /// A DDTree adapted to store and access objects with no dimension 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PositionDDTree<T> : DDTree<T>
        where T : IPosition
    {
        public PositionDDTree(IList<T> items, int maxDivisions = 10, double minCellSize = 1) : base(items, maxDivisions, minCellSize)
        {
        }

        public override double DistanceSquaredBetween(Vector pt, T entry)
        {
            return pt.DistanceToSquared(entry.Position);
        }

        public override double MaxXOf(T entry)
        {
            return entry.Position.X;
        }

        public override double MaxYOf(T entry)
        {
            return entry.Position.Y;
        }

        public override double MaxZOf(T entry)
        {
            return entry.Position.Z;
        }

        /*
        public override double MinDistanceSquaredBetween(T entryA, T entryB)
        {
            return entryA.Position.DistanceToSquared(entryB.Position);
        }
        */

        public override double MinXOf(T entry)
        {
            return entry.Position.X;
        }

        public override double MinYOf(T entry)
        {
            return entry.Position.Y;
        }

        public override double MinZOf(T entry)
        {
            return entry.Position.Z;
        }

        public override double PositionInDimension(Dimension dimension, T entry)
        {
            return entry.Position[dimension];
        }
    }
}
