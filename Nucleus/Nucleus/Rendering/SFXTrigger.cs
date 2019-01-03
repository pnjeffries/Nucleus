using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// A class that is used to trigger a special effect
    /// </summary>
    [Serializable]
    public class SFXTrigger
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the KeyWord property
        /// </summary>
        private string _KeyWord;

        /// <summary>
        /// The keyword which defines the type of special effect to be created
        /// </summary>
        public string KeyWord
        {
            get { return _KeyWord; }
            set { _KeyWord = value; }
        }

        /// <summary>
        /// Private backing member variable for the Position property
        /// </summary>
        private Vector _Position = Vector.Unset;

        /// <summary>
        /// The position at which the effect is to be created, if applicable.
        /// </summary>
        public Vector Position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        /// <summary>
        /// Private backing member variable for the Direction property
        /// </summary>
        private Vector _Direction = Vector.Unset;

        /// <summary>
        /// The direction in/path along which the effect should be orientated/moved, if applicable
        /// </summary>
        public Vector Direction
        {
            get { return _Direction; }
            set { _Direction = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an SFX trigger for the specified keyword
        /// </summary>
        /// <param name="keyword"></param>
        public SFXTrigger(string keyword)
        {
            KeyWord = keyword;
        }

        /// <summary>
        /// Creates an SFX trigger for the specified keyword at the specified position
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="position"></param>
        public SFXTrigger(string keyword, Vector position) : this(keyword)
        {
            Position = position;
        }

        /// <summary>
        /// Creates an SFX trigger for the specified keyword, position and direction
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public SFXTrigger(string keyword, Vector position, Vector direction) : this(keyword, position)
        {
            Direction = direction;
        }

        #endregion
    }
}
