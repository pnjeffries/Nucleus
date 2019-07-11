using Nucleus.Base;
using Nucleus.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// A standard evaluation context
    /// </summary>
    [Serializable]
    public class EvaluationContext : Unique, IEvaluationContext
    {
        #region Properties

        /// <summary>
        /// Private backing field for Variables property
        /// </summary>
        private Dictionary<string, Expression> _Variables = new Dictionary<string, Expression>();

        /// <summary>
        /// The set of stored named variables in the evaluation context
        /// </summary>
        public Dictionary<string, Expression> Variables
        {
            get { return _Variables; }
        }

        #endregion

        public Expression GetVariable(string name)
        {
            if (Variables.ContainsKey(name)) return Variables[name];
            else
                throw new ExpressionException("There is no stored variable with the name '" + name + "' in the current context.");
        }

        public void SetVariable(string name, Expression value)
        {
            Variables[name] = value;
        }
    }
}
