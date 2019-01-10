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

using Nucleus.Base;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Conversion
{
    /// <summary>
    /// A basic set of options used when converting between types.
    /// </summary>
    [Serializable]
    public class ConversionOptions : NotifyPropertyChangedBase
    {
        #region Properties

        /// <summary>
        /// Private backing field for DeleteMissingObjects property
        /// </summary>
        private bool _DeleteObjects = true;

        /// <summary>
        /// Should objects missing from the source dataset during synchronisation be deleted from the
        /// target model?
        /// </summary>
        /// <summary>
        /// Read/Write Panel Elements?
        /// </summary>
        [AutoUI(100, ToolTip = "Delete objects missing or marked as deleted in the target model?")]
        public bool DeleteObjects
        {
            get { return _DeleteObjects; }
            set { _DeleteObjects = value;  NotifyPropertyChanged("DeleteObjects"); }
        }

        #endregion
    }
}
