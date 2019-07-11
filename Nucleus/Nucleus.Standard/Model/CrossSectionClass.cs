using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// The classification of a profile cross-section, indicating the extent to which
    /// the resistance and rotation capacity of cross sections is limited by its local
    /// buckling resistance.
    /// </summary>
    public enum CrossSectionClass
    {
        /// <summary>
        /// The cross-section type has not been or cannot be determined
        /// </summary>
        Indeterminate = 0,

        /// <summary>
        /// Class 1 cross-sections are those which can form
        /// a plastic hinge with the rotation capacity required from plastic analysis
        /// without reduction of the resistance.
        /// </summary>
        One = 1,

        /// <summary>
        /// Class 2 cross-sections are those that can develop their plastic moment 
        /// resistance, but have limited rotation capacity because of local buckling.
        /// </summary>
        Two = 2,

        /// <summary>
        /// Class 3 cross-sections are those in which the stress in the extreme 
        /// compression fibre of the steel (or aluminium) member assuming an elastic 
        /// distribution of stresses can reach the yield strength, but local buckling 
        /// is liable to prevent development of the plastic moment resistance. 
        /// </summary>
        Three = 3,

        /// <summary>
        /// Class 4 cross-sections are those in which local buckling will occur 
        /// before the attainment of yield stress in one or more parts of the cross-section.
        /// </summary>
        Four = 4

    }
}
