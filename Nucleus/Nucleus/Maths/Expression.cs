using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Nucleus.Maths
{
    /// <summary>
    /// A mathematical equation which can be expressed as a string,
    /// parsed and evaluated to return a number
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpressionTypeConverter))]
    public abstract class Expression
    {
        #region Static Properties

        /// <summary>
        /// The default global context which will be used when evaluating expressions
        /// in cases where a context is not specified.  By default this is null
        /// and is not used - it must be set specifically to a non-null value in order
        /// to come into use.
        /// </summary>
        public static IEvaluationContext DefaultContext { get; set; } = null;

        #endregion

        #region Properties

        /// <summary>
        /// Protected backing field for Description property
        /// </summary>
        protected string _Description;

        /// <summary>
        /// The expression description string
        /// </summary>
        public string Description { get { return _Description; } }

        #endregion

        #region Constructor

        public Expression(string description)
        {
            _Description = description;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluate the expression and return the calculated value
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract object Evaluate(IEvaluationContext context = null);

        /// <summary>
        /// Evaluate the expression and return the calculated value
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public T Evaluate<T>(IEvaluationContext context = null)
        {
            object obj = Evaluate(context);
            if (obj is T) return (T)obj;
            else return default(T);
        }

        /// <summary>
        /// Does this expression contain a reference to the specified variable?
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public abstract bool ContainsReference(string variableName);

        /// <summary>
        /// Parse a string description as an expression
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Expression Parse(string description)
        {
            string opOut = null;
            bool dualOperator = true;
            bool takeFirst = false;
            int i = FindNextOperator(description, ref opOut, ref dualOperator, ref takeFirst);
            if (i >= 0) // Operator found
            {
                if (dualOperator && i > 0)
                {
                    if (opOut == "") opOut = "*";
                    Expression operandA = Parse(description.Substring(0, i));
                    Expression operandB;
                    if (i < description.Length - opOut.Length)
                    {
                        operandB = Parse(description.Substring(i + opOut.Length));
                    }
                    else operandB = Parse("");
                    return new BinaryOperationExpression(description, operandA, operandB, opOut);
                }
                else
                {
                    Expression operand;
                    if (takeFirst) operand = Parse(description.Substring(0, i));
                    else operand = Parse(description.Substring(i + opOut.Length));
                    return new UnaryOperationExpression(description, operand, opOut);
                }

            }
            else // Non-operation expression
            {
                string trimmed = description.Trim(new char[] { ' ', '(', ')', '[', ']' });
                if (trimmed.IsNumeric()) // Is a number
                {
                    return new ValueExpression(trimmed, double.Parse(trimmed));
                }
                else
                {
                    return new ReferenceExpression(trimmed);
                }
            }
        }

        public override string ToString()
        {
            return Description;
        }

        /// <summary>
        /// Find the next operation to be performed (in reverse order).
        /// </summary>
        /// <param name="operatorOut"></param>
        /// <param name="dualOperand"></param>
        /// <param name="takeFirst"></param>
        /// <returns></returns>
        protected static int FindNextOperator(string description, ref string operatorOut, ref bool dualOperand, ref bool takeFirst)
        {
            int LowestScore = 0;
            int multiplier = 1;
            int nextIndex = -1;
            string word = "";
            bool numStart = true;

            for (int i = 0; i <= description.Length - 1; i++)
            {
                char character = description[i];
                int score = 0;
                bool endWord = false;
                bool dual = true;
                bool first = false;
                int iOut = i;
                bool implied = false;

                switch (character)
                {
                    case '(':
                        //Enter brackets
                        if (((!string.IsNullOrEmpty(word) && word.Length < 2) || (i > 0 && (description[i - 1] == ')' || description[i - 1] == ']'))))
                        {
                            score = 5 * multiplier;
                            implied = true;
                        }
                        multiplier *= 10;
                        //An operator inside brackets is worth more (since it must be executed first)
                        endWord = true;
                        break;
                    case ')':
                        //Exit brackets
                        if (multiplier > 1)
                        {
                            multiplier /= 10;
                        }
                        endWord = true;
                        break;
                    case '[':
                        //Square brackets can be used to specify units
                        if (((!string.IsNullOrEmpty(word) && word.Length < 2) || (i > 0 && (description[i - 1] == ')' || description[i - 1] == ']'))))
                        {
                            score = 5 * multiplier;
                            implied = true;
                        }
                        else if (i == 0)
                        {
                            score = 7 * multiplier;
                        }
                        multiplier *= 10;
                        //An operator inside brackets is worth more (since it must be executed first)
                        endWord = true;
                        break;
                    case ']':
                        //Exit brackets
                        if (multiplier > 1)
                        {
                            multiplier /= 10;
                        }
                        endWord = true;
                        break;
                    case '=':
                        if (word == "=" || word == "<" || word == ">" || word == "!" || word == ":")
                        {
                            score = 1 * multiplier;
                            iOut = i - 1;
                        }
                        else
                        {
                            score = 2 * multiplier;
                            //endWord = True
                        }
                        break;
                    case '↔':
                    case '≡':
                        //Equality
                        score = 2 * multiplier;
                        endWord = true;
                        break;
                    case '≠':
                        //Inequality
                        score = 2 * multiplier;
                        endWord = true;
                        break;
                    case '<':
                        score = 2 * multiplier;
                        break;
                    case '>':
                        score = 2 * multiplier;
                        break;
                    case '≤':
                        score = 2 * multiplier;
                        endWord = true;
                        break;
                    case '≥':
                        score = 2 * multiplier;
                        endWord = true;
                        break;
                    case '+':
                        score = 3 * multiplier;
                        endWord = true;
                        break;
                    case '-':
                        score = 4 * multiplier;
                        endWord = true;
                        break;
                    case '*':
                        score = 5 * multiplier;
                        endWord = true;
                        break;
                    case '/':
                    case '÷':
                        score = 6 * multiplier;
                        endWord = true;
                        break;
                    case '^':
                        score = 7 * multiplier;
                        endWord = true;
                        break;
                    case '²':
                    case '³':
                        score = 8 * multiplier;
                        endWord = true;
                        break;
                    case '√':
                        if (LowestScore == 0)
                        {
                            score = 7 * multiplier;
                            dual = false;
                        }
                        break;
                    case ' ':
                        endWord = true;
                        break;
                    case '˄':
                    case '&':
                        //AND
                        score = 7 * multiplier;
                        endWord = true;
                        break;
                    case '˅':
                        //OR
                        score = 7 * multiplier;
                        endWord = true;
                        break;
                    case '¬':
                    case '~':
                        //NOT
                        score = 7 * multiplier;
                        dual = false;
                        endWord = true;
                        break;
                    case '⊕':
                        //XOR
                        score = 7 * multiplier;
                        endWord = true;
                        break;
                    case '↓':
                        //NOR
                        score = 7 * multiplier;
                        endWord = true;
                        break;
                    case '↑':
                        //NAND
                        score = 7 * multiplier;
                        endWord = true;
                        break;
                    case '·':
                        score = 5 * multiplier;
                        endWord = true;
                        break;
                    default:
                        if (numStart && i > 0)
                        {
                            if (!char.IsDigit(character) && word.IsNumeric() && !(character == '.'))
                            {
                                score = 5 * multiplier;
                                endWord = true;
                                implied = true;
                            }
                        }
                        break;
                }

                if (!(char.IsDigit(character) || character == '(' || character == ')' || character == '.'))
                {
                    numStart = false;
                }

                if (endWord)
                {
                    word = "";
                }

                word += character;

                if (word.Length == 4)
                {
                    switch (word)
                    {
                        case "NAND":
                            score = 7 * multiplier;
                            iOut = i - (word.Length - 1);
                            break;
                    }
                }
                else if (word.Length == 3)
                {
                    switch (word)
                    {
                        case "AND":
                            score = 7 * multiplier;
                            iOut = i - (word.Length - 1);
                            break;
                        case "XOR":
                            score = 7 * multiplier;
                            iOut = i - (word.Length - 1);
                            break;
                        case "NOR":
                            score = 7 * multiplier;
                            iOut = i - (word.Length - 1);
                            break;
                    }
                }
                else if (word.Length == 2)
                {
                    switch (word)
                    {
                        case "OR":
                            score = 7 * multiplier;
                            iOut = i - (word.Length - 1);
                            break;
                    }
                }

                //lowestScore must = 0 because these should not be evaluated until they are the first thinks in the expression
                if (LowestScore == 0)
                {
                    if (word.Length == 3)
                    {
                        switch (word)
                        {
                            case "sin":
                                score = 7 * multiplier;
                                dual = false;
                                iOut = i - (word.Length - 1);
                                break;
                            case "cos":
                                score = 7 * multiplier;
                                dual = false;
                                iOut = i - (word.Length - 1);
                                break;
                            case "tan":
                                score = 7 * multiplier;
                                dual = false;
                                iOut = i - (word.Length - 1);
                                break;
                            case "NOT":
                                score = 7 * multiplier;
                                dual = false;
                                iOut = i - (word.Length - 1);
                                break;
                        }
                    }
                    else if (word.Length == 4)
                    {
                        switch (word)
                        {
                            case "asin":
                                score = 7 * multiplier;
                                dual = false;
                                iOut = i - (word.Length - 1);
                                break;
                            case "acos":
                                score = 7 * multiplier;
                                dual = false;
                                iOut = i - (word.Length - 1);
                                break;
                            case "atan":
                                score = 7 * multiplier;
                                dual = false;
                                iOut = i - (word.Length - 1);
                                break;
                        }
                    }
                }
                else if (LowestScore == 8)
                {
                    if (!(character == ')') && !(character == ' '))
                    {
                        //Throw New Exception("Operator required.")
                    }
                }

                if (score > 0 && (LowestScore == 0 || score <= LowestScore))
                {
                    LowestScore = score;
                    nextIndex = iOut;
                    if (!implied)
                    {
                        operatorOut = word;
                    }
                    else
                    {
                        operatorOut = "";
                    }
                    dualOperand = dual;
                    takeFirst = first;
                }

                if (endWord || (word.Length > 1 && word.EndsWith("=")))
                {
                    word = "";
                }

            }

            return nextIndex;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Convert a double value into an expression
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator Expression(double v)
        {
            return new ValueExpression(v);
        }

        /// <summary>
        /// Convert an expression to a double value
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator double(Expression v)
        {
            return Convert.ToDouble(v.Evaluate());
        }

        /// <summary>
        /// Convert a string value into an expression
        /// </summary>
        /// <param name="s"></param>
        public static implicit operator Expression(string s)
        {
            return Parse(s);
        }

        /// <summary>
        /// Convert an expression to a string
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator string(Expression v)
        {
            return v.ToString();
        }

        #endregion
    }

    /// <summary>
    /// Type converter for the expression class
    /// </summary>
    public class ExpressionTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            else if (sourceType.IsAssignableFrom(typeof(double))) return true;
            else return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string) return Expression.Parse((string)value);
            return base.ConvertFrom(context, culture, value);
        }

    }
}
