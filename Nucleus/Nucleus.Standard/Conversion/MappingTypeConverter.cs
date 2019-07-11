// Copyright (c) 2016 Paul Jeffries
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

#if !JS

using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Conversion
{

    /// <summary>
    /// Converter which provides a customisable property mapping from one type to another
    /// </summary>
    [Serializable]
    public class MappingTypeConverter : NotifyPropertyChangedBase, ITypeConverter
    {

#region Properties

        /// <summary>
        /// Private backing member variable for TypeA property
        /// </summary>
        private Type _TypeA;

        /// <summary>
        /// The first type to map to/from
        /// </summary>
        public Type TypeA
        {
            get { return _TypeA; }
            set
            {
                _TypeA = value;
                NotifyPropertyChanged("TypeA");
            }
        }

        /// <summary>
        /// Private backing member variable for TypeB property
        /// </summary>
        private Type _TypeB;

        /// <summary>
        /// The second type to map to/from
        /// </summary>
        public Type TypeB
        {
            get { return _TypeB; }
            set
            {
                _TypeB = value;
                NotifyPropertyChanged("TypeB");
            }
        }



        /// <summary>
        /// The collection of property mappings that describe how the properties of one type should be converted into another
        /// </summary>
        public ObservableCollection<PropertyMapping> PropertyMap { get; } = new ObservableCollection<PropertyMapping>();

        /// <summary>
        /// Description implementation
        /// </summary>
        public string Description
        {
            get
            {
                return "Mapping: " + TypeA.Name + " ->" + TypeB.Name;
            }
        }

#endregion

#region Methods

        public object Convert(object fromObject)
        {
            throw new NotImplementedException();
        }

#endregion
    }
}

#endif
