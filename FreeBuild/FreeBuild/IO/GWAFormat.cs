using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.IO
{
    public class GWAFormat : TextFormat
    {
        #region Constructors

        public GWAFormat() : base()
        {
            Add(typeof(Node), "NODE.2, {NumericID}, {Position.X}, {Position.Y}, {Position.Z}");
            Add(typeof(LinearElement), "ELEMENT.2, {NumericID}, {StartNode.NumericID}, {EndNode.NumericID}");
        }

        #endregion
    }
}
