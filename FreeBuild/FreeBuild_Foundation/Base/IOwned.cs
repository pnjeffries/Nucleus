using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Interface for objects which are 'owned' by some specific other object
    /// </summary>
    public interface IOwned<TOwner>
    {
        /// <summary>
        /// This oject that this object 'belongs' to
        /// </summary>
        TOwner Owner { get; }
    }
}
