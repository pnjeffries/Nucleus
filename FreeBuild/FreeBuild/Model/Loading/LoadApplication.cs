using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract base class for loading application rules.
    /// Determines how a particular load is applied to the model.
    /// </summary>
    public abstract class LoadApplication : Unique, ILoadApplication
    {
    }
}
