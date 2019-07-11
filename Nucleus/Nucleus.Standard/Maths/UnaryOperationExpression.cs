using Nucleus.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// An expression operation acting on a single operand
    /// </summary>
    [Serializable]
    public class UnaryOperationExpression : Expression
    {
        #region Properties

        /// <summary>
        /// Private backing field for Operand property
        /// </summary>
        private Expression _Operand;

        /// <summary>
        /// The operand
        /// </summary>
        Expression Operand
        {
            get { return _Operand; }
        }

        /// <summary>
        /// Private backing field for Operator property
        /// </summary>
        private string _Operator;

        /// <summary>
        /// The operator that should be applied to the operands
        /// </summary>
        public string Operator
        {
            get { return _Operator; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialis a unary operation expression
        /// </summary>
        /// <param name="description"></param>
        /// <param name="operand"></param>
        /// <param name="@operator"></param>
        public UnaryOperationExpression(string description, Expression operand, string @operator) : base(description)
        {
            _Operand = operand;
            _Operator = @operator;
        }

        #endregion

        #region Methods

        public override object Evaluate(IEvaluationContext context = null)
        {
            dynamic value = _Operand.Evaluate(context);

            switch (_Operator)
            {
                case "-":
                    return -value;
                case "+":
                    return +value;
                case "√":
                    return Math.Sqrt(value);
                case "sin":
                    return Math.Sin(value);
                case "cos":
                    return Math.Cos(value);
                case "tan":
                    return Math.Tan(value);
                case "asin":
                    return Math.Asin(value);
                case "acos":
                    return Math.Acos(value);
                case "atan":
                    return Math.Atan(value);
                case "NOT":
                case "¬":
                case "~":
                    return !value;
                case "*":
                    return value;
                case "²":
                    return value * value;
                case "³":
                    return value * value * value;
                // TODO: Units
            }

            throw new ExpressionException("Invalid syntax.  Expression could not be evaluated.");
        }

        public override bool ContainsReference(string variableName)
        {
            return (_Operand.ContainsReference(variableName));
        }

        #endregion
    }
}
