using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Object that represents a level in a model
    /// </summary>
    [Serializable]
    public class Level : DataOwner<DataStore,object>
    {
        #region Properties

        /// <summary>
        /// Private backing field for the Z property
        /// </summary>
        private double _Z = 0;

        /// <summary>
        /// The z-coordinate of the level
        /// </summary>
        public double Z
        {
            get { return _Z; }
            set { _Z = value; NotifyPropertyChanged("Z"); }
        }

        /// <summary>
        /// The name of this level.
        /// </summary>
        public override string Name
        {
            get
            {
                if (_Name == null)
                {
                    string result = "Level ";
                    result += string.Format("{0:+0.00;-0.00", Z);
                    return result;
                }
                return base.Name;
            }

            set
            {
                base.Name = value;
            }
        }

        #endregion

        /// <summary>
        /// Default constructor.  Initialises a new level situated at Z = 0.
        /// </summary>
        public Level() { }

        /// <summary>
        /// Initialise a new level at the specified Z-level.
        /// </summary>
        /// <param name="z"></param>
        public Level(double z) : this()
        {
            Z = z;
        }

        /// <summary>
        /// Initialise a new level with the specified name at the specified Z-level.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="z"></param>
        public Level(string name, double z) : this(z)
        {
            Name = name;
        }
    }
}
