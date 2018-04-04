using Nucleus.Alerts;
using Nucleus.Geometry;
using Nucleus.Model;
using Nucleus.Model.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFE = BriefFiniteElementNet;

namespace Nucleus.BriefFE
{
    /// <summary>
    /// A manager class to analyse a model using BriefFE
    /// </summary>
    public class BriefFEClient
    {
        public CurveCollection AnalyseModel(Model.Model model, AlertLog alertLog = null)
        {
            // Create model:
            var feModel = new BFE.Model();

            // Create nodes:
            var nodesMap = new Dictionary<Guid, BFE.Node>();
            foreach (var node in model.Nodes)
            {
                var bfeNode = ToBFE.Convert(node);
                feModel.Nodes.Add(bfeNode);
                nodesMap.Add(node.GUID, bfeNode);
            }

            // Create materials:
            /*var materialsMap = new Dictionary<Guid, BFE.Materials.BaseMaterial>();
            foreach (var material in model.Materials)
            {
                var bfeMat = ToBFE.Convert(material);
                materialsMap.Add(material.GUID, bfeMat);
            }*/

            // Create sections:
            /*foreach (var family in model.Families)
            {
                if (family is SectionFamily)
                {
                    SectionFamily section = (SectionFamily)family;
                    //TODO?
                }
            }*/

            // Create elements:
            var elementMap = new Dictionary<Guid, BFE.FrameElement2Node>();
            foreach (var element in model.Elements)
            {
                if (element is LinearElement)
                {
                    var linEl = (LinearElement)element;
                    if (linEl.StartNode != null && linEl.EndNode != null && linEl.Family != null)
                    {
                        var el = new BFE.FrameElement2Node(
                            nodesMap[linEl.StartNode.GUID], nodesMap[linEl.EndNode.GUID]);
                        //TODO: Releases
                        //TODO: Orientation
                        //TODO: Offsets
                        if (linEl.Family.Profile?.Perimeter != null)
                        {
                            var profile = ToBFE.Convert(linEl.Family.Profile.Perimeter);
                            el.Geometry = profile;
                            el.UseOverridedProperties = false;
                            //TODO: Hollow sections
                        }
                        el.E = linEl.Family.GetPrimaryMaterial()?.GetE(Geometry.Direction.X) ?? 210e9;
                        feModel.Elements.Add(el);
                        elementMap.Add(element.GUID, el);
                    }
                    else
                        alertLog.RaiseAlert("INCOMPLETE DATA", linEl, "Incomplete data.  Will be excluded from analysis.");
                }
            }

            //Loading time!
            // TODO: Limit to specific load cases?
            foreach (var load in model.Loads)
            {
                if (load is LinearElementLoad)
                {
                    var lEL = (LinearElementLoad)load;
                    if (lEL.IsMoment) alertLog.RaiseAlert("MOMENTS UNSUPPORTED", load, "Moment line loads are not supported.");
                    else
                    {
                        // TODO: Set load case
                        var bLoad = new BFE.UniformLoad1D(lEL.Value, ToBFE.Convert(lEL.Direction), ToBFE.Convert(lEL.Axes));

                        // TODO: Generalise
                        var elements = lEL.AppliedTo.Items;
                        foreach (var el in elements)
                        {
                            elementMap[el.GUID].Loads.Add(bLoad);
                        }
                    }
                }
                else alertLog.RaiseAlert("LOAD TYPE UNSUPPORTED", load, "Load type is not supported.");
            }

            feModel.Solve();

            foreach (var kvp in nodesMap)
            {
                kvp.Value.GetNodalDisplacement();
            }

            /*foreach (var element in model.Elements)
            {
                bNS = nodesMap[element]
            }*/
            return null;
        }
    }
}
