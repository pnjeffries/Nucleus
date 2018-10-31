using Nucleus.Extensions;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UI
{
    /// <summary>
    /// Manager class which implements basic functionality to support 
    /// property binding behaviour.
    /// </summary>
    public class DataBinding
    {
        #region Properties

        /// <summary>
        /// Private backing field for DataContext property
        /// </summary>
        private object _DataContext = null;

        /// <summary>
        /// Get or set the datacontext of the binding.
        /// </summary>
        public object DataContext
        {
            get
            {
                return _DataContext;
            }
            set { _DataContext = value; }
        }

        /// <summary>
        /// Private backing member variable for the Path property
        /// </summary>
        private string _Path;

        /// <summary>
        /// Get or set the path to the binding source property
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }

        /// <summary>
        /// Private backing member variable for the StringFormat property
        /// </summary>
        private string _StringFormat = "{0}";

        /// <summary>
        /// Gets or sets a string that specifies how to format the binding if it displays the bound value as a string.
        /// </summary>
        public string StringFormat
        {
            get { return _StringFormat; }
            set { _StringFormat = value; }
        }

        /// <summary>
        /// Is a refresh of the UI necessary?
        /// If true, this should be performed on the next Update()
        /// </summary>
        private bool _UIRefreshRequired = true;

        /// <summary>
        /// Get or set whether a refresh of the UI should be performed
        /// on the next update.
        /// </summary>
        public bool UIRefreshRequired
        {
            get { return _UIRefreshRequired; }
            set { _UIRefreshRequired = value; }
        }

        /// <summary>
        /// Private backing field for BindingRefreshRequired
        /// </summary>
        private int _BindingRefreshIndex = 0;

        /// <summary>
        /// Get or set an integer value indicating whether the
        /// binding chain should be refreshed and if so from
        /// which position.  This is indicated by setting the value
        /// to the index of the item in the chain which should be
        /// refreshed.  If the value is lower than 0, no refresh is
        /// required.
        /// </summary>
        public int BindingRefreshIndex
        {
            get { return _BindingRefreshIndex; }
            set { _BindingRefreshIndex = value; }
        }

        /// <summary>
        /// Get or set whether a Binding refresh is required
        /// </summary>
        public bool BindingRefreshRequired
        {
            get { return _BindingRefreshIndex >= 0; }
            set
            {
                if (value) _BindingRefreshIndex = 0;
                else _BindingRefreshIndex = -1;
            }
        }

        /// <summary>
        /// Private backing member variable for the BindingChain property
        /// </summary>
        private IList<BindingChainLink> _BindingChain = new List<BindingChainLink>();

        /// <summary>
        /// The binding chain - the sequence of objects and properties that lead 
        /// from the data context to the target of the binding.
        /// </summary>
        public IList<BindingChainLink> BindingChain
        {
            get { return _BindingChain; }
            set { _BindingChain = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the current value of the bound property
        /// </summary>
        /// <returns></returns>
        public object GetBoundValue()
        {
            if (DataContext != null)
            {
                if (string.IsNullOrWhiteSpace(Path)) return DataContext;
                var result = DataContext.GetFromPath(Path);
                return result;
            }
            return null;
        }

        /// <summary>
        /// Set the current value of the bound property
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetBoundValue(object value)
        {
            if (DataContext != null)
            {
                if (string.IsNullOrWhiteSpace(Path)) return false;
                DataContext.SetByPath(Path, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Rebuild the binding chain to establish property change monitoring
        /// </summary>
        public void RefreshBinding()
        {
            BindingRefreshRequired = false;

            // Clear old binding monitoring:
            if (BindingChain != null)
            {
                BindingChain.RemovePropertyChangedHandler(Source_PropertyChanged);
            }

            object context = DataContext;
            if (context != null && !string.IsNullOrEmpty(Path))
            {
                BindingChain = context.GenerateBindingChain(Path);
                BindingChain.AddPropertyChangedHandler(Source_PropertyChanged);
            }
        }

        /// <summary>
        /// Handles propertychanged events on bound source objects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (BindingChain != null && sender is INotifyPropertyChanged)
            {
                int i = BindingChain.IndexOfSource((INotifyPropertyChanged)sender);
                if (i >= 0)
                {
                    var link = BindingChain[i];
                    if (e.PropertyName.EqualsIgnoreCase(link.PropertyName))
                    {
                        UIRefreshRequired = true;

                        if (i < BindingChain.Count - 1 && i < BindingRefreshIndex)
                            BindingRefreshIndex = i;
                    }
                }
            }
        }

        #endregion
    }
}
