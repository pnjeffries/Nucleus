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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Conversion
{
    /// <summary>
    /// Converter which will convert from one type to another via a static method
    /// </summary>
    [Serializable]
    public class MethodTypeConverter : ITypeConverter
    {
        #region Properties

        /// <summary>
        /// The method used to perform the conversion
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// The description text of this converter
        /// </summary>
        public string Description
        {
            get
            {
                return Method.ToString(); //Does this do what I want?
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor creating a new MethodTypeConverter by specifying the method to use
        /// </summary>
        public MethodTypeConverter(MethodInfo method)
        {
            Method = method;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Apply the conversion by calling the method
        /// </summary>
        /// <param name="fromObject"></param>
        /// <returns></returns>
        public object Convert(object fromObject)
        {
            return Method.Invoke(null, new object[] { fromObject });
        }

        #endregion

    }
}
