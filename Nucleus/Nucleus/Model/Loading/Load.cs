﻿// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Nucleus.Extensions;
using Nucleus.Maths;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model.Loading
{
    /// <summary>
    /// Abstract base class for objects which represent a load of some kind applied to the model to
    /// be considered during analysis.
    /// </summary>
    [Serializable]
    public abstract class Load : ModelObject
    {
        #region Properties

        /// <summary>
        /// Private backing field for Case property
        /// </summary>
        private LoadCase _Case;

        /// <summary>
        /// The load case to which this load belongs
        /// </summary>
        [AutoUIComboBox(Order=300, ItemsSource ="Model.LoadCases")]
        public LoadCase Case
        {
            get { return _Case; }
            set { ChangeProperty(ref _Case, value, "Case"); }
        }

        /// <summary>
        /// Private backing field for Value property
        /// </summary>
        private Expression _Value;

        /// <summary>
        /// The value of the load
        /// </summary>
        [AutoUI(600)]
        public Expression Value
        {
            get { return _Value; }
            set { ChangeProperty(ref _Value, value, "Value"); }
        }

        [AutoUI(Order = 250, Label = "Type")]
        public virtual string LoadType
        {
            get { return GetType().Name.AutoSpace(); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is this load applied to the specified model object?
        /// </summary>
        /// <param name="mObj"></param>
        /// <returns></returns>
        public abstract bool IsAppliedTo(ModelObject mObj);

        /// <summary>
        /// Test whether this load can be applied to an object of the
        /// specified type.
        /// </summary>
        /// <returns></returns>
        public abstract bool CanApplyToType(Type type);

        /// <summary>
        /// Apply this load to an object, if possible
        /// </summary>
        /// <param name="mObj"></param>
        /// <returns>True if the load could be applied.</returns>
        public abstract bool ApplyTo(ModelObject mObj);

        #endregion
    }

    /// <summary>
    /// Generic abstract base class for objects which represent a load of some kind applied to the model
    /// to be considered during an analysis
    /// </summary>
    /// <typeparam name="TAppliedTo">The type of the set of objects to which this load can be applied</typeparam>
    [Serializable]
    public abstract class Load<TAppliedTo, TItem> : Load
        where TAppliedTo : ModelObjectSetBase, new()
        where TItem : ModelObject
    {
        #region Properties

        /// <summary>
        /// Private backing field for AppliedTo property
        /// </summary>
        private TAppliedTo _AppliedTo;

        /// <summary>
        /// The set of objects that this load is applied to
        /// </summary>
        [AutoUI(500, SubProperty = "Definition")]
        public TAppliedTo AppliedTo
        {
            get
            {
                if (_AppliedTo == null)
                {
                    _AppliedTo = new TAppliedTo();
                    _AppliedTo.PropertyChanged += _AppliedTo_PropertyChanged;
                }
                _AppliedTo.Model = Model;
                return _AppliedTo;
            }
        }

        private void _AppliedTo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("AppliedTo"); //Bubble up property changed event
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called immediately after deserialisation to re-register all objects
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_AppliedTo != null) _AppliedTo.PropertyChanged += _AppliedTo_PropertyChanged;
        }

        /// <summary>
        /// Is this load applied to the specified model object?
        /// </summary>
        /// <param name="mObj"></param>
        /// <returns></returns>
        public override bool IsAppliedTo(ModelObject mObj)
        {
            return ((IModelObjectSet)AppliedTo).Contains(mObj);
        }

        /// <summary>
        /// Test whether this load can be applied to an object of the
        /// specified type.
        /// </summary>
        /// <returns></returns>
        public override bool CanApplyToType(Type type)
        {
            return typeof(TItem).IsAssignableFrom(type);
        }

        /// <summary>
        /// Apply this load to the specified object, if possible.
        /// </summary>
        /// <param name="mObj"></param>
        /// <returns>True on success, else false.</returns>
        public override bool ApplyTo(ModelObject mObj)
        {
            if (mObj is TItem)
            {
                return ((IModelObjectSet)AppliedTo).Add(mObj);
            }
            return false;
        }

        /// <summary>
        /// Get a string which descibes the units in which this load's
        /// value is expressed
        /// </summary>
        public virtual string GetValueUnits()
        {
            return "";
        }

        #endregion
    }
}
