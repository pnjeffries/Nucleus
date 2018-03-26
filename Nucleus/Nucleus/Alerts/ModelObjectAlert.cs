using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Alerts
{
    /// <summary>
    /// Base class for alerts which refer to model objects of a particular type
    /// </summary>
    /// <typeparam name="TItem">The type of item to which this alert refers</typeparam>
    /// <typeparam name="TCollection">The collection which holds the specifed type</typeparam>
    public abstract class ModelObjectAlert<TItem, TCollection> : Alert
        where TItem : ModelObject
        where TCollection : ModelObjectCollection<TItem>, new()
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Items property
        /// </summary>
        private TCollection _Items = new TCollection();

        /// <summary>
        /// The collection of items to which this alert refers
        /// </summary>
        public TCollection Items
        {
            get { return _Items; }
        }

        /// <summary>
        /// The name of the type of object this alert refers to
        /// </summary>
        public virtual string TypeName
        {
            get { return typeof(TItem).Name; }
        }

        /// <summary>
        /// The text which is displayed in the UI to describe this alert
        /// to the user.  By default consists of the set Message, but may
        /// be overridden to allow composite messages to be displayed.
        /// </summary>
        public override string DisplayText
        {
            get
            {
                string s = "";
                if (Items.Count > 1) s = "s";
                return TypeName + s + " " + Items.ToString() + ": " + Message;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new generic alert with the specified ID and message
        /// </summary>
        /// <param name="alertID">The ID of the alert.  Multiple alerts raised with the same ID may be merged.</param>
        /// <param name="message">The alert message to display.</param>
        /// <param name="level">The alert level of the message - indicates how serious the alert is.</param>
        public ModelObjectAlert(string alertID, TItem item, string message, AlertLevel level = AlertLevel.Information) 
            : base(alertID, message, level)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Initialise a new generic alert with the specified message
        /// </summary>
        /// <param name="message">The alert message to display.</param>
        /// <param name="level">The alert level of the message - indicates how serious the alert is.</param>
        public ModelObjectAlert(string message, TItem item, AlertLevel level = AlertLevel.Information)
            :base(message, level)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Initialise a new generic alert with the specified ID and message
        /// </summary>
        /// <param name="alertID">The ID of the alert.  Multiple alerts raised with the same ID may be merged.</param>
        /// <param name="message">The alert message to display.</param>
        /// <param name="level">The alert level of the message - indicates how serious the alert is.</param>
        public ModelObjectAlert(string alertID, IList<TItem> items, string message, AlertLevel level = AlertLevel.Information)
            : base(alertID, message, level)
        {
            Items.AddRange(items);
        }

        /// <summary>
        /// Initialise a new generic alert with the specified message
        /// </summary>
        /// <param name="message">The alert message to display.</param>
        /// <param name="level">The alert level of the message - indicates how serious the alert is.</param>
        public ModelObjectAlert(string message, IList<TItem> items, AlertLevel level = AlertLevel.Information)
            : base(message, level)
        {
            Items.AddRange(items);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Merge another alert into this one.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override void Merge(Alert other)
        {
            if (other is ModelObjectAlert<TItem, TCollection>)
            {
                var mOther = (ModelObjectAlert<TItem, TCollection>)other;
                Items.AddRange(mOther.Items);
            }
            base.Merge(other);
        }

        #endregion
    }
}
