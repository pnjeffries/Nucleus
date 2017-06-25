using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// An expression that forms a reference to a variable
    /// </summary>
    [Serializable]
    public class ReferenceExpression : Expression
    {
        #region Constructors

        /// <summary>
        /// Initialise a variable reference
        /// </summary>
        /// <param name="description"></param>
        public ReferenceExpression(string description) : base(description)
        { }

        #endregion

        #region Methods

        public override object Evaluate(IEvaluationContext context = null)
        {
            return context.GetVariable(_Description);
            //TODO: Fallback behaviour with no context?
        }

        public override bool ContainsReference(string variableName)
        {
            return (variableName == Description);
        }

        #endregion
    }
}
