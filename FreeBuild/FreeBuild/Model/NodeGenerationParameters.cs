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

using FreeBuild.Actions;
using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A set of parameters used in the generation of nodes
    /// </summary>
    public class NodeGenerationParameters
    {
        #region Properties

        /// <summary>
        /// The execution info for the operation that the node generation is taking part as part of
        /// </summary>
        public ExecutionInfo ExInfo { get; set; } = null;

        /// <summary>
        /// The distance tolerance for creating connections
        /// </summary>
        public double ConnectionTolerance { get; set; } = 0;

        /// <summary>
        /// Delete nodes that are no longer connected to elements?
        /// Used during model-level GenerateNodes calls only.
        /// </summary>
        public bool DeleteUnusedNodes { get; set; } = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a set of parameters with the connection tolerance using 
        /// the current global geometric tolerance setting.
        /// </summary>
        public NodeGenerationParameters()
        {
            ConnectionTolerance = Tolerance.Distance;
        }

        /// <summary>
        /// Initialises a set of parameters with the specified values.
        /// </summary>
        /// <param name="connectionTolerance"></param>
        /// <param name="deleteUnusedNodes"></param>
        public NodeGenerationParameters(double connectionTolerance, bool deleteUnusedNodes = false)
        {
            ConnectionTolerance = connectionTolerance;
            DeleteUnusedNodes = deleteUnusedNodes;
        }

        /// <summary>
        /// Initialises a set of parameters with the specified DeleteUnusedNodes value and all
        /// others set to defaults.
        /// </summary>
        /// <param name="deleteUnusedNodes"></param>
        public NodeGenerationParameters(bool deleteUnusedNodes) : this()
        {
            DeleteUnusedNodes = deleteUnusedNodes;
        }

        public NodeGenerationParameters(ExecutionInfo exInfo)
        {
            ExInfo = exInfo;
        }

        #endregion
    }
}
