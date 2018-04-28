using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Model.Loading;

namespace Nucleus.Model
{
    public class LoadCombinationCase : FactoredLoadCaseCollection, ILoadCase
    {
        public Guid GUID
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Contains(Load load)
        {
            throw new NotImplementedException();
        }
    }
}
