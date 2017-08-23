using Nucleus.Base;
using Nucleus.Conversion;
using Nucleus.Extensions;
using Nucleus.Geometry;
using Nucleus.Maths;
using Nucleus.Model;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Robot
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
                Robot.Project.New(IRobotProjectType.I_PT_SHELL);
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
                if (filePath.Exists)
                    RaiseMessage("Robot file saved to " + filePath);

                return filePath.Exists;
            }
            catch (COMException ex) { RaiseMessage(ex.Message); }
            return false;
        }

        /// <summary>
        /// Release control over Robot
        /// </summary>
        public void Release(bool quitRobot = true)
        {
            if (_Robot != null && quitRobot) _Robot.Quit(IRobotQuitOption.I_QO_DISCARD_CHANGES); //?
            _Robot = null;
            COMMessageFilter.Revoke();
            RaiseMessage("Robot link released.");
        }

        /// <summary>
        /// Close the current Robot file
        /// </summary>
        public void Close()
        {
            Robot.Project.Close();
            RaiseMessage("Robot file closed.");
        }

        /// <summary>
        /// Make the robot application visible
        /// </summary>
        public void Show()
        {
            Robot.Visible = 1;
        }

        #region Robot to Nucleus

        /// <summary>
        /// Load a Nucleus model from a Robot file
        /// </summary>
        /// <param name="filePath">The filepath of the Robot file to be opened</param>
        /// <param name="idMap">The ID mapping table to be used.  If null, an empty table will automatically be initialised.
        /// Mapping data between the Robot and Nucleus model will be written into this map - store it for later if you wish
        /// to synchronise the models in future.</param>
        /// <param name="options">The conversion options.  If null, the default options will be used.</param>
        /// <returns></returns>
        public Model.Model LoadModelFromRobot(FilePath filePath, ref RobotIDMappingTable idMap, RobotConversionOptions options = null)
        {
            if (Open(filePath)) return LoadModelFromRobot(ref idMap, options);
            else return null;
        }

        /// <summary>
        /// Load a Nucleus model from the currently open Robot file.  A robot file must have been previously opened before
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
        /// Update a Nucleus model by loading data from the currently open Robot file
        /// </summary>
        /// <param name="model">The model to be updated</param>
        /// <param name="map">The ID mapping table that relates model objects to Robot ones</param>
        /// <returns></returns>
        public bool UpdateModelFromRobot(Model.Model model, RobotConversionContext context)
        {
            RobotConversionOptions options = context.Options;
            RaiseMessage("Reading data from Robot...");
            IRobotCollection robotNodes = Robot.Project.Structure.Nodes.GetAll();
            if (options.Families)
            {
                UpdateModelSectionsFromRobotFile(model, context);
                UpdateModelBuildUpsFromRobotFile(model, context);
            }
            if (options.Nodes) UpdateModelNodesFromRobotFile(model, robotNodes, context);
            if (options.LinearElements) UpdateModelLinearElementsFromRobotFile(model, robotNodes, context);
            if (options.PanelElements) UpdateModelPanelElementsFromRobotFile(model, robotNodes, context);
            RaiseMessage("Data reading completed.");
            return false;
        }

        /// <summary>
        /// Update nodes in a Nucleus model by loading data from the currently open Robot file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="map"></param>
        /// <param name="context"></param>
        private void UpdateModelNodesFromRobotFile(Model.Model model, IRobotCollection robotNodes, RobotConversionContext context)
        {
            RaiseMessage("Reading Nodes...");
            //Delete all mapped nodes:
            if (context.Options.DeleteObjects) context.IDMap.AllMappedNodes(model).DeleteAll();

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

                    if (robotNode.HasLabel(IRobotLabelType.I_LT_SUPPORT) != 0)
                    {
                        // Support condition:
                        node.SetData(ROBtoFB.Convert(robotNode.GetLabel(IRobotLabelType.I_LT_SUPPORT).Data));
                    }

                    //TODO: Copy over data


                    //Store mapping data:
                    context.IDMap.Add(node, robotNode);
                }
            }
        }

        /// <summary>
        /// Update section properties in a Nucleus model by loading data from the currently open Robot file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        private void UpdateModelSectionsFromRobotFile(Model.Model model, RobotConversionContext context)
        {
            RaiseMessage("Reading Sections...");
            IRobotCollection sections = Robot.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_BAR_SECTION);
            for (int i = 1; i <= sections.Count; i++)
            {
                IRobotLabel label = sections.Get(i);
                if (label != null)
                {
                    SectionFamily section = context.IDMap.GetMappedSectionFamily(label, model);
                    if (section == null)
                        section = model.Create.SectionFamily(context.ExInfo);

                    //TODO: Copy over data
                    RobotBarSectionData data = label.Data;
                    section.Name = data.Name;
                    section.Profile = CreateProfileFromRobotSectionData(data);

                    //Store mapping data:
                    context.IDMap.Add(section, label);
                }
            }
        }

        private void UpdateModelBuildUpsFromRobotFile(Model.Model model, RobotConversionContext context)
        {
            RaiseMessage("Reading Thicknesses...");
            IRobotCollection thicknesses = Robot.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_PANEL_THICKNESS);
            for (int i = 1; i <= thicknesses.Count; i++)
            {
                IRobotLabel label = thicknesses.Get(i);
                if (label != null)
                {
                    BuildUpFamily buildUp = context.IDMap.GetMappedPanelFamily(label, model);
                    if (buildUp == null)
                        buildUp = model.Create.BuildUpFamily(context.ExInfo);

                    //TODO: Copy over data
                    RobotThicknessData data = label.Data;
                    buildUp.Name = label.Name;
                    Material material = null; //TODO
                    if (data.ThicknessType == IRobotThicknessType.I_TT_HOMOGENEOUS)
                    {
                        IRobotThicknessHomoData homoData = (IRobotThicknessHomoData)data.Data;
                        buildUp.Layers.Clear();
                        buildUp.Layers.Add(new BuildUpLayer(homoData.ThickConst, material));
                    }

                    //Store mapping data:
                    context.IDMap.Add(buildUp, label);
                }
            }
        }

        /// <summary>
        /// Update linear elements in a Nucleus model by loading data from the currently open Robot file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="robotNodes"></param>
        /// <param name="map"></param>
        /// <param name="context"></param>
        private void UpdateModelLinearElementsFromRobotFile(Model.Model model, IRobotCollection robotNodes, RobotConversionContext context)
        {
            RaiseMessage("Reading Bars...");

            //Delete all previously mapped linear elements:
            if (context.Options.DeleteObjects) context.IDMap.AllMappedLinearElements(model).DeleteAll();

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

                    // Section
                    if (bar.HasLabel(IRobotLabelType.I_LT_BAR_SECTION) != 0)
                    {
                        string sectionID = bar.GetLabelName(IRobotLabelType.I_LT_BAR_SECTION);
                        element.Family = context.IDMap.GetMappedSectionFamily(sectionID, model);
                    }
                    else element.Family = null; // Clear family

                    // Releases
                    if (bar.HasLabel(IRobotLabelType.I_LT_BAR_RELEASE) != 0)
                    {
                        IRobotLabel rlsLabel = bar.GetLabel(IRobotLabelType.I_LT_BAR_RELEASE);
                        RobotBarReleaseData data = rlsLabel.Data;
                        element.Start.SetReleases(ROBtoFB.Convert(data.StartNode));
                        element.End.SetReleases(ROBtoFB.Convert(data.EndNode));
                    }
                    else
                    {
                        // No releases...
                        element.Start.ClearReleases();
                        element.End.ClearReleases();
                    }

                    //TODO: Copy over more data

                    //Store mapping:
                    context.IDMap.Add(element, bar);
                }
            }
        }

        private void UpdateModelPanelElementsFromRobotFile(Model.Model model, IRobotCollection robotNodes, RobotConversionContext context)
        {
            RaiseMessage("Reading Slabs...");

            //Delete all previously mapped panel elements:
            if (context.Options.DeleteObjects) context.IDMap.AllMappedPanelElements(model).DeleteAll();

            IRobotCollection objs = Robot.Project.Structure.Objects.GetAll();
            for (int i = 1; i <= objs.Count; i++)
            {
                RobotObjObject rOO = objs.Get(i);
                if (rOO != null && rOO.Host < 1) // && rOO.StructuralType == IRobotObjectStructuralType.I_OST_SLAB
                {
                    RobotGeoObject geometry = rOO.Main.Geometry;
                    Curve border = ROBtoFB.Convert(geometry);
                    if (border != null)
                    {
                        PlanarRegion region = new PlanarRegion(border);
                        RobotSelection holes = rOO.GetHostedObjects();
                        if (holes != null && holes.Count > 0)
                        {
                            for (int j = 1; j <= holes.Count; j++)
                            {
                                // TODO
                            }
                        }

                        PanelElement pEl = context.IDMap.GetMappedPanelElement(rOO, model);
                        if (pEl == null) //Create new element
                            pEl = model.Create.PanelElement(region, context.ExInfo);
                        else //Exising mapped element found
                        {
                            pEl.Geometry = region;
                            pEl.Undelete();
                        }
                        if (rOO.HasLabel(IRobotLabelType.I_LT_PANEL_THICKNESS) != 0)
                        {
                            //Assign Build-up family
                            pEl.Family = context.IDMap.GetMappedPanelFamily(rOO.GetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS), model);
                        }
                        // TODO: Copy over more data

                        // Store mapping:
                        context.IDMap.Add(pEl, rOO);
                    }
                }
            }
        }

        /// <summary>
        /// Update the load cases in a Nucleus model to match those in a Robot file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        private void UpdateModelLoadsFromRobotFile(Model.Model model, RobotConversionContext context)
        {
            RaiseMessage("Reading Loads...");

            RobotCaseCollection cases = Robot.Project.Structure.Cases.GetAll();
            for (int i = 1; i <= cases.Count; i++)
            {
                IRobotCase rCase = cases.Get(i);
                LoadCase lCase = context.IDMap.GetMappedLoadCase(rCase.Number.ToString(), model);
                if (lCase == null) //Create new load case
                    lCase = model.Create.LoadCase(rCase.Name, context.ExInfo);
                else
                {
                    lCase.Name = rCase.Name;
                    lCase.Undelete();
                }
                // TODO: Copy over more data
                if (rCase is RobotSimpleCase)
                {
                    RobotSimpleCase sCase = (RobotSimpleCase)rCase;
                    var records = sCase.Records;
                    for (int j = 1; j <= records.Count; j++)
                    {
                        RobotLoadRecord record = (RobotLoadRecord)records.Get(i);
                        Load load = context.IDMap.GetMappedLoad(record.UniqueId.ToString(), model);
                        if (record.Type == IRobotLoadRecordType.I_LRT_NODE_FORCE)
                        {
                            //TODO: Create node load
                        }
                        else if (record.Type == IRobotLoadRecordType.I_LRT_BAR_UNIFORM)
                        {
                            //TODO: Create UDL
                        }
                        else if (record.Type == IRobotLoadRecordType.I_LRT_UNIFORM)
                        {
                            //TODO: Panel load
                        }
                        else if (record.Type == IRobotLoadRecordType.I_LRT_IN_CONTOUR)
                        {
                            //TODO: Area load?
                        }
                    }
                }

                // Store mapping:
                context.IDMap.Add(lCase, rCase);
            }
        }

        #endregion

        #region Nucleus to Robot

        /// <summary>
        /// Save a Nucleus model to a Robot file at the specified location
        /// </summary>
        /// <param name="filePath">The filePath of the Robot file to be written to</param>
        /// <param name="model">The model to be written from</param>
        /// <param name="idMap">The ID mapping table to be used.  If null, an empty table will automatically be initialised.
        /// Mapping data between the Robot and Nucleus model will be written into this map - store it for later if you wish
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
        /// Update a Robot file from a Nucleus model at the specified location
        /// </summary>
        /// <param name="filePath">The filePath of the Robot file to be written to</param>
        /// <param name="model">The model to be written from</param>
        /// <param name="idMap">The ID mapping table to be used.  If null, an empty table will automatically be initialised.
        /// Mapping data between the Robot and Nucleus model will be written into this map - store it for later if you wish
        /// to synchronise the models in future.</param>
        /// <param name="options">The conversion options.  If null, the default options will be used.</param>
        /// <returns></returns>
        public bool UpdateRobotFromModel(FilePath filePath, Model.Model model, ref RobotIDMappingTable idMap, RobotConversionOptions options = null)
        {
            if (Open(filePath) || New())
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
        /// Update a robot file based on a Nucleus model 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool UpdateRobotFromModel(Model.Model model, RobotConversionContext context)
        {
            RaiseMessage("Writing data to Robot...");

            if (context.Options.Nodes)
            {
                NodeCollection nodes = model.Nodes;
                if (context.Options.Update) nodes = nodes.Modified(context.Options.UpdateSince);
                if (nodes.Count > 0) RaiseMessage("Writing Nodes...");
                UpdateRobotNodesFromModel(model, nodes, context);
            }

            if (context.Options.Families)
            {
                FamilyCollection properties = model.Families;
                if (context.Options.Update) properties = properties.Modified(context.Options.UpdateSince);
                if (properties.Count > 0) RaiseMessage("Writing Properties...");
                UpdateRobotPropertiesFromModel(model, properties, context);
            }

            if (context.Options.LinearElements)
            {
                LinearElementCollection linearElements = model.Elements.LinearElements;
                if (context.Options.Update) linearElements = linearElements.Modified(context.Options.UpdateSince);
                if (linearElements.Count > 0) RaiseMessage("Writing Bars...");
                UpdateRobotBarsFromModel(model, linearElements, context);
            }

            if (context.Options.PanelElements)
            {
                PanelElementCollection panelElements = model.Elements.PanelElements;
                if (context.Options.Update) panelElements = panelElements.Modified(context.Options.UpdateSince);
                if (panelElements.Count > 0) RaiseMessage("Writing Panels...");
                UpdateRobotPanelsFromModel(model, panelElements, context);
            }

            RaiseMessage("Data writing completed.");
            return true;
        }

        /// <summary>
        /// Update the nodes in the open Robot model from those in a Nucleus model
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
        /// Update the section, face properties etc. in the open Robot model from those in a Nucleus model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="properties"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool UpdateRobotPropertiesFromModel(Model.Model model, FamilyCollection properties, RobotConversionContext context)
        {
            foreach (Family property in properties)
            {
                if (property is SectionFamily)
                {
                    UpdateRobotSection((SectionFamily)property, context);
                }
                else if (property is BuildUpFamily)
                {
                    UpdateRobotThickness((BuildUpFamily)property, context);
                }
            }
            return true;
        }

        /// <summary>
        /// Update the bars in the open Robot model from those in a Nucleus model
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
        /// Update the panel elements in the open Robot model from those in a Nucleus model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="panelElements"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool UpdateRobotPanelsFromModel(Model.Model model, PanelElementCollection panelElements, RobotConversionContext context)
        {
            foreach (PanelElement element in panelElements)
            {
                UpdateRobotPanel(element, context);
            }
            return true;
        }

        private bool UpdateRobotLoadCasesFromModel(Model.Model model, LoadCaseCollection loadCases, RobotConversionContext context)
        {
            //TODO
            return false;
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
        /// Create a new panel object within the currently open Robot document
        /// </summary>
        /// <param name="id">Optional.  The ID number to be assigned to the object.
        /// If omitted or lower than 1, the next free number will be used.</param>
        /// <returns></returns>
        public IRobotObjObject CreateRobotPanel(Curve perimeter, int id = -1)
        {
            if (id < 1) id = Robot.Project.Structure.Objects.FreeNumber;
            Robot.Project.Structure.Objects.CreateContour(id, FBtoROB.Convert(perimeter.Facet(Angle.FromDegrees(10))));
            return (RobotObjObject)Robot.Project.Structure.Objects.Get(id);
            //return Robot.Project.Structure.Objects.Create(id);
        }

        /// <summary>
        /// Get an offset label for the start and end offsets of an element.
        /// This will reuse any that already exist with the appropriate values
        /// or create a new label if necessary.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IRobotLabel GetOffset(Vector start, Vector end)
        {
            if (start.IsZero() && end.IsZero()) return null;
            else
            {
                int hash = start.GetHashCode() ^ end.GetHashCode();
                RobotLabelServer labels = Robot.Project.Structure.Labels;
                string name = hash.ToString();
                if (labels.Exist(IRobotLabelType.I_LT_BAR_OFFSET, name) != 0)
                {
                    return labels.Get(IRobotLabelType.I_LT_BAR_OFFSET, name);
                }
                else
                {
                    IRobotLabel label = labels.Create(IRobotLabelType.I_LT_BAR_OFFSET, name);
                    RobotBarOffsetData data = label.Data;
                    data.Start.UX = start.X;
                    data.Start.UY = start.Y;
                    data.Start.UZ = start.Z;
                    data.End.UX = end.X;
                    data.End.UY = end.Y;
                    data.End.UZ = end.Z;
                    return label;
                }
            }
        }

        /// <summary>
        /// Get (or create) the Robot support label that describes the given fixity conditions
        /// </summary>
        /// <param name="fixity"></param>
        /// <returns></returns>
        public IRobotLabel GetSupport(Bool6D fixity)
        {
            string name = fixity.ToRestraintDescription();
            RobotLabelServer labels = Robot.Project.Structure.Labels;
            if (labels.Exist(IRobotLabelType.I_LT_SUPPORT, name) != 0)
                return labels.Get(IRobotLabelType.I_LT_SUPPORT, name);
            else
            {
                IRobotLabel result = labels.Create(IRobotLabelType.I_LT_SUPPORT, name);
                RobotNodeSupportData data = result.Data;
                if (fixity.X) data.UX = 1;
                else data.UX = 0;
                if (fixity.Y) data.UY = 1;
                else data.UY = 0;
                if (fixity.Z) data.UZ = 1;
                else data.UZ = 0;
                if (fixity.XX) data.RX = 1;
                else data.RX = 0;
                if (fixity.YY) data.RY = 1;
                else data.RY = 0;
                if (fixity.ZZ) data.RZ = 1;
                else data.RZ = 0;
                labels.Store(result);
                return result;
            }
        }

        /// <summary>
        /// Retrieve or create a Robot release label for the specified element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public IRobotLabel GetReleases(Element element)
        {
            if (element is LinearElement)
            {
                var lEl = (LinearElement)element;
                ElementVertex start = lEl.Start;
                ElementVertex end = lEl.End;
                Bool6D sRls = start.Releases;
                Bool6D eRls = end.Releases;
                if (!sRls.AllFalse || !eRls.AllFalse)
                {
                    string name = sRls.ToRestraintDescription() + " - " + eRls.ToRestraintDescription();
                    RobotLabelServer labels = Robot.Project.Structure.Labels;
                    if (labels.Exist(IRobotLabelType.I_LT_BAR_RELEASE, name) != 0)
                    {
                        IRobotLabel label = labels.Get(IRobotLabelType.I_LT_BAR_RELEASE, name);
                        return label;
                    }
                    else
                    {
                        IRobotLabel result = labels.Create(IRobotLabelType.I_LT_BAR_RELEASE, name);
                        RobotBarReleaseData data = result.Data;
                        if (sRls.X)
                            data.StartNode.UX = IRobotBarEndReleaseValue.I_BERV_STD;
                        if (sRls.Y)
                            data.StartNode.UY = IRobotBarEndReleaseValue.I_BERV_STD;
                        if (sRls.Z)
                            data.StartNode.UZ = IRobotBarEndReleaseValue.I_BERV_STD;
                        if (sRls.XX)
                            data.StartNode.RX = IRobotBarEndReleaseValue.I_BERV_STD;
                        if (sRls.YY)
                            data.StartNode.RY = IRobotBarEndReleaseValue.I_BERV_STD;
                        if (sRls.ZZ)
                            data.StartNode.RZ = IRobotBarEndReleaseValue.I_BERV_STD;
                        if (eRls.X)
                            data.EndNode.UX = IRobotBarEndReleaseValue.I_BERV_STD;
                        if (eRls.Y)
                            data.EndNode.UY = IRobotBarEndReleaseValue.I_BERV_STD;
                        if (eRls.Z)
                            data.EndNode.UZ = IRobotBarEndReleaseValue.I_BERV_STD;
                        if (eRls.XX)
                            data.EndNode.RX = IRobotBarEndReleaseValue.I_BERV_STD;
                        if (eRls.YY)
                            data.EndNode.RY = IRobotBarEndReleaseValue.I_BERV_STD;
                        if (eRls.ZZ)
                            data.EndNode.RZ = IRobotBarEndReleaseValue.I_BERV_STD;
                        //TODO: Check this is the correct way to do this
                        return result;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Update or create a robot node linked to the specified Nucleus node
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
                if (node.IsDeleted && context.Options.DeleteObjects)
                {
                    Robot.Project.Structure.Nodes.Delete(mappedID);
                    context.IDMap.Remove(node);
                }
                else if (Robot.Project.Structure.Nodes.Exist(mappedID) != 0)
                    rNode = Robot.Project.Structure.Nodes.Get(mappedID) as IRobotNode;
            }
            if (!node.IsDeleted)
            {
                if (rNode == null)
                {
                    rNode = CreateRobotNode(node.Position.X, node.Position.Y, node.Position.Z, mappedID);
                }
                else
                {
                    rNode.SetPosition(node.Position);
                }

                // Support conditions:
                NodeSupport support = node.GetData<NodeSupport>();
                if (support != null && !support.Fixity.AllFalse) // Set support
                    rNode.SetLabel(IRobotLabelType.I_LT_SUPPORT, GetSupport(support.Fixity).Name);
                else if (rNode.HasLabel(IRobotLabelType.I_LT_SUPPORT) != 0) // Remove existing support
                    rNode.RemoveLabel(IRobotLabelType.I_LT_SUPPORT);
                //TODO: Moar Data!

                //Store mapping:
                context.IDMap.Add(node, rNode);
            }
            return rNode;
        }

        /// <summary>
        /// Update or create a robot bar linked to the specified Nucleus element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IRobotBar UpdateRobotBar(LinearElement element, RobotConversionContext context)
        {
            int mappedID = -1;
            IRobotBar bar = null;

            if (context.IDMap.HasSecondID(context.IDMap.BarCategory, element.GUID))
            {
                mappedID = int.Parse(context.IDMap.GetSecondID(context.IDMap.BarCategory, element.GUID));
                if (element.IsDeleted && context.Options.DeleteObjects)
                {
                    Robot.Project.Structure.Bars.Delete(mappedID);
                    context.IDMap.Remove(element);
                }
                else if (Robot.Project.Structure.Bars.Exist(mappedID) != 0)
                    bar = Robot.Project.Structure.Bars.Get(mappedID) as IRobotBar;
            }
            if (!element.IsDeleted)
            {
                Node startNode = element.StartNode; //TODO: Make bulletproof!
                Node endNode = element.EndNode; //TODO: Make bulletproof!
                int nodeID0 = GetMappedNodeID(startNode, context);
                int nodeID1 = GetMappedNodeID(endNode, context);

                if (bar == null)
                {
                    bar = CreateRobotBar(nodeID0, nodeID1, mappedID);
                }
                else
                {
                    bar.StartNode = nodeID0;
                    bar.EndNode = nodeID1;
                }

                // Orientation:
                bar.Gamma = element.Orientation.Degrees;

                // Section:
                if (element.Family != null)
                {
                    bar.SetLabel(IRobotLabelType.I_LT_BAR_SECTION, this.GetMappedSectionID(element.Family, context));
                }

                // Offsets:
                if (element.Geometry != null && element.Geometry.Vertices.HasNodalOffsets)
                {
                    IRobotLabel offsets = GetOffset(element.Start.Offset, element.End.Offset);
                    bar.SetLabel(IRobotLabelType.I_LT_BAR_OFFSET, offsets.Name);
                }
                else
                {
                    bar.RemoveLabel(IRobotLabelType.I_LT_BAR_OFFSET);
                }

                // Releases:
                IRobotLabel rlsLabel = GetReleases(element);
                if (rlsLabel != null) bar.SetLabel(IRobotLabelType.I_LT_BAR_RELEASE, rlsLabel.Name);
                else bar.RemoveLabel(IRobotLabelType.I_LT_BAR_RELEASE);

                //TODO: More data

                context.IDMap.Add(element, bar);
            }

            return bar;
        }

        /// <summary>
        /// Update or create a Robot panel object linked to the specified Nucleus element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IRobotObjObject UpdateRobotPanel(PanelElement element, RobotConversionContext context)
        {
            int mappedID = -1;
            IRobotObjObject obj = null;

            if (context.IDMap.HasSecondID(context.IDMap.PanelCategory, element.GUID))
            {
                mappedID = int.Parse(context.IDMap.GetSecondID(context.IDMap.PanelCategory, element.GUID));
                if (element.IsDeleted)
                {
                    Robot.Project.Structure.Objects.Delete(mappedID);
                    context.IDMap.Remove(element);
                }
                else if (Robot.Project.Structure.Objects.Exist(mappedID) != 0)
                    obj = Robot.Project.Structure.Objects.Get(mappedID) as IRobotObjObject;
            }
            if (!element.IsDeleted)
            {
                if (obj == null)
                {
                    var pR = (PlanarRegion)element.Geometry;
                    if (element.Geometry is PlanarRegion)
                    {
                        obj = CreateRobotPanel(pR.Perimeter, mappedID);
                    }
                    else if (element.Geometry is Mesh)
                    {
                        //TODO: Meshes!
                    }
                }

                if (obj != null)
                {
                    //TODO: Setup geometry:
                    /*if (element.Geometry is PlanarRegion)
                    {
                        var pR = (PlanarRegion)element.Geometry;
                        obj.Main.Geometry = FBtoROB.Convert(pR.Perimeter);
                    }*/

                    obj.Main.Attribs.Meshed = 1; //True
                    if (element.Family != null)
                    {
                        obj.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, GetMappedThicknessID(element.Family, context));
                    }

                    Vector dir = new Vector(element.Orientation);
                    obj.Main.Attribs.SetDirX(IRobotObjLocalXDirDefinitionType.I_OLXDDT_CARTESIAN, dir.X, dir.Y, dir.Z);
                    
                    obj.Initialize();
                    obj.Update();

                    context.IDMap.Add(element, obj);
                }
            }
            else if (obj != null && context.Options.DeleteObjects)
            {
                Robot.Project.Structure.Objects.Delete(obj.Number);
            }
            return obj;
        }

        /// <summary>
        /// Update or create a Robot section linked to a Nucleus section property
        /// </summary>
        /// <param name="section"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IRobotLabel UpdateRobotSection(SectionFamily section, RobotConversionContext context)
        {
            string mappedID;
            IRobotLabel label = null;
            if (context.IDMap.HasSecondID(context.IDMap.SectionCategory, section.GUID))
            {
                mappedID = context.IDMap.GetSecondID(context.IDMap.SectionCategory, section.GUID);
                if (section.IsDeleted)
                {
                    Robot.Project.Structure.Labels.Delete(IRobotLabelType.I_LT_BAR_SECTION, mappedID);
                }
                else if (Robot.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_BAR_SECTION, mappedID) != 0)
                    label = Robot.Project.Structure.Labels.Get(IRobotLabelType.I_LT_BAR_SECTION, mappedID) as IRobotLabel;
                //label = Robot.Project.Structure.Labels.FindWithId(mappedID);
            }
            if (!section.IsDeleted)
            {
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
            }

            return label;
        }

        /// <summary>
        /// Update the values stored in a Robot section data to match those in a Nucleus profile
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
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_H, rProfile.Depth - rProfile.FlangeThickness * 2);
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
                else if (profile is AngleProfile) //I Section
                {
                    var rProfile = (SymmetricIProfile)profile;
                    data.Type = IRobotBarSectionType.I_BST_NS_L;
                    data.ShapeType = IRobotBarSectionShapeType.I_BSST_UUAP;
                    RobotBarSectionNonstdData nsdata = data.CreateNonstd(0);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_H, rProfile.Depth - rProfile.FlangeThickness);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_B, rProfile.Width);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_TF, rProfile.FlangeThickness);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_TW, rProfile.WebThickness);
                }
                else if (profile is ChannelProfile)
                {
                    var rProfile = (ChannelProfile)profile;
                    data.Type = IRobotBarSectionType.I_BST_NS_C;
                    data.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_C_SHAPE;
                    RobotBarSectionNonstdData nsdata = data.CreateNonstd(0);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_H, rProfile.Depth - rProfile.FlangeThickness * 2);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_B, rProfile.Width);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TF, rProfile.FlangeThickness);
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TW, rProfile.WebThickness);
                }
                //TODO: Offset?

            }
        }

        /// <summary>
        /// Create a Nucleus section profile geometry from Robot section data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public SectionProfile CreateProfileFromRobotSectionData(RobotBarSectionData data)
        {
            SectionProfile result = null;
            if (data != null)
            {
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
                else if (equivalent == typeof(AngleProfile))
                {
                    var angleProfile = new AngleProfile();
                    angleProfile.Depth = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                    angleProfile.Width = data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
                    angleProfile.FlangeThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
                    angleProfile.WebThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
                    //TODO: Fillet radius
                    angleProfile.RootRadius = data.GetValue(IRobotBarSectionDataValue.I_BSDV_RA); //????
                    result = angleProfile;
                }

                if (result != null && data.Type == IRobotBarSectionType.I_BST_STANDARD)
                {
                    result.CatalogueName = data.Name;
                }
            }

            return result; //Profile could not be created
        }

        /// <summary>
        /// Get the equivalent Nucleus Profile subtype for the specified value of the Robot
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
            else if (type == IRobotBarSectionShapeType.I_BSST_USER_C_SHAPE
                || type == IRobotBarSectionShapeType.I_BSST_CCL 
                || type == IRobotBarSectionShapeType.I_BSST_URND
                || type == IRobotBarSectionShapeType.I_BSST_COLD_C_PLUS
                || type == IRobotBarSectionShapeType.I_BSST_CONCR_COL_C)
                return typeof(ChannelProfile);
            else
                return null;
        }

        /// <summary>
        /// Update or create a Robot Thickness property linked to a Nucleus panel family
        /// </summary>
        /// <param name="family"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IRobotLabel UpdateRobotThickness(BuildUpFamily family, RobotConversionContext context)
        {
            string mappedID;
            IRobotLabel label = null;
            if (context.IDMap.HasSecondID(context.IDMap.ThicknessCategory, family.GUID))
            {
                mappedID = context.IDMap.GetSecondID(context.IDMap.ThicknessCategory, family.GUID);
                if (family.IsDeleted)
                {
                    Robot.Project.Structure.Labels.Delete(IRobotLabelType.I_LT_PANEL_THICKNESS, mappedID);
                }
                else if (Robot.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_PANEL_THICKNESS, mappedID) != 0)
                    label = Robot.Project.Structure.Labels.Get(IRobotLabelType.I_LT_PANEL_THICKNESS, mappedID) as IRobotLabel;
            }
            if (!family.IsDeleted)
            {
                if (label == null)
                {
                    if (family.Name == null) family.Name = "Test"; //TEMP!
                    label = Robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_PANEL_THICKNESS, family.Name); //TODO: Enforce name uniqueness?
                }
            }

            RobotThicknessData rData = label.Data as RobotThicknessData;
            rData.ThicknessType = IRobotThicknessType.I_TT_HOMOGENEOUS; //TEMP
            IRobotThicknessHomoData homogeneousData = (IRobotThicknessHomoData)rData.Data;
            homogeneousData.Type = IRobotThicknessHomoType.I_THT_CONSTANT;
            homogeneousData.ThickConst = family.Layers.TotalThickness;

            context.IDMap.Add(family, label);

            Robot.Project.Structure.Labels.Store(label);

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
        public string GetMappedSectionID(SectionFamily section, RobotConversionContext context)
        {
            if (context.IDMap.HasSecondID(context.IDMap.SectionCategory, section.GUID))
                return context.IDMap.GetSecondID(context.IDMap.SectionCategory, section.GUID);
            else
                return UpdateRobotSection(section, context).Name;
        }

        /// <summary>
        /// Retrieve a mapped Robot thickness label name for the specified family.
        /// A new thickness in Robot will be generated if nothing is currently mapped.
        /// </summary>
        /// <param name="family"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetMappedThicknessID(BuildUpFamily family, RobotConversionContext context)
        {
            if (context.IDMap.HasSecondID(context.IDMap.ThicknessCategory, family.GUID))
                return context.IDMap.GetSecondID(context.IDMap.ThicknessCategory, family.GUID);
            else
                return UpdateRobotThickness(family, context).Name;
        }

        #endregion

        #region Get ID Methods

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
        /// Extract a list of all storey names in the currently open project
        /// </summary>
        /// <returns></returns>
        public IList<string> AllStoreyNames()
        {
            RobotStoreyMngr storeys = Robot.Project.Structure.Storeys;
            IList<string> result = new List<string>(storeys.Count);
            for (int i = 1; i <= storeys.Count; i++)
            {
                RobotStorey storey = storeys.Get(i);
                if (storey != null) result.Add(storey.Name);
                
            }
            return result;
        }

        #endregion

        #region Extract Individual Data Items By Type

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

        #endregion
    }
}