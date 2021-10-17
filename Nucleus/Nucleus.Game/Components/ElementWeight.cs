using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Component which describes the weight of a game object
    /// </summary>
    [Serializable]
    public class ElementWeight : Unique, IElementDataComponent
    {
        private double _Weight = 70;

        /// <summary>
        /// The base weight of the element
        /// </summary>
        public double Weight
        {
            get { return _Weight; }
            set { ChangeProperty(ref _Weight, value); }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ElementWeight() { }

        /// <summary>
        /// Weight constructor
        /// </summary>
        /// <param name="weight"></param>
        public ElementWeight(double weight)
        {
            _Weight = weight;
        }

        /// <summary>
        /// Get the knockback modifier for the current weight 
        /// </summary>
        public int KnockbackModifier
        {
            get
            {
                // TODO: Get combined weight?
                if (_Weight <= 50) return 1; // Small
                else if (_Weight <= 100) return 0; // Medium
                else if (_Weight <= 500) return -1; // Large
                else return -2; //Huge
            }
        }
    }
}
