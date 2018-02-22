using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Model.Loading;

namespace Nucleus.Model
{
    /// <summary>
    /// A combination of (potentially factored) cases
    /// </summary>
    public class CombinationCase : ResultsCase
    {

        #region Properties

        /// <summary>
        /// Private backing member variable for the Cases property
        /// </summary>
        private FactoredCaseCollection _Cases;

        /// <summary>
        /// The collection of factored cases which make up this combination case
        /// </summary>
        public FactoredCaseCollection Cases
        {
            get
            {
                if (_Cases == null) _Cases = new FactoredCaseCollection();
                return _Cases;
            }
        }

        #endregion

        #region Methods

        public override bool Contains(Load load)
        {
            if (_Cases != null)
                foreach (var fCase in _Cases)
                    if (fCase.Case.Contains(load)) return true;

            return false;
        }

        /// <summary>
        /// Does this combination include the specified case
        /// (either directly or as part of a sub-case)?
        /// </summary>
        /// <param name="rCase"></param>
        /// <returns></returns>
        public bool Contains(ResultsCase rCase)
        {
            if (_Cases != null)
                foreach (var fCase in _Cases)
                    if (fCase.Case == rCase ||
                        (fCase.Case is CombinationCase & ((CombinationCase)fCase.Case).Contains(rCase)))
                        return true;
            return false;
        }

        /// <summary>
        /// Does this combination case definition contain any circular references
        /// </summary>
        /// <returns></returns>
        public bool IsCircular()
        {
            if (_Cases != null)
                foreach (var fCase in _Cases)
                {
                    if (fCase.Case == this) return true;
                    else if (fCase.Case is CombinationCase)
                    {
                        CombinationCase cCase = (CombinationCase)fCase.Case;
                        if (cCase.Contains(this)) return true;
                        else if (cCase.IsCircular()) return true;
                    }
                }
            return false;
        }

        /// <summary>
        /// Build and return a string description of this combination case,
        /// optionally expanding other referenced combinations
        /// </summary>
        /// <param name="expand"></param>
        /// <returns></returns>
        public string ToDefinition(bool expand = false)
        {
            var sb = new StringBuilder();
            foreach (var fCase in Cases)
            {
                if (fCase.Case != null)
                {
                    if (sb.Length > 0 || fCase.Factor < 0)
                    {
                        if (fCase.Factor < 0) sb.Append(" - ");
                        else sb.Append(" + ");
                    }
                    if (fCase.Factor != 1.0) sb.Append(Math.Abs(fCase.Factor));
                    if (fCase.Case is CombinationCase && expand)
                    {
                        sb.Append("(").Append(((CombinationCase)fCase.Case).ToDefinition(expand)).Append(")");
                    }
                    else
                    {
                        sb.Append(fCase.Case.Name);
                    }
                }
            }
            return sb.ToString();
        }

        #endregion
    }
}
