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

        #endregion
    }
}
