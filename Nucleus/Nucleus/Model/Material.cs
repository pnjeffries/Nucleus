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

using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Base class for materials
    /// </summary>
    [Serializable]
    public abstract class Material : DataOwner<MaterialDataStore, IMaterialDataComponent, Material>
    {
        //TODO: Add material properties

        #region Constants

        /// <summary>
        /// Default Steel material
        /// </summary>
        public static Material Steel { get { return new IsoMaterial("Steel", MaterialCategory.Steel); } }

        /// <summary>
        /// Default Concrete material
        /// </summary>
        public static Material Concrete
        {
            get
            {
                return new IsoMaterial("Concrete", MaterialCategory.Concrete)
                { Density = 2400 };
            }
        }

        /// <summary>
        /// Get a default Wood material
        /// </summary>
        public static Material Wood
        {
            get
            {
                return new IsoMaterial("Wood", MaterialCategory.Wood)
                { Density = 500 };
            }
        }

        /// <summary>
        /// Default Aluminium material
        /// </summary>
        public static Material Aluminium
        {
            get
            {
                return new IsoMaterial("Aluminium", MaterialCategory.Aluminium)
                { Density = 2720 };
            }
        }

        /// <summary>
        /// Default Glass material
        /// </summary>
        public static Material Glass
        {
            get
            {
                return new IsoMaterial("Glass", MaterialCategory.Glass)
                { Density = 2500 };
            }
        }

        /// <summary>
        /// Get a collection of all the default materials
        /// </summary>
        public static MaterialCollection Defaults
        {
            get
            {
                var result = new MaterialCollection();
                result.Add(Steel);
                result.Add(Concrete);
                result.Add(Wood);
                result.Add(Aluminium);
                result.Add(Glass);
                return result;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Private backing field for Density property
        /// </summary>
        private double _Density = 8050;

        /// <summary>
        /// The density of the material, in kg/m³
        /// </summary>
        //[AutoUI(300)]
        public double Density
        {
            get { return _Density; }
            set { ChangeProperty(ref _Density, value, "Density"); }
        }

        /// <summary>
        /// Private backing field for Category property
        /// </summary>
        private MaterialCategory _Category = MaterialCategory.Undefined;

        /// <summary>
        /// The preset category under which this material falls.
        /// Used to determine applicable design codes and to map
        /// to equivalent material properties in other software.
        /// </summary>
        [AutoUI(200)]
        public MaterialCategory Category
        {
            get { return _Category; }
            set { ChangeProperty(ref _Category, value, "Category"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Material() : base() { }

        /// <summary>
        /// Name constructor
        /// </summary>
        /// <param name="name"></param>
        public Material(string name, MaterialCategory category = MaterialCategory.Undefined) 
            : base(name)
        {
            _Category = category;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the Elastic (or, Young's) Modulus of this material
        /// in the specified direction, in N/m²
        /// </summary>
        public abstract double GetE(Direction direction);

        /// <summary>
        /// Get the yield strength of this material in the specified
        /// direction, in Pa.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public abstract double GetYieldStrength(Direction direction);

        protected override MaterialDataStore NewDataStore()
        {
            return new MaterialDataStore(this);
        }

        #endregion
    }
}
