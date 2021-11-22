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
    /// Set-out logic for the generation of features in rooms
    /// </summary>
    [Serializable]
    public class FeatureSetOut
    {
        /// <summary>
        /// The delegate function for creating features
        /// </summary>
        /// <returns></returns>
        public delegate GameElement FeatureCreator();

        /// <summary>
        /// The function delegate used to create the feature
        /// </summary>
        public FeatureCreator Create { get; set; }

        /// <summary>
        /// The range of numbers of features to generate.
        /// If this is an IntInterval, it is given in whole numbers,
        /// if it is an Interval, it is defined as a proportion of the
        /// overall viable area.
        /// </summary>
        public IInterval Number { get; set; } = new Interval(1);

        /// <summary>
        /// The collection of masks which determine possible cell placement
        /// </summary>
        public CellMaskCollection PlacementMasks { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="create"></param>
        /// <param name="number"></param>
        /// <param name="placementMasks"></param>
        public FeatureSetOut(FeatureCreator create, IInterval number, params CellMask[] placementMasks)
        {
            Create = create;
            Number = number;
            PlacementMasks = new CellMaskCollection();
            foreach (var mask in placementMasks)
            {
                PlacementMasks.Add(mask);
            }
        }

        /// <summary>
        /// Randomly determine the number of cells to fill with the feature
        /// </summary>
        /// <typeparam name="TCell"></typeparam>
        /// <param name="rng"></param>
        /// <param name="cells"></param>
        /// <returns></returns>
        public int DetermineNumber<TCell>(Random rng, IList<TCell> cells)
        {
            if (Number is IntInterval intInt) return Math.Min(intInt.Random(rng), cells.Count);
            else if (Number is Interval interval)
            {
                return (int)Math.Round(interval.Random(rng) * cells.Count);
            }
            else throw new NotSupportedException("Interval type not supported");
        }
    }
}
