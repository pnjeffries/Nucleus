﻿using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nucleus.Unity
{
    /// <summary>
    /// Base class for data binding scripts
    /// </summary>
    public abstract class BindingBase : DataContextOwner, IDataContext
    {
        #region Fields

        /// <summary>
        /// The path of the property to be bound to
        /// </summary>
        public string Path = null;

        /// <summary>
        /// The format of the binding for text binding
        /// </summary>
        public string StringFormat = "{0:.}";

        #endregion

        #region Properties

        /// <summary>
        /// Get the data binding
        /// </summary>
        public abstract DataBinding Binding { get; }

        /// <summary>
        /// The data context of the binding - the object that
        /// the source data is drawn from.
        /// </summary>
        public override object DataContext
        {
            get { return Binding.DataContext; }
            set { Binding.DataContext = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Perform initialisation of the binding
        /// </summary>
        protected void InitialiseBinding()
        {
            Binding.Path = Path;
            Binding.StringFormat = StringFormat;

            RefreshUI();
        }

        /// <summary>
        /// Refresh the visual representation of the data
        /// </summary>
        public abstract void RefreshUI();

        /// <summary>
        /// Is the UI currently locked for editing?
        /// </summary>
        /// <returns></returns>
        public virtual bool IsLocked()
        {
            return false;
        }

        /// <summary>
        /// Process necessary updates due to binding changes
        /// </summary>
        public virtual void BindingUpdates()
        {
            if (Binding.BindingRefreshRequired)
            {
                Binding.RefreshBinding();
            }
            if (Binding.UIRefreshRequired)
            {
                //Prevent updating if the field is being edited:
                if (!IsLocked())
                {
                    Binding.UIRefreshRequired = false;
                    RefreshUI();
                }
            }
        }
        #endregion

    }

    
}
