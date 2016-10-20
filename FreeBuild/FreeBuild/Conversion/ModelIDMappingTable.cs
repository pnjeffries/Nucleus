using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// Base class for ID mapping tables where the first ID set reference model objects
    /// </summary>
    /// <typeparam name="TSecondID"></typeparam>
    public class ModelIDMappingTable<TSecondID> : IDMappingTable<Guid, TSecondID>
    {
        #region Constructors

        public ModelIDMappingTable(string firstIDName, string secondIDName) : base(firstIDName, secondIDName)
        {
        }

        #endregion
    }
}
