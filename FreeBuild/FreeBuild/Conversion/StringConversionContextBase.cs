using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// Abstract base class for String conversion contexts.
    /// Implements a basic version of the required functionality.
    /// </summary>
    public abstract class StringConversionContextBase : IStringConversionContext
    {
        /// <summary>
        /// The index of the current subcomponent being written
        /// </summary>
        public virtual int SubComponentIndex { get; set; } = 0;

        /// <summary>
        /// The current source object
        /// </summary>
        public object SourceObject { get; set; } = null;

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
        public virtual bool HasSubComponentsToWrite(object source)
        {
            return false;
        }

        /// <summary>
        /// Set the source object.
        /// </summary>
        /// <param name="source"></param>
        public virtual void SetSourceObject(object source)
        {
            SourceObject = source;
        }
    }
}
