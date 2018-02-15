using Nucleus.Base;
using Nucleus.Conversion;
using Nucleus.Extensions;
using Nucleus.Geometry;
using Nucleus.IO;
using Nucleus.Maths;
using Nucleus.Model;
using Nucleus.Model.Loading;
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
    public class RobotController : MessageRaiser, IApplicationClient
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
                ReadSections(model, context);
                ReadBuildUps(model, context);
            }
            if (options.Nodes) ReadNodes(model, robotNodes, context);
            if (options.LinearElements) UpdateModelLinearElementsFromRobotFile(model, robotNodes, context);
            if (options.PanelElements) UpdateModelPanelElementsFromRobotFile(model, robotNodes, context);
            if (options.Sets) UpdateModelSetsFromRobotFile(model, context);
            if (options.Loading) ReadLoads(model, context);
            RaiseMessage("Data reading completed.");
            return false;
        }

        /// <summary>
        /// Update nodes in a Nucleus model by loading data from the currently open Robot file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="map"></param>
        /// <param name="context"></param>
        private void ReadNodes(Model.Model model, IRobotCollection robotNodes, RobotConversionContext context)
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
        private void ReadSections(Model.Model model, RobotConversionContext context)
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
                    section.Name = label.Name;
                    section.Profile = CreateProfileFromRobotSectionData(data);

                    //Store mapping data:
                    context.IDMap.Add(section, label);
                }
            }
        }

        private void ReadBuildUps(Model.Model model, RobotConversionContext context)
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
                    Curve geometry = ROBtoFB.GeometryOf(bar, Robot.Project.Structure.Nodes);
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

                    // Orientation
                    element.Orientation = Angle.FromDegrees(bar.Gamma);

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
            RaiseMessage("Reading Panels...");

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

                        // Orientation:
                        double oX, oY, oZ;
                        rOO.Main.Attribs.GetDirX(out oX, out oY, out oZ);
                        pEl.OrientateToVector(new Vector(oX, oY, oZ));

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
        private void ReadLoads(Model.Model model, RobotConversionContext context)
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
                        if (record.Type == IRobotLoadRecordType.I_LRT_NODE_FORCE) // Node load
                        {
                            NodeLoad nLoad = load as NodeLoad;
                            if (nLoad == null)
                                nLoad = model.Create.NodeLoad(lCase);
                            else
                                nLoad.Case = lCase;

                            // Load value, direction, axis:
                            Vector force = new Vector
                                (
                                record.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_FX),
                                record.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_FY),
                                record.GetValue((short)IRobotNodeForceRecordValues.I_NFRV_FZ));
                            nLoad.SetForce(force);

                            // Nodes applied to:
                            RobotSelection appliedTo = record.Objects;
                            nLoad.AppliedTo.Clear();
                            for (int k = 1; k <= appliedTo.Count; k++)
                            {
                                Node node = context.IDMap.GetMappedModelNode(appliedTo.Get(k), model);
                                if (node != null) nLoad.AppliedTo.Add(node);
                            }

                            context.IDMap.Add(nLoad, record);
                        }
                        else if (record.Type == IRobotLoadRecordType.I_LRT_DEAD)//?
                        {
                            GravityLoad gLoad = load as GravityLoad;
                            if (gLoad == null)
                                gLoad = model.Create.GravityLoad(lCase);
                            else
                                gLoad.Case = lCase;

                            // Load value:
                            Vector force = new Vector(
                            record.GetValue((short)IRobotDeadRecordValues.I_DRV_X),
                             record.GetValue((short)IRobotDeadRecordValues.I_DRV_Y),
                              record.GetValue((short)IRobotDeadRecordValues.I_DRV_Z));
                            gLoad.SetForce(force);

                            //TODO: Entire model option
                            // Elements applied to:
                            RobotSelection appliedTo = record.Objects;
                            gLoad.AppliedTo.Clear();
                            for (int k = 1; k <= appliedTo.Count; k++)
                            {
                                LinearElement element = context.IDMap.GetMappedLinearElement(appliedTo.Get(k), model);
                                if (element != null) gLoad.AppliedTo.Add(element);
                            }

                            context.IDMap.Add(gLoad, record);
                        }
                        else if (record.Type == IRobotLoadRecordType.I_LRT_BAR_UNIFORM)
                        {
                            // Create UDL
                            LinearElementLoad eLoad = load as LinearElementLoad;
                            if (eLoad == null)
                                eLoad = model.Create.LinearElementLoad(lCase);
                            else
                                eLoad.Case = lCase;

                            // Load value:
                            Vector force = new Vector(
                            record.GetValue((short)IRobotUniformRecordValues.I_URV_PX),
                             record.GetValue((short)IRobotUniformRecordValues.I_URV_PY),
                              record.GetValue((short)IRobotUniformRecordValues.I_URV_PZ));
                            eLoad.SetForce(force);
                            eLoad.SetUniform();

                            // Elements applied to:
                            RobotSelection appliedTo = record.Objects;
                            eLoad.AppliedTo.Clear();
                            for (int k = 1; k <= appliedTo.Count; k++)
                            {
                                LinearElement element = context.IDMap.GetMappedLinearElement(appliedTo.Get(k), model);
                                if (element != null) eLoad.AppliedTo.Add(element);
                            }

                            context.IDMap.Add(eLoad, record);

                        }
                        else if (record.Type == IRobotLoadRecordType.I_LRT_UNIFORM)
                        {
                            //TODO: Panel load
                        }
                        else if (record.Type == IRobotLoadRecordType.I_LRT_IN_CONTOUR)
                        {
                            //TODO: Area load?
                            // Create area load
                            /*AreaLoad aLoad = load as AreaLoad;
                            if (aLoad == null) aLoad = model.Create.AreaLoad(lCase);
                            else aLoad.Case = lCase;

                            double pressure = record.GetValue((short)IRobotInContourRecordValues.I_ICRV_PX1); //?

                            aLoad.Value = pressure;

                            // Elements applied to:
                            RobotSelection appliedTo = record.Objects;
                            aLoad.AppliedTo.Clear();
                            for (int k = 1; k <= appliedTo.Count; k++)
                            {
                                PanelElement element = context.IDMap.GetMappedPanelElement(appliedTo.Get(k), model);
                                if (element != null) aLoad.AppliedTo.Add(element);
                            }

                            context.IDMap.Add(aLoad, record);*/
                        }
                        else if (record.Type == IRobotLoadRecordType.I_LRT_PRESSURE)
                        {
                            // Area load?
                            // Create area load
                            PanelLoad aLoad = load as PanelLoad;
                            if (aLoad == null) aLoad = model.Create.PanelLoad(lCase);
                            else aLoad.Case = lCase;

                            double pressure = record.GetValue((short)IRobotPressureRecordValues.I_PRV_P);

                            aLoad.Value = pressure;

                            // Elements applied to:
                            RobotSelection appliedTo = record.Objects;
                            aLoad.AppliedTo.Clear();
                            for (int k = 1; k <= appliedTo.Count; k++)
                            {
                                PanelElement element = context.IDMap.GetMappedPanelElement(appliedTo.Get(k), model);
                                if (element != null) aLoad.AppliedTo.Add(element);
                            }

                            context.IDMap.Add(aLoad, record);
                        }
                    }
                }

                // Store mapping:
                context.IDMap.Add(lCase, rCase);
            }
        }

        private void UpdateModelSetsFromRobotFile(Model.Model model, RobotConversionContext context)
        {
            RaiseMessage("Reading Groups...");

            // Delete all previously mapped sets ?
            RobotGroupServer groups = Robot.Project.Structure.Groups;
            foreach (var groupType in new IRobotObjectType[] { IRobotObjectType.I_OT_BAR, IRobotObjectType.I_OT_NODE })
            {
                for (int i = 1; i <= groups.GetCount(groupType); i++)
                {
                    RobotGroup group = groups.Get(groupType, i);
                    if (group != null)
                    {
                        IModelObjectSet set = null;
                        if (groupType == IRobotObjectType.I_OT_BAR)
                            set = new LinearElementSet();
                        else if (groupType == IRobotObjectType.I_OT_NODE)
                            set = new NodeSet();
                        else if (groupType == IRobotObjectType.I_OT_PANEL)
                            set = new PanelElementSet();

                        if (set != null)
                        {
                            try
                            {
                                set.Name = group.Name;
                                var ids = group.SelList.ToIDSet();
                                foreach (int id in ids)
                                {
                                    ModelObject item = context.IDMap.GetMapped(groupType, id.ToString(), model);
                                    if (item != null) set.Add(item);
                                }
                                model.Sets.TryAdd(set);
                                context.IDMap.Add(set, i);
                            }
                            catch (Exception ex)
                            {
                                RaiseMessage("Parsing group failed: " + ex.Message);
                            }
                        }
                    }
                }
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

            if (context.Options.Sets)
            {
                ModelObjectSetCollection sets = model.Sets;
                //if (context.Options.Update) sets = //TODO?
                if (sets.Count > 0) RaiseMessage("Writing Groups...");
                WriteGroups(sets, context);
            }

            if (context.Options.Loading)
            {
                var loadCases = model.LoadCases;
                //if (context.Options.Update) loadCases = //TODO?
                if (loadCases.Count > 0) RaiseMessage("Writing Loads...");
                WriteLoads(model, loadCases, context);
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

        /// <summary>
        /// Update the groups in the open Robot model from a collection of sets
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sets"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool WriteGroups(ModelObjectSetCollection sets, RobotConversionContext context)
        {
            foreach (var set in sets)
            {
                WriteGroup(set, context);
            }
            return true;
        }

        /// <summary>
        /// Update the load cases in the open Robot model from a collection of load cases
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loadCases"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool WriteLoads(Model.Model model, LoadCaseCollection loadCases, RobotConversionContext context)
        {
            foreach (var lc in loadCases)
            {
                WriteLoadCase(lc, context);
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
        public IRobotLabel GetOffset(Vector start, Vector end, SectionFamily section)
        {
            if (start.IsZero() && end.IsZero() && 
                (section?.Profile == null ||
                (!section.Profile.HorizontalSetOut.IsEdge()
                && !section.Profile.VerticalSetOut.IsEdge()))) return null;
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
                    if (section?.Profile != null)
                    {
                        data.Position = FBtoROB.Convert(section.Profile.HorizontalSetOut, section.Profile.VerticalSetOut);
                    }
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
                    string name = sRls.ToString("R","F") + "-" + eRls.ToString("R","F");
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
                        labels.Store(result);
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
                    IRobotLabel offsets = GetOffset(element.Start.Offset, element.End.Offset, element.Family);
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

                    Vector dir = element.LocalCoordinateSystem()?.X ?? Vector.Unset;
                    if (dir.IsValid())
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
                    nsdata.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, cProfile.Diameter); //?
                }
                else if (profile is AngleProfile) //L Section
                {
                    var rProfile = (AngleProfile)profile;
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
                    RobotBarSectionNonstdData nsdata = data.GetNonstd(1);
                    if (nsdata != null)
                    {
                        iProfile.FlangeThickness = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF);
                        iProfile.Depth = 2 * iProfile.FlangeThickness + nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H);
                        iProfile.Width = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B);
                        iProfile.WebThickness = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW);
                    }
                    else
                    { 
                        iProfile.Depth = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                        iProfile.Width = data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
                        iProfile.FlangeThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
                        iProfile.WebThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
                        iProfile.RootRadius = data.GetValue(IRobotBarSectionDataValue.I_BSDV_RA);
                    }
                    result = iProfile;
                }
                else if (equivalent == typeof(RectangularHollowProfile)) //Rectangular Hollow Profile
                {
                    var rProfile = new RectangularHollowProfile();
                    RobotBarSectionNonstdData nsdata = data.GetNonstd(1);
                    if (nsdata != null)
                    {
                        rProfile.FlangeThickness = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TF);
                        rProfile.Depth = 2 * rProfile.FlangeThickness + nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_H);
                        rProfile.Width = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_B);
                        rProfile.WebThickness = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TW);
                    }
                    else
                    {
                        rProfile.Depth = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                        rProfile.Width = data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
                        rProfile.FlangeThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
                        rProfile.WebThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
                    }
                    result = rProfile;
                }
                else if (equivalent == typeof(RectangularProfile)) //Filled Rectangular Profile
                {
                    var rProfile = new RectangularProfile();
                    RobotBarSectionNonstdData nsdata = data.GetNonstd(1);
                    if (nsdata != null)
                    {
                        rProfile.Depth = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H);
                        rProfile.Width = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B);
                    }
                    else
                    {
                        rProfile.Depth = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                        rProfile.Width = data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
                    }
                    result = rProfile;
                }
                else if (equivalent == typeof(CircularHollowProfile))
                {
                    var cProfile = new CircularHollowProfile();
                    RobotBarSectionNonstdData nsdata = data.GetNonstd(1);
                    if (nsdata != null)
                    {
                        cProfile.Diameter = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D);
                        cProfile.WallThickness = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T);
                    }
                    else
                    {
                        cProfile.Diameter = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                        cProfile.WallThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW); //?
                    }
                    result = cProfile;
                }
                else if (equivalent == typeof(CircularProfile))
                {
                    var cProfile = new CircularProfile();
                    RobotBarSectionNonstdData nsdata = data.GetNonstd(1);
                    if (nsdata != null)
                    {
                        cProfile.Diameter = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D); //?
                    }
                    else
                    { 
                        cProfile.Diameter = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                    }
                    result = cProfile;
                }
                else if (equivalent == typeof(AngleProfile))
                {
                    var angleProfile = new AngleProfile();
                    RobotBarSectionNonstdData nsdata = data.GetNonstd(1);
                    if (nsdata != null)
                    {
                        angleProfile.FlangeThickness = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_TF);
                        angleProfile.Depth = angleProfile.FlangeThickness + nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_H);
                        angleProfile.Width = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_B);
                        angleProfile.WebThickness = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_TW);
                    }
                    else
                    {
                        angleProfile.Depth = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                        angleProfile.Width = data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
                        angleProfile.FlangeThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
                        angleProfile.WebThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
                        angleProfile.RootRadius = data.GetValue(IRobotBarSectionDataValue.I_BSDV_RA);
                    }
                    result = angleProfile;
                }
                else if (equivalent == typeof(ChannelProfile))
                {
                    var cProfile = new ChannelProfile();
                    RobotBarSectionNonstdData nsdata = data.GetNonstd(1);
                    if (nsdata != null)
                    {
                        cProfile.FlangeThickness = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TF);
                        cProfile.Depth = 2 * cProfile.FlangeThickness + nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_H);
                        cProfile.Width = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_B);
                        cProfile.WebThickness = nsdata.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TW);
                    }
                    else
                    {
                        cProfile.Depth = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                        cProfile.Width = data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
                        cProfile.FlangeThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
                        cProfile.WebThickness = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
                        cProfile.RootRadius = data.GetValue(IRobotBarSectionDataValue.I_BSDV_RA);
                    }
                    result = cProfile;
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
                || type == IRobotBarSectionShapeType.I_BSST_TREC
                || type == IRobotBarSectionShapeType.I_BSST_TRND
                || type == IRobotBarSectionShapeType.I_BSST_BOX)
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
            else if (type == IRobotBarSectionShapeType.I_BSST_UUAP)
                return typeof(AngleProfile);
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
                    if (family.Name == null) family.Name = "[Unnamed]"; //TEMP!
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
        /// Get the type (in Robot terms) that the specified set contains
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public IRobotObjectType GroupTypeOf(ModelObjectSetBase set)
        {
            if (set is LinearElementSet) return IRobotObjectType.I_OT_BAR;
            else if (set is PanelElementSet) return IRobotObjectType.I_OT_PANEL;
            else if (set is ElementSet)
            {
                var elSet = (ElementSet)set;
                if (elSet.Items.IsAllPanels)
                    return IRobotObjectType.I_OT_PANEL;
                else
                    return IRobotObjectType.I_OT_BAR;
            }
            else if (set is NodeSet) return IRobotObjectType.I_OT_NODE;
            else return IRobotObjectType.I_OT_UNDEFINED;
        }

        /// <summary>
        /// Update or create a Robot Group by a Nucleus Set
        /// </summary>
        /// <param name="set"></param>
        /// <param name="context"></param>
        public void WriteGroup(ModelObjectSetBase set, RobotConversionContext context)
        {
            string mappedID;
            RobotGroup group = null;
            RobotGroupServer groups = Robot.Project.Structure.Groups;
            IRobotObjectType groupType = GroupTypeOf(set);
            int intID = -1;
            if (context.IDMap.HasSecondID(context.IDMap.SetsCategory, set.GUID))
            {
                mappedID = context.IDMap.GetSecondID(context.IDMap.SetsCategory, set.GUID);
                intID = int.Parse(mappedID);
                if (set.IsDeleted)
                {
                    if (context.Options.DeleteObjects) groups.Delete(groupType, intID);
                }
                else group = groups.Get(groupType, intID);
            }
            if (!set.IsDeleted)
            {
                if (group == null)
                    intID = groups.Create(groupType, set.Name, context.IDMap.ToIDString(set));
                else
                    group.SelList = context.IDMap.ToIDString(set);
            }

            context.IDMap.Add(set, intID);
        }

        /// <summary>
        /// Write a load case to Robot
        /// </summary>
        /// <param name="lCase"></param>
        /// <param name="context"></param>
        public void WriteLoadCase(LoadCase lCase, RobotConversionContext context)
        {
            string mappedID;
            IRobotCase rCase = null;
            int intID = -1;
            if (context.IDMap.HasSecondID(context.IDMap.CaseCategory, lCase.GUID))
            {
                mappedID = context.IDMap.GetSecondID(context.IDMap.CaseCategory, lCase.GUID);
                intID = int.Parse(mappedID);
                if (lCase.IsDeleted)
                {
                    if (context.Options.DeleteObjects)
                        Robot.Project.Structure.Cases.Delete(intID);
                }
                else rCase = Robot.Project.Structure.Cases.Get(intID);
            }
            if (!lCase.IsDeleted)
            {
                if (rCase == null)
                {
                    intID = Robot.Project.Structure.Cases.FreeNumber;
                    rCase = Robot.Project.Structure.Cases.CreateSimple(
                        intID, lCase.Name, IRobotCaseNature.I_CN_PERMANENT,
                        IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
                }
                //TODO: Populate nature + type better
                else
                {
                    rCase.Name = lCase.Name;
                }
            }
            context.IDMap.Add(lCase, rCase);

            if (rCase != null && rCase is RobotSimpleCase)
            {
                LoadCollection loads = lCase.Loads();
                foreach (var load in loads)
                {
                    WriteLoad(load, (RobotSimpleCase)rCase, context);
                }
            }
        }

        /// <summary>
        /// Write a Nucleus load to Robot
        /// </summary>
        /// <param name="load"></param>
        /// <param name="rCase"></param>
        /// <param name="context"></param>
        public void WriteLoad(Load load, RobotSimpleCase rCase, RobotConversionContext context)
        {
            
            string mappedID;
            RobotLoadRecord lRecord = null;
            int intID = -1;
            if (context.IDMap.HasSecondID(context.IDMap.LoadCategory, load.GUID))
            {
                mappedID = context.IDMap.GetSecondID(context.IDMap.LoadCategory, load.GUID);
                intID = int.Parse(mappedID);
                if (load.IsDeleted)
                {
                    if (context.Options.DeleteObjects)
                        rCase.Records.Delete(intID);
                }
                else lRecord = rCase.Records.Get(intID) as RobotLoadRecord; //TODO: Get by uniqueID instead!
            }
            if (!load.IsDeleted)
            {
                IRobotLoadRecordType type;
                if (load is NodeLoad) // Node point load
                {
                    NodeLoad nLoad = (NodeLoad)load;
                    type = IRobotLoadRecordType.I_LRT_NODE_FORCE;
                    if (lRecord == null || lRecord.Type != type)
                        lRecord = rCase.Records.Create(type) as RobotLoadRecord;

                    lRecord.Objects.FromText(context.IDMap.ToIDString(nLoad.AppliedTo));
                    //TODO: Axis
                    Vector force = nLoad.Direction.Vector() * nLoad.Value; //TODO: Provide evaluation context?
                    if (nLoad.IsMoment)
                    {
                        lRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_CX, force.X);
                        lRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_CY, force.Y);
                        lRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_CZ, force.Z);
                    }
                    else
                    {
                        lRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_FX, force.X);
                        lRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_FY, force.Y);
                        lRecord.SetValue((short)IRobotNodeForceRecordValues.I_NFRV_FZ, force.Z);
                    }
                }
                else if (load is GravityLoad)
                {
                    GravityLoad lELoad = (GravityLoad)load;
                    type = IRobotLoadRecordType.I_LRT_DEAD; //TODO: Variable loads
                    if (lRecord == null || lRecord.Type != type)
                        lRecord = rCase.Records.Create(type) as RobotLoadRecord;

                    lRecord.Objects.FromText(context.IDMap.ToIDString(lELoad.AppliedTo));
                    //TODO: Axis
                    Vector gravVector = lELoad.Direction.Vector() * lELoad.Value; //TODO: Provide evaluation context?
                    if (!lELoad.IsMoment)
                    {
                        lRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_X, gravVector.X);
                        lRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_Y, gravVector.Y);
                        lRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_Z, gravVector.Z);
                    }
                }
                else if (load is LinearElementLoad)
                {
                    LinearElementLoad lELoad = (LinearElementLoad)load;
                    type = IRobotLoadRecordType.I_LRT_BAR_UNIFORM; //TODO: Variable loads
                    if (lRecord == null || lRecord.Type != type)
                        lRecord = rCase.Records.Create(type) as RobotLoadRecord;

                    lRecord.Objects.FromText(context.IDMap.ToIDString(lELoad.AppliedTo));
                    //TODO: Axis
                    Vector force = lELoad.Direction.Vector() * lELoad.Value; //TODO: Provide evaluation context?
                    if (!lELoad.IsMoment)
                    {
                        lRecord.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PX, force.X);
                        lRecord.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PY, force.Y);
                        lRecord.SetValue((short)IRobotBarUniformRecordValues.I_BURV_PZ, force.Z);
                    }
                }
                else if (load is PanelLoad)
                {
                    PanelLoad aLoad = (PanelLoad)load;
                    type = IRobotLoadRecordType.I_LRT_PRESSURE; //?
                    if (lRecord == null || lRecord.Type != type)
                        lRecord = rCase.Records.Create(type) as RobotLoadRecord;

                    lRecord.Objects.FromText(context.IDMap.ToIDString(aLoad.AppliedTo));
                    lRecord.SetValue((short)IRobotPressureRecordValues.I_PRV_P, aLoad.Value);
                    //TODO
                }

                context.IDMap.Add(load, lRecord);

                
            }
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