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

using FreeBuild.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Interface for objects which can be duplicated.
    /// Duplicating produces a shallow clone with the exception
    /// of property values which are expected to be unique per instance
    /// of the object, which will themselves be duplicated.
    /// </summary>
    public interface IDuplicatable
    {
    }

    /// <summary>
    /// Extension methods for the IDuplicatable interface
    /// </summary>
    public static class IDuplicatableExtensions
    {
        /// <summary>
        /// Produce a duplicated copy of this object.
        /// Property references will be copied, save for those which
        /// are intended to be unique to this object, which will themselves
        /// be duplicated.
        /// </summary>
        /// <returns>A duplicated copy of this object</returns>
        public static T Duplicate<T>(this T obj) where T : IDuplicatable
        {
            Dictionary<object, object> objMap = null;
            return obj.Duplicate(ref objMap);
        }

        /// <summary>
        /// Produce a duplicated copy of this object.
        /// Property references will be copied, save for those which
        /// are intended to be unique to this object, which will themselves
        /// be duplicated.
        /// </summary>
        /// <param name="objectMap">The map of original objects to duplicated objects.</param>
        /// <returns>A duplicated copy of this object</returns>
        public static T Duplicate<T>(this T obj, ref Dictionary<object, object> objectMap) where T : IDuplicatable
        {
            T clone = (T)Activator.CreateInstance(obj.GetType()); //Create a blank instance of the relevant type
            if (objectMap == null) objectMap = new Dictionary<object, object>();
            objectMap[obj] = clone; //Store the original-clone relationship in the map
            clone.CopyFieldsFrom(obj, ref objectMap);
            if (obj.GetType().IsEnumerable())
            {
                IEnumerable source = (IEnumerable)obj;
                IEnumerable target = (IEnumerable)clone;
                foreach(object item in source)
                {

                }
            }
            //TODO: Deal with duplicating collections

            return clone;
        }

        /// <summary>
        /// Popualate the fields of this object by copying them from equivelent fields on 
        /// another object.  The fields to be copied must share names and types in order to
        /// be successfully transferred.
        /// </summary>
        /// <param name="source">The object to copy fields from.</param>
        public static void CopyFieldsFrom(this object target, object source)
        {
            Dictionary<object, object> objectMap = null;
            target.CopyFieldsFrom(source, ref objectMap);
        }

        /// <summary>
        /// Populate the fields of this object by copying them from equivelent fields on 
        /// another object.  The fields to be copied must share names and types in order to
        /// be successfully transferred.
        /// The CopyAttribute will be used to determine the correct behaviour when copying fields
        /// accross - first on the field itself and then, if not set, on the type of the field.
        /// If neither of these is specified the default is to do a 'shallow' or reference-copy.
        /// </summary>
        /// <param name="source">The object to copy fields from.</param>
        /// <param name="objectMap">A map of original objects to their copies.  Used when duplicating multiple
        /// objects at once to create links between them of the same relative relationships.</param>
        public static void CopyFieldsFrom(this object target, object source, ref Dictionary<object, object> objectMap)
        {
            Type targetType = target.GetType();
            Type sourceType = source.GetType();
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            FieldInfo[] fields = targetType.GetFields(flags);
            foreach (FieldInfo targetField in fields)
            {
                FieldInfo sourceField = sourceType.GetField(targetField.Name, flags);
                if (sourceField != null && targetField.FieldType.IsAssignableFrom(sourceField.FieldType))
                {
                    //Have found a matching property - check for copy behaviour attributes:
                    //Currently this is done on the source field.  Might it also be safer to check the target
                    //field as well, for at least certain values?
                    CopyAttribute copyAtt = sourceField.GetCustomAttribute(typeof(CopyAttribute)) as CopyAttribute;
                    //If copy attribute is not set on the field, we will try it on the type:
                    if (copyAtt == null) copyAtt = sourceField.FieldType.GetCustomAttribute(typeof(CopyAttribute)) as CopyAttribute;

                    CopyBehaviour behaviour = CopyBehaviour.COPY;
                    if (copyAtt != null) behaviour = copyAtt.Behaviour;
                    if (behaviour != CopyBehaviour.DO_NOT_COPY)
                    {
                        object value = sourceField.GetValue(source);
                        if (behaviour == CopyBehaviour.MAP ||
                            behaviour == CopyBehaviour.MAP_OR_COPY ||
                            behaviour == CopyBehaviour.MAP_OR_DUPLICATE)
                        {
                            //Attempt to map:
                            if (objectMap != null && value != null && objectMap.ContainsKey(value))
                            {
                                value = objectMap[value];
                                targetField.SetValue(target, value);
                            }
                            //Fallback behaviours on mapping fail:
                            else if (behaviour == CopyBehaviour.MAP_OR_COPY) behaviour = CopyBehaviour.COPY;
                            else if (behaviour == CopyBehaviour.MAP_OR_DUPLICATE) behaviour = CopyBehaviour.DUPLICATE;
                            else behaviour = CopyBehaviour.DO_NOT_COPY;
                        }
                        //Non-mapping behaviours:
                        if (behaviour == CopyBehaviour.DUPLICATE)
                        {
                            if (value is IDuplicatable)
                            {
                                IDuplicatable dupObj = value as IDuplicatable;
                                value = dupObj.Duplicate(ref objectMap);
                                targetField.SetValue(target, value);
                            }
                            else behaviour = CopyBehaviour.COPY;
                        }
                        if (behaviour == CopyBehaviour.COPY)
                        {
                            targetField.SetValue(target, value);
                        }
                    }
                }
            }
        }
    }
}
