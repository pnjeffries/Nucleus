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
                    if (section.Profile.CatalogueName != null)
                        return "STD CAT " + section.Profile.CatalogueName;
                }
            }
            return "";
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
