using Nucleus.Conversion;
using Nucleus.Model;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Nucleus.Base;

namespace Nucleus.Robot
{
    /// <summary>
    /// A mapping table from Nucleus model objects to Robot objects
    /// </summary>
    [Serializable]
    public class RobotIDMappingTable : IDMappingTable<Guid, string>
    {
        #region Properties

        /// <summary>
        /// The name of the category under which nodes are stored
        /// </summary>
        public string NodeCategory { get { return "Nodes"; } }

        /// <summary>
        /// The name of the category under which bars are stored
        /// </summary>
        public string BarCategory { get { return "Bars"; } }

        /// <summary>
        /// The name of the category under which panels are stored
        /// </summary>
        public string PanelCategory { get { return "Panel"; } }

        /// <summary>
        /// The name of the category under which section properties are stored
        /// </summary>
        public string SectionCategory { get { return "Sections"; } }

        /// <summary>
        /// The name of the category under which thickness properties are stored
        /// </summary>
        public string ThicknessCategory { get { return "Thickness"; } }

        /// <summary>
        /// The name of the category under which sets are stored
        /// </summary>
        public string SetsCategory { get { return "Sets"; } }

        /// <summary>
        /// The name of the category under which load cases are stored
        /// </summary>
        public string CaseCategory { get { return "Cases"; } }

        /// <summary>
        /// The name of the category under which loads are stored
        /// </summary>
        public string LoadCategory { get { return "Loads"; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mappedIDName">The name of the first ID set</param>
        public RobotIDMappingTable(string mappedIDName = "Nucleus") : base(mappedIDName, "Robot")
        {

        }

        protected RobotIDMappingTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the Nucleus node, if any, mapped to the specified robot node ID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Node GetMappedModelNode(int robotID, Model.Model model)
        {
            if (HasFirstID(NodeCategory, robotID.ToString())) return model.Nodes.TryGet(GetFirstID(NodeCategory, robotID.ToString()));
            return null;
        }

        /// <summary>
        /// Get the Nucleus node, if any, mapped to the specified robot node ID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Node GetMappedModelNode(string robotID, Model.Model model)
        {
            if (HasFirstID(NodeCategory, robotID)) return model.Nodes.TryGet(GetFirstID(NodeCategory, robotID));
            return null;
        }

        /// <summary>
        /// Get the Nucleus node, if any, mapped to the ID number of the specified robot node
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Node GetMappedModelNode(IRobotNode node, Model.Model model)
        {
            return GetMappedModelNode(node.Number, model);
        }

        /// <summary>
        /// Get the Nucleus element, if any, mapped to the specified robot bar ID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public LinearElement GetMappedLinearElement(int robotID, Model.Model model)
        {
            //TODO: Get for sub-elements (when introduced)
            if (HasFirstID(BarCategory, robotID.ToString()))
                return model.Elements.TryGet(GetFirstID(BarCategory, robotID.ToString())) as LinearElement;
            else return null;
        }

        /// <summary>
        /// Get the Nucleus element, if any, mapped to the specified robot bar ID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public LinearElement GetMappedLinearElement(string robotID, Model.Model model)
        {
            //TODO: Get for sub-elements (when introduced)
            if (HasFirstID(BarCategory, robotID))
                return model.Elements.TryGet(GetFirstID(BarCategory, robotID)) as LinearElement;
            else return null;
        }

        /// <summary>
        /// Get the Nucleus element, if any, mapped to the specified robot panel ID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public PanelElement GetMappedPanelElement(int robotID, Model.Model model)
        {
            if (HasFirstID(PanelCategory, robotID.ToString()))
                return model.Elements.TryGet(GetFirstID(PanelCategory, robotID.ToString())) as PanelElement;
            else return null;
        }

        /// <summary>
        /// Get the Nucleus element, if any, mapped to the specified robot panel ID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public PanelElement GetMappedPanelElement(string robotID, Model.Model model)
        {
            if (HasFirstID(PanelCategory, robotID))
                return model.Elements.TryGet(GetFirstID(PanelCategory, robotID)) as PanelElement;
            else return null;
        }

        /// <summary>
        /// Get the Nucleus element, if any, mapped to the ID of the specified robot bar
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public LinearElement GetMappedLinearElement(IRobotBar bar, Model.Model model)
        {
            return GetMappedLinearElement(bar.Number, model);
        }

        /// <summary>
        /// Get the Nucleus element, if any, mapped to the ID of the specified robot object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public PanelElement GetMappedPanelElement(RobotObjObject obj, Model.Model model)
        {
            return GetMappedPanelElement(obj.Number, model);
        }

        /// <summary>
        /// Get the Nucleus section family, if any, mapped to the specified robot section label ID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public SectionFamily GetMappedSectionFamily(string robotID, Model.Model model)
        {
            if (HasFirstID(SectionCategory, robotID)) return model.Families.TryGet(GetFirstID(SectionCategory, robotID)) as SectionFamily;
            return null;
        }

        /// <summary>
        /// Get the Nucleus section property, if any, mapped to the ID of the specified label
        /// </summary>
        /// <param name="label"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public SectionFamily GetMappedSectionFamily(IRobotLabel label, Model.Model model)
        {
            return GetMappedSectionFamily(label.Name, model);
        }

        /// <summary>
        /// Get the Nucleus panel family, if any, mapped to the specified robot thickness label ID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BuildUpFamily GetMappedPanelFamily(string robotID, Model.Model model)
        {
            if (HasFirstID(ThicknessCategory, robotID)) return model.Families.TryGet(GetFirstID(ThicknessCategory, robotID)) as BuildUpFamily;
            return null;
        }

        /// <summary>
        /// Get the Nucleus panel family, if any, mapped to the specified robot thickness label
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BuildUpFamily GetMappedPanelFamily(IRobotLabel label, Model.Model model)
        {
            return GetMappedPanelFamily(label.Name, model);
        }

        /// <summary>
        /// Get the Nucleus load case, if any, mapped to the specified robotID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public LoadCase GetMappedLoadCase(string robotID, Model.Model model)
        {
            if (HasFirstID(CaseCategory, robotID)) return model.LoadCases.TryGet(GetFirstID(CaseCategory, robotID)) as LoadCase;
            return null;
        }

        /// <summary>
        /// Get the Nucleus load, if any, mapped to the specified robotID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Load GetMappedLoad(string robotID, Model.Model model)
        {
            if (HasFirstID(CaseCategory, robotID)) return model.Loads.TryGet(GetFirstID(LoadCategory, robotID)) as Load;
            return null;
        }

        /// <summary>
        /// Get the Nucleus set, if any, mapped to the specified robotID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ModelObjectSetBase GetMappedSet(string robotID, Model.Model model)
        {
            if (HasFirstID(SetsCategory, robotID)) return model.Sets.TryGet(GetFirstID(SetsCategory, robotID)) as ModelObjectSetBase;
            return null;
        }

        public ModelObject GetMapped(IRobotObjectType type, string robotID, Model.Model model)
        {
            if (type == IRobotObjectType.I_OT_NODE) return GetMappedModelNode(robotID, model);
            else if (type == IRobotObjectType.I_OT_PANEL) return GetMappedPanelElement(robotID, model);
            else return GetMappedLinearElement(robotID, model);
            //TODO: Other types
        }

        /// <summary>
        /// Add a new Node entry to this mapping table
        /// </summary>
        /// <param name="fbNode"></param>
        /// <param name="rNode"></param>
        public void Add(Node fbNode, IRobotNode rNode)
        {
            Add(NodeCategory, fbNode.GUID, rNode.Number.ToString());
        }

        /// <summary>
        /// Add a new Bar entry to this mapping table
        /// </summary>
        /// <param name="element"></param>
        /// <param name="bar"></param>
        public void Add(LinearElement element, IRobotBar bar)
        {
            Add(BarCategory, element.GUID, bar.Number.ToString());
        }

        /// <summary>
        /// Add a new Panel entry to this mapping table
        /// </summary>
        /// <param name="element"></param>
        /// <param name="obj"></param>
        public void Add(PanelElement element, IRobotObjObject obj)
        {
            Add(PanelCategory, element.GUID, obj.Number.ToString());
        }

        /// <summary>
        /// Add a new Section entry to this mapping table
        /// </summary>
        /// <param name="section"></param>
        /// <param name="label"></param>
        public void Add(SectionFamily section, IRobotLabel label)
        {
            Add(SectionCategory, section.GUID, label.Name);
        }

        /// <summary>
        /// Add a new Panel Family entry to this mapping table
        /// </summary>
        /// <param name="family"></param>
        /// <param name="label"></param>
        public void Add(BuildUpFamily family, IRobotLabel label)
        {
            Add(ThicknessCategory, family.GUID, label.Name);
        }

        /// <summary>
        /// Add a new Load Case entry to this mapping table
        /// </summary>
        /// <param name="loadCase"></param>
        /// <param name="rCase"></param>
        public void Add(LoadCase loadCase, IRobotCase rCase)
        {
            Add(CaseCategory, loadCase.GUID, rCase.Number.ToString());
        }

        /// <summary>
        /// Add a new Load entry to this mapping table
        /// </summary>
        /// <param name="load"></param>
        /// <param name="rLoad"></param>
        public void Add(Load load, RobotLoadRecord rLoad)
        {
            Add(CaseCategory, load.GUID, rLoad.UniqueId.ToString());
        }

        /// <summary>
        /// Add a new Set entry to this mapping table
        /// </summary>
        /// <param name="set"></param>
        /// <param name="rGroup"></param>
        public void Add(IModelObjectSet set, int groupID)
        {
            Add(SetsCategory, set.GUID, groupID.ToString());
        }

        /// <summary>
        /// Remove a node record
        /// </summary>
        /// <param name="node"></param>
        public void Remove(Node node)
        {
            Remove(NodeCategory, node.GUID);
        }

        /// <summary>
        /// Remove an element record
        /// </summary>
        /// <param name="element"></param>
        public void Remove(LinearElement element)
        {
            Remove(BarCategory, element.GUID);
        }

        /// <summary>
        /// Remove a Panel entry
        /// </summary>
        /// <param name="element"></param>
        public void Remove(PanelElement element)
        {
            Remove(PanelCategory, element.GUID);
        }

        /// <summary>
        /// Remove a section entry
        /// </summary>
        /// <param name="section"></param>
        public void Remove(SectionFamily section)
        {
            Remove(SectionCategory, section.GUID);
        }

        /// <summary>
        /// Remove a panel family entry
        /// </summary>
        /// <param name="family"></param>
        public void Remove(BuildUpFamily family)
        {
            Remove(ThicknessCategory, family.GUID);
        }

        /// <summary>
        /// Remove a load case entry
        /// </summary>
        /// <param name="lCase"></param>
        public void Remove(LoadCase lCase)
        {
            Remove(CaseCategory, lCase.GUID);
        }

        /// <summary>
        /// Remove a set entry
        /// </summary>
        /// <param name="set"></param>
        public void Remove(ModelObjectSetBase set)
        {
            Remove(SetsCategory, set.GUID);
        }

        /// <summary>
        /// Get all nodes in the specified model which have a mapping entry in this table
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        public NodeCollection AllMappedNodes(Model.Model inModel)
        {
            var result = new NodeCollection();
            if (ContainsKey(NodeCategory))
            {
                foreach (Guid guid in this[NodeCategory].Keys)
                {
                    if (inModel.Nodes.Contains(guid)) result.Add(inModel.Nodes[guid]);
                }
            }
            return result;
        }

        /// <summary>
        /// Get all linear elements in the specified model which have a mapping entry in this table
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        public ElementCollection AllMappedLinearElements(Model.Model inModel)
        {
            var result = new ElementCollection();
            if (ContainsKey(BarCategory))
            {
                foreach (Guid guid in this[BarCategory].Keys)
                {
                    if (inModel.Elements.Contains(guid)) result.Add(inModel.Elements[guid]);
                }
            }
            return result;
        }

        /// <summary>
        /// Get all linear elements in the specified model which have a mapping entry in this table
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        public ElementCollection AllMappedPanelElements(Model.Model inModel)
        {
            var result = new ElementCollection();
            if (ContainsKey(PanelCategory))
            {
                foreach (Guid guid in this[PanelCategory].Keys)
                {
                    if (inModel.Elements.Contains(guid)) result.Add(inModel.Elements[guid]);
                }
            }
            return result;
        }

        /// <summary>
        /// Get all section properties in the specified model whcih have a mapping extry in this table
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        public FamilyCollection AllMappedSections(Model.Model inModel)
        {
            var result = new FamilyCollection();
            if (ContainsKey(SectionCategory))
            {
                foreach (Guid guid in this[SectionCategory].Keys)
                {
                    if (inModel.Families.Contains(guid)) result.Add(inModel.Families[guid]);
                }
            }
            return result;
        }

        /// <summary>
        /// Construct a string of IDs from the items in the given set, mapped
        /// to their robot equivalents.
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public string ToIDString(IModelObjectSet set)
        {
            var sb = new StringBuilder();
            foreach (var obj in set.GetItems())
            {
                if (obj is Unique)
                {
                    var unique = (Unique)obj;
                    if (HasSecondID(unique.GUID))
                    {
                        if (sb.Length > 0) sb.Append(" ");
                        sb.Append(GetSecondID(unique.GUID));
                    }
                }
            }
            return sb.ToString();
        }

        #endregion
    }
}
