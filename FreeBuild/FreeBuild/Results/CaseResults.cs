using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Results
{
    [Serializable]
    public class CaseResults : ResultsDictionary<Guid, IModelObjectResults>
    {
    }
}
