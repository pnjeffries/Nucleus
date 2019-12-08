using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Interface for objects which are selectable by the user and which
    /// raise events to notify 
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Event raised when the object is selected
        /// </summary>
        event EventHandler Selected;

        /// <summary>
        /// Event raised when the object is unselected
        /// </summary>
        event EventHandler Unselected;

        /// <summary>
        /// Raises the Selected event on this object
        /// </summary>
        void NotifySelected();

        /// <summary>
        /// Raises the Unselected event on this object
        /// </summary>
        void NotifyUnselected();
    }

    /// <summary>
    /// Extension methods for the ISelectable Interface
    /// </summary>
    public static class ISelectableExtensions
    {
        /// <summary>
        /// Raise the relevant selection or deselection events on this selectable object
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="selected"></param>
        public static void NotifySelectedStatusChanged(this ISelectable selectable, bool selected)
        {
            if (selected) selectable.NotifySelected();
            else selectable.NotifyUnselected();
        }
    }
}
