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

namespace FreeBuild.Actions
{
    /// <summary>
    /// A class which contains information about the context in which an action has been executed.
    /// This is used during object history storage to determine the object's creation circumstances
    /// and can be used to update that object in future.
    /// </summary>
    public class ExecutionInfo
    {
        /// <summary>
        /// The unique identifier of this particular execution.
        /// This will be different each time and can be used to tell execution runs apart.
        /// </summary>
        public Guid ExecutionID { get; } = Guid.NewGuid();

        /// <summary>
        /// The reference string used to identify the source of execution of an action,
        /// used to update previously created output instead of generating it afresh when used
        /// as part of a parametric process.
        /// For example, this may be the GUID of the calling grasshopper component.
        /// Defaults to null when the source is not parametric (i.e. when called manually).
        /// </summary>
        public string SourceReference { get; set; } = null;

        /// <summary>
        /// The iteration number of this execution.  This will increment when the same source calls
        /// this action multiple times during the same update cycle - for example when a list of inputs
        /// is plugged into a Grasshopper component.
        /// </summary>
        public int Iteration { get; set; } = 0;

        /// <summary>
        /// The number of history-tracked items that have been created or updated during this execution.
        /// This will be incremented automatically by the source history manager 
        /// </summary>
        public int HistoryItemCount { get; set; } = 0;

        /// <summary>
        /// Default constructor for manual execution
        /// </summary>
        public ExecutionInfo()
        {
        }

        /// <summary>
        /// Constructor from parametric source
        /// </summary>
        /// <param name="sourceReference"></param>
        /// <param name="interation"></param>
        public ExecutionInfo(string sourceReference, int iteration)
        {
            SourceReference = sourceReference;
            Iteration = iteration;
        }
    }
}
