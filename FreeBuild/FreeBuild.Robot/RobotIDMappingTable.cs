using FreeBuild.Conversion;
using FreeBuild.Model;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Robot
{
    /// <summary>
    /// A mapping table from freebuild model objects to Robot objects
    /// </summary>
    public class RobotIDMappingTable : IDMappingTable<Guid, int>
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
        /// The name of the category under which section properties are stored
        /// </summary>
        public string SectionCategory { get { return "Sections"; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mappedIDName">The name of the first ID set</param>
        public RobotIDMappingTable(string mappedIDName = "FreeBuild") : base(mappedIDName, "Robot")
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the FreeBuild node, if any, mapped to the specified robot node ID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Node GetMappedModelNode(int robotID, Model.Model model)
        {
            if (HasFirstID(NodeCategory, robotID)) return model.Nodes.TryGet(GetFirstID(NodeCategory, robotID));
            return null;
        }

        /// <summary>
        /// Get the FreeBuild node, if any, mapped to the ID number of the specified robot node
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Node GetMappedModelNode(IRobotNode node, Model.Model model)
        {
            return GetMappedModelNode(node.Number, model);
        }

        /// <summary>
        /// Get the FreeBuild element, if any, mapped to the specified robot bar ID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public LinearElement GetMappedLinearElement(int robotID, Model.Model model)
        {
            //TODO: Get for sub-elements (when introduced)
            if (HasFirstID(BarCategory, robotID)) return model.Elements.TryGet(GetFirstID(BarCategory, robotID)) as LinearElement;
            return null;
        }

        /// <summary>
        /// Get the FreeBuild element, if any, mapped to the ID of the specified robot bar
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public LinearElement GetMappedLinearElement(IRobotBar bar, Model.Model model)
        {
            return GetMappedLinearElement(bar.Number, model);
        }

        /// <summary>
        /// Get the FreeBuild section property, if any, mapped to the specified robot section label ID
        /// </summary>
        /// <param name="robotID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public SectionProperty GetMappedSectionProperty(int robotID, Model.Model model)
        {
            if (HasFirstID(SectionCategory, robotID)) return model.Properties.TryGet(GetFirstID(SectionCategory, robotID)) as SectionProperty;
            return null;
        }

        /// <summary>
        /// Get the FreeBuild section property, if any, mapped to the ID of the specified 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public SectionProperty GetMappedSectionProperty(IRobotLabel label, Model.Model model)
        {
            return GetMappedSectionProperty(label.UniqueId, model);
        }

        /// <summary>
        /// Add a new Node entry to this mapping table
        /// </summary>
        /// <param name="fbNode"></param>
        /// <param name="rNode"></param>
        public void Add(Node fbNode, IRobotNode rNode)
        {
            Add(NodeCategory, fbNode.GUID, rNode.Number);
        }

        /// <summary>
        /// Add a new Bar entry to this mapping table
        /// </summary>
        /// <param name="element"></param>
        /// <param name="bar"></param>
        public void Add(LinearElement element, IRobotBar bar)
        {
            Add(BarCategory, element.GUID, bar.Number);
        }

        /// <summary>
        /// Add a new Section entry to this mapping table
        /// </summary>
        /// <param name="section"></param>
        /// <param name="label"></param>
        public void Add(SectionProperty section, IRobotLabel label)
        {
            Add(SectionCategory, section.GUID, label.UniqueId);
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
        /// Get all section properties in the specified model whcih have a mapping extry in this table
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        public VolumetricPropertyCollection AllMappedSections(Model.Model inModel)
        {
            var result = new VolumetricPropertyCollection();
            if (ContainsKey(SectionCategory))
            {
                foreach (Guid guid in this[SectionCategory].Keys)
                {
                    if (inModel.Properties.Contains(guid)) result.Add(inModel.Properties[guid]);
                }
            }
            return result;
        }

        #endregion
    }
}
