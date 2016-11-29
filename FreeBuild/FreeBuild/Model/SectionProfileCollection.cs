using FreeBuild.Base;
using FreeBuild.IO;
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

        #region Methods

        /// <summary>
        /// Find and return the first profile in this collection
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SectionProfile GetByCatalogueName(string name)
        {
            foreach (SectionProfile profile in this)
            {
                if (profile.CatalogueName == name) return profile;
            }
            return null;
        }

        /// <summary>
        /// Add profiles to this collection by loading them from a CSV library file
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadFromCSV(FilePath filePath)
        {
            var parser = new CSVParser<SectionProfile>();
            AddRange(parser.Parse(filePath));
        }

        /// <summary>
        /// Add profiles to this collection by loading them from a CSV string
        /// </summary>
        /// <param name="csvString"></param>
        public void LoadFromCSV(string csvString)
        {
            var parser = new CSVParser<SectionProfile>();
            AddRange(parser.Parse(csvString));
        }

        #endregion
    }
}
