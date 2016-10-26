﻿using FreeBuild.Base;
using FreeBuild.Conversion;
using FreeBuild.Extensions;
using FreeBuild.Geometry;
using FreeBuild.Maths;
using FreeBuild.Model;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Robot
{
    /// <summary>
    /// A controller class to interact with Robot
    /// </summary>
    public class RobotController : MessageRaiser
    {
        #region Properties

        /// <summary>
        /// Private backing field for Robot property
        /// </summary>
        private RobotApplication _Robot;

        /// <summary>
        /// The current Robot application.
        /// This will be initialised when called if necessary.
        /// </summary>
        public RobotApplication Robot
        {
            get
            {
                // Uses lazy initialisation to ensure that a RobotApplication is available 
                // whenever it is needed:
                if (_Robot == null)
                {
                    _Robot = new RobotApplication();
                }
                return _Robot;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Open the Robot file at the specified filepath
        /// </summary>
        /// <param name="filePath">The filepath to open.  (Note that this can be expressed as a string)</param>
        /// <returns>True if the specified file could be opened, false if this was prevented in some way.</returns>
        public bool Open(FilePath filePath)
        {
            try
            {
                Robot.Project.Open(filePath);
                return true;
            }
            catch (COMException)
            {
            }
            return false;
        }

        /// <summary>
        /// Open a new Robot file
        /// </summary>
        /// <returns>True if the new project could be created</returns>
        public bool New()
        {
            try
            {
                Robot.Project.New(IRobotProjectType.I_PT_BUILDING);
                return true;
            }
            catch (COMException ex) { RaiseMessage(ex.Message); }
            return false;
        }

        /// <summary>
        /// Save the currently open Robot file to the specified location
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool Save(FilePath filePath)
        {
            try
            {
                Robot.Project.SaveAs(filePath);
                return true;
            }
            catch (COMException ex) { RaiseMessage(ex.Message); }
            return false;
        }

        /// <summary>
        /// Release control over Robot
        /// </summary>
        public void Release()
        {
            _Robot = null;
        }

        #region Robot to FreeBuild

        /// <summary>
        /// Load a FreeBuild model from a Robot file
        /// </summary>
        /// <param name="filePath">The filepath of the Robot file to be opened</param>
        /// <param name="idMap">The ID mapping table to be used.  If null, an empty table will automatically be initialised.
        /// Mapping data between the Robot and FreeBuild model will be written into this map - store it for later if you wish
        /// to synchronise the models in future.</param>
        /// <param name="options">The conversion options.  If null, the default options will be used.</param>
        /// <returns></returns>
        public Model.Model LoadModelFromRobot(FilePath filePath, ref RobotIDMappingTable idMap, RobotConversionOptions options = null)
        {
            if (Open(filePath)) return LoadModelFromRobot(ref idMap, options);
            else return null;
        }

        /// <summary>
        /// Load a FreeBuild model from the currently open Robot file.  A robot file must have been previously opened before
        /// calling this function.
        /// </summary>
        /// <returns></returns>
        public Model.Model LoadModelFromRobot(ref RobotIDMappingTable idMap, RobotConversionOptions options = null)
        {
            var model = new Model.Model();
            if (idMap == null) idMap = new RobotIDMappingTable();
            if (options == null) options = new RobotConversionOptions();
            var context = new RobotConversionContext(idMap, options);
            UpdateModelFromRobot(model, context);
            return model;
        }

        /// <summary>
        /// Update a FreeBuild model by loading data from the currently open Robot file
        /// </summary>
        /// <param name="model">The model to be updated</param>
        /// <param name="map">The ID mapping table that relates model objects to Robot ones</param>
        /// <returns></returns>
        public bool UpdateModelFromRobot(Model.Model model, RobotConversionContext context)
        {
            IRobotCollection robotNodes = Robot.Project.Structure.Nodes.GetAll();
            UpdateModelSectionsFromRobotFile(model, context);
            UpdateModelNodesFromRobotFile(model, robotNodes, context);
            UpdateModelLinearElementsFromRobotFile(model, robotNodes, context);
            return false;
        }

        /// <summary>
        /// Update nodes in a FreeBuild model by loading data from the currently open Robot file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="map"></param>
        /// <param name="context"></param>
        private void UpdateModelNodesFromRobotFile(Model.Model model, IRobotCollection robotNodes, RobotConversionContext context)
        {
            //Delete all mapped nodes:
            if (context.Options.DeleteMissingObjects) context.IDMap.AllMappedNodes(model).DeleteAll();

            for (int i = 1; i <= robotNodes.Count; i++)
            {
                IRobotNode robotNode = robotNodes.Get(i);
                if (robotNode != null)
                {
                    Vector nodePosition = ROBtoFB.PositionOf(robotNode);
                    Node node = context.IDMap.GetMappedModelNode(robotNode, model);
                    if (node == null)  //Create new node
                        node = model.Create.Node(nodePosition, 0, context.ExInfo);
                    else //Existing mapped node found
                    {
                        node.Position = nodePosition;
                        node.Undelete();
                    }
                    //TODO: Copy over data

                    //Store mapping data:
                    context.IDMap.Add(node, robotNode);
                }
            }
        }

        /// <summary>
        /// Update section properties in a FreeBuild model by loading data from the currently open Robot file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        private void UpdateModelSectionsFromRobotFile(Model.Model model, RobotConversionContext context)
        {
            IRobotCollection sections = Robot.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_BAR_SECTION);
            for (int i = 1; i <= sections.Count; i++)
            {
                IRobotLabel label = sections.Get(i);
                if (label != null)
                {
                    SectionProperty section = context.IDMap.GetMappedSectionProperty(label, model);
                    if (section == null)
                        section = model.Create.SectionProperty(null, context.ExInfo);

                    //TODO: Copy over data

                    //Store mapping data:
                    context.IDMap.Add(section, label);
                }
            }
        }

        /// <summary>
        /// Update linear elements in a FreeBuild model by loading data from the currently open Robot file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="robotNodes"></param>
        /// <param name="map"></param>
        /// <param name="context"></param>
        private void UpdateModelLinearElementsFromRobotFile(Model.Model model, IRobotCollection robotNodes, RobotConversionContext context)
        {
            //Delete all previously mapped linear elements:
            if (context.Options.DeleteMissingObjects) context.IDMap.AllMappedLinearElements(model).DeleteAll();

            IRobotCollection bars = Robot.Project.Structure.Bars.GetAll();
            for (int i = 1; i <= bars.Count; i++)
            {
                IRobotBar bar = bars.Get(i);
                if (bar != null)
                {
                    Curve geometry = ROBtoFB.GeometryOf(bar, robotNodes);
                    LinearElement element = context.IDMap.GetMappedLinearElement(bar, model);
                    if (element == null) //Create new element
                        element = model.Create.LinearElement(geometry, context.ExInfo);
                    else //Existing mapped element found
                    {
                        element.Geometry = geometry;
                        element.Undelete();
                    }
                    element.Geometry.Start.Node = context.IDMap.GetMappedModelNode(bar.StartNode, model);
                    element.Geometry.End.Node = context.IDMap.GetMappedModelNode(bar.EndNode, model);
                    element.Property = context.IDMap.GetMappedSectionProperty()
                    //TODO: Copy over data

                    //Store mapping:
                    context.IDMap.Add(element, bar);
                }
            }
        }

        #endregion

        #region FreeBuild to Robot

        /// <summary>
        /// Save a FreeBuild model to a Robot file at the specified location
        /// </summary>
        /// <param name="filePath">The filePath of the Robot file to be written to</param>
        /// <param name="model">The model to be written from</param>
        /// <param name="idMap">The ID mapping table to be used.  If null, an empty table will automatically be initialised.
        /// Mapping data between the Robot and FreeBuild model will be written into this map - store it for later if you wish
        /// to synchronise the models in future.</param>
        /// <param name="options">The conversion options.  If null, the default options will be used.</param>
        /// <returns></returns>
        public bool WriteModelToRobot(FilePath filePath, Model.Model model, ref RobotIDMappingTable idMap, RobotConversionOptions options = null)
        {
            if (New())
            {
                if (idMap == null) idMap = new RobotIDMappingTable();
                if (options == null) options = new RobotConversionOptions();
                var context = new RobotConversionContext(idMap, options);
                UpdateRobotFromModel(model, context);
                return Save(filePath);
            }
            else return false;
        }

        /// <summary>
        /// Update a robot file based on a FreeBuild model 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool UpdateRobotFromModel(Model.Model model, RobotConversionContext context)
        {
            UpdateRobotNodesFromModel(model, model.Nodes, context);
            UpdateRobotBarsFromModel(model, model.Elements.LinearElements, context);
            return true;
        }

        /// <summary>
        /// Update the nodes in the open Robot model from those in a FreeBuild model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="nodes"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool UpdateRobotNodesFromModel(Model.Model model, NodeCollection nodes, RobotConversionContext context)
        {
            foreach (Node node in nodes)
            {
                UpdateRobotNode(node, context);
            }
            return true;
        }

        /// <summary>
        /// Update the bars in the open Robot model from those in a FreeBuild model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="linearElements"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool UpdateRobotBarsFromModel(Model.Model model, ElementCollection linearElements, RobotConversionContext context)
        {
            foreach (LinearElement element in linearElements)
            {
                UpdateRobotBar(element, context);
            }
            return true;
        }

        #endregion

        #region Robot Object Creation

        /// <summary>
        /// Create a new node within the currently open Robot document
        /// </summary>
        /// <param name="x">The x-coordinate of the node</param>
        /// <param name="y">The y-coordinate of the node</param>
        /// <param name="z">The z-coordinate of the node</param>
        /// <param name="id">Optional.  The ID number to be assigned to the node.
        /// If omitted or lower than 1, the next free number will be used.</param>
        /// <returns>The newly created node</returns>
        public IRobotNode CreateRobotNode(double x, double y, double z, int id = -1)
        {
            //Create new node
            if (id < 1) id = Robot.Project.Structure.Nodes.FreeNumber;
            Robot.Project.Structure.Nodes.Create(id, x, y, z);
            return Robot.Project.Structure.Nodes.Get(id) as IRobotNode;
        }


        /// <summary>
        /// Create a new bar within the currently open Robot document
        /// </summary>
        /// <param name="startNode">The start node number</param>
        /// <param name="endNode">The end node number</param>
        /// <param name="id">Optional.  The ID number to be assigned to the bar.
        /// If omitted or lower than 1, the next free number will be used.</param>
        /// <returns></returns>
        public IRobotBar CreateRobotBar(int startNode, int endNode, int id = -1)
        {
            if (id < 1) id = Robot.Project.Structure.Bars.FreeNumber;
            Robot.Project.Structure.Bars.Create(id, startNode, endNode);
            return Robot.Project.Structure.Bars.Get(id) as IRobotBar;
        }

        /// <summary>
        /// Update or create a robot node linked to the specified FreeBuild node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="context"></param>
        /// <param name="updatePosition"></param>
        /// <returns></returns>
        public IRobotNode UpdateRobotNode(Node node, RobotConversionContext context)
        {
            int mappedID = -1;
            IRobotNode rNode = null;
            if (context.IDMap.HasSecondID(context.IDMap.NodeCategory, node.GUID))
            {
                mappedID = context.IDMap.GetSecondID(context.IDMap.NodeCategory, node.GUID);
                if (Robot.Project.Structure.Nodes.Exist(mappedID) != 0)
                    rNode = Robot.Project.Structure.Nodes.Get(mappedID) as IRobotNode;
            }
            if (rNode == null)
            {
                rNode = CreateRobotNode(node.Position.X, node.Position.Y, node.Position.Z, mappedID);
            }
            else
            {
                rNode.SetPosition(node.Position);
            }
            //TODO: Moar Data!

            //Store mapping:
            context.IDMap.Add(node, rNode);
            return rNode;
        }

        /// <summary>
        /// Update or create a robot bar linked to the specified FreeBuild element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IRobotBar UpdateRobotBar(LinearElement element, RobotConversionContext context)
        {
            int mappedID = -1;
            IRobotBar bar = null;
            Node startNode = element.Geometry.Start.Node; //TODO: Make bulletproof!
            Node endNode = element.Geometry.End.Node; //TODO: Make bulletproof!
            int nodeID0 = GetMappedNodeID(startNode, context);
            int nodeID1 = GetMappedNodeID(endNode, context);

            if (context.IDMap.HasSecondID(context.IDMap.BarCategory, element.GUID))
            {
                mappedID = context.IDMap.GetSecondID(context.IDMap.BarCategory, element.GUID);
                if (Robot.Project.Structure.Bars.Exist(mappedID) != 0)
                    bar = Robot.Project.Structure.Bars.Get(mappedID) as IRobotBar;
            }
            if (bar == null)
            {
                bar = CreateRobotBar(nodeID0, nodeID1, mappedID);
            }
            else
            {
                bar.StartNode = nodeID0;
                bar.EndNode = nodeID1;
            }
            
            if (element.Property != null)
            {
                bar.SetLabel(IRobotLabelType.I_LT_BAR_SECTION, element.Property.Name); ///Cannot rely on this! Bulletproof it!
            }
            //TODO: More data

            context.IDMap.Add(element, bar);

            return bar;
        }

        public IRobotLabel UpdateRobotSection(SectionProperty section, RobotConversionContext context)
        {
            int mappedID = -1;
            IRobotLabel label = null;
            if (context.IDMap.HasSecondID(context.IDMap.SectionCategory, section.GUID))
            {
                mappedID = context.IDMap.GetSecondID(context.IDMap.SectionCategory, section.GUID);
                //if (Robot.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_BAR_SECTION, mappedID) != 0)
                //    label = Robot.Project.Structure.Labels.Get(IRobotLabelType.I_LT_BAR_SECTION, mappedID) as IRobotLabel;
                label = Robot.Project.Structure.Labels.FindWithId(mappedID);
            }
            if (label == null)
            {
                label = Robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, section.Name); //TODO: Enforce name uniqueness?
                //TODO
            }

            RobotBarSectionData rData = label.Data as RobotBarSectionData;
            rData.Name = section.Name;
            //TODO: More data

            context.IDMap.Add(section, label);

            return label;
        }

        /// <summary>
        /// Retrieve a mapped robot node ID for the specified node.
        /// A new node in robot will be generated if nothing is currently mapped.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public int GetMappedNodeID(Node node, RobotConversionContext context)
        {
            if (context.IDMap.HasSecondID(context.IDMap.NodeCategory, node.GUID))
                return context.IDMap.GetSecondID(context.IDMap.NodeCategory, node.GUID);
            else
                return UpdateRobotNode(node, context).Number;
        }

        #endregion

        /// <summary>
        /// Extract a list of all node IDs in the currently open project
        /// </summary>
        /// <returns></returns>
        public IList<int> AllNodeIDs()
        {
            IRobotCollection nodes = Robot.Project.Structure.Nodes.GetAll();
            IList<int> result = new List<int>(nodes.Count);
            for (int i = 1; i <= nodes.Count; i++)
            {
                IRobotDataObject node = nodes.Get(i);
                if (node != null)
                {
                    result.Add(node.Number);
                }
            }
            return result;
        }

        /// <summary>
        /// Extract a list of IDs of all restrained nodes in the currently open project
        /// </summary>
        /// <returns></returns>
        public IList<int> AllSupportNodeIDs()
        {
            IRobotCollection nodes = Robot.Project.Structure.Nodes.GetAll();
            IList<int> result = new List<int>(nodes.Count);
            for (int i = 1; i <= nodes.Count; i++)
            {
                IRobotNode node = nodes.Get(i);
                if (node != null && node.HasLabel(IRobotLabelType.I_LT_SUPPORT) != 0)
                {
                    result.Add(node.Number);
                }
            }
            return result;
        }

        /// <summary>
        /// Extract a list of all case IDs in the currently open project
        /// </summary>
        /// <returns></returns>
        public IList<int> AllCaseIDs()
        {
            RobotCaseCollection cases = Robot.Project.Structure.Cases.GetAll();
            IList<int> result = new List<int>(cases.Count);
            for (int i = 1; i <= cases.Count; i++)
            {
                IRobotCase rCase = cases.Get(i);
                if (rCase != null)
                {
                    result.Add(rCase.Number);
                }
            }
            return result;
        }

        /// <summary>
        /// Extract a list of all Bar element IDs in the currently open project
        /// </summary>
        /// <returns></returns>
        public IList<int> AllBarIDs()
        {
            IRobotCollection bars = Robot.Project.Structure.Bars.GetAll();
            IList<int> result = new List<int>(bars.Count);
            for (int i = 1; i <= bars.Count; i++)
            {
                IRobotDataObject bar = bars.Get(i);
                if (bar != null) result.Add(bar.Number);
            }
            return result;
        }

        /// <summary>
        /// Extract a list of all section names in the currently open project
        /// </summary>
        /// <returns></returns>
        public IList<string> AllSectionNames()
        {
            IRobotCollection sections = Robot.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_BAR_SECTION);
            IList<string> result = new List<string>(sections.Count);
            for (int i = 1; i <= sections.Count; i++)
            {
                IRobotLabel label = sections.Get(i);
                if (label != null) result.Add(label.Name);
            }
            return result;
        }

        /// <summary>
        /// Extract data of the specified type for the specified node
        /// </summary>
        /// <param name="nodeID">The node ID</param>
        /// <param name="dataType">The type of data to be extracted for the node.</param>
        /// <param name="caseID">Optional.  The case ID to extract data for.  Not valid for all extraction types.</param>
        /// <returns></returns>
        public object ExtractData(int nodeID, NodeDataType dataType, int caseID = 1)
        {
            if (dataType == NodeDataType.Node_ID) return nodeID;
            //Case properties:
            if (dataType == NodeDataType.Case_ID) return caseID;

            RobotStructure structure = Robot.Project.Structure;

            if (dataType == NodeDataType.Case_Name)
            {
                IRobotCase rCase = structure.Cases.Get(caseID);
                return rCase.Name;
            }

            //Node properties:
            if (dataType.IsNodeData())
            {
                IRobotNode node = structure.Nodes.Get(nodeID) as IRobotNode;
                if (dataType == NodeDataType.X) return node.X;
                if (dataType == NodeDataType.Y) return node.Y;
                if (dataType == NodeDataType.Z) return node.Z;
            }

            if (dataType.IsDisplacementData())
            {
                IRobotDisplacementData displacements = structure.Results.Nodes.Displacements.Value(nodeID, caseID);
                if (dataType == NodeDataType.Displacement_Ux) return displacements.UX;
                if (dataType == NodeDataType.Displacement_Uy) return displacements.UY;
                if (dataType == NodeDataType.Displacement_Uz) return displacements.UZ;
                if (dataType == NodeDataType.Displacement_Rxx) return displacements.RX;
                if (dataType == NodeDataType.Displacement_Ryy) return displacements.RY;
                if (dataType == NodeDataType.Displacmenet_Rzz) return displacements.RZ;
            }

            RobotReactionData reactions = structure.Results.Nodes.Reactions.Value(nodeID, caseID);
            if (dataType == NodeDataType.Reactions_Fx) return reactions.FX;
            if (dataType == NodeDataType.Reactions_Fy) return reactions.FY;
            if (dataType == NodeDataType.Reactions_Fz) return reactions.FZ;
            if (dataType == NodeDataType.Reactions_Mxx) return reactions.MX;
            if (dataType == NodeDataType.Reactions_Myy) return reactions.MY;
            if (dataType == NodeDataType.Reactions_Mzz) return reactions.MZ;

            return null;
        }

        /// <summary>
        /// Extract data of the specified type for the specified bar
        /// </summary>
        /// <param name="barID"></param>
        /// <param name="dataType"></param>
        /// <param name="caseID"></param>
        /// <returns></returns>
        public object ExtractData(int barID, BarDataType dataType, int caseID = 1, string position = "0.5")
        {
            if (dataType == BarDataType.Bar_ID) return barID;
            if (dataType == BarDataType.Case_ID) return caseID;

            RobotStructure structure = Robot.Project.Structure;

            if (dataType == BarDataType.Case_Name)
            {
                IRobotCase rCase = Robot.Project.Structure.Cases.Get(caseID);
                return rCase.Name;
            }

            if (dataType.IsBarData())
            {
                IRobotBar bar = structure.Bars.Get(barID) as IRobotBar;
                if (dataType == BarDataType.Bar_Name) return bar.Name;
                if (dataType == BarDataType.StartNode) return bar.StartNode;
                if (dataType == BarDataType.EndNode) return bar.EndNode;
                if (dataType == BarDataType.Length) return bar.Length;
                if (dataType == BarDataType.Section_Name)
                {
                    IRobotLabel sectionLabel = bar.GetLabel(IRobotLabelType.I_LT_BAR_SECTION);
                    if (sectionLabel != null) return sectionLabel.Name;
                    else return null;
                }
            }


            if (position.IsNumeric())
            {
                if (dataType.IsDisplacement())
                {
                    IRobotDisplacementData displacements = structure.Results.Bars.Displacements.Value(barID, double.Parse(position), caseID);
                    if (dataType == BarDataType.Displacement_Ux) return displacements.UX;
                    if (dataType == BarDataType.Displacement_Uy) return displacements.UY;
                    if (dataType == BarDataType.Displacement_Uz) return displacements.UZ;
                    if (dataType == BarDataType.Displacement_Rxx) return displacements.RX;
                    if (dataType == BarDataType.Displacement_Ryy) return displacements.RY;
                    if (dataType == BarDataType.Displacmenet_Rzz) return displacements.RZ;
                }

                if (dataType.IsForce())
                {
                    RobotBarForceData forces = structure.Results.Bars.Forces.Value(barID, caseID, double.Parse(position));
                    if (dataType == BarDataType.Force_Fx) return forces.FX;
                    if (dataType == BarDataType.Force_Fy) return forces.FY;
                    if (dataType == BarDataType.Force_Fz) return forces.FZ;
                    if (dataType == BarDataType.Force_Mxx) return forces.MX;
                    if (dataType == BarDataType.Force_Myy) return forces.MY;
                    if (dataType == BarDataType.Force_Mzz) return forces.MZ;
                }

                if (dataType.IsStress())
                {
                    RobotBarStressData stresses = structure.Results.Bars.Stresses.Value(barID, caseID, double.Parse(position));
                    if (dataType == BarDataType.Stress_Axial) return stresses.FXSX;
                    if (dataType == BarDataType.Stress_Max) return stresses.Smax;
                    if (dataType == BarDataType.Stress_Max_Myy) return stresses.SmaxMY;
                    if (dataType == BarDataType.Stress_Max_Mzz) return stresses.SmaxMZ;
                    if (dataType == BarDataType.Stress_Min) return stresses.Smin;
                    if (dataType == BarDataType.Stress_Min_Myy) return stresses.SminMY;
                    if (dataType == BarDataType.Stress_Min_Mzz) return stresses.SminMZ;
                    if (dataType == BarDataType.Stress_Shear_Y) return stresses.ShearY;
                    if (dataType == BarDataType.Stress_Shear_Z) return stresses.ShearZ;
                    if (dataType == BarDataType.Stress_Torsion) return stresses.Torsion;
                }
            }
            else
            {
                RobotSelection barSel = structure.Selections.Create(IRobotObjectType.I_OT_BAR);
                barSel.AddOne(barID);
                RobotSelection caseSel = structure.Selections.Create(IRobotObjectType.I_OT_CASE);
                caseSel.AddOne(caseID);
                RobotExtremeParams exParams = Robot.CmpntFactory.Create(IRobotComponentType.I_CT_EXTREME_PARAMS);
                exParams.Selection.Set(IRobotObjectType.I_OT_BAR, barSel);
                exParams.Selection.Set(IRobotObjectType.I_OT_CASE, caseSel);
                // ? exParams.BarDivision = 11;

                if (dataType.IsDisplacement())
                {
                    if (dataType == BarDataType.Displacement_Ux) exParams.ValueType = IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_UX;
                    else if (dataType == BarDataType.Displacement_Uy) exParams.ValueType = IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_UY;
                    else if (dataType == BarDataType.Displacement_Uz) exParams.ValueType = IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_UZ;
                    else if (dataType == BarDataType.Displacement_Rxx) exParams.ValueType = IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_RX;
                    else if (dataType == BarDataType.Displacement_Ryy) exParams.ValueType = IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_RY;
                    else if (dataType == BarDataType.Displacmenet_Rzz) exParams.ValueType = IRobotExtremeValueType.I_EVT_DISPLACEMENT_BAR_RZ;
                }
                else if (dataType.IsForce())
                {
                    if (dataType == BarDataType.Force_Fx) exParams.ValueType = IRobotExtremeValueType.I_EVT_FORCE_BAR_FX;
                    else if (dataType == BarDataType.Force_Fy) exParams.ValueType = IRobotExtremeValueType.I_EVT_FORCE_BAR_FY;
                    else if (dataType == BarDataType.Force_Fz) exParams.ValueType = IRobotExtremeValueType.I_EVT_FORCE_BAR_FZ;
                    else if (dataType == BarDataType.Force_Mxx) exParams.ValueType = IRobotExtremeValueType.I_EVT_FORCE_BAR_MX;
                    else if (dataType == BarDataType.Force_Myy) exParams.ValueType = IRobotExtremeValueType.I_EVT_FORCE_BAR_MY;
                    else if (dataType == BarDataType.Force_Mzz) exParams.ValueType = IRobotExtremeValueType.I_EVT_FORCE_BAR_MZ;
                }
                else if (dataType.IsStress())
                {
                    if (dataType == BarDataType.Stress_Axial) exParams.ValueType = IRobotExtremeValueType.I_EVT_STRESS_BAR_FX_SX;
                    else if (dataType == BarDataType.Stress_Max) exParams.ValueType = IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX;
                    else if (dataType == BarDataType.Stress_Max_Myy) exParams.ValueType = IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX_MY;
                    else if (dataType == BarDataType.Stress_Max_Mzz) exParams.ValueType = IRobotExtremeValueType.I_EVT_STRESS_BAR_SMAX_MZ;
                    else if (dataType == BarDataType.Stress_Min) exParams.ValueType = IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN;
                    else if (dataType == BarDataType.Stress_Min_Myy) exParams.ValueType = IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN_MY;
                    else if (dataType == BarDataType.Stress_Min_Mzz) exParams.ValueType = IRobotExtremeValueType.I_EVT_STRESS_BAR_SMIN_MZ;
                    else if (dataType == BarDataType.Stress_Shear_Y) exParams.ValueType = IRobotExtremeValueType.I_EVT_STRESS_BAR_TY;
                    else if (dataType == BarDataType.Stress_Shear_Z) exParams.ValueType = IRobotExtremeValueType.I_EVT_STRESS_BAR_TZ;
                    else if (dataType == BarDataType.Stress_Torsion) exParams.ValueType = IRobotExtremeValueType.I_EVT_STRESS_BAR_T;
                }
                else return null;

                if (position.ToUpper() == "MAX") return structure.Results.Extremes.MaxValue(exParams).Value;
                else if (position.ToUpper() == "MIN") return structure.Results.Extremes.MinValue(exParams).Value;
                else //AbsMax
                {
                    double min = structure.Results.Extremes.MinValue(exParams).Value;
                    double max = structure.Results.Extremes.MaxValue(exParams).Value;
                    return new Interval(min, max).AbsMax;
                }

            }
            return null;
        }

        #endregion
    }
}