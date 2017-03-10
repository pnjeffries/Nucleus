using FreeBuild.Conversion;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.IO
{
    public class GWAContext : StringConversionContextBase
    {
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
            return "BEAM";
            //TODO
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
        /// Get the description of the material of the current section object
        /// </summary>
        /// <returns></returns>
        public string SectionMaterial()
        {
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
            if (SourceObject is ModelObject)
            {
                ModelObject mObj = (ModelObject)SourceObject;
                return mObj.NumericID.ToString();
            }
            else return "";
        }
    }
}
