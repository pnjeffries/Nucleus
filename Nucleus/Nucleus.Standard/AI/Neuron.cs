using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.AI
{

    /// <summary>
    /// A neuron in an ANN
    /// </summary>
    [Serializable]
    public class Neuron : Unique
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Weights property
        /// </summary>
        private float[] _Weights = null;

        /// <summary>
        /// The indexed list of weightings for neurons in the preceeding layer
        /// </summary>
        public float[] Weights
        {
            get { return _Weights; }
        }

        #endregion

        #region Constructors

        public Neuron()
        { }

        public Neuron(int prevLayerSize)
        {
            Initialise(prevLayerSize);
        }

        /// <summary>
        /// Private initialisation function
        /// </summary>
        /// <param name="prevLayerSize"></param>
        private void Initialise(int prevLayerSize)
        {
            _Weights = new float[prevLayerSize];
        }

        #endregion

    }
}
