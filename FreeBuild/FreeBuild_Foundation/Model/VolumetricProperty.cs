using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A shared property that describes how to resolve
    /// an element's editable set-out geometry into a full
    /// 3D solid object.
    /// </summary>
    [Serializable]
    public abstract class VolumetricProperty : Unique
    {
    }
}
