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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Structural
{
    /// <summary>
    /// Represents a point of nodal support in a structural analysis model
    /// </summary>
    [Serializable]
    public class Support : Unique
    {
        #region properties

        private Bool6D _Directions = new Bool6D(false);

        /// <summary>
        /// The directions in which this support acts
        /// </summary>
        public Bool6D Directions
        {
            get { return _Directions; }
            set { _Directions = value;  NotifyPropertyChanged("Directions"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new support with no properties set.
        /// </summary>
        public Support()
        {
        }

        /// <summary>
        /// Initialises a new support with the specified fixed directions
        /// </summary>
        /// <param name="directions"></param>
        public Support(Bool6D directions)
        {
            Directions = directions;
        }

        #endregion
    }
}
