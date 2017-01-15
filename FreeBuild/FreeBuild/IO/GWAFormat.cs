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
            Add(typeof(Node), 
                "NODE.2, {[CONTEXT].GetID()}, {Name}, NO_RGB, {Position.X}, {Position.Y}, {Position.Z}");
            Add(typeof(LinearElement), 
                "ELEMENT.2, {[CONTEXT].GetID()}, {Name}, NO_RGB, {[CONTEXT].ElementType()}, {Family.[CONTEXT].GetID()}, {[CONTEXT].ElementGroup()}, {StartNode.[CONTEXT].GetID()}, {EndNode.[CONTEXT].GetID()}, , {Orientation.Degrees}");
            Add(typeof(SectionFamily), 
                "PROP_SEC.1, {[CONTEXT].GetID()}, {Name}, , {[CONTEXT].SectionMaterial()}, {[CONTEXT].SectionDescription()}");
        }

        #endregion
    }
}
