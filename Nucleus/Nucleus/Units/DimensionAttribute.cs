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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Units
{
    /// <summary>
    /// Attribute to be applied to properties in order to specify the dimensions
    /// of the quantities they represent.
    /// </summary>
    [Serializable]
    public class DimensionAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// The type of dimension of the annotated property
        /// </summary>
        public DimensionType Type { get; } = DimensionType.Dimensionless;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        public DimensionAttribute(DimensionType type)
        {
            Type = type;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Helper function to get the (first) ActionInput attribute from the specified PropertyInfo
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public static DimensionAttribute ExtractFrom(PropertyInfo pInfo)
        {
            object[] actionAtts = pInfo.GetCustomAttributes(typeof(DimensionAttribute), false);
            if (actionAtts.Count() > 0)
            {
                return (DimensionAttribute)actionAtts[0];
            }
            return null;
        }

        #endregion
    }
}
