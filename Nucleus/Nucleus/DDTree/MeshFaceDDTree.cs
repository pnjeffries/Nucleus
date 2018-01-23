using Nucleus.Extensions;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.DDTree
{
    public class MeshFaceDDTree : DDTree<MeshFace>
    {
        #region Constructor

        public MeshFaceDDTree(IList<MeshFace> items, int maxDivisions = 10, double minCellSize = 1) : base(items, maxDivisions, minCellSize)
        {
        }

        #endregion

        #region Methods

        public override double DistanceSquaredBetween(Vector pt, MeshFace entry)
        {
            throw new NotImplementedException();
        }

        public override double MaxXOf(MeshFace entry)
        {
            return entry.MaxDelegateValue(i => i.X);
        }

        public override double MaxYOf(MeshFace entry)
        {
            return entry.MaxDelegateValue(i => i.Y);
        }

        public override double MaxZOf(MeshFace entry)
        {
            return entry.MaxDelegateValue(i => i.Z);
        }

        public override double MinDistanceSquaredBetween(MeshFace entryA, MeshFace entryB)
        {
            throw new NotImplementedException();
        }

        public override double MinXOf(MeshFace entry)
        {
            return entry.MinDelegateValue(i => i.X);
        }

        public override double MinYOf(MeshFace entry)
        {
            return entry.MinDelegateValue(i => i.Y);
        }

        public override double MinZOf(MeshFace entry)
        {
            return entry.MinDelegateValue(i => i.Z);
        }

        public override double PositionInDimension(Dimension dimension, MeshFace entry)
        {
            return entry.AverageDelegateValue(i => i.Position[dimension]);
        }

        #endregion
    }
}
