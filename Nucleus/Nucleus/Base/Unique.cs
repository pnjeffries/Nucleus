﻿// Copyright (c) 2016 Paul Jeffries
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Abstract base class implementing the IUnique interface.
    /// Unique objects are uniquely identifiable via a GUID.  
    /// </summary>
    [Serializable]
    public abstract class Unique 
        : NotifyPropertyChangedBase, 
        IUnique, 
        IDuplicatable, 
        IUniqueWithModifiableGUID
    {
        #region Properties

        /// <summary>
        /// Private backing field for GUID property
        /// </summary>
        [Copy(CopyBehaviour.DO_NOT_COPY)]
        private Guid _GUID = Guid.NewGuid();

        /// <summary>
        /// The GUID of this object, which can be used to uniquely identify it. 
        /// </summary>
        public Guid GUID { get { return _GUID; } }

        void IUniqueWithModifiableGUID.SetGUID(Guid guid)
        {
            _GUID = guid;
        }

        #endregion

        
    }
}
