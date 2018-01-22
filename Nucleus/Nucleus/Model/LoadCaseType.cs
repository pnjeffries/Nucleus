using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Enumerated value representing the different possible types of
    /// a load case
    /// </summary>
    public enum LoadCaseType
    {
        Undefined = 0,
        Dead = 10,
        Live = 20,
        Wind = 30,
        Seismic = 100,
        Thermal = 200
    }
}
