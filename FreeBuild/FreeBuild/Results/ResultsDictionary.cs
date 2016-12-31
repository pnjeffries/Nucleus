using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Results
{
    /// <summary>
    /// Abstract base class for dictionaries that store analysis results,
    /// either directly or within further sub-dictionaries.
    /// </summary>
    [Serializable]
    public abstract class ResultsDictionary<TKey, TValue> : Dictionary<TKey,TValue>
    {
    }
}
