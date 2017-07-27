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

using Nucleus.Base;
using Nucleus.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A collection of section profiles
    /// </summary>
    [Serializable]
    public class SectionProfileCollection : UniquesCollection<SectionProfile>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SectionProfileCollection() { }

        /// <summary>
        /// Initialise a new SectionProfileCollection containing the specified profile
        /// </summary>
        /// <param name="profile"></param>
        public SectionProfileCollection(SectionProfile profile)
        {
            if (profile != null) Add(profile);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Find and return the first profile in this collection
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SectionProfile GetByCatalogueName(string name)
        {
            foreach (SectionProfile profile in this)
            {
                if (profile.CatalogueName == name) return profile;
            }
            return null;
        }

        /// <summary>
        /// Add profiles to this collection by loading them from a CSV library file
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadFromCSV(FilePath filePath)
        {
            var parser = new CSVParser<SectionProfile>();
            AddRange(parser.Parse(filePath));
        }

        /// <summary>
        /// Add profiles to this collection by loading them from a CSV string
        /// </summary>
        /// <param name="csvString"></param>
        public void LoadFromCSV(string csvString)
        {
            var parser = new CSVParser<SectionProfile>();
            AddRange(parser.Parse(csvString));
        }

        /// <summary>
        /// Find and return the section profile in this collection with the smallest
        /// cross-sectional area that is greater than the specified value
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public SectionProfile GetByMinimumArea(double area)
        {
            SectionProfile result = null;
            foreach (SectionProfile sP in this)
            {
                if (sP.Area > area && (result == null || sP.Area < result.Area))
                {
                    result = sP;
                }
            }
            return result;
        }

        #endregion
    }
}
