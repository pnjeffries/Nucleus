using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A special subtype of GameElement which represents a resource.
    /// </summary>
    [Serializable]
    public class ResourceElement : GameElement
    {
        public ResourceElement()
        {
        }

        public ResourceElement(string name) : base(name)
        {
        }

        public ResourceElement(string name, params IElementDataComponent[] data) : base(name, data)
        {
        }
    }
}
