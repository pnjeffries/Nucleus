using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of elements
    /// </summary>
    [Serializable]
    public class ElementCollection : UniquesCollection<IElement>
    {
    }
}
