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

using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A property which describes the cross-thickness
    /// makeup of a panel element in order to produce
    /// a 3D solid geometry
    /// </summary>
    [Serializable]
    public class BuildUpFamily : Family
    {
        #region Properties

        private BuildUpLayerCollection _Layers;

        /// <summary>
        /// The collection of build-up layers that define the through-thickness
        /// properties of this family
        /// </summary>
        public BuildUpLayerCollection Layers
        {
            get
            {
                if (_Layers == null) _Layers = new BuildUpLayerCollection(this);
                return _Layers;
            }
        }

        /// <summary>
        /// Private backing field for the SetOut property
        /// </summary>
        private VerticalSetOut _SetOut = VerticalSetOut.Centroid;

        /// <summary>
        /// The set-out position of the layers
        /// </summary>
        public VerticalSetOut SetOut
        {
            get { return _SetOut; }
            set { ChangeProperty(ref _SetOut, value, "SetOut"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new blank Panel Family
        /// </summary>
        public BuildUpFamily() : base() { }

        /// <summary>
        /// Initialse a new Panel Family with the given name
        /// </summary>
        /// <param name="name"></param>
        public BuildUpFamily(string name) : this()
        {
            Name = name;
        }

        // <summary>
        /// Initialise a new Panel Family with the specified name, thickness and material
        /// </summary>
        /// <param name="thickness"></param>
        /// <param name="material"></param>
        public BuildUpFamily(string name, double thickness, Material material) : this(thickness, material)
        {
            Name = name;
        }

        /// <summary>
        /// Initialise a new Panel Family with the specified thickness and material
        /// </summary>
        /// <param name="thickness"></param>
        /// <param name="material"></param>
        public BuildUpFamily(double thickness, Material material) : this()
        {
            Layers.Add(new BuildUpLayer(thickness, material));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Notify this family that one of its constituant layers has been modified
        /// </summary>
        /// <param name="layer"></param>
        internal void NotifyBuildUpChanged(BuildUpLayer layer)
        {
            NotifyPropertyChanged("BuildUp");
        }

        /// <summary>
        /// Get the material of the first layer in this build-up
        /// </summary>
        /// <returns></returns>
        public override Material GetPrimaryMaterial()
        {
            if (Layers != null && Layers.Count > 0)
                return Layers.First()?.Material;
            else
                return null;
        }

        /// <summary>
        /// Get the overall thickness of the build-up or of any layers with the specified material within this build-up
        /// </summary>
        /// <param name="material">The material to filter by.  If this is null then all layers will be counted regardless of material.</param>
        /// <returns></returns>
        public double GetThickness(Material material = null)
        {
            double result = 0;

            foreach (var layer in Layers)
            {
                if (material == null || layer.Material == material) result += layer.Thickness;
            }

            return result;
        }

        #endregion
    }
}
