using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Physics
{
    /// <summary>
    /// A data component used during physical simulations to model
    /// linear elements as strings
    /// </summary>
    public class Spring : IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// The element this spring represents and is attached to
        /// </summary>
        public LinearElement Element { get; set; }

        /// <summary>
        /// The length of the spring at which it is at rest and
        /// is exerting no force, in m
        /// </summary>
        public double RestLength { get; set; }

        /// <summary>
        /// The stiffness of the spring in N/m
        /// </summary>
        public double Stiffness { get; set; }

        #endregion

        #region Constructors

        public Spring(LinearElement element)
        {
            Element = element;
            Reset();
        }

        #endregion

        #region Methods

        // Reset this spring to its initial state
        public void Reset()
        {
            if (Element?.Geometry != null)
            {
                RestLength = Element.Geometry.Length;
                if (Element.Family != null)
                {
                    Stiffness = Element.Family.GetAxialStiffness();
                }
                else Stiffness = 0;
            }
        }

        #endregion
    }
}
