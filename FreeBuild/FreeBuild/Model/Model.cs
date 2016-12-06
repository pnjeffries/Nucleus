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
using FreeBuild.Geometry;
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
        /// Private backing field for Elements property
        /// </summary>
        private ElementTable _Elements;

        /// <summary>
        /// Get the collection of elements that form the geometric representation 
        /// of this model.
        /// </summary>
        public ElementTable Elements
        {
            get
            {
                if (_Elements == null)
                {
                    _Elements = new ElementTable(this);
                    _Elements.CollectionChanged += HandlesInternalCollectionChanged;
                }
                return _Elements;
            }
        }

        /// <summary>
        /// Private backing field for Nodes property
        /// </summary>
        private NodeTable _Nodes;

        /// <summary>
        /// Get the collection of nodes that belong to this model.
        /// </summary>
        public NodeTable Nodes
        {
            get
            {
                if (_Nodes == null)
                {
                    _Nodes = new NodeTable(this);
                    _Nodes.CollectionChanged += HandlesInternalCollectionChanged;
                }
                return _Nodes;
            }
        }

        /// <summary>
        /// Private backing field for Levels property
        /// </summary>
        private LevelTable _Levels;

        /// <summary>
        /// Get the collection of Levels that belong to this model
        /// </summary>
        public LevelTable Levels
        {
            get
            {
                if (_Levels == null)
                {
                    _Levels = new LevelTable(this);
                    _Levels.CollectionChanged += HandlesInternalCollectionChanged;
                }
                return _Levels;
            }
        }

        /// <summary>
        /// Private backing field for Properties property
        /// </summary>
        private VolumetricPropertyTable _Properties;

        /// <summary>
        /// Get the collection of volumetric properties that belong to this model.
        /// </summary>
        public VolumetricPropertyTable Properties
        {
            get
            {
                if (_Properties == null)
                {
                    _Properties = new VolumetricPropertyTable(this);
                    _Properties.CollectionChanged += HandlesInternalCollectionChanged;
                }
                return _Properties;
            }
        }

        /// <summary>
        /// Private backing field for Materials property
        /// </summary>
        private MaterialTable _Materials;

        /// <summary>
        /// Get the collection of materials that belong to this model
        /// </summary>
        public MaterialTable Materials
        {
            get
            {
                if (_Materials == null)
                {
                    _Materials = new MaterialTable(this);
                    _Materials.CollectionChanged += HandlesInternalCollectionChanged;
                }
                return _Materials;
            }
        }

        /// <summary>
        /// Get a single flat collection which contains all sub-objects within
        /// this model, for easy iteration through the entire database in one go.
        /// This collection is compiled when called from the different sub-tables
        /// within the model - it is not stored and modifying this collection
        /// will not result in any changes being made to the model itself.
        /// Use the Elements, Nodes, Properties, Materials etc. properties instead 
        /// to modify the model.
        /// </summary>
        public ModelObjectCollection Everything
        {
            get
            {
                return new ModelObjectCollection(new IEnumerable<ModelObject>[]
                    {_Elements, _Nodes, _Levels, _Properties, _Materials});
            }
        }

        /// <summary>
        /// Private backing field for BoundingBox property
        /// </summary>
        [NonSerialized]
        private BoundingBox _BoundingBox = null;

        /// <summary>
        /// Get the bounding box of this model.  Calculated and cached as needed
        /// based on element geometry.
        /// </summary>
        public BoundingBox BoundingBox
        {
            get
            {
                if (_BoundingBox == null) _BoundingBox = CalculateBoundingBox();
                return _BoundingBox;
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

        /// <summary>
        /// Get the object with the specified GUID, if it can be found in this model.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ModelObject this[Guid guid]
        {
            get
            {
                return GetObject(guid);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new empty model.
        /// </summary>
        public Model()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a new element to this model, if it does not already exist within it.
        /// </summary>
        /// <param name="element">The element to be added</param>
        /// <returns>True if the element could be added, false if it already existed within
        /// the model.</returns>
        public bool Add(Element element)
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
        /// Add a new level to this model, if it does not already exist within it
        /// </summary>
        /// <param name="level">The level to be added.</param>
        /// <returns>True if the level could be added, false if it already existed within
        /// the model.</returns>
        public bool Add(Level level)
        {
            return Levels.TryAdd(level);
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
        /// Add a new material to this model, if it does not already exist within it
        /// </summary>
        /// <param name="material">The material to be added.</param>
        /// <returns>True if the material could be added, false if it already existed within
        /// the model.</returns>
        public bool Add(Material material)
        {
            return Materials.TryAdd(material);
        }

        /// <summary>
        /// Register a new object with this model for event handling
        /// </summary>
        /// <param name="unique"></param>
        protected void Register(ModelObject unique)
        {
            unique.PropertyChanged += HandlesObjectPropertyChanged;
        }

        /// <summary>
        /// Get the object with the specified GUID, if it can be found in this model.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ModelObject GetObject(Guid guid)
        {
            if (Elements.Contains(guid)) return Elements[guid];
            else if (Nodes.Contains(guid)) return Nodes[guid];
            else if (Properties.Contains(guid)) return Properties[guid];
            else if (Materials.Contains(guid)) return Materials[guid];
            else return null;
        }

        /// <summary>
        /// Generate or update nodes within this model to structurally represent
        /// the vertices of elements.
        /// </summary>
        /// <param name="options"></param>
        public void RegenerateNodes(NodeGenerationParameters options)
        {
            foreach (Element element in Elements)
            {
                element.RegenerateNodes(options);
            }
        }

        /// <summary>
        /// Calculate and return the bounding box of this model.
        /// This is called automatically and the result cached by the
        /// BoundingBox property - so it is usually more efficient to
        /// use that rather than calling this function.
        /// </summary>
        /// <returns></returns>
        public BoundingBox CalculateBoundingBox()
        {
            BoundingBox box = Elements.BoundingBox();
            if (box == null) box = new BoundingBox(0,0,0);
            box.Expand(3);
            return box;
        }

        /// <summary>
        /// Called immediately after deserialisation to re-register all objects
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            //Restore collection changed event handling
            if (_Elements != null) _Elements.CollectionChanged += HandlesInternalCollectionChanged;
            if (_Nodes != null) _Nodes.CollectionChanged += HandlesInternalCollectionChanged;
            if (_Properties != null) _Properties.CollectionChanged += HandlesInternalCollectionChanged;
            if (_Materials != null) _Materials.CollectionChanged += HandlesInternalCollectionChanged;
            if (_Levels != null) _Levels.CollectionChanged += HandlesInternalCollectionChanged;

            foreach (ModelObject unique in Everything)
            {
                Register(unique);
            }
        }

        /// <summary>
        /// Clear any volatile cached data that should be disposed when data within this model
        /// is updated. 
        /// </summary>
        private void ClearCachedData()
        {
            _BoundingBox = null;
        }

        #endregion

        #region Event Handlers

        private void HandlesInternalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //New item added to a collection
            foreach (object obj in e.NewItems)
            {
                if (obj is ModelObject)
                {
                    Register((ModelObject)obj);
                    RaiseEvent(ObjectAdded, new ModelObjectAddedEventArgs((Unique)obj));
                    ClearCachedData();
                }
            }
        }

        private void HandlesObjectPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Bubble property changed events upwards:
            RaiseEvent(ObjectPropertyChanged, sender, e);
            ClearCachedData();
        }

        #endregion


    }
}
