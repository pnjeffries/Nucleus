using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of loads
    /// </summary>
    [Serializable]
    public class LoadTable : LoadCollection
    {
        #region Constructors

        public LoadTable(Model model) : base(model)
        {
        }

        #endregion

    }
}
