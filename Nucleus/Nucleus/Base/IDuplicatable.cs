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
using Nucleus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
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
        /// Family references will be copied, save for those which
        /// are intended to be unique to this object, which will themselves
        /// be duplicated.
        /// </summary>
        /// <param name="itemsBehaviour">The duplication behaviour of items contained within
        /// this object, if this object is a collection</param>
        /// <returns>A duplicated copy of this object</returns>
        public static T Duplicate<T>(this T obj, CopyBehaviour itemsBehaviour = CopyBehaviour.COPY) where T : IDuplicatable
        {
            Dictionary<object, object> objMap = null;
            return obj.Duplicate(ref objMap, itemsBehaviour);
        }

        /// <summary>
        /// Produce a duplicated copy of this object.
        /// Family references will be copied, save for those which
        /// are intended to be unique to this object, which will themselves
        /// be duplicated.
        /// </summary>
        /// <param name="objectMap">The map of original objects to duplicated objects.</param>
        /// <returns>A duplicated copy of this object</returns>
        public static T Duplicate<T>(this T obj, ref Dictionary<object, object> objectMap, CopyBehaviour itemsBehaviour = CopyBehaviour.COPY) 
            where T : IDuplicatable
        {
            T clone;
            if (obj.GetType().HasParameterlessConstructor())
            {
                clone = (T)Activator.CreateInstance(obj.GetType(), true); //Create a blank instance of the relevant type
            }
            else
            {
                // As a (potentially dangerous) fallback:
                clone = (T)FormatterServices.GetUninitializedObject(obj.GetType());
            }

            if (objectMap == null) objectMap = new Dictionary<object, object>();
            objectMap[obj] = clone; //Store the original-clone relationship in the map
            clone.CopyFieldsFrom(obj, ref objectMap);

            if (obj.GetType().IsCollection() && itemsBehaviour != CopyBehaviour.DO_NOT_COPY)
            {
                ICollection source = (ICollection)obj;
                ICollection target = (ICollection)clone;
                foreach (object item in source)
                {
                    CopyBehaviour behaviour = itemsBehaviour;
                    if (item != null && item is IDuplicatable)
                    {
                        CopyAttribute cAtt = item.GetType().GetCustomAttribute<CopyAttribute>();
                        if (cAtt != null) behaviour = cAtt.Behaviour;
                    }
                    object value = ValueToAssign(item, ref behaviour, itemsBehaviour, ref objectMap);
                    if (target is IList)
                    {
                        ((IList)target).Add(value);
                    }
                    // TODO: Other types of collections?
                }
            }

            return clone;
        }

    /// <summary>
    /// Populate the properties of this object by copying them from equivalent public
    /// fields on another object.  The properties to be copied must share names and types
    /// in order to be successfully transferred.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    public static void CopyPropertiesFrom(this object target, object source)
        {
            Type targetType = target.GetType();
            Type sourceType = source.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            PropertyInfo[] properties = targetType.GetProperties(flags);
            foreach (PropertyInfo targetProperty in properties)
            {
                if (targetProperty.CanWrite && targetProperty.GetSetMethod() != null && !targetProperty.GetSetMethod().IsAssembly)
                {
                    PropertyInfo sourceProperty = sourceType.GetProperty(targetProperty.Name, flags);
                    if (sourceProperty != null && sourceProperty.CanRead && targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    {
                        object value = sourceProperty.GetValue(source, null);
                        targetProperty.SetValue(target, value, null);
                    }
                }
            }
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
        /// Populate the fields of this object by copying them from equivalent fields on 
        /// another object.  The fields to be copied must share names and types in order to
        /// be successfully transferred.
        /// The CopyAttribute will be used to determine the correct behaviour when copying fields
        /// accross - first on the field itself and then, if not set, on the type of the field.
        /// If neither of these is specified the default is to do a 'shallow' or reference-copy on all objects
        /// except for collection type, where the default is to not copy any fields *unless* they are
        /// specifically annotated.
        /// </summary>
        /// <param name="source">The object to copy fields from.</param>
        /// <param name="objectMap">A map of original objects to their copies.  Used when duplicating multiple
        /// objects at once to create links between them of the same relative relationships.</param>
        public static void CopyFieldsFrom(this object target, object source, ref Dictionary<object, object> objectMap)
        {
            Type targetType = target.GetType();
            Type sourceType = source.GetType();
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            ICollection<FieldInfo> fields = targetType.GetAllFields(flags);
            foreach (FieldInfo targetField in fields)
            {
                FieldInfo sourceField = sourceType.GetBaseField(targetField.Name, flags);
                if (sourceField != null && targetField.FieldType.IsAssignableFrom(sourceField.FieldType))
                {
                    //Have found a matching property - check for copy behaviour attributes:
                    //Currently this is done on the source field.  Might it also be safer to check the target
                    //field as well, for at least certain values?
                    CopyAttribute copyAtt = sourceField.GetAttribute<CopyAttribute>();
                    //If copy attribute is not set on the field, we will try it on the type:
                    if (copyAtt == null) copyAtt = sourceField.FieldType.GetCustomAttribute<CopyAttribute>();

                    CopyBehaviour behaviour = CopyBehaviour.COPY;
                    CopyBehaviour itemsBehaviour = CopyBehaviour.COPY;
                    if (sourceType.IsCollection())
                    {
                        behaviour = CopyBehaviour.DO_NOT_COPY; //By default, do not copy fields from collection types
                    }
                    if (copyAtt != null)
                    {
                        behaviour = copyAtt.Behaviour;
                        if (copyAtt is CollectionCopyAttribute)
                        {
                            itemsBehaviour = ((CollectionCopyAttribute)copyAtt).ItemsBehaviour;
                        }
                    }

                    if (behaviour != CopyBehaviour.DO_NOT_COPY)
                    {
                        object value = sourceField.GetValue(source);
                        value = ValueToAssign(value, ref behaviour, itemsBehaviour, ref objectMap);
                        if (behaviour != CopyBehaviour.DO_NOT_COPY) targetField.SetValue(target, value);
                    }
                }
            }
        }

        /// <summary>
        /// Convert the value extracted from the source object into an equivalent value suitable
        /// to be assigned to the target property, based on the copying behaviour assigned
        /// </summary>
        /// <param name="value"></param>
        /// <param name="behaviour"></param>
        /// <param name="objectMap"></param>
        /// <returns></returns>
        private static object ValueToAssign(object value, ref CopyBehaviour behaviour, 
            CopyBehaviour itemsBehaviour, ref Dictionary<object, object> objectMap)
        {
            if (behaviour == CopyBehaviour.MAP ||
                            behaviour == CopyBehaviour.MAP_OR_COPY ||
                            behaviour == CopyBehaviour.MAP_OR_DUPLICATE)
            {
                //Attempt to map:
                if (objectMap != null && value != null && objectMap.ContainsKey(value))
                {
                    value = objectMap[value];
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
                    value = dupObj.Duplicate(ref objectMap, itemsBehaviour);
                }
                else behaviour = CopyBehaviour.COPY;
            }
            return value;
        }
        
    }
}
