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

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract base class for objects which represent a load of some kind applied to the model to
    /// be considered during analysis.
    /// </summary>
    [Serializable]
    public abstract class Load : ModelObject
    {

    }

    /// <summary>
    /// Generic abstract base class for objects which represent a load of some kind applied to the model
    /// to be considered during analysis
    /// </summary>
    /// <typeparam name="TApplication">The type of application rule applicable to this load</typeparam>
    [Serializable]
    public abstract class Load<TApplication> : Load
        where TApplication : class, ILoadApplication
    {
        /// <summary>
        /// Private backing field for AppliedTo property
        /// </summary>
        private LoadApplicationCollection<TApplication> _AppliedTo = null;

        /// <summary>
        /// The set of application rules for this load
        /// </summary>
        public LoadApplicationCollection<TApplication> AppliedTo
        {
            get
            {
                if (_AppliedTo == null) _AppliedTo = new LoadApplicationCollection<TApplication>();
                return _AppliedTo;
            }
        }
    }
}
