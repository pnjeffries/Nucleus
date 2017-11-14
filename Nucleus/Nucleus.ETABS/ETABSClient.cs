using Nucleus.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Base;
using ETABS2016;
using Nucleus.Conversion;
using Nucleus.Model;
using Nucleus.Geometry;

namespace Nucleus.ETABS
{
    /// <summary>
    /// Class responsible for reading and writing data from/to ETABS
    /// </summary>
    public class ETABSClient : MessageRaiser, IApplicationClient
    {
        #region Properties

        /// <summary>
        /// Backing field for ETABS property
        /// </summary>
        private cOAPI _ETABS = null;

        /// <summary>
        /// The current ETABS application
        /// </summary>
        public cOAPI ETABS
        {
            get
            {
                if (_ETABS == null)
                {
                    RaiseMessage("Establishing ETABS link...");
                    cHelper helper = new Helper();
                    _ETABS = helper.CreateObjectProgID("CSI.ETABS.API.ETABSObject");
                    RaiseMessage("Starting application...");
                    _ETABS.ApplicationStart();
                    RaiseMessage("ETABS link established.");
                }
                return _ETABS;
            }
        }

        /// <summary>
        /// Private backing cache for SapModel property
        /// </summary>
        private cSapModel _SapModel = null;

        /// <summary>
        /// The current model in ETABS
        /// </summary>
        public cSapModel SapModel
        {
            get
            {
                if (_SapModel == null)
                {
                    _SapModel = default(cSapModel);
                    _SapModel = ETABS.SapModel;
                }
                return _SapModel;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Close the linked ETABS application
        /// </summary>
        public void Close()
        {
            ETABS.ApplicationExit(false);
        }

        /// <summary>
        /// Open a new ETABS Model
        /// </summary>
        /// <returns></returns>
        public bool New()
        {
            return SapModel.InitializeNewModel(eUnits.N_m_C) == 0;
        }

        /// <summary>
        /// Open a saved ETABS Model
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool Open(FilePath filePath)
        {
            return SapModel.File.OpenFile(filePath) == 0;
        }

        /// <summary>
        /// Save the current ETABS model to a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool Save(FilePath filePath)
        {
            RaiseMessage("Writing file to '" + filePath + "'...");
            return SapModel.File.Save(filePath) == 0;
        }

        /// <summary>
        /// Release the ETABS link
        /// </summary>
        public void Release()
        {
            _ETABS = null;
            _SapModel = null;
            RaiseMessage("ETABS link released.");
        }

        /// <summary>
        /// Write a Nucleus model to an ETABS file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        /// <param name="idMap"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public bool WriteModelToEtabs(FilePath filePath, Model.Model model, ref ETABSIDMappingTable idMap, ETABSConversionOptions options = null)
        {
            if (New())
            {
                if (idMap == null) idMap = new ETABSIDMappingTable();
                if (options == null) options = new ETABSConversionOptions();
                var context = new ETABSConversionContext(idMap, options);
                if (!WriteToETABS(model, context)) return false;
                return Save(filePath);
            }
            else return false;
        }

        /// <summary>
        /// Write a Nucleus model to the currently open ETABS model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool WriteToETABS(Model.Model model, ETABSConversionContext context)
        {
            RaiseMessage("Writing data to ETABS...");

            SapModel.File.NewBlank(); //TODO: check if updating
            //SapModel.File.NewSteelDeck(0, 12, 12, 0, 0, 24, 24);

            if (context.Options.Nodes)
            {
                NodeCollection nodes = model.Nodes;
                if (context.Options.Update) nodes = nodes.Modified(context.Options.UpdateSince);
                if (nodes.Count > 0) RaiseMessage("Writing nodes...");
                WriteNodes(nodes, context);
            }

            if (context.Options.Families)
            {
                FamilyCollection families = model.Families;
                if (context.Options.Update) families = families.Modified(context.Options.UpdateSince);
                if (families.Count > 0) RaiseMessage("Writing properties...");
                WriteFamilies(families, context);
            }

            if (context.Options.LinearElements)
            {
                LinearElementCollection linearElements = model.Elements.LinearElements;
                if (context.Options.Update) linearElements = linearElements.Modified(context.Options.UpdateSince);
                if (linearElements.Count > 0) RaiseMessage("Writing linear elements...");
                WriteLinearElements(linearElements, context);
            }

            if (context.Options.PanelElements)
            {
                PanelElementCollection panelElements = model.Elements.PanelElements;
                if (context.Options.Update) panelElements = panelElements.Modified(context.Options.UpdateSince);
                if (panelElements.Count > 0) RaiseMessage("Writing Panels...");
                WritePanelElements(panelElements, context);
            }

            /*if (context.Options.Sets)
            {
                ModelObjectSetCollection sets = model.Sets;
                //if (context.Options.Update) sets = //TODO?
                if (sets.Count > 0) RaiseMessage("Writing Groups...");
                UpdateRobotGroupsFromModel(model, sets, context);
            }*/

            return true;
        }

        /// <summary>
        /// Get the name of the equivalent ETABS material
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        private string GetEquivalentMaterial(Material material)
        {
            if (material.Name.StartsWith("Concrete")) return "4000Psi";
            return "A992Fy50";
        }

        /// <summary>
        /// Write/update ETABS nodes from a collection of Nucleus nodes
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="context"></param>
        private void WriteNodes(NodeCollection nodes, ETABSConversionContext context)
        {
            // Only writes restrained nodes...
            foreach (Node node in nodes)
            {
                string id = "";
                if (node.HasData<NodeSupport>())
                {
                    var ns = node.GetData<NodeSupport>();
                    if (!ns.IsFree)
                    {
                        SapModel.PointObj.AddCartesian(node.Position.X, node.Position.Y, node.Position.Z, ref id);
                        bool[] value = ns.Fixity.ToArray();
                        SapModel.PointObj.SetRestraint(id, ref value);
                    }
                }
                //TODO: Update previously restrained nodes when unrestrained...
            }
        }

        private void WriteFamilies(FamilyCollection families, ETABSConversionContext context)
        {
            foreach (Family family in families)
            {
                if (family is SectionFamily)
                {
                    WriteSection((SectionFamily)family, context);
                }
                else if (family is BuildUpFamily)
                {
                    WriteBuildUp((BuildUpFamily)family, context);
                }
            }
        }

        private void WriteSection(SectionFamily section, ETABSConversionContext context)
        {
            string name = section.Name;
            string matProp = GetEquivalentMaterial(section.GetPrimaryMaterial());

            if (section.Profile == null)
            {
                var profile = section.Profile;
                SapModel.PropFrame.SetGeneral(name, matProp, 0, 0, 0,
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                //TODO: ?
            }
            else if (section.Profile is SymmetricIProfile)
            {
                var profile = (SymmetricIProfile)section.Profile;
                SapModel.PropFrame.SetISection(name, matProp, profile.Depth, profile.Width, profile.FlangeThickness, profile.WebThickness, profile.Width, profile.FlangeThickness);
            }
            else if (section.Profile is RectangularHollowProfile)
            {
                var profile = (RectangularHollowProfile)section.Profile;
                SapModel.PropFrame.SetTube(name, matProp, profile.Depth, profile.Width, profile.FlangeThickness, profile.WebThickness);
            }
            else if (section.Profile is RectangularProfile)
            {
                var profile = (RectangularProfile)section.Profile;
                SapModel.PropFrame.SetRectangle(name, matProp, profile.Depth, profile.Width);
            }
            else if (section.Profile is CircularHollowProfile)
            {
                var profile = (CircularHollowProfile)section.Profile;
                SapModel.PropFrame.SetPipe(name, matProp, profile.Diameter, profile.WallThickness);
            }
            else if (section.Profile is CircularProfile)
            {
                var profile = (CircularProfile)section.Profile;
                SapModel.PropFrame.SetCircle(name, matProp, profile.Diameter);
            }
            else if (section.Profile is AngleProfile)
            {
                var profile = (AngleProfile)section.Profile;
                SapModel.PropFrame.SetAngle(name, matProp, profile.Depth, profile.Width, profile.FlangeThickness, profile.WebThickness);
            }
            else if (section.Profile is ChannelProfile)
            {
                var profile = (ChannelProfile)section.Profile;
                SapModel.PropFrame.SetChannel(name, matProp, profile.Depth, profile.Width, profile.FlangeThickness, profile.WebThickness);
            }
            else if (section.Profile is TProfile)
            {
                var profile = (TProfile)section.Profile;
                SapModel.PropFrame.SetTee(name, matProp, profile.Depth, profile.Width, profile.FlangeThickness, profile.WebThickness);
            }
            else
            {
                var profile = section.Profile;
                SapModel.PropFrame.SetGeneral(name, matProp, profile.OverallDepth, profile.OverallWidth, profile.Area,
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0); //TODO: Replace with calculated properties
            }
        }

        public void WriteBuildUp(BuildUpFamily buildUp, ETABSConversionContext context)
        {
            string name = buildUp.Name;
            string matProp = GetEquivalentMaterial(buildUp.GetPrimaryMaterial());

            //SapModel.PropArea.SetDeck(name, eDeckType.SolidSlab, eShellType.ShellThin, matProp, buildUp.Layers.TotalThickness);
            SapModel.PropArea.SetSlab(name, eSlabType.Slab, eShellType.ShellThin, matProp, buildUp.Layers.TotalThickness);
            // TODO: Check if working
        }

        public void WriteLinearElements(LinearElementCollection elements, ETABSConversionContext context)
        {
            foreach (LinearElement element in elements)
            {
                if (!element.IsDeleted)
                {
                    Vector s = element.Start.Position;
                    Vector e = element.End.Position;
                    string id = context.IDMap.GetSecondID(element) ?? "";
                    SapModel.FrameObj.AddByCoord(s.X, s.Y, s.Z, e.X, e.Y, e.Z, ref id, element.Family?.Name, element.Name);
                    //Releases:
                    bool[] sRls = element.Start.Releases.ToArray();
                    bool[] eRls = element.End.Releases.ToArray();
                    double[] sStf = new double[6]; //Temp
                    double[] eStf = new double[6]; //Temp
                    SapModel.FrameObj.SetReleases(id, ref sRls, ref eRls, ref sStf, ref eStf);
                    SapModel.FrameObj.SetLocalAxes(id, element.Orientation.Degrees); // May need adjustment for columns!
                    context.IDMap.Add(element, id);
                }
            }
        }

        public void WritePanelElements(PanelElementCollection elements, ETABSConversionContext context)
        {
            foreach (PanelElement element in elements)
            {
                if (!element.IsDeleted && element.Geometry != null)
                {
                    string id = context.IDMap.GetSecondID(element) ?? "";
                    if (element.Geometry is PlanarRegion)
                    {
                        var region = (PlanarRegion)element.Geometry;
                        Vector[] pts = region.Perimeter.Facet(Angle.FromDegrees(5));
                        var x = pts.XCoordinates();
                        var y = pts.YCoordinates();
                        var z = pts.ZCoordinates();
                        string propName = element.Family?.Name ?? "";
                        SapModel.AreaObj.AddByCoord(pts.Length, ref x, ref y, ref z, ref id, propName, element.Name);
                        // TODO: Build-Up
                        context.IDMap.Add(element, id);
                    }
                }
            }
        }

        #endregion

    }
}
