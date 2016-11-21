using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of section profiles
    /// </summary>
    public class SectionProfileCollection : UniquesCollection<SectionProfile>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SectionProfileCollection() { }

        /// <summary>
        /// Initialise a new SectionProfileCollection containing the specified profile
        /// </summary>
        /// <param name="profile"></param>
        public SectionProfileCollection(SectionProfile profile)
        {
            if (profile != null) Add(profile);
        }

        #endregion
    }
}
