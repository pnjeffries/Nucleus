using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maps
{
    /// <summary>
    /// 
    /// </summary>
    public class Road : WidePathBasic
    {
        #region Constructor

        public Road(Curve spine, double width) : base(spine, width)
        {
        }

        #endregion

    }
}
