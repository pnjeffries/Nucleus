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
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A set of loading which is applied to the model under a particular condition
    /// </summary>
    [Serializable]
    public class LoadCase : ResultsCase
    {
        #region Properties

        /// <summary>
        /// Private backing field for CaseType
        /// </summary>
        private LoadCaseType _CaseType = LoadCaseType.Undefined;

        /// <summary>
        /// The type of loading that this case represents
        /// </summary>
        [AutoUIComboBox(Order = 500)]
        public LoadCaseType CaseType
        {
            get { return _CaseType; }
            set { ChangeProperty(ref _CaseType, value, "Type"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new blank load case.
        /// </summary>
        public LoadCase() : base() { }

        /// <summary>
        /// Name constructor.  Initialises a new load case with the specified name.
        /// </summary>
        /// <param name="name">The load case name</param>
        public LoadCase(string name) : base(name) { }

        /// <summary>
        /// Type constructor.  Initialises a new load case of the specified type.
        /// The name will be automatically set to the string value of the caseType.
        /// </summary>
        /// <param name="caseType"></param>
        public LoadCase(LoadCaseType caseType) : base()
        {
            CaseType = caseType;
            Name = caseType.ToString();
        }

        /// <summary>
        /// Name and type constructor.  Initialises a new load case with the
        /// specified properties.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="caseType"></param>
        public LoadCase(string name, LoadCaseType caseType) : base(name)
        {
            CaseType = caseType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the collection of loads which belong to this load case.
        /// This collection is automatically generated when called by
        /// searching through the load table of the model this case
        /// is part of.  This case thus needs to have been added to a
        /// model before calling this function and adding/removing items
        /// from the returned collection will not affect case assignments.
        /// </summary>
        public LoadCollection Loads()
        {
            if (Model != null)
            {
                return Model.Loads.AllInCase(this);
            }
            return new LoadCollection();
        }

        #endregion
    }
}
