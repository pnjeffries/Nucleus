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

using Nucleus.Model.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A collection of loads
    /// </summary>
    [Serializable]
    public class LoadCollection : ModelObjectCollection<Load>
    {
        #region Constructors

        public LoadCollection()
        {
        }

        protected LoadCollection(Model model) : base(model)
        {
        }

        public LoadCollection(IEnumerable<IEnumerable<Load>> toBeCombined) : base(toBeCombined)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the subset of this collection which has a recorded modification after the specified date and time
        /// </summary>
        /// <param name="since">The date and time to filter by</param>
        /// <returns></returns>
        public LoadCollection Modified(DateTime since)
        {
            return this.Modified<LoadCollection, Load>(since);
        }

        /// <summary>
        /// Get the subset of this collection which has an attached data component of the specified type
        /// </summary>
        /// <typeparam name="TData">The type of data component to check for</typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public LoadCollection AllWithDataComponent<TData>()
            where TData : class
        {
            return this.AllWithDataComponent<LoadCollection, Load, TData>();
        }

        /// <summary>
        /// Get all loads in this collection that belong to the specified load case
        /// </summary>
        /// <param name="loadCase"></param>
        /// <returns></returns>
        public LoadCollection AllInCase(LoadCase loadCase)
        {
            var result = new LoadCollection();
            foreach (Load load in this)
            {
                if (load.Case == loadCase) result.Add(load);
            }
            return result;
        }

        /// <summary>
        /// Get all loads in this collection which are applied to the specified
        /// model object.
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="mObj"></param>
        /// <returns></returns>
        public LoadCollection AllAppliedTo<TObj>(TObj mObj)
            where TObj : ModelObject
        {
            var result = new LoadCollection();
            foreach (Load load in this)
            {
                if (load.IsAppliedTo(mObj)) result.Add(load);
            }
            return result;
        }

        #endregion

    }
}
