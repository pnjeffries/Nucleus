using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A record of a proficiency level in a named skill
    /// </summary>
    [Serializable]
    public class Proficiency : Named
    {
        #region Constructors

        public Proficiency(string name) : base(name) { }

        #endregion
    }
}
