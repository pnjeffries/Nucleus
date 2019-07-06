using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// 
    /// </summary>
    public class OriginalDomainGeometryAttributes : GeometryAttributes
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the SubDomain property
        /// </summary>
        private Interval _SubDomain;

        /// <summary>
        /// The subdomain of this curve within a longer parent curve
        /// </summary>
        public Interval SubDomain
        {
            get { return _SubDomain; }
            set { _SubDomain = value; }
        }

        #endregion
    }
}
