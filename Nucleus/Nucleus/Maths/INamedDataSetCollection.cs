using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// Interface for collections of NamedDataSets
    /// </summary>
    public interface INamedDataSetCollection : IEnumerable
    {
        IList<string> GetAllKeys();
    }
}
