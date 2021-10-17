using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Represents a type of resource.
    /// </summary>
    [Serializable]
    public class ResourceType : Named
    {
        private string _Symbol = "";

        /// <summary>
        /// The shorthand symbol used to represent the resource type
        /// </summary>
        public string Symbol
        {
            get { return _Symbol; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public ResourceType(string name) : base(name) { }

        /// <summary>
        /// Name + symbol constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="symbol"></param>
        public ResourceType(string name, string symbol) : this(name)
        {
            _Symbol = symbol;
        }

        public override bool Equals(object obj)
        {
            if (obj is DamageType) return Name == ((DamageType)obj).Name;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
