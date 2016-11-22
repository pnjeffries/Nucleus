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
    /// A property which describes a section which should be
    /// swept along the set-out curve of a linear element in
    /// order to produce a 3D solid geometry.
    /// </summary>
    [Serializable]
    public class SectionProperty : VolumetricProperty
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Profile property
        /// </summary>
        private SectionProfile _Profile = null;

        /// <summary>
        /// The description of the profile of this section property
        /// </summary>
        public SectionProfile Profile
        {
            get { return _Profile; }
            set
            {
                if (_Profile != null) _Profile.Section = null;
                _Profile = value;
                if (_Profile != null) _Profile.Section = this;
                NotifyPropertyChanged("Profile");
                NotifyPropertyChanged("Profiles");
            }
        }

        /// <summary>
        /// The collection of profiles which make up the 
        /// </summary>
        public SectionProfileCollection  Profiles
        {
            get
            {
                return new SectionProfileCollection(Profile);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new section property with blank data 
        /// </summary>
        public SectionProperty(){ }

        /// <summary>
        /// Initialises a section property with the given profile
        /// </summary>
        /// <param name="profile"></param>
        public SectionProperty(SectionProfile profile)
        {
            Profile = profile;
        }

        /// <summary>
        /// Initialises a section property with the given name and profile
        /// </summary>
        /// <param name="name"></param>
        /// <param name="profile"></param>
        public SectionProperty(string name, SectionProfile profile)
        {
            Name = name;
            Profile = profile;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Notify this section that one of its constutuent profiles has been modified
        /// </summary>
        /// <param name="profile"></param>
        internal void NotifyProfileChanged(SectionProfile profile)
        {
            NotifyPropertyChanged("Profile");
            NotifyPropertyChanged("Profiles");
        }

        #endregion
    }
}
