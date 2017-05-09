// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
    /// A component of a model which acts as a factory class to create new objects 
    /// in the model or update previously created ones with matching execution information.
    /// </summary>
    public class ModelObjectCreator
    {
        #region Properties

        private Model _Model;

        /// <summary>
        /// The model that this creator works on
        /// </summary>
        protected Model Model
        {
            get { return _Model; }
            private set { _Model = value; }
        }

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
                //result = (Node)Model.History.Update(exInfo, result);
                result.Position = position;
            }
            else result.Undelete();
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
        /// Create a new (or update an existing) panel element in the model
        /// </summary>
        /// <param name="geometry">The set-out geometry of the element</param>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns>A new or previously created Panel elemet</returns>
        public PanelElement PanelElement(Surface geometry, ExecutionInfo exInfo = null)
        {
            PanelElement result = new PanelElement();
            result = (PanelElement)Model.History.Update(exInfo, result);
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
        /// Create a new (or update an existing) panel element as a copy of another one
        /// </summary>
        /// <param name="element">The element to copy.</param>
        /// <param name="newGeometry">Optional.  The set-out geometry to be used for the new element.</param>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns></returns>
        public PanelElement CopyOf(PanelElement element, Surface newGeometry = null, ExecutionInfo exInfo = null)
        {
            PanelElement result = element.Duplicate();
            result = (PanelElement)Model.History.Update(exInfo, result);
            if (newGeometry != null) result.ReplaceGeometry(newGeometry);
            Model.Add(result);
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) element as a copy of another one
        /// </summary>
        /// <param name="element">The element to copy.</param>
        /// <param name="newGeometry">Optional.  The set-out geometry to be used for the new element.
        /// Should be of the appropriate type for the element to be copied.</param>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns></returns>
        public Element CopyOf(Element element, VertexGeometry newGeometry = null, ExecutionInfo exInfo = null)
        {
            if (element is LinearElement) return CopyOf(element, newGeometry as Curve, exInfo);
            else return null;
        }

        /// <summary>
        /// Create a new (or update an existing) section family in the model
        /// </summary>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns></returns>
        public SectionFamily SectionFamily(ExecutionInfo exInfo = null)
        {
            SectionFamily result = new SectionFamily();
            result = (SectionFamily)Model.History.Update(exInfo, result);
            if (result.Name == null) result.Name = Model.Families.NextAvailableName("Section", result, true);
            Model.Add(result);
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) section family in the model
        /// </summary>
        /// <param name="name">The name of the section. 
        /// May be modified with a numerical suffix if the name already exists in
        /// the model.</param>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns>The created or updated </returns>
        public SectionFamily SectionFamily(string name, ExecutionInfo exInfo = null)
        {
            SectionFamily result = new SectionFamily();
            result = (SectionFamily)Model.History.Update(exInfo, result);
            result.Name = Model.Families.NextAvailableName(name, result);
            Model.Add(result);
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) section family in the model
        /// </summary>
        /// <param name="profile">The section profile to assign to the new property.</param>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns>The created or updated section family</returns>
        public SectionFamily SectionFamily(SectionProfile profile, ExecutionInfo exInfo = null)
        {
            SectionFamily result = new SectionFamily();
            result = (SectionFamily)Model.History.Update(exInfo, result);
            result.Profile = profile;
            if (result.Name == null) result.Name = Model.Families.NextAvailableName("Section", result, true);
            Model.Add(result);
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) face family in the model
        /// </summary>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns></returns>
        public PanelFamily FaceFamily(ExecutionInfo exInfo = null)
        {
            PanelFamily result = new PanelFamily();
            result = (PanelFamily)Model.History.Update(exInfo, result);
            if (result.Name == null) result.Name = Model.Families.NextAvailableName("Face Family", result, true);
            Model.Add(result);
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) face family in the model
        /// </summary>
        /// <param name="name">The name of the family. 
        /// May be modified with a numerical suffix if the name already exists in
        /// the model.</param>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns>The created or updated </returns>
        public PanelFamily FaceFamily(string name, ExecutionInfo exInfo = null)
        {
            PanelFamily result = new PanelFamily();
            result = (PanelFamily)Model.History.Update(exInfo, result);
            result.Name = Model.Families.NextAvailableName(name, result);
            Model.Add(result);
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) coordinate system in the model
        /// </summary>
        /// <param name="cSystem">The geometry coordinate system to assign as this user system</param>
        /// <param name="name">Optional.  The name of the user coordinate system.  If not specified a
        /// name will be automatically generated.</param>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns>The created or updated user coordinate system.</returns>
        public UserCoordinateSystemReference UserCoordinateSystemReference(ICoordinateSystem cSystem, string name = null, ExecutionInfo exInfo = null)
        {
            UserCoordinateSystemReference result = new UserCoordinateSystemReference();
            result = (UserCoordinateSystemReference)Model.History.Update(exInfo, result);
            if (name != null) result.Name = name;
            if (result.Name == null) result.Name = Model.CoordinateSystems.NextAvailableName("Coordinate System", result, true);
            Model.Add(result);
            return result;
        }

        /// <summary>
        /// Create a new (or update an existing) load case in the model
        /// </summary>
        /// <param name="name">The name of the load case</param>
        /// <param name="exInfo">Optional.  The execution information of the current action.
        /// If an object has been created previously with matching execution information then
        /// instead of creating a new item this previous one will be updated and returned instead.
        /// This enables this method to be used parametrically.</param>
        /// <returns>The created or updated load case.</returns>
        public LoadCase LoadCase(string name, ExecutionInfo exInfo = null)
        {
            LoadCase result = new LoadCase();
            result = (LoadCase)Model.History.Update(exInfo, result);
            if (name != null) result.Name = name;
            if (result.Name == null) result.Name = Model.LoadCases.NextAvailableName("Load Case", result, true);
            Model.Add(result);
            return result;
        }

        #endregion
    }
}
