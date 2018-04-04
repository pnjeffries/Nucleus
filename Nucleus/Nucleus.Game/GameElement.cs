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
    /// An interactive entity for use in a game or simulation
    /// </summary>
    /// <typeparam name="TFamily"></typeparam>
    public class GameElement : Element<Point, GameFamily>
        //TODO: Genericise family?
    {
        public override Vector GetNominalPosition()
        {
            return Geometry.Position;
        }

        public override void OrientateToVector(Vector vector)
        {
            Orientation = vector.Angle;
        }
    }
}
