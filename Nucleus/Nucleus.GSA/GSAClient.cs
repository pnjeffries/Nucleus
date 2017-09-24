using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interop.gsa_8_7;
using Nucleus.IO;
using Nucleus.Model;
using Nucleus.Geometry;
using Nucleus.Extensions;

namespace Nucleus.GSA
{
    /// <summary>
    /// Controller class to drive GSA
    /// </summary>
    public class GSAClient : MessageRaiser, IApplicationClient
    {
        #region Properties

        /// <summary>
        /// Private backing field for GSA Property
        /// </summary>
        private ComAuto _GSA = null;

        /// <summary>
        /// The current GSA application
        /// </summary>
        public ComAuto GSA
        {
            get
            {
                if (_GSA == null)
                {
                    _GSA = new ComAuto();
                }
                return _GSA;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Open the GSA file at the specified filepath
        /// </summary>
        /// <param name="filePath">The filepath to open.  (Note that this can be expressed as a string)</param>
        /// <returns>True if the specified file could be opened, false if this was prevented in some way.</returns>
        public bool Open(FilePath filePath)
        {
            try
            {
                RaiseMessage("Opening GSA file '" + filePath + "'...");
                return GSA.Open(filePath) == 0;
            }
            catch (Exception ex)
            {
                RaiseMessage("Error: Opening GSA file '" + filePath + "' failed.  " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Open a new file in the target application
        /// </summary>
        /// <returns></returns>
        public bool New()
        {
            try
            {
                RaiseMessage("Creating new GSA model...");
                return GSA.NewFile() == 0;
            }
            catch (Exception ex)
            {
                RaiseMessage("Error: Creating new GSA model failed.  " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Save the currently open GSA file to the specified file location
        /// </summary>
        /// <param name="filePath">The filepath to save to</param>
        /// <returns></returns>
        public bool Save(FilePath filePath)
        {
            try
            {
                RaiseMessage("Writing GSA file to '" + filePath + "'...");
                switch (GSA.SaveAs(filePath))
                {
                    case 0:
                        RaiseMessage("...Complete.");
                        return true;
                    case 1:
                        RaiseMessage("...Failed.  No GSA file is open!");
                        return false;
                    case 2:
                        RaiseMessage("...Failed.  Invalid file extension!");
                        return false;
                    default:
                        RaiseMessage("...Failed!");
                        return false;
                }
            }
            catch (Exception ex)
            {
                RaiseMessage("Error: Writing GSA file to '" + filePath + "' failed.  " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Close the currently open file in the target application
        /// </summary>
        public void Close()
        {
            GSA.Close();
        }

        /// <summary>
        /// Issue a GWA command to GSA
        /// </summary>
        /// <param name="command">The command to be issued, in GWA format</param>
        /// <returns></returns>
        public dynamic GWACommand(string command)
        {
            return GSA.GwaCommand(command);
        }

        public void ReadNodes()
        {
            int[] ids;
            GSA.EntitiesInList("all", GsaEntity.NODE, out ids);
            if (ids != null)
            {
                GsaNode[] nodes;
                GSA.Nodes(ids, out nodes);
                foreach (GsaNode node in nodes)
                {
                    //TODO (?)
                }
            }

        }

        public void ReadElements()
        {
            int[] ids;
            GSA.EntitiesInList("all", GsaEntity.ELEMENT, out ids);
            if (ids != null)
            {
                GsaElement[] elements;
                GSA.Elements(ids, out elements);
                foreach (GsaElement element in elements)
                {
                    //TODO (?)
                }
            }
        }

        /// <summary>
        /// Get the number of nodes that are needed to represent
        /// the specified element type
        /// </summary>
        /// <param name="elementType">The element type in GWA format</param>
        /// <returns></returns>
        public int NodeCountOf(string elementType)
        {
            if (elementType == null) return 0;
            switch (elementType.ToUpper())
            {
                case "TRI3": return 3;
                case "TRI6": return 6;
                case "QUAD4": return 4;
                case "QUAD8": return 8;
                default: return 2;
            }
        }

        /// <summary>
        /// Update the properties of a Nucleus node from a GWA string
        /// in EL.2 version syntax
        /// </summary>
        /// <param name="element"></param>
        /// <param name="gwa"></param>
        public void UpdateModelElementFromGSA(string gwa, Model.Model model, GSAConversionContext context)
        {
            // EL.2 | num | name | colour | type | prop | group | topo() | orient_node | orient_angle |
            // is_rls { | rls { | k } }
            // is_offset { | ox | oy | oz } | dummy

            Element element;

            var tr = new TokenReader(gwa);
            tr.Next(); // EL
            string gsaID = tr.Next(); // num
            string name = tr.Next(); // name
            tr.Next(); // colour
            int nodeCount = NodeCountOf(tr.Next()); // type
            string propID = tr.Next(); // prop
            tr.NextInt(); // group
            if (nodeCount == 0) return; //Not valid!
            else if (nodeCount == 2)
            {
                // Linear element
                var linEl = context.IDMap.GetModelObject<LinearElement>(model, gsaID.ToString());
                if (linEl == null) linEl = model.Create.LinearElement(null);
                element = linEl;
                linEl.Family = context.IDMap.GetModelObject<SectionFamily>(model, propID);
                Node n0 = context.IDMap.GetModelObject<Node>(model, tr.Next()); // Start node
                Node n1 = context.IDMap.GetModelObject<Node>(model, tr.Next()); // End node
                linEl.Geometry = new Line(n0, n1);
            }
            else
            {
                //Panel element
                var panEl = context.IDMap.GetModelObject<PanelElement>(model, gsaID.ToString());
                if (panEl == null) panEl = model.Create.PanelElement(null);
                element = panEl;
                panEl.Family = context.IDMap.GetModelObject<BuildUpFamily>(model, propID);
                var nodes = new NodeCollection();
                for (int i = 0; i < nodeCount; i++)
                {
                    var node = context.IDMap.GetModelObject<Node>(model, tr.Next());
                    if (node != null && !nodes.Contains(node.GUID))
                        nodes.Add(node);
                }
                panEl.Geometry = new Mesh(nodes, true);
            }
            tr.Next(); // orient_node   TODO: Make orientation relative to this
            element.Orientation = Angle.FromDegrees(tr.NextDouble());
            var verts = element.ElementVertices;
            string is_rls = tr.Next(); // is_rls
            if (is_rls.EqualsIgnoreCase("RLS") || is_rls.EqualsIgnoreCase("STIFF"))
            {
                for (int i = 0; i < nodeCount; i++)
                {
                    string rls = tr.Next(); // rls
                    if (i < verts.Count)
                    {
                        Bool6D fixity = Bool6D.FromTokensList(rls.ToCharArray(), 0, 'R');
                        ElementVertex v = verts[i];
                        v.Releases = fixity;

                        if (is_rls.EqualsIgnoreCase("STIFF"))
                        {
                            for (int j = 0; j < rls.Length; j++)
                            {
                                char c = rls[j];
                                if (c.EqualsIgnoreCase('K'))
                                {
                                    v.Stiffness = v.Stiffness.With(j, tr.NextDouble()); // k
                                }
                            }
                        }
                    }
                }
            }
            if (tr.NextIs("OFFSET")) // is_offset
            {
                for (int i = 0; i < nodeCount; i++)
                {
                    if (verts.Count > i)
                    {
                        var vert = verts[i];
                        vert.Offset = vert.Offset.WithX(tr.NextDouble()); // ox
                        vert.Offset = vert.Offset.WithY(tr.NextDouble()); // oy
                        vert.Offset = vert.Offset.WithZ(tr.NextDouble()); // oz
                    }
                    else
                    {
                        tr.Skip(3);
                    }
                }
                //TODO: Local offsets
            }
            // TODO: Dummy?
            context.IDMap.Add()
        }

        /// <summary>
        /// Update the properties of a Nucleus node from a GWA string
        /// in NODE.2 version syntax
        /// </summary>
        /// <param name="node"></param>
        /// <param name="gwa"></param>
        public void UpdateModelNodeFromGSA(Node node, string gwa)
        {
            // NODE.2 | num | name | colour | x | y | z |
            // is_grid { | grid_plane | datum | grid_line_a | grid_line_b } | axis |
            // is_rest { | rx | ry | rz | rxx | ryy | rzz } |
            // is_stiff { | Kx | Ky | Kz | Kxx | Kyy | Kzz } |
            // is_mesh { | edge_length | radius | tie_to_mesh | column_rigidity | column_prop | column_node | column_angle | column_factor | column_slab_factor }

            var tr = new TokenReader(gwa);
            tr.Next(); // NODE
            int gsaID = tr.NextInt(); // num
            node.Name = tr.Next(); // name
            tr.Next(); // colour
            node.Position = tr.Next3AsVector(); // x | y | z
            if (tr.NextIs("GRID")) tr.Skip(4); // is_grid { | grid_plane | datum | grid_line_a | grid_line_b }
            tr.Next(); // axis !!!TODO!!!
            Bool6D fixity = new Bool6D();
            if (tr.NextIs("REST")) // is_rest
            {
                fixity = tr.Next6AsBool6D();
            }
            SixVector stiffness = null;
            if (tr.NextIs("STIFF")) // is_stiff
            {
                stiffness = tr.Next6AsSixVector();
            }
            if (!fixity.AllFalse || (stiffness != null && !stiffness.IsZero()))
            {
                var nodeSupport = new NodeSupport(fixity);
                if (stiffness != null) nodeSupport.Stiffness = stiffness;
                //TODO: Axis
                node.SetData(nodeSupport);
            }
            else node.Data.RemoveData<NodeSupport>(); //Clear restraints on existing nodes
        }

        #endregion
    }
}
