using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.AI
{
    /// <summary>
    /// An Artificial Neural Network consisting of several layers of neurons
    /// </summary>
    [Serializable]
    public class Network
    {
        #region Properties

        private IList<NeuralLayer> _Layers = new List<NeuralLayer>();

        /// <summary>
        /// The neural layers in the network
        /// </summary>
        public IList<NeuralLayer> Layers
        {
            get { return _Layers; }
            set { _Layers = value; }
        }

        #endregion
    }
}
