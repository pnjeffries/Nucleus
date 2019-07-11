using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// A colour gradient that smoothly varies between colours keyed
    /// at different double parameters
    /// </summary>
    [Serializable]
    public class ColourGradient : LinearDataSet<Colour>
    {
        #region Constants

        /// <summary>
        /// A linear gradient from red to green
        /// </summary>
        public static ColourGradient RedToGreen = new ColourGradient(Colour.Red, Colour.Green);

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new blank ColourGradient
        /// </summary>
        public ColourGradient() : base() { }

        /// <summary>
        /// Initialise a new data set containing the specified values.
        /// Each value will be plotted against it's index in the list
        /// </summary>
        /// <param name="values"></param>
        public ColourGradient(IList<Colour> values) : base(values) { }

        /// <summary>
        /// Initialise a new data set containing the specified values.
        /// Each value will be plotted against it's index in the list
        /// </summary>
        /// <param name="values"></param>
        public ColourGradient(params Colour[] values) : this((IList<Colour>)values) { }

        /// <summary>
        /// Initialise a new data set containing the specified initial pairing
        /// of values
        /// </summary>
        /// <param name="firstKey"></param>
        /// <param name="firstValue"></param>
        public ColourGradient(double firstKey, Colour firstValue) : this()
        {
            Add(firstKey, firstValue);
        }

        /// <summary>
        /// Initialise a new data set containing the specified constant value between
        /// 0 and 1.0.
        /// </summary>
        /// <param name="constantValue"></param>
        public ColourGradient(Colour constantValue) : this(0, constantValue, 1, constantValue) { }

        /// <summary>
        /// Initialise a new data set containing the two specified initial pairings
        /// </summary>
        /// <param name="firstKey"></param>
        /// <param name="firstValue"></param>
        /// <param name="secondKey"></param>
        /// <param name="secondValue"></param>
        public ColourGradient(double firstKey, Colour firstValue, double secondKey, Colour secondValue)
            : this(firstKey, firstValue)
        {
            Add(secondKey, secondValue);
        }

        /// <summary>
        /// Initialise a new data set containing the two specified initial pairings
        /// </summary>
        /// <param name="firstKey"></param>
        /// <param name="firstValue"></param>
        /// <param name="secondKey"></param>
        /// <param name="secondValue"></param>
        public ColourGradient(double firstKey, Colour firstValue, double secondKey, Colour secondValue, double thirdKey, Colour thirdValue)
            : this(firstKey, firstValue)
        {
            Add(secondKey, secondValue);
            Add(thirdKey, thirdValue);
        }

        #endregion

        #region Methods

        protected override Colour Interpolate(Colour v0, Colour v1, double u)
        {
            return Interpolation.Linear.Interpolate(v0, v1, u);
        }

        #endregion
    }
}
