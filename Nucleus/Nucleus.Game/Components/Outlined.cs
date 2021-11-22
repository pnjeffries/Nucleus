using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Component which is used to indicate that an outline should be generated around this element and others
    /// that share this component
    /// </summary>
    [Serializable]
    public class Outlined : Unique, IElementDataComponent
    {
    }
}
