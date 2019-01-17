using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;
using Nucleus.Logs;

namespace Nucleus.Game
{
    /// <summary>
    /// Trigger an SFX
    /// </summary>
    [Serializable]
    public class SFXEffect : BasicEffect
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

        /// <summary>
        /// Private backing member variable for the UseTargetPosition property
        /// </summary>
        private bool _UseTargetPosition = false;

        /// <summary>
        /// If true, the position of the target will be taken as the position of the effect
        /// </summary>
        public bool UseTargetPosition
        {
	        get { return _UseTargetPosition; }
            set { _UseTargetPosition = value; }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Trigger a special effect with the specified keyword
        /// </summary>
        /// <param name="keyword"></param>
        public SFXEffect(string keyword)
        {
            KeyWord = keyword;
        }

        /// <summary>
        /// Trigger a special effect with the specified keyword at the specified position
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="position"></param>
        public SFXEffect(string keyword, Vector position) : this(keyword)
        {
            Position = position;
        }

        /// <summary>
        /// Trigger a special effect with the specified keyword at the specified position
        /// in the specified direction
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public SFXEffect(string keyword, Vector position, Vector direction) : this(keyword, position)
        {
            Direction = direction;
        }

        /// <summary>
        /// Trigger a special effect with the specified keyword at (optionally)
        /// the position of the target
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="useTargetPosition"></param>
        public SFXEffect(string keyword, bool useTargetPosition) : this(keyword)
        {
            UseTargetPosition = useTargetPosition;
        }

        /// <summary>
        /// Trigger a special effect with the specified keyword at (optionally)
        /// the position of the target
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="useTargetPosition"></param>
        /// <param name="direction"></param>
        public SFXEffect(string keyword, bool useTargetPosition, Vector direction) : this(keyword, useTargetPosition)
        {
            Direction = direction;
        }

        #endregion

        #region Methods

        public override bool Apply(IActionLog log, EffectContext context)
        {
            if (UseTargetPosition)
            {
                context.SFX.Trigger(KeyWord, context.Target?.GetData<MapData>()?.Position ?? Vector.Unset, Direction);
            }
            else context.SFX.Trigger(KeyWord, Position, Direction);
            return true;
        }

        #endregion
    }
}
