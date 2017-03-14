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
                "NODE\t {[CONTEXT].GetID()}\t{Name}\tNO_RGB\t{Position.X}\t{Position.Y}\t{Position.Z}\tNO_GRID\tGLOBAL\tREST\t{Data.Item[NodeSupport].Fixity.X.[CONTEXT].ToInt()}\t{Data.Item[NodeSupport].Fixity.Y.[CONTEXT].ToInt()}\t{Data.Item[NodeSupport].Fixity.Z.[CONTEXT].ToInt()}\t{Data.Item[NodeSupport].Fixity.XX.[CONTEXT].ToInt()}\t{Data.Item[NodeSupport].Fixity.YY.[CONTEXT].ToInt()}\t{Data.Item[NodeSupport].Fixity.ZZ.[CONTEXT].ToInt()}");
            Add(typeof(LinearElement), 
                "EL\t {[CONTEXT].GetID()}\t{Name}\tNO_RGB\t{[CONTEXT].ElementType()}\t{Family.[CONTEXT].GetID()}\t{[CONTEXT].ElementGroup()}\t{StartNode.[CONTEXT].GetID()}\t{EndNode.[CONTEXT].GetID()}\t\t{Orientation.Degrees}");
            Add(typeof(SectionFamily), 
                "PROP_SEC\t{[CONTEXT].GetID()}\t{Name}\tNO_RGB\t{[CONTEXT].SectionMaterial()}\t{[CONTEXT].SectionDescription()}");
        }

        #endregion
    }
}
