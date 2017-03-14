using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A parametrically-defined set of Linear elements.  
    /// Allows linear element collections to be defined via a base collection and a set of logical 
    /// filters which act upon that collection.
    /// </summary>
    public class LinearElementSet : ModelObjectSet<LinearElement, LinearElementCollection,
        ISetFilter<LinearElement>, SetFilterCollection<ISetFilter<LinearElement>, LinearElement>,
        LinearElementSet, ModelObjectSetCollection<LinearElementSet>>
    {
        protected override LinearElementCollection GetItemsInModel()
        {
            return Model?.Elements?.LinearElements;
        }
    }
}
