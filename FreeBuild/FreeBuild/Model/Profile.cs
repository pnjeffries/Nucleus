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

using FreeBuild.Base;
using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Base class for objects representing the profile of a SectionProperty.
    /// </summary>
    [Serializable]
    public abstract class Profile : Unique
    {
        #region Properties

        /// <summary>
        /// The outer perimeter curve of this section profile.
        /// </summary>
        public abstract Curve Perimeter { get; }

        /// <summary>
        /// The collection of curves which denote the voids within this section profile.
        /// </summary>
        public abstract CurveCollection Voids { get; }

        /// <summary>
        /// Private backing field for Material property.
        /// </summary>
        private Material _Material;

        /// <summary>
        /// The primary material assigned to this profile.
        /// </summary>
        public Material Material
        {
            get { return _Material; }
            set { _Material = value;  NotifyPropertyChanged("Material"); }
        }

        #endregion
    }
}
