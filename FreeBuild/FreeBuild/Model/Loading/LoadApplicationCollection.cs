using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of load application rules
    /// </summary>
    public class LoadApplicationCollection<TApplication> : UniquesCollection<TApplication>
        where TApplication : class, ILoadApplication
    {
    }
}
