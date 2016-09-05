using FreeBuild.Actions;
using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Create new objects in the model or update previously created ones.
    /// </summary>
    public class ModelObjectCreator
    {
        #region Properties

        /// <summary>
        /// The model that this creator works on
        /// </summary>
        protected Model Model { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new ModelObjectCreator for the specified model
        /// </summary>
        /// <param name="model"></param>
        internal ModelObjectCreator(Model model)
        {
            Model = model;
        }

        #endregion

        #region Methods

        public Node Node(Vector position, double reuseTolerance = 0, ExecutionInfo exInfo = null)
        {
            Node result = null;
            //TODO: Check for previous 
            if (reuseTolerance > 0)
                result = Model.Nodes.ClosestNodeTo(position, reuseTolerance);

            if (result == null)
            {
                result = new Node(position);
            }
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) linear element in the model.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public LinearElement LinearElement(Curve geometry, ExecutionInfo exInfo = null)
        {
            LinearElement result;
            //TODO: Check for previously generated elements
            result = new LinearElement();
            result.Geometry = geometry;
            return result;
        }

        #endregion
    }
}
