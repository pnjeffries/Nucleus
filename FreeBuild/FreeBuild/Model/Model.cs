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

using FreeBuild.Base;
using FreeBuild.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A self-contained data structure that represents an entire
    /// BIM or analysis model.
    /// </summary>
    [Serializable]
    public class Model : Unique
    {
        #region Events

        /// <summary>
        /// Event raised when an object is added to the model
        /// </summary>
        [field: NonSerialized]
        [field: Copy(CopyBehaviour.DO_NOT_COPY)]
        public event EventHandler<ModelObjectAddedEventArgs> ObjectAdded;

        /// <summary>
        /// Event raised when a property of an object in this model is changed.
        /// Bubbles the property changed event upwards.
        /// </summary>
        [field: NonSerialized]
        [field: Copy(CopyBehaviour.DO_NOT_COPY)]
        public event PropertyChangedEventHandler ObjectPropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Get the collection of elements that form the geometric representation 
        /// of this model.
        /// </summary>
        public ElementTable Elements { get; }

        /// <summary>
        /// Get the collection of nodes that belong to this model.
        /// </summary>
        public NodeTable Nodes { get; }

        /// <summary>
        /// Get the collection of properties that belong to this model.
        /// </summary>
        public VolumetricPropertyCollection Properties { get; }

        /// <summary>
        /// Get a single flat collection which contains all sub-objects within
        /// this model.
        /// </summary>
        public UniquesCollection Everything
        {
            get
            {
                return new UniquesCollection( new IEnumerable<IUnique>[]
                    {Elements, Nodes});
            }
        }

        [NonSerialized]
        private ModelObjectCreator _Create;

        /// <summary>
        /// Create new objects in this model.  Returns a ModelObjectCreator instance which
        /// provides factory functionality to create new objects within this model and to
        /// track their creation history.  The functions accessed via this property will normally
        /// create new objects and add them to the model, but if the execution information matches
        /// that of a stored object that object will be updated and returned instead.  This
        /// allows objects to be easily created and updated via parametric processes.
        /// Model objects may alternately be instantiated and added to the model manually,
        /// however this will not track their creation history.
        /// </summary>
        public ModelObjectCreator Create
        {
            get
            {
                if (_Create == null) _Create = new ModelObjectCreator(this);
                return _Create;
            }
        }

        /// <summary>
        /// Private backing field for History property
        /// </summary>
        private ModelSourceHistory _History;

        /// <summary>
        /// Record of the creation history of objects within this model.
        /// Used by the model object creator to replace or update previously created objects from
        /// the same source.
        /// </summary>
        public ModelSourceHistory History
        {
            get
            {
                if (_History == null) _History = new ModelSourceHistory();
                return _History;
            }
        }
          

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Model()
        {
            //Initialise collections
            Elements = new ElementTable();
            Nodes = new NodeTable();
            Properties = new VolumetricPropertyCollection();

            //Attach handlers:
            Elements.CollectionChanged += HandlesInternalCollectionChanged;
            Nodes.CollectionChanged += HandlesInternalCollectionChanged;
            Properties.CollectionChanged += HandlesInternalCollectionChanged;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a new element to this model, if it does not already exist within it.
        /// </summary>
        /// <param name="element">The element to be added</param>
        /// <returns>True if the element could be added, false if it already existed within
        /// the model.</returns>
        public bool Add(IElement element)
        {
            return Elements.TryAdd(element);
        }

        /// <summary>
        /// Add a new node to this model, if it does not already exist within it
        /// </summary>
        /// <param name="node">The node to be added.</param>
        /// <returns>True if the node could be added, false if it already existed within
        /// the model.</returns>
        public bool Add(Node node)
        {
            return Nodes.TryAdd(node);
        }

        /// <summary>
        /// Add a new property to this model, if it does not already exist within it
        /// </summary>
        /// <param name="property">The property to be added.</param>
        /// <returns>True if the property could be added, false if it already existed within
        /// the model.</returns>
        public bool Add(VolumetricProperty property)
        {
            return Properties.TryAdd(property);
        }

        /// <summary>
        /// Register a new object with this model for event handling
        /// </summary>
        /// <param name="unique"></param>
        protected void Register(Unique unique)
        {
            unique.PropertyChanged += HandlesObjectPropertyChanged;
        }

        /// <summary>
        /// Called immediately after deserialisation to re-register all objects
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        private void OnDeserialisation(StreamingContext context)
        {
            foreach (Unique unique in Everything)
            {
                Register(unique);
            }
        }

        #endregion

        #region Event Handlers

        private void HandlesInternalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //New item added to a collection
            foreach (object obj in e.NewItems)
            {
                if (obj is Unique)
                {
                    Register((Unique)obj);
                    RaiseEvent(ObjectAdded, new ModelObjectAddedEventArgs((Unique)obj));
                }
            }
        }

        private void HandlesObjectPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Bubble property changed events upwards:
            RaiseEvent(ObjectPropertyChanged, sender, e);
        }

        #endregion


    }
}
