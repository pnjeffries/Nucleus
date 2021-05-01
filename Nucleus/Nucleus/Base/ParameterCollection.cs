using Nucleus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// A collection of parameters.
    /// Each parameter within this collection must have a unique name.
    /// </summary>
    [Serializable]
    public class ParameterCollection : ObservableKeyedCollection<string, Parameter>
    {
        #region Constructors

        /// <summary>
        /// Creates a new empty ParameterCollection
        /// </summary>
        public ParameterCollection() { }

        /// <summary>
        /// Creates a new ParameterCollection containing the specified parameters
        /// </summary>
        /// <param name="paras"></param>
        public ParameterCollection(params Parameter[] paras)
        {
            AddRange(paras);
        }

        /// <summary>
        /// Creates a new ParameterCollection containing the parameters contained within another
        /// </summary>
        /// <param name="other"></param>
        public ParameterCollection(ParameterCollection other)
        {
            AddRange(other);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the key value from the parameter item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override string GetKeyForItem(Parameter item)
        {
            return item.Name;
        }

        /// <summary>
        /// Get the parameters in this collection sorted into their assigned groups.
        /// This will return a dictionary of separate lists for each group.  The
        /// dictionary returned will be sorted according to the Order weighting of
        /// the groups.
        /// </summary>
        /// <returns></returns>
        public IDictionary<ParameterGroup, IList<Parameter>> GetGroupedParameters()
        {
            var result = new SortedDictionary<ParameterGroup, IList<Parameter>>();
            var nullGroup = new ParameterGroup("", 10000000);
            foreach (var para in this)
            {
                ParameterGroup group = para.Group;
                if (group == null) group = nullGroup;
                if (!result.ContainsKey(group))
                {
                    result.Add(group, new ParameterCollection());
                }
                result[group].Add(para);
            }
            return result;
        }

        /// <summary>
        /// Write the current values of this parameter collection
        /// to a string in INI format
        /// </summary>
        /// <returns></returns>
        public string ToINI()
        {
            var sb = new StringBuilder();
            var groups = this.GetGroupedParameters();
            foreach (var kvp in groups)
            {
                sb.Append("[").Append(kvp.Key).AppendLine("]");
                foreach (var param in kvp.Value)
                {
                    sb.Append(param.Name).Append("=").AppendLine(param.GetValue().ToString());
                    //TODO: Units?
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Add all parameter names in this collection to the specified collection of strings
        /// </summary>
        /// <param name="populate"></param>
        public void ExtractAllParameterNames(ICollection<string> populate, ICollection<string> units = null)
        {
            foreach (var parameter in this)
            {
                if (!populate.Contains(parameter.Name))
                {
                    populate.Add(parameter.Name);
                    if (units != null)
                    {
                        units.Add(parameter.Units?.Symbol);
                    }
                }
            }
        }

        /// <summary>
        /// Populate this ParameterCollection with parameters matching the 
        /// properties of a source object.
        /// </summary>
        /// <param name="source">The object from which to extract the parameters</param>
        /// <param name="namePrefix">A prefix to be prepended to the start of the generated property names</param>
        public void GenerateFromProperties(object source, string namePrefix = "")
        {
            var type = source.GetType();
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                var pType = typeof(Parameter<>);
                var gType = pType.MakeGenericType(new Type[] { prop.PropertyType });
                Parameter parameter = (Parameter)Activator.CreateInstance(
                    gType,
                    new object[] { namePrefix + prop.Name });
                object value = prop.GetValue(source);
                parameter.SetValue(value);

                var dimAtt = DimensionAttribute.ExtractFrom(prop);
                if (dimAtt != null)
                {
                    parameter.Units = dimAtt.Type.SIUnit();
                }

                Add(parameter);
            }
        }

        /// Get a parameter of a generic type from these settings, if it exists.
        /// If it does not, will return null.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="key">The name of the parameter</param>
        /// <returns></returns>
        public virtual TParameter GetParameter<TParameter>(string key)
            where TParameter : Parameter
        {
            if (this.Contains(key)) return this[key] as TParameter;
            else return null;
        }

        /// <summary>
        /// Get the value of the parameter with the specified key name and holding a value of
        /// the specified type, if it exists.  Otherwise will return the default value of the
        /// value type.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TValue GetParameterValue<TValue>(string key)
        {
            var parameter = GetParameter<Parameter>(key);

            if (parameter == null) return default(TValue);

            var result = parameter.GetValue();

            if (result is TValue) return (TValue)result;
            else return default(TValue);
        }

        /// <summary>
        /// Copy the values from equivalent parameters in another parameter collection
        /// </summary>
        /// <param name="other"></param>
        public virtual void CopyValuesFrom(ParameterCollection other)
        {
            foreach (var otherParam in other)
            {
                if (this.Contains(otherParam.Name))
                {
                    var thisParam = this[otherParam.Name];
                    if (thisParam.ValueType.IsAssignableFrom(otherParam.ValueType))
                    {
                        thisParam.SetValueFrom(otherParam);
                    }
                }
            }
        }

        #endregion
    }
}
