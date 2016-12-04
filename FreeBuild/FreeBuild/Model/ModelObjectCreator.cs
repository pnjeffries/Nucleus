using FreeBuild.Actions;
using FreeBuild.Base;
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
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
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
        /// <param name="geometry">The set-out geometry of the element</param>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
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
        /// Create a new (or update an existing) linear element as a copy of another one
        /// </summary>
        /// <param name="element">The element to copy.</param>
        /// <param name="newGeometry">Optional.  The set-out geometry to be used for the new element.</param>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns></returns>
        public LinearElement CopyOf(LinearElement element, Curve newGeometry = null, ExecutionInfo exInfo = null)
        {
            LinearElement result = element.Duplicate();
            result = (LinearElement)Model.History.Update(exInfo, result);
            if (newGeometry != null) result.ReplaceGeometry(newGeometry);
            Model.Add(result);
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) section property in the model
        /// </summary>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns></returns>
        public SectionProperty SectionProperty(ExecutionInfo exInfo = null)
        {
            SectionProperty result = new SectionProperty();
            result = (SectionProperty)Model.History.Update(exInfo, result);
            if (result.Name == null) result.Name = Model.Properties.NextAvailableName("Section", result, true);
            Model.Add(result);
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) section property in the model
        /// </summary>
        /// <param name="name">The name of the section. 
        /// May be modified with a numerical suffix if the name already exists in
        /// the model.</param>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns></returns>
        public SectionProperty SectionProperty(string name, ExecutionInfo exInfo = null)
        {
            SectionProperty result = new SectionProperty();
            result = (SectionProperty)Model.History.Update(exInfo, result);
            result.Name = Model.Properties.NextAvailableName(name, result);
            Model.Add(result);
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) section property in the model
        /// </summary>
        /// <param name="profile">The section profile to assign to the new property.</param>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns></returns>
        public SectionProperty SectionProperty(SectionProfile profile, ExecutionInfo exInfo = null)
        {
            SectionProperty result = new SectionProperty();
            result = (SectionProperty)Model.History.Update(exInfo, result);
            result.Profile = profile;
            if (result.Name == null) result.Name = Model.Properties.NextAvailableName("Section", result, true);
            Model.Add(result);
            return result;
        }

        #endregion
    }
}
