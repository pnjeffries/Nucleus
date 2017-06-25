using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Conversion
{
    /// <summary>
    /// Interface for objects used as a context when extracting values from
    /// object via the GetValue(path) object extension method.
    /// </summary>
    public interface IStringConversionContext
    {
        /// <summary>
        /// The index of the current subcomponent being written
        /// </summary>
        int SubComponentIndex { get; set; }

        /// <summary>
        /// Set the source object about which this context object
        /// should return data.
        /// </summary>
        /// <param name="source"></param>
        void SetSourceObject(object source);

        /// <summary>
        /// Does the specified item still have sub-components which will
        /// need to be written out individually?
        /// Sub-components are child items of the object which need to
        /// be written still utilising properties from the parent -
        /// for example each mesh face in a PanelElement may need to be
        /// treated as a separate element.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        bool HasSubComponentsToWrite(object source);
    }
}
