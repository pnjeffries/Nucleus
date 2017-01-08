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

namespace FreeBuild.Model
{
    /// <summary>
    /// A family which describes a section which should be
    /// swept along the set-out curve of a linear element in
    /// order to produce a 3D solid geometry.
    /// </summary>
    [Serializable]
    public class SectionFamily : Family
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
                SectionProfile oldProfile = _Profile;
                _Profile = value;
                if (_Profile != null) _Profile.Section = this;
                NotifyPropertyChanged("Profile");
                if (oldProfile?.GetType() != _Profile?.GetType()) NotifyPropertyChanged("ProfileType");
                NotifyPropertyChanged("Profiles");
            }
        }

        /// <summary>
        /// Get or set the type of the assigned profile
        /// </summary>
        public Type ProfileType
        {
            get
            {
                return _Profile?.GetType();
            }
            set
            {
                ChangeProfieType(value);
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
        public SectionFamily(){ }

        /// <summary>
        /// Initialises a section property with the given profile
        /// </summary>
        /// <param name="profile"></param>
        public SectionFamily(SectionProfile profile)
        {
            Profile = profile;
        }

        /// <summary>
        /// Initialises a section property with the given name and profile
        /// </summary>
        /// <param name="name"></param>
        /// <param name="profile"></param>
        public SectionFamily(string name, SectionProfile profile)
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
        
        /// <summary>
        /// Change the type of profile of this section, copying across as much data as possible
        /// </summary>
        /// <typeparam name="TProfile">The type of the new profile</typeparam>
        /// <returns>The </returns>
        public TProfile ChangeProfileType<TProfile>() where TProfile : SectionProfile, new()
        {
            SectionProfile oldProfile = Profile;
            TProfile newProfile = new TProfile();
            if (oldProfile != null) newProfile.CopyFieldsFrom(oldProfile);
            newProfile.CatalogueName = null;
            Profile = newProfile;
            return newProfile;
        }

        /// <summary>
        /// Change the type of profile of this section, copying across as much data as possible
        /// </summary>
        /// <param name="newType">The type of the new profile.  Must be a non-abstract subtype of SectionProfile</param>
        /// <returns></returns>
        public SectionProfile ChangeProfieType(Type newType)
        {
            SectionProfile oldProfile = Profile;
            SectionProfile newProfile = Activator.CreateInstance(newType) as SectionProfile;
            if (newProfile != null)
            {
                if (oldProfile != null) newProfile.CopyPropertiesFrom(oldProfile);
                newProfile.CatalogueName = null;
                Profile = newProfile;
            }
            return newProfile;
        }

        #endregion
    }
}
