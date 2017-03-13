using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// An interface for objects which own data stores
    /// </summary>
    public interface IDataOwner
    {
        /// <summary>
        /// Notify this owner that a property of a data component has been changed.
        /// This may then be 'bubbled' upwards with a new event.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="propertyName"></param>
        void NotifyComponentPropertyChanged(object component, string propertyName);
    }
}
