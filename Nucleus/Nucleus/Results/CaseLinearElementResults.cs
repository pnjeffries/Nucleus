using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// Analysis results for a linear element under a specific case
    /// </summary>
    [Serializable]
    public class CaseLinearElementResults : CaseResults<LinearElementResultTypes, LinearIntervalDataSet>
    {
    }
}
