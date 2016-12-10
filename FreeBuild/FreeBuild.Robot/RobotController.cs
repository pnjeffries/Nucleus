using FreeBuild.Base;
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
                    RaiseMessage("Establishing Robot link...");
                    COMMessageFilter.Register();
                    _Robot = new RobotApplication();
                    RaiseMessage("Robot link established.");
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
                RaiseMessage("Opening Robot file '" + filePath + "'...");
                Robot.Project.Open(filePath);
                return true;
            }
            catch (COMException ex)
            {
                RaiseMessage("Error: Opening Robot file '" + filePath + "' failed.  " + ex.Message);
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
                RaiseMessage("Creating new Robot project...");
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
                RaiseMessage("Saving Robot file...");
                Robot.Project.SaveAs(filePath);
                RaiseMessage("Robot file saved to " + filePath);
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
            COMMessageFilter.Revoke();
            RaiseMessage("Robot link released.");
        }

        /// <summary>
        /// Close Robot
        /// </summary>
        public void Close()
        {
            Robot.Project.Close();
            RaiseMessage("Robot file closed.");
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
            RaiseMessage("Reading data from Robot...");
            IRobotCollection robotNodes = Robot.Project.Structure.Nodes.GetAll();
            UpdateModelSectionsFromRobotFile(model, context);
            UpdateModelNodesFromRobotFile(model, robotNodes, context);
            UpdateModelLinearElementsFromRobotFile(model, robotNodes, context);
            RaiseMessage("Data reading completed.");
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
                        section = model.Create.SectionProperty(context.ExInfo);

                    //TODO: Copy over data
                    RobotBarSectionData data = label.Data;
                    section.Name = data.Name;
                    section.Profile = CreateProfileFromRobotSectionData(data);

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
                    element.StartNode = context.IDMap.GetMappedModelNode(bar.StartNode, model);
                    element.EndNode = context.IDMap.GetMappedModelNode(bar.EndNode, model);
                    if (bar.HasLabel(IRobotLabelType.I_LT_BAR_SECTION) != 0)
                    {
                        string sectionID = bar.GetLabelName(IRobotLabelType.I_LT_BAR_SECTION);
                        element.Property = context.IDMap.GetMappedSectionProperty(sectionID, model);
                    }
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
            RaiseMessage("Writing data to Robot...");
            UpdateRobotNodesFromModel(model, model.Nodes, context);
            UpdateRobotPropertiesFromModel(model, model.Properties, context);
            UpdateRobotBarsFromModel(model, model.Elements.LinearElements, context);
            RaiseMessage("Data writing completed.");
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
        /// Update the section, face properties etc. in the open Robot model from those in a FreeBuild model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="properties"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool UpdateRobotPropertiesFromModel(Model.Model model, VolumetricPropertyCollection properties, RobotConversionContext context)
        {
            foreach (VolumetricProperty property in properties)
            {
                if (property is SectionProperty)
                {
                    UpdateRobotSection((SectionProperty)property, context);
                }
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
        private bool UpdateRobotBarsFromModel(Model.Model model, LinearElementCollection linearElements, RobotConversionContext context)
        {
            foreach (LinearElement element in linearElements)
            {
                UpdateRobotBar(element, context);
            }
            return true;
        }

        /// <summary>
        /// Update the panel elements in the open Robot model from those in a FreeBuild model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="panelElements"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool UpdateRobotPanelsFromModel(Model.Model model, PanelElementCollection panelElements, RobotConversionContext context)
        {
            foreach (PanelElement element in panelElements)
            {
                //TODO
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
                mappedID = int.Parse(context.IDMap.GetSecondID(context.IDMap.NodeCategory, node.GUID));
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
            Node startNode = element.StartNode; //TODO: Make bulletproof!
            Node endNode = element.EndNode; //TODO: Make bulletproof!
            int nodeID0 = GetMappedNodeID(startNode, context);
            int nodeID1 = GetMappedNodeID(endNode, context);

            if (context.IDMap.HasSecondID(context.IDMap.BarCategory, element.GUID))
            {
                mappedID = int.Parse(context.IDMap.GetSecondID(context.IDMap.BarCategory, element.GUID));
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
                bar.SetLabel(IRobotLabelType.I_LT_BAR_SECTION, this.GetMappedSectionID(element.Property, context));
            }
            //TODO: More data

            context.IDMap.Add(element, bar);

            return bar;
        }

        /// <summary>
        /// Update or create a Robot section linked to a FreeBuild section property
        /// </summary>
        /// <param name="section"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IRobotLabel UpdateRobotSection(SectionProperty section, RobotConversionContext context)
        {
            string mappedID;
            IRobotLabel label = null;
            if (context.IDMap.HasSecondID(context.IDMap.SectionCategory, section.GUID))
            {
                mappedID = context.IDMap.GetSecondID(context.IDMap.SectionCategory, section.GUID);
                if (Robot.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_BAR_SECTION, mappedID) != 0)
                    label = Robot.Project.Structure.Labels.Get(IRobotLabelType.I_LT_BAR_SECTION, mappedID) as IRobotLabel;
                //label = Robot.Project.Structure.Labels.FindWithId(mappedID);
            }
            if (label == null)
            {
                if (section.Name == null) section.Name = "Test"; //TEMP!
                label = Robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, section.Name); //TODO: Enforce name uniqueness?
                //TODO
            }

            RobotBarSectionData rData = label.Data as RobotBarSectionData;
            rData.Name = section.Name;
            UpdateRobotSectionGeometry(rData, section.Profile);
            //TODO: More data

            context.IDMap.Add(section, label);

            Robot.Project.Structure.Labels.Store(label);

            return label;
        }

        /// <summary>
        /// Update the values stored in a Robot section data to match those in a FreeBuild profile
        /// </summary>
        /// <param name="data"></param>
        /// <param name="profile"></param>
        protected void UpdateRobotSectionGeometry(RobotBarSectionData data, SectionProfile profile)
        {
            if (profile != null)
            {
                //Attempt to load catalogue section:
                if (!string.IsNullOrWhiteSpace(profile.CatalogueName) && data.LoadFromDBase(profile.CatalogueName) != 0) return;

                if (profile is SymmetricIProfile) //I Section
                {
                    var rProfile = (SymmetricIProfile)profile;
                    data.Type = IRobotBarSectionType.I_BST_NS_I;
                    data.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_BISYM;
                    /*data.SetValue(IRobotBarSectionDataValue.I_BSDV_D, rProfile.Depth);
                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_BF, rProfile.Width);
                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_TF, rProfile.FlangeThickness);
                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_TW, rProfile.WebThickness);
                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_RA, rProfile.RootRadius); //???
                    */
                    RobotBarSectionNonstdData nsdata = data.CreateNonstd(0);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, rProfile.Depth - rProfile.FlangeThickness * 2);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B, rProfile.Width);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF, rProfile.FlangeThickness);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW, rProfile.WebThickness);
                }
                else if (profile is RectangularHollowProfile) //Rectangular Hollow Sections
                {
                    var rProfile = (RectangularHollowProfile)profile;
                    data.Type = IRobotBarSectionType.I_BST_NS_BOX;
                    data.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX;
                    /*data.SetValue(IRobotBarSectionDataValue.I_BSDV_D, rProfile.Depth);
                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_BF, rProfile.Width);
                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_TF, rProfile.FlangeThickness);
                    data.SetValue(IRobotBarSectionDataValue.I_BSDV_TW, rProfile.WebThickness);*/
                    RobotBarSectionNonstdData nsdata = data.CreateNonstd(0);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_H, rProfile.Depth - rProfile.FlangeThickness*2);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_B, rProfile.Width);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TF, rProfile.FlangeThickness);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TW, rProfile.WebThickness);
                }
                else if (profile is RectangularProfile) //Filled Rectangular Sections
                {
                    var rProfile = (RectangularProfile)profile;
                    data.Type = IRobotBarSectionType.I_BST_NS_RECT;
                    data.ShapeType = IRobotBarSectionShapeType.I_BSST_RECT_FILLED;
                    //data.SetValue(IRobotBarSectionDataValue.I_BSDV_D, rProfile.Depth);
                    //data.SetValue(IRobotBarSectionDataValue.I_BSDV_BF, rProfile.Width);
                    RobotBarSectionNonstdData nsdata = data.CreateNonstd(0);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, rProfile.Depth);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, rProfile.Width);
                }
                else if (profile is CircularHollowProfile)
                {
                    var cProfile = (CircularHollowProfile)profile;
                    data.Type = IRobotBarSectionType.I_BST_NS_TUBE;
                    data.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_TUBE;
                    //data.SetValue(IRobotBarSectionDataValue.I_BSDV_D, cProfile.Diameter);
                    //data.SetValue(IRobotBarSectionDataValue.I_BSDV_TW, cProfile.WallThickness);
                    RobotBarSectionNonstdData nsdata = data.CreateNonstd(0);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, cProfile.Diameter);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T, cProfile.WallThickness);
                }
                else if (profile is CircularProfile)
                {
                    var cProfile = (CircularProfile)profile;
                    data.Type = IRobotBarSectionType.I_BST_NS_TUBE;
                    //data.ShapeType = IRobotBarSectionShapeType.I_BSST_CIRC_FILLED;
                    //data.SetValue(IRobotBarSectionDataValue.I_BSDV_D, cProfile.Diameter);
                    RobotBarSectionNonstdData nsdata = data.CreateNonstd(0);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, cProfile.Diameter);
                }

                
            }
        }

        /// <summary>
        /// Create a FreeBuild section profile geometry from Robot section data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public SectionProfile CreateProfileFromRobotSectionData(RobotBarSectionData data)
        {
            SectionProfile result = null;
            Type equivalent = EquivalentProfileType(data.ShapeType);
            if (equivalent == typeof(SymmetricIProfile))
            {
                var iProfile = new SymmetricIProfile();
                iProfile.Depth = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                iProfile.Width = data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
                iProfile.FlangeThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
                iProfile.WebThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
                //TODO: Fillet radius
                iProfile.RootRadius = data.GetValue(IRobotBarSectionDataValue.I_BSDV_RA); //????
                result = iProfile;
            }
            else if (equivalent == typeof(RectangularHollowProfile)) //Rectangular Hollow Profile
            {
                var rProfile = new RectangularHollowProfile();
                rProfile.Depth = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                rProfile.Width = data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
                rProfile.FlangeThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
                rProfile.WebThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
                result = rProfile;
            }
            else if (equivalent == typeof(RectangularProfile)) //Filled Rectangular Profile
            {
                var rProfile = new RectangularProfile();
                rProfile.Depth = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                rProfile.Width = data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
                result = rProfile;
            }
            else if (equivalent == typeof(CircularHollowProfile))
            {
                var cProfile = new CircularHollowProfile();
                cProfile.Diameter = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                cProfile.WallThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW); //?
                result = cProfile;
            }
            else if (equivalent == typeof(CircularProfile))
            {
                var cProfile = new CircularProfile();
                cProfile.Diameter = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                result = cProfile;
            }

            if (data.Type == IRobotBarSectionType.I_BST_STANDARD)
            {
                result.CatalogueName = data.Name;
            }

            return result; //Profile could not be created
        }

        /// <summary>
        /// Get the equivalent FreeBuild Profile subtype for the specified value of the Robot
        /// IRobotBarSectionShapeType enum
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type EquivalentProfileType(IRobotBarSectionShapeType type)
        {
            if (type == IRobotBarSectionShapeType.I_BSST_USER_I_BISYM
                || type == IRobotBarSectionShapeType.I_BSST_IPE
                || type == IRobotBarSectionShapeType.I_BSST_HEA)
                return typeof(SymmetricIProfile);
            else if (type == IRobotBarSectionShapeType.I_BSST_USER_BOX
                || type == IRobotBarSectionShapeType.I_BSST_USER_RECT
                || type == IRobotBarSectionShapeType.I_BSST_TREC)
                return typeof(RectangularHollowProfile);
            else if (type == IRobotBarSectionShapeType.I_BSST_RECT_FILLED)
                return typeof(RectangularProfile);
            else if (type == IRobotBarSectionShapeType.I_BSST_TRON
                || type == IRobotBarSectionShapeType.I_BSST_TUBE)
                return typeof(CircularHollowProfile);
            else if (type == IRobotBarSectionShapeType.I_BSST_CIRC_FILLED
                || type == IRobotBarSectionShapeType.I_BSST_USER_CIRC_FILLED)
                return typeof(CircularProfile);
            else
                return null;
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
                return int.Parse(context.IDMap.GetSecondID(context.IDMap.NodeCategory, node.GUID));
            else
                return UpdateRobotNode(node, context).Number;
        }

        /// <summary>
        /// Retrieve a mapped Robot section label name for the specified section.
        /// A new section in Robot will be generated if nothing is currently mapped.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetMappedSectionID(SectionProperty section, RobotConversionContext context)
        {
            if (context.IDMap.HasSecondID(context.IDMap.SectionCategory, section.GUID))
                return context.IDMap.GetSecondID(context.IDMap.SectionCategory, section.GUID);
            else
                return UpdateRobotSection(section, context).Name;
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
        /// Extract a list of all finite element IDs in the currently open project
        /// </summary>
        /// <returns></returns>
        public IList<int> AllFiniteElementIDs()
        {
            IRobotCollection elems = Robot.Project.Structure.FiniteElems.GetAll();
            IList<int> result = new List<int>(elems.Count);
            for (int i = 1; i <= elems.Count; i++)
            {
                IRobotDataObject elem = elems.Get(i);
                if (elem != null) result.Add(elem.Number);
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
                if (rCase != null)
                {
                    return rCase.Name;
                }
                else return null;
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
                return rCase?.Name;
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

        public object ExtractData(string sectionID, SectionDataType dataType)
        {
            if (dataType == SectionDataType.Name) return sectionID;

            IRobotLabel label = Robot.Project.Structure.Labels.Get(IRobotLabelType.I_LT_BAR_SECTION, sectionID);
            RobotBarSectionData data = label.Data;

            if (dataType == SectionDataType.Material) return data.MaterialName;
            else if (dataType == SectionDataType.Type) return data.Type.ToString();
            else if (dataType == SectionDataType.ShapeType) return data.ShapeType.ToString();
            else if (dataType == SectionDataType.Depth) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
            else if (dataType == SectionDataType.Width) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
            else if (dataType == SectionDataType.Width_2) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF2);
            else if (dataType == SectionDataType.Flange_Thickness) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
            else if (dataType == SectionDataType.Flange_Thickness_2) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF2);
            else if (dataType == SectionDataType.Web_Thickness) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
            else if (dataType == SectionDataType.Fillet_Radius) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_RA);
            else if (dataType == SectionDataType.Gamma_Angle) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_GAMMA);
            else if (dataType == SectionDataType.Weight) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_WEIGHT);
            else if (dataType == SectionDataType.Perimeter) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_SURFACE);
            else if (dataType == SectionDataType.Area_X) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_AX);
            else if (dataType == SectionDataType.Area_Y) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_AY);
            else if (dataType == SectionDataType.Area_Z) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_AZ);
            else if (dataType == SectionDataType.I_XX) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_IX);
            else if (dataType == SectionDataType.I_YY) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_IY);
            else if (dataType == SectionDataType.I_ZZ) return data.GetValue(IRobotBarSectionDataValue.I_BSDV_IZ);
            return null;
        }

        #endregion
    }
}