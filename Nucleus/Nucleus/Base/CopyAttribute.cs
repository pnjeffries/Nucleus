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

using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Attribute applied to types and fields to determine the correct procedure for dealing
    /// with them when automatically copying their values to another object.
    /// By default, fields are 'shallow-copied' - i.e. values and references are copied.
    /// It is only necessary to apply this attribute to fields where this behaviour should
    /// be changed.  Attributes applied to types define the defauly behaviour of that type,
    /// but field-level attributes will override this where present.
    /// </summary>
    public class CopyAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// The behaviour of the annotated field during a copy operation.
        /// By default, properties have their values 'shallow-copied', but may
        /// instead be prevented from being copied or be duplicated instead,
        /// provided the property type is duplicatable.
        /// </summary>
        public CopyBehaviour Behaviour { get; set; } = CopyBehaviour.COPY;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.  Initialise this attribute with the specified copying
        /// behaviour.
        /// </summary>
        /// <param name="behaviour">The copying behaviour of this field.</param>
        public CopyAttribute(CopyBehaviour behaviour)
        {
            Behaviour = behaviour;
        }

        #endregion

        #region Static Fields

        /// <summary>
        /// Cached attributes for fields
        /// </summary>
        private static IDictionary<FieldInfo, CopyAttribute> _FieldCache = new Dictionary<FieldInfo, CopyAttribute>();

        #endregion

        #region Static Methods

        /// <summary>
        /// Get the copy attribute for the specified field.  This is cached to speed up retrieval
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static CopyAttribute GetFor(FieldInfo field)
        {
            if (_FieldCache.ContainsKey(field)) return _FieldCache[field];

            CopyAttribute copyAtt = field.GetAttribute<CopyAttribute>();
            // If copy attribute is not set on the field, we will try it on the type:
            if (copyAtt == null) copyAtt = field.FieldType.GetCustomAttribute<CopyAttribute>();

            _FieldCache[field] = copyAtt;

            return copyAtt;
        }

        #endregion
    }
}
