using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Model.Loading;

namespace Nucleus.Model
{
    /// <summary>
    /// A particular condition to be analysed.
    /// </summary>
    public class AnalysisCase : ResultsCase
    {
        public override bool Contains(Load load)
        {
            throw new NotImplementedException();
        }
    }
}
