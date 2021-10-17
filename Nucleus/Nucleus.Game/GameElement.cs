using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An element which is specialised towards functioning as an entity
    /// in a game.
    /// </summary>
    public class GameElement : PointElement
    {
        #region Constructors

        public GameElement()
        {
        }

        public GameElement(string name) : base(name)
        {
        }

        public GameElement(string name, params IElementDataComponent[] data) : this(name)
        {
            SetData(data);
        }

        #endregion

        #region Methods

        public override Vector GetNominalPosition()
        {
            MapData mD = GetData<MapData>();
            if (mD != null)
            {
                return mD.Position;
            }
            return base.GetNominalPosition();
        }

        #endregion
    }
}
