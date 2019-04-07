using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.AI
{
    /// <summary>
    /// A layer of neurons in a neural network
    /// </summary>
    [Serializable]
    public class NeuralLayer : List<Neuron>
    {
        #region Properties

        /// <summary>
        /// Private backing field for Weight property
        /// </summary>
        private double _Weight = 1.0;

        /// <summary>
        /// The weighting of this layer
        /// </summary>
        public double Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }

        #endregion

        #region Constructor

        public NeuralLayer() { }

        public NeuralLayer(int count, double weight = 1.0)
        {
            for (int i = 0; i < count; i++)
            {
                Add(new Neuron());
            }
        }

        #endregion
    }
}
