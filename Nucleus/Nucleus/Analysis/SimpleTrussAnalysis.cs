using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Analysis
{
    /// <summary>
    /// Class to perform simple analysis on trusses via the method of sections
    /// NOT CURRENTLY IMPLEMENTED
    /// </summary>
    public class SimpleTrussAnalysis : BeamAnalysisBase
    {
        /// <summary>
        /// Perform the method of sections to calculate axial forces in the truss members
        /// </summary>
        private void MethodOfSections()
        {
            double cellHeight = 4;
            int cellCount = 8;
            double cellWidth = Length / cellCount;
            

        }
    }
}
