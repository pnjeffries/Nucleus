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
using Nucleus.DDTree;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
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

        /// <summary>
        /// Get the bounding box of all geometry on visible layers within this table
        /// </summary>
        public BoundingBox VisibleBoundingBox
        {
            get
            {
                if (Count > 0)
                {
                    BoundingBox result = null;// this[0].BoundingBox;
                    for (int i = 0; i < Count; i++)
                    {
                        GeometryLayer layer = this[i];
                        if (layer.Visible)
                        {
                            if (result == null) result = layer.BoundingBox;
                            result.Include(layer);
                        }
                    }
                    return result;
                }
                else return null;
            }
        }

        /// <summary>
        /// Get the number of geometry objects contained on all layers of this table
        /// </summary>
        public int ObjectCount
        {
            get 
            {
                int result = 0;
                foreach (var layer in this) result += layer.Count;
                return result;
            }
        }

        /// <summary>
        /// Private backing field for VertexTree property
        /// </summary>
        [NonSerialized]
        private VertexDDTree _VertexTree = null;

        /// <summary>
        /// Get a Divided-Dimension Tree structure containing all vertices belonging to
        /// geometry in this table
        /// </summary>
        public VertexDDTree VertexTree
        {
            get
            {
                if (_VertexTree == null) _VertexTree = CreateVertexTree();
                return _VertexTree;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove any instances of the geometry with the specified
        /// key from any layer in this table
        /// </summary>
        /// <param name="key">The GUID of the geometry to be removed</param>
        /// <returns>True if any geometry was removed, else false.</returns>
        public bool RemoveGeometry(Guid key)
        {
            bool found = false;
            foreach (var layer in this)
            {
                if (layer.Remove(key)) found = true;
            }
            return found;
        }

        /// <summary>
        /// Remove any instances of the geometry with the specified
        /// key from any layer in this table
        /// </summary>
        /// <param name="geometry">The geometry to remove</param>
        /// <returns>True if any geometry was removed, else false.</returns>
        public bool RemoveGeometry(VertexGeometry geometry)
        {
            return RemoveGeometry(geometry.GUID);
        }

        protected override string GetKeyForItem(GeometryLayer item)
        {
            return item.Name;
        }

        protected override void OnCollectionChanged()
        {
            base.OnCollectionChanged();
            NotifyPropertyChanged("BoundingBox");
            NotifyPropertyChanged("VisibleBoundingBox");
            _VertexTree = null;
        }

        protected override void InsertItem(int index, GeometryLayer item)
        {
            base.InsertItem(index, item);
            item.CollectionChanged += Item_CollectionChanged;
        }

        protected override void SetItem(int index, GeometryLayer item)
        {
            if (index >= 0 && index < Count)
            {
                GeometryLayer pItem = this[index];
                pItem.CollectionChanged -= Item_CollectionChanged;
            }
            base.SetItem(index, item);
            item.CollectionChanged += Item_CollectionChanged;
        }

        protected override void ClearItems()
        {
            foreach (GeometryLayer item in this)
            {
                item.CollectionChanged -= Item_CollectionChanged;
            }
            base.ClearItems();
        }

        protected override void RemoveItem(int index)
        {
            if (index >= 0 && index < Count)
            {
                GeometryLayer item = this[index];
                item.CollectionChanged -= Item_CollectionChanged;
            }
            base.RemoveItem(index);
        }

        private void Item_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged();
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

        /// <summary>
        /// Get all the geometry objects contained within this layer table as
        /// a single flat collection
        /// </summary>
        /// <returns></returns>
        public VertexGeometryCollection AllGeometry()
        {
            var result = new VertexGeometryCollection();
            foreach (GeometryLayer layer in this)
            {
                foreach (VertexGeometry geometry in layer)
                {
                    result.Add(geometry);
                }
            }
            return result;
        }

        /// <summary>
        /// Get all the vertices contained within all the geometry on all the
        /// layers in this table in a single flat collection.
        /// </summary>
        /// <returns></returns>
        public VertexCollection AllVertices()
        {
            return AllGeometry().AllVertices();
        }

        /// <summary>
        /// Create a Divided-Dimension Tree for the vertices of all the geometry in this table
        /// </summary>
        /// <returns></returns>
        public VertexDDTree CreateVertexTree()
        {
            return new VertexDDTree(AllGeometry().AllVertices());
        }

        /// <summary>
        /// Get the geometry in this table that falls within the specified axis-aligned bounding box
        /// </summary>
        /// <param name="bounds">The bounding box to check</param>
        /// <param name="inclusive">If true, the output can include objects that lie partially within the bounds, else
        /// the geometry must be entirely contained by the box to be included</param>
        /// <param name="visibleLayersOnly">If true (default) only layers which are currently visible will be considered</param>
        public VertexGeometryCollection GeometryInBounds(BoundingBox bounds, bool inclusive, bool visibleLayersOnly = true)
        {
            var result = new VertexGeometryCollection();
            foreach (GeometryLayer layer in this)
            {
                if (!visibleLayersOnly || layer.Visible)
                    layer.GeometryInBounds(bounds, inclusive, result);
            }
            return result;
        }

        /// <summary>
        /// Convert any polycurves that consist only of line segments into polylines
        /// </summary>
        public void RationalisePolyCurves()
        {
            foreach (GeometryLayer layer in this)
            {
                layer.RationalisePolycurves();
            }
        }

        /// <summary>
        /// Move all geometry in this layer table along the specified translation vector
        /// </summary>
        /// <param name="translation"></param>
        public void MoveAll(Vector translation)
        {
            foreach (var layer in this)
            {
                layer.MoveAll(translation);
            }
        }

        #endregion
    }
}
