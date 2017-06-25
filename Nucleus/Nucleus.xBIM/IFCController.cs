using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBuild.Model;
using Xbim.Ifc;
using FreeBuild.Base;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc2x3.Extensions;
using Xbim.Geometry;

namespace FreeBuild.xBIM
{
    public class IFCController
    {
        public void Convert(Model.Model model)
        {
            //IfcStore store = IfcStore.;

        }

        /// <summary>
        /// Load a FreeBuild model from an IFC file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Model.Model Load(FilePath filePath)
        {
            Model.Model result = new Model.Model();
            using (IfcStore store = IfcStore.Open(filePath))
            {
                var beams = store.Instances.OfType<IIfcBeam>();
                foreach (IIfcBeam beam in beams)
                {
                    var representation = beam.Representation;
                    //beam.GetBodyRepresentation();
                    var shapeRep = beam.Representation.Representations.OfType<IIfcShapeRepresentation>().Where(
                        r => string.Compare(r.RepresentationIdentifier.GetValueOrDefault(), "Body", true) == 0).FirstOrDefault();
                    
                    //TODO
                }
            }
        }
    }
}
