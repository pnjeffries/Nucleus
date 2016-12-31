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

using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A table of geometry layer objects, keyed by name
    /// </summary>
    [Serializable]
    public class GeometryLayerTable : ObservableKeyedCollection<string, GeometryLayer>
    {
        #region Properties

        /// <summary>
        /// Get the bounding box of all geometry on all layers within this table
        /// </summary>
        public BoundingBox BoundingBox
        {
            get
            {
                if (Count > 0)
                {
                    BoundingBox result = this[0].BoundingBox;
                    for (int i = 1; i < Count; i++)
                    {
                        result.Include(this[1]);
                    }
                    return result;
                }
                else return null;
            }
        }

        #endregion

        #region Methods

        protected override string GetKeyForItem(GeometryLayer item)
        {
            return item.Name;
        }

        protected override void OnCollectionChanged()
        {
            base.OnCollectionChanged();
            NotifyPropertyChanged("BoundingBox");
        }

        #endregion
    }
}
