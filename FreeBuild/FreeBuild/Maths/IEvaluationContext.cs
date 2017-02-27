using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Maths
{
    /// <summary>
    /// Interface for objects which provide an evaluation context for Expressions
    /// </summary>
    public interface IEvaluationContext
    {
        /// <summary>
        /// Get the stored expression variable
        /// </summary>
        /// <param name="name">The name of the variable to retrieve</param>
        /// <returns></returns>
        Expression GetVariable(string name);

        /// <summary>
        /// Set the stored expression variable with the given name
        /// </summary>
        /// <param name="name">The name of the variable to store</param>
        /// <param name="value">The expression to store</param>
        void SetVariable(string name, Expression value);
    }
}
