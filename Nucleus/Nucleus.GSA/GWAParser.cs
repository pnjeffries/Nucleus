using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Geometry;
using Nucleus.IO;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.GSA
{
    /// <summary>
    /// A class which can be used to parse GWA syntax into Nucleus objects
    /// </summary>
    public class GWAParser
    {
        #region Methods

        /// <summary>
        /// Read a model from a GWA syntax text file
        /// </summary>
        /// <returns></returns>
        public Model.Model ReadGWAFile(FilePath filePath, ref GSAIDMappingTable idMap, GSAConversionOptions options = null)
        {
            if (idMap == null) idMap = new GSAIDMappingTable();
            if (options == null) options = new GSAConversionOptions();
            var context = new GSAConversionContext(idMap, options);
            return ReadGWAFile(filePath, context);
        }

        /// <summary>
        /// Read a model from a GWA syntax text file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Model.Model ReadGWAFile(FilePath filePath, GSAConversionContext context)
        {
            using (var reader = new StreamReader(filePath))
            {
                var result = ReadGWAFile(reader, context);
                reader.Close();
                return result;
            }
        }

        /// <summary>
        /// Read a model from a GWA syntax text reader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Model.Model ReadGWAFile(TextReader reader, GSAConversionContext context)
        {
            Model.Model model = new Model.Model();
            ReadGWAFile(model, reader, context);
            return model;
        }

        /// <summary>
        /// Read data from a GWA syntax text reader into an existing model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="reader"></param>
        /// <param name="context"></param>
        public void ReadGWAFile(Model.Model model, TextReader reader, GSAConversionContext context)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                ParseGWALine(line, model, context);
            }
        }

        /// <summary>
        /// Parse a text line of GWA format data and use it to populate a Nucleus model
        /// </summary>
        /// <param name="gwa"></param>
        /// <param name="model"></param>
        /// <param name="context"></param>
        public void ParseGWALine(string gwa, Model.Model model, GSAConversionContext context)
        {
            gwa = gwa.Trim();
            if (gwa.StartsWith("NODE", StringComparison.CurrentCultureIgnoreCase))
                ReadNode(gwa, model, context);
            else if (gwa.StartsWith("EL", StringComparison.CurrentCultureIgnoreCase))
                ReadElement(gwa, model, context);
            else if (gwa.StartsWith("PROP_2D", StringComparison.CurrentCultureIgnoreCase))
                ReadBuildUp(gwa, model, context);
            else if (gwa.StartsWith("PROP_SEC", StringComparison.CurrentCultureIgnoreCase))
                ReadSection(gwa, model, context);
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
        /// Update or create a Nucleus build-up family from a GWA string
        /// </summary>
        /// <param name="gwa"></param>
        /// <param name="model"></param>
        /// <param name="context"></param>
        public void ReadBuildUp(string gwa, Model.Model model, GSAConversionContext context)
        {
            // PROP_2D.2 | num | name | colour | axis | mat | type | thick | mass | bending | inplane | weight
            // PROP_2D.2 | num | name | colour | axis | mat | LOAD | support | edge

            var tr = new TokenReader(gwa);
            tr.Next(); // PROP_2D
            string gsaID = tr.Next(); // num

            var buildup = context.IDMap.GetModelObject<BuildUpFamily>(model, gsaID);
            if (buildup == null) buildup = model.Create.BuildUpFamily();

            buildup.Name = tr.Next(); // name
            tr.Next(); // colour
            tr.Next(); // axis

            string matID = tr.Next(); // mat
            var material = context.IDMap.GetModelObject<Material>(model, matID);

            tr.Next(); // type
            buildup.Layers.Clear();
            buildup.Layers.Add(new BuildUpLayer(tr.NextDouble(), material)); // thick

            context.IDMap.Add(buildup, gsaID);
        }

        /// <summary>
        /// Update or create the properties of a Nucleus section family from a GWA string
        /// </summary>
        /// <param name="gwa"></param>
        /// <param name="model"></param>
        /// <param name="context"></param>
        public void ReadSection(string gwa, Model.Model model, GSAConversionContext context)
        {
            // PROP_SEC | num | name | colour | mat | desc | principal | type | cost |
            // is_prop { | area | I11 | I22 | J | K1 | K2 } |
            // is_mod { | area_to_by | area_m | I11_to_by | I11_m | I22_to_by | I22_m | J_to_by | J_m | K1_to_by | K1_m | K2_to_by | K2_m | mass | stress } |
            // plate_type | calc_J

            var tr = new TokenReader(gwa);
            tr.Next(); // PROP_SEC
            string gsaID = tr.Next(); // num

            var sec = context.IDMap.GetModelObject<SectionFamily>(model, gsaID);
            if (sec == null) sec = model.Create.SectionFamily();

            sec.Name = tr.Next(); // name
            tr.Next(); // colour
            string matID = tr.Next(); // mat
            var material = context.IDMap.GetModelObject<Material>(model, matID);
            //TODO: profile description
            SectionProfile profile = ReadProfile(tr.Next(), context); //desc
            if (profile != null)
            {
                profile.Material = material;
                sec.Profile = profile;
            }

            context.IDMap.Add(sec, gsaID);
        }

        /// <summary>
        /// Parse a Nucleus section profile from a GSA section description
        /// </summary>
        /// <param name="description"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SectionProfile ReadProfile(string description, GSAConversionContext context)
        {
            var tr = new TokenReader(description, ' ', '.');
            string type = tr.Next();
            if (type.EqualsIgnoreCase("CAT")) // Catalogue section
            {
                // CAT <Type ID> <Sec name> <Date>
                // TODO: Try to find equivalent section in catalogue
            }
            else if (type.EqualsIgnoreCase("STD")) // Standard section
            {
                // STD <Shape(unit)> <Dimension> <Dimension>…
                double scale = 0.001; //Default mm
                string units = description.NextBracketed();
                if (units != null)
                {
                    if (units.EqualsIgnoreCase("m")) scale = 1;
                    else if (units.EqualsIgnoreCase("cm")) scale = 0.01;
                    else if (units.EqualsIgnoreCase("in")) scale = 0.0254;
                    //TODO: Others?
                }

                string sType = tr.Next()?.Before('('); // <Shape(unit)>

                if (sType.EqualsIgnoreCase("R")) // Rectangle R d, b
                    return new RectangularProfile(tr.NextDouble() * scale, tr.NextDouble() * scale);
                else if (sType.EqualsIgnoreCase("RHS")) // RHS d, b, tw, tf
                    return new RectangularHollowProfile()
                    {
                        Depth = tr.NextDouble() * scale,
                        Width = tr.NextDouble() * scale,
                        WebThickness = tr.NextDouble() * scale,
                        FlangeThickness = tr.NextDouble() * scale
                    };
                else if (sType.EqualsIgnoreCase("C")) // C d
                    return new CircularProfile(tr.NextDouble() * scale);
                else if (sType.EqualsIgnoreCase("CHS")) // CHS d, t
                    return new CircularHollowProfile(tr.NextDouble() * scale, tr.NextDouble() * scale);
                else if (sType.EqualsIgnoreCase("I")) // I d, b, tw, tf
                    return new SymmetricIProfile()
                    {
                        Depth = tr.NextDouble() * scale,
                        Width = tr.NextDouble() * scale,
                        WebThickness = tr.NextDouble() * scale,
                        FlangeThickness = tr.NextDouble() * scale
                    };
                else if (sType.EqualsIgnoreCase("T")) // T d, b, tw, tf
                    return new TProfile()
                    {
                        Depth = tr.NextDouble() * scale,
                        Width = tr.NextDouble() * scale,
                        WebThickness = tr.NextDouble() * scale,
                        FlangeThickness = tr.NextDouble() * scale
                    };
                else if (sType.EqualsIgnoreCase("CH")) // CH d, b, tw, tf
                    return new ChannelProfile()
                    {
                        Depth = tr.NextDouble() * scale,
                        Width = tr.NextDouble() * scale,
                        WebThickness = tr.NextDouble() * scale,
                        FlangeThickness = tr.NextDouble() * scale
                    };
                else if (sType.EqualsIgnoreCase("A")) // A d, b, tw, tf
                    return new AngleProfile()
                    {
                        Depth = tr.NextDouble() * scale,
                        Width = tr.NextDouble() * scale,
                        WebThickness = tr.NextDouble() * scale,
                        FlangeThickness = tr.NextDouble() * scale
                    };
                else if (sType.EqualsIgnoreCase("TR")) // TR d, bt, bb
                    return new TrapezoidProfile(tr.NextDouble() * scale, tr.NextDouble() * scale, tr.NextDouble() * scale);
                //TODO: Other types go here
            }
            return null;
        }

        /// <summary>
        /// Update the properties of a Nucleus node from a GWA string
        /// in EL.2 version syntax
        /// </summary>
        /// <param name="element"></param>
        /// <param name="gwa"></param>
        public void ReadElement(string gwa, Model.Model model, GSAConversionContext context)
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
            context.IDMap.Add(element, gsaID);
        }

        /// <summary>
        /// Update the properties of a Nucleus node from a GWA string
        /// in NODE.2 version syntax
        /// </summary>
        /// <param name="node"></param>
        /// <param name="gwa"></param>
        public void ReadNode(string gwa, Model.Model model, GSAConversionContext context)
        {
            // NODE.2 | num | name | colour | x | y | z |
            // is_grid { | grid_plane | datum | grid_line_a | grid_line_b } | axis |
            // is_rest { | rx | ry | rz | rxx | ryy | rzz } |
            // is_stiff { | Kx | Ky | Kz | Kxx | Kyy | Kzz } |
            // is_mesh { | edge_length | radius | tie_to_mesh | column_rigidity | column_prop | column_node | column_angle | column_factor | column_slab_factor }

            var tr = new TokenReader(gwa);
            tr.Next(); // NODE
            string gsaID = tr.Next(); // num

            string name = tr.Next(); // name
            tr.Next(); // colour
            Vector position = tr.Next3AsVector(); // x | y | z

            var node = context.IDMap.GetModelObject<Node>(model, gsaID);
            if (node == null) node = model.Create.Node(position);
            else node.Position = position;

            node.Name = name;

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

            context.IDMap.Add(node, gsaID);
        }

        #endregion
    }
}
