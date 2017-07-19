using Nucleus.Base;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// A set of named data to be displayed in a spider diagram
    /// </summary>
    [Serializable]
    public class DiagramData : Named
    {
        #region Properties

        /// <summary>
        /// Private backing field for Data property
        /// </summary>
        private IDictionary<string, Interval> _Data;

        /// <summary>
        /// The data set
        /// </summary>
        public IDictionary<string, Interval> Data
        {
            get { return _Data; }
            set { ChangeProperty(ref _Data, value, "Data"); }
        }

        /// <summary>
        /// Private backing field for Colour property
        /// </summary>
        private Colour _Colour = Colour.Black;

        /// <summary>
        /// The colour to use to display this dataset
        /// </summary>
        public Colour Colour
        {
            get { return _Colour; }
            set { ChangeProperty(ref _Colour, value, "Colour"); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        internal DiagramData()
        {
            _Data = new Dictionary<string, Interval>();
        }

        /// <summary>
        /// Instantiate a new DiagramData with the specified name and dataset.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public DiagramData(string name, IDictionary<string, Interval> data) : base(name)
        {
            _Data = data;
        }

        /// <summary>
        /// Instantiate a new DiagramData with the specified name and dataset.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public DiagramData(string name, IDictionary<string, Interval> data, Colour colour) : this(name, data)
        {
            _Colour = colour;
        }

        /// <summary>
        /// Instantiate a new DiagramData with the specified name and dataset.
        /// The double data values will be automatically converted to a Interval from
        /// 0 to the specified value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public DiagramData(string name, IDictionary<string, double> data) : base(name)
        {
            _Data = new Dictionary<string, Interval>();
            foreach (KeyValuePair<string, double> kvp in data)
            {
                _Data.Add(kvp.Key, new Interval(0, kvp.Value));
            }
        }

        /// <summary>
        /// Instantiate a new DiagramData with the specified name and dataset.
        /// The double data values will be automatically converted to a Interval from
        /// 0 to the specified value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public DiagramData(string name, IDictionary<string, double> data, Colour colour) : this(name, data)
        {
            _Colour = Colour;
        }

        /// <summary>
        /// Instantiate a new blank DiagramData with the specified name
        /// </summary>
        /// <param name="name"></param>
        public DiagramData(string name) : base(name)
        {
            _Data = new Dictionary<string, Interval>();
        }

        /// <summary>
        /// Instantiate a new blank DiagramData with the specified name
        /// </summary>
        /// <param name="name"></param>
        public DiagramData(string name, Colour colour) : this(name)
        {
            _Colour = colour;
        }

        #endregion
    }
}
