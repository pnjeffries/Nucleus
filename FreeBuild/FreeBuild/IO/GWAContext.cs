using FreeBuild.Conversion;
using FreeBuild.Extensions;
using FreeBuild.Geometry;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.IO
{
    /// <summary>
    /// Contextual operations involved with writing out a GWA-format file
    /// </summary>
    public class GWAContext : StringConversionContextBase
    {
        #region Fields

        /// <summary>
        /// The dictionary of IDs for various types
        /// </summary>
        public Dictionary<Type, int> _NextID = new Dictionary<Type, int>();

        #endregion

        #region Properties

        private Dictionary<Guid, IList<int>> _IDMap = new Dictionary<Guid, IList<int>>();

        public Dictionary<Guid, IList<int>> IDMap
        {
            get { return _IDMap; }
        }

        #endregion

        public GWAContext()
        {
            //Initialise ID starters
            _NextID.Add(typeof(Node), 1);
            _NextID.Add(typeof(Element), 1);
            _NextID.Add(typeof(SectionFamily), 1);
            _NextID.Add(typeof(BuildUpFamily), 1);
        }

        /// <summary>
        /// Get the next available GSA ID for the specified object.
        /// Will increment the stored next available ID for the relevant type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetNextIDFor(object obj)
        {
            Type type = obj.GetType();
            type = _NextID.Keys.ClosestAncestor(type);
            int result = _NextID[type];
            _NextID[type] = result + 1;
            return result;
        }

        public override bool HasSubComponentsToWrite(object source)
        {
            if (source is PanelElement)
            {
                PanelElement pEl = (PanelElement)source;
                if (pEl.Geometry is Mesh)
                {
                    Mesh mesh = (Mesh)pEl.Geometry;
                    return (mesh.Faces.Count > this.SubComponentIndex);
                }
            }
            return base.HasSubComponentsToWrite(source);
        }

        /// <summary>
        /// Convert to integer
        /// </summary>
        /// <returns></returns>
        public string ToInt()
        {
            if (SourceObject is bool && ((bool)SourceObject) == true) return "1";
            else return "0";
        }

        /// <summary>
        /// Get the element type of the current element object
        /// </summary>
        /// <returns></returns>
        public string ElementType()
        {
            if (SourceObject is LinearElement)
            {
                return "BEAM"; //TODO
            }
            else
            {
                MeshFace face = CurrentPanelFace();
                if (face.IsTri) return "TRI3";
                else return "QUAD4";
            }
            //TODO
        }

        /// <summary>
        /// Get a node topology description for the current sub-element
        /// </summary>
        /// <returns></returns>
        public string ElementTopo()
        {
            StringBuilder sb = new StringBuilder();
            if (SourceObject is PanelElement)
            {
                MeshFace face = CurrentPanelFace();
                for (int i = 0; i < Math.Min(4, face.Count);i++)
                {
                    if (i > 0) sb.Append("\t");
                    sb.Append(GetID(face[i].Node));
                }
            }
            return sb.ToString();
            //TODO : Multi-span linear elements?
        }

        /// <summary>
        /// Get the current sub-component mesh face of the current panel element
        /// source object
        /// </summary>
        /// <returns></returns>
        public MeshFace CurrentPanelFace()
        {
            if (SourceObject is PanelElement)
            {
                PanelElement el = (PanelElement)SourceObject;
                if (el.Geometry is Mesh)
                {
                    Mesh mesh = (Mesh)el.Geometry;
                    return mesh.Faces[SubComponentIndex];
                }
            }
            return null;
        }

        /// <summary>
        /// Get the group number of the current element object
        /// </summary>
        /// <returns></returns>
        public string ElementGroup()
        {
            return "1";
            //TODO
        }

        /// <summary>
        /// Get the description of the material of the current family object
        /// </summary>
        /// <returns></returns>
        public string FamilyMaterial()
        {
            if (SourceObject is Family)
            {
                Family family = (Family)SourceObject;
                Material material = family.GetPrimaryMaterial();
                if (material != null) return material.Name;
            }
            return "STEEL";
            //TODO
        }

        /// <summary>
        /// Get a GSA section description of the current object
        /// </summary>
        /// <returns></returns>
        public string SectionDescription()
        {
            if (SourceObject is SectionFamily)
            {
                SectionFamily section = (SectionFamily)SourceObject;
                // TODO: Create section description
                if (section.Profile != null)
                {
                    //if (section.Profile.CatalogueName != null)
                    //   return "CAT " + section.Profile.CatalogueName;
                    if (section.Profile is SymmetricIProfile)
                    {
                        var profile = (SymmetricIProfile)section.Profile;
                        return string.Format("STD I({0}) {1} {2} {3} {4}", "m", profile.Depth, profile.Width, profile.WebThickness, profile.FlangeThickness);
                    }
                    else if (section.Profile is RectangularHollowProfile)
                    {
                        var profile = (RectangularHollowProfile)section.Profile;
                        return string.Format("STD RHS({0}) {1} {2} {3} {4}", "m", profile.Depth, profile.Width, profile.WebThickness, profile.FlangeThickness);
                    }
                    else if (section.Profile is RectangularProfile)
                    {
                        var profile = (RectangularProfile)section.Profile;
                        return string.Format("STD R({0}) {1} {2}", "m", profile.Depth, profile.Width);
                    }
                    else if (section.Profile is CircularHollowProfile)
                    {
                        var chsSection = (CircularHollowProfile)section.Profile;
                        string.Format("STD CHS({0}) {1} {2}", "m", chsSection.Diameter, chsSection.WallThickness);
                    }
                    else if (section.Profile is CircularProfile)
                    {
                        var profile = (CircularProfile)section.Profile;
                        return string.Format("STD C({0}) {1}", "m", profile.Diameter);
                    }
                    else if (section.Profile is TProfile)
                    {
                        var profile = (TProfile)section.Profile;
                        return string.Format("STD T({0}) {1} {2} {3} {4}", "m", profile.Depth, profile.Width, profile.WebThickness, profile.FlangeThickness);
                    }
                    // TODO: Other types
                }
            }
            return "EXP";
        }

        /// <summary>
        /// Get the GSA ID of the specified object
        /// </summary>
        /// <returns></returns>
        public string GetID()
        {
            return GetID(SourceObject);
        }

        /// <summary>
        /// Get the GSA ID of the specified object
        /// </summary>
        /// <returns></returns>
        public string GetID(object obj)
        {
            if (obj is ModelObject)
            {
                ModelObject mObj = (ModelObject)obj;
                if (IDMap.ContainsKey(mObj.GUID))
                {
                    IList<int> IDs = IDMap[mObj.GUID];
                    if (IDs.Count > SubComponentIndex) return IDs[SubComponentIndex].ToString();
                    else
                    {
                        int ID = GetNextIDFor(obj);
                        IDs.Add(ID); //This depends on this being accessed in order...
                        return ID.ToString();
                    }
                }
                else
                {
                    IList<int> IDs = new List<int>();
                    int ID = GetNextIDFor(obj);
                    IDs.Add(ID);
                    IDMap.Add(mObj.GUID, IDs);
                    return ID.ToString();
                }
                //return mObj.NumericID.ToString();
            }
            else return "";
        }
    }
}
