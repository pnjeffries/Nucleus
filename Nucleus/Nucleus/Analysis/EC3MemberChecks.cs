using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Analysis
{
    /// <summary>
    /// Steel member design checks to Eurocode 3 
    /// </summary>
    [Serializable]
    public class EC3MemberChecks : NotifyPropertyChangedBase
    {
        //TODO: Shear lag, local buckling & shear buckling to EN 1993-1-5

        #region Properties

        /// <summary>
        /// Private backing member variable for the Section property
        /// </summary>
        private SectionProfile _Section;

        /// <summary>
        /// The cross-section of the member
        /// </summary>
        public SectionProfile Section
        {
            get { return _Section; }
            set
            {
                _Section = value;
                NotifyPropertyChanged("Section");
            }
        }

        #endregion

        /// <summary>
        /// Calculate the gross area of the section
        /// According to EC3 6.2.2.1
        /// </summary>
        /// <returns></returns>
        public double GrossArea()
        {
            return Section.Area;
            // TODO: Subtract major penetrations
        }

        /// <summary>
        /// Calculate the net area of the section
        /// According to EC3 6.2.2.2
        /// </summary>
        /// <returns></returns>
        public double NetArea()
        {
            return GrossArea();
            // TODO: Subtract holes & openings
        }


    }
}
