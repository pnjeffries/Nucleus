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
                        result.Include(this[i]);
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

        /// <summary>
        /// Find and return the layer (if any) that the specified piece
        /// of geometry is on.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns>The layer the geometry is on, else null if no layer 
        /// containing the geometry can be found</returns>
        public GeometryLayer LayerOf(VertexGeometry geometry)
        {
            if (geometry.Attributes?.LayerName != null && Contains(geometry.Attributes.LayerName))
            {
                GeometryLayer result = this[geometry.Attributes.LayerName];
                if (result.Contains(geometry.GUID)) return result;
            }

            foreach (GeometryLayer layer in this)
            {
                if (layer.Contains(geometry.GUID)) return layer;
            }

            return null; // Nothing found!
        }

        /// <summary>
        /// Determines whether or not the layers within this table contain the
        /// specified piece of geometry
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public bool Contains(VertexGeometry geometry)
        {
            return LayerOf(geometry) != null;
        }

        /// <summary>
        /// Get the layer with the specified name if it already exists or if
        /// not create and return a new layer in this table with the given name
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public GeometryLayer GetOrCreate(string layerName)
        {
            if (Contains(layerName)) return this[layerName];
            else
            {
                GeometryLayer result = new GeometryLayer(layerName);
                Add(result);
                return result;
            }
        }

        #endregion
    }
}
