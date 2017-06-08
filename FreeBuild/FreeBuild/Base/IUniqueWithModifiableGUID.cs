using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Interface for unique objects which allow you to modify their
    /// GUID.  This is generally a very bad idea and is intended for use
    /// only when deserialising uniques or when wanting to produce an
    /// *exact* copy between two separate models.
    /// The main purpose of this interface is to make setting the GUID
    /// difficult to do without due consideration - implement this interface
    /// explicitly to prevent its members showing up in intellisense.
    /// </summary>
    interface IUniqueWithModifiableGUID : IUnique
    {
        void SetGUID(Guid guid);
    }
}
