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

namespace Nucleus.Model
{
    /// <summary>
    /// An extended FamilyCollection that also contains additional
    /// temporary data structures for fast lookup operations.
    /// </summary>
    [Serializable]
    public class FamilyTable : FamilyCollection
    {
        #region Properties

        /// <summary>
        /// Private backing field for NextNumericID property
        /// </summary>
        private long _NextNumericID = 1;

        /// <summary>
        /// The numeric ID that will be assigned to the next element to be added to this table
        /// </summary>
        public long NextNumericID
        {
            get { return _NextNumericID; }
        }

        /// <summary>
        /// Private backing field for Sections property
        /// </summary>
        private SectionFamilyCollection _Sections = null;

        /// <summary>
        /// The subset of section properties in this table.
        /// Generated when necessary and cached.  Do not modify - modifications to this
        /// collection will have no effect on the source table.
        /// </summary>
        public SectionFamilyCollection Sections
        {
            get
            {
                if (_Sections == null) _Sections = GetSections();
                return _Sections;
            }
        }

        /// <summary>
        /// Private backing field for FaceFamilies property
        /// </summary>
        private BuildUpFamilyCollection _PanelFamilies = null;

        /// <summary>
        /// The subset of panel properties in this table.
        /// Generated when necessary and cached.  Do not modify - modifications to this
        /// collection will have no effect on the source table.
        /// </summary>
        public BuildUpFamilyCollection PanelFamilies
        {
            get
            {
                if (_PanelFamilies == null) _PanelFamilies = GetPanelFamilies();
                return _PanelFamilies;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new VolumetricPropertyTable belonging to the specified model
        /// </summary>
        /// <param name="model"></param>
        public FamilyTable(Model model) : base(model) { }

        #endregion

        #region Methods

        protected override void OnCollectionChanged()
        {
            //Clear cached data:
            _Sections = null;
            _PanelFamilies = null;
        }

        protected override void SetNumericID(Family item)
        {
            item.NumericID = NextNumericID;
            _NextNumericID++;
        }

        #endregion
    }
}
