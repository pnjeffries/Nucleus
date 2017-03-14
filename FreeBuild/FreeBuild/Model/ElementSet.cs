using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A parametrically-defined set of elements.  
    /// Allows element collections to be defined via a base collection and a set of logical 
    /// filters which act upon that collection.
    /// </summary>
    public class ElementSet : ModelObjectSet<Element, ElementCollection,
        ISetFilter<Element>, SetFilterCollection<ISetFilter<Element>, Element>,
        ElementSet, ModelObjectSetCollection<ElementSet>>
    {
        protected override ElementCollection GetItemsInModel()
        {
            return Model?.Elements;
        }
    }
}
