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
    public class RobotController
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
        /// Release control over Robot
        /// </summary>
        public void Release()
        {
            _Robot = null;
        }

        /// <summary>
        /// Update a FreeBuild model by loading data from the currently open Robot file
        /// </summary>
        /// <param name="model">The model to be updated</param>
        /// <param name="map">The ID mapping table that relates model objects to Robot ones</param>
        /// <returns></returns>
        public bool UpdateModelFromRobotFile(Model.Model model, RobotIDMappingTable map, RobotConversionContext context)
        {
            IRobotCollection robotNodes = Robot.Project.Structure.Nodes.GetAll();
            UpdateModelNodesFromRobotFile(model, robotNodes, map, context);
            UpdateModelLinearElementsFromRobotFile(model, robotNodes, map, context);
            return false;
        }

        /// <summary>
        /// Update nodes in a FreeBuild model by loading data from the currently open Robot file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="map"></param>
        /// <param name="context"></param>
        private void UpdateModelNodesFromRobotFile(Model.Model model, IRobotCollection robotNodes, RobotIDMappingTable map, RobotConversionContext context)
        {
            //Delete all mapped nodes:
            if (context.Options.DeleteMissingObjects) map.AllMappedNodes(model).DeleteAll();

            for (int i = 1; i <= robotNodes.Count; i++)
            {
                IRobotNode robotNode = robotNodes.Get(i);
                if (robotNode != null)
                {
                    Vector nodePosition = ROBtoFB.PositionOf(robotNode);
                    Node node = map.GetMappedModelNode(robotNode, model);
                    if (node == null)  //Create new node
                        node = model.Create.Node(nodePosition, 0, context.ExInfo);
                    else //Existing mapped node found
                    {
                        node.Position = nodePosition;
                        node.Undelete();
                    }
                    //TODO: Copy over data

                    //Store mapping data:
                    map.Add(node, robotNode);
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
        private void UpdateModelLinearElementsFromRobotFile(Model.Model model, IRobotCollection robotNodes, RobotIDMappingTable map, RobotConversionContext context)
        {
            //Delete all linear elements:
            if (context.Options.DeleteMissingObjects) map.AllMappedLinearElements(model).DeleteAll();

            IRobotCollection bars = Robot.Project.Structure.Bars.GetAll();
            for (int i = 1; i <= bars.Count; i++)
            {
                IRobotBar bar = bars.Get(i);
                if (bar != null)
                {
                    Curve geometry = ROBtoFB.GeometryOf(bar, robotNodes);
                    LinearElement element = map.GetMappedLinearElement(bar, model);
                    if (element == null) //Create new element
                        element = model.Create.LinearElement(geometry, context.ExInfo);
                    else //Existing mapped element found
                    {
                        element.Geometry = geometry;
                        element.Undelete();
                    }
                    //TODO: Copy over data

                    //Store mapping:
                    map.Add(element, bar);
                }
            }
        }

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