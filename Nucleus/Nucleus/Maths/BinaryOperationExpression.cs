using Nucleus.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// Represents an expression preforming an operation 
    /// </summary>
    [Serializable]
    public class BinaryOperationExpression : Expression
    {
        #region Properties

        /// <summary>
        /// Private backing field for OperandA property
        /// </summary>
        private Expression _OperandA;

        /// <summary>
        /// The first operand
        /// </summary>
        Expression OperandA
        {
            get { return _OperandA; }
        }

        /// <summary>
        /// Private backing field for OperandB property
        /// </summary>
        private Expression _OperandB;

        /// <summary>
        /// The second operand
        /// </summary>
        public Expression OperandB
        {
            get { return _OperandB; }
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
        /// Initialise a new binary operation expression
        /// </summary>
        /// <param name="description"></param>
        /// <param name="operandA"></param>
        /// <param name="operandB"></param
        /// <param name="@operator"></param>
        public BinaryOperationExpression(string description, Expression operandA, Expression operandB, string @operator) : base(description)
        {
            _OperandA = operandA;
            _OperandB = operandB;
            _Operator = @operator;
        }

        #endregion

        #region Methods

        public override object Evaluate(IEvaluationContext context = null)
        {
            if (context == null) context = Expression.DefaultContext;
           
            if (_Operator == "=") //Assignment
            {
                if (!(_OperandA is ReferenceExpression))
                    throw new ExpressionException("The left-hand side of an assignment must be a variable.");
                if (_OperandB.ContainsReference(_OperandA.Description))
                    throw new ExpressionException("Cannot assign an expression to a variable that contains a reference to that same variable.  Use := operator to assign current value.");
                context.SetVariable(_OperandA.Description, _OperandB);
                return _OperandB.Evaluate(context);
            }
            else if (_Operator == ":=") // Assign current value
            {
                if (!(_OperandA is ReferenceExpression))
                    throw new ExpressionException("The left-hand side of an assignment must be a variable.");
                object result = _OperandB.Evaluate(context);
                context.SetVariable(_OperandA.Description, new ValueExpression(result?.ToString(), result));
                return result;
            }
            else
            {
                dynamic valueA = _OperandA.Evaluate(context);
                dynamic valueB = _OperandB.Evaluate(context);

                //TODO: Check units

                switch (_Operator)
                {
                    case "+":
                        return valueA + valueB;
                    case "-":
                        return valueA - valueB;
                    case "*":
                    case ".":
                        return valueA * valueB; //TODO: Set units
                    case "==":
                    case "↔":
                    case "≡":
                        return valueA == valueB;
                    case "!=":
                    case "≠":
                        return valueA != valueB;
                    case ">":
                        return valueA > valueB;
                    case ">=":
                    case "≥":
                        return valueA >= valueB;
                    case "<":
                        return valueA < valueB;
                    case "<=":
                    case "≤":
                        return valueA <= valueB;
                    case "AND":
                    case "^":
                    case "&":
                        return valueA & valueB;
                    case "OR":
                    case "˅":
                        return valueA || valueB;
                    case "NAND":
                    case "↑":
                        return !(valueA & valueB);
                    case "NOR":
                    case "↓":
                        return !(valueA | valueB);
                    case "XOR":
                    case "⊕":
                        return (valueA ^ valueB);
                    case "²":
                        return (valueA * valueA * valueB);
                    case "³":
                        return (valueA * valueA * valueA * valueB);

                }
            }
            throw new ExpressionException("Invalid syntax.  Expression could not be evaluated.");
        }

        public override bool ContainsReference(string variableName)
        {
            return (_OperandA.ContainsReference(variableName) || _OperandB.ContainsReference(variableName));
        }

        #endregion
    }
}
