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

        /// <summary>
        /// Create a new (or update an existing) node in the model
        /// </summary>
        /// <param name="position"></param>
        /// <param name="reuseTolerance"></param>
        /// <param name="exInfo"></param>
        /// <returns></returns>
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
            Model.Add(result);
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) linear element in the model.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public LinearElement LinearElement(Curve geometry, ExecutionInfo exInfo = null)
        {
            LinearElement result = new LinearElement();
            result = (LinearElement)Model.History.Update(exInfo, result);
            result.ReplaceGeometry(geometry);
            Model.Add(result);
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) section property in the model
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="exInfo"></param>
        /// <returns></returns>
        public SectionProperty SectionProperty(SectionProfile profile, ExecutionInfo exInfo = null)
        {
            SectionProperty result = new SectionProperty();
            result = (SectionProperty)Model.History.Update(exInfo, result);
            result.Profile = profile;
            if (result.Name == null) result.Name = Model.Properties.NextAvailableName("Section", true);
            Model.Add(result);
            return result;
        }

        #endregion
    }
}
