using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Maths
{
    /// <summary>
    /// An expression which represents an explicit value
    /// </summary>
    [Serializable]
    public class ValueExpression : Expression
    {
        #region Properties

        /// <summary>
        /// Private backing field for Value property
        /// </summary>
        private object _Value;

        /// <summary>
        /// The value represented by this expression
        /// </summary>
        public object Value
        {
            get { return _Value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new number expression
        /// </summary>
        /// <param name="description"></param>
        public ValueExpression(string description, object value) : base(description)
        {
            _Value = value;
        }

        #endregion

        #region Methods

        public override object Evaluate(IEvaluationContext context = null)
        {
            return _Value;
        }

        public override bool ContainsReference(string variableName)
        {
            return false;
        }

        #endregion
    }
}
