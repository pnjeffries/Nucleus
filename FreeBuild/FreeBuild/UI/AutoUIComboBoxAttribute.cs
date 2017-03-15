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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.UI
{
    /// <summary>
    /// Attribute for automatic UI generation.  Add this attribute to properties
    /// for which you wish to have UI controls generated automatically.
    /// Properties tagged with this attribute will be represented by a ComboBox.
    /// </summary>
    public class AutoUIComboBoxAttribute : AutoUIAttribute
    {
        #region Properties

        /// <summary>
        /// The path of the property to use for the items source of the combobox
        /// </summary>
        public string ItemsSource { get; set; }

        /// <summary>
        /// The path of the property to use for additional 'special' items to be included
        /// in the 
        /// </summary>
        public string ExtraItemsSource { get; set; } = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemsSource"></param>
        public AutoUIComboBoxAttribute(string itemsSource):base()
        {
            ItemsSource = itemsSource;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemsSource"></param>
        public AutoUIComboBoxAttribute(double order, string itemsSource):base(order)
        {
            ItemsSource = itemsSource;
        }


        #endregion
    }
}
