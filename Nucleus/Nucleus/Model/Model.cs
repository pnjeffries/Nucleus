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

using Nucleus.Base;
using Nucleus.Events;
using Nucleus.Extensions;
using Nucleus.Geometry;
using Nucleus.Model.Loading;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A self-contained data structure that represents an entire
    /// BIM or analysis model.
    /// </summary>
    [Serializable]
    public class Model : Named
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
        /// Private backing field for CoordinateSystems property
        /// </summary>
        private UserCoordinateSystemReferenceTable _CoordinateSystems;

        /// <summary>
        /// Get the collection of user-defined coordinate systems that belong to this model
        /// </summary>
        public UserCoordinateSystemReferenceTable CoordinateSystems
        {
            get
            {
                if (_CoordinateSystems == null)
                {
                    _CoordinateSystems = new UserCoordinateSystemReferenceTable(this); //TODO: Link to model?
                    _CoordinateSystems.CollectionChanged += HandlesInternalCollectionChanged;
                }
                return _CoordinateSystems;
            }
        }

        /// <summary>
        /// Private backing field for Families property
        /// </summary>
        private FamilyTable _Families;

        /// <summary>
        /// Get the collection of families that belong to this model.
        /// </summary>
        public FamilyTable Families
        {
            get
            {
                if (_Families == null)
                {
                    _Families = new FamilyTable(this);
                    _Families.CollectionChanged += HandlesInternalCollectionChanged;
                }
                return _Families;
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
        /// Private backing field for the Sets property
        /// </summary>
        private ModelObjectSetTable _Sets;

        /// <summary>
        /// The collection of stored sets that belong to this model.
        /// Sets represent parametric groupings of model objects that can be referenced
        /// in load applications etc.
        /// </summary>
        public ModelObjectSetTable Sets
        {
            get
            {
                if (_Sets == null)
                {
                    _Sets = new ModelObjectSetTable(this);
                    _Sets.CollectionChanged += HandlesInternalCollectionChanged;
                }
                return _Sets;
            }
        }

        /// <summary>
        /// Private backing field for Loads property
        /// </summary>
        private LoadTable _Loads;

        /// <summary>
        /// The loads applied to this model
        /// </summary>
        public LoadTable Loads
        {
            get
            {
                if (_Loads == null)
                {
                    _Loads = new LoadTable(this);
                    _Loads.CollectionChanged += HandlesInternalCollectionChanged;
                }
                return _Loads;
            }

        }

        /// <summary>
        /// Private backing field for Loading property
        /// </summary>
        private LoadCaseTable _LoadCases;

        /// <summary>
        /// The load cases stored in this model
        /// </summary>
        public LoadCaseTable LoadCases
        {
            get
            {
                if (_LoadCases == null)
                {
                    _LoadCases = new LoadCaseTable(this);
                    _LoadCases.CollectionChanged += HandlesInternalCollectionChanged;
                }
                return _LoadCases;
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
                return new ModelObjectCollection(AllTables);
            }
        }

        /// <summary>
        /// Get all of the object tables in this Model.
        /// </summary>
        public IEnumerable<IEnumerable<ModelObject>> AllTables
        {
            get
            {
                return new IEnumerable<ModelObject>[]
                    {_CoordinateSystems, _Materials, _Families, _Levels, _Nodes, _Elements, _Sets, _LoadCases, _Loads};
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
        /// Try to add a collection of modelobjects to the model, if they are of a
        /// suitable type and do not already exist within it.  The correct storage
        /// table(s) will be automatically selected based on the object's type.
        /// </summary>
        /// <param name="objects">The collection of objects to add to the model</param>
        /// <returns>True if any of the objects in the collection could be added,
        /// else false.</returns>
        public bool Add(ModelObjectCollection objects)
        {
            bool result = false;
            if (objects != null)
            {
                foreach (var obj in objects)
                {
                    if (Add(obj)) result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Try to add a new modelobject to the model, if it is of a suitable type
        /// and does not already exist within it.  The correct storage table will be
        /// automatically selected based on the object's type.
        /// </summary>
        /// <param name="mObj"></param>
        /// <returns></returns>
        public bool Add(ModelObject mObj)
        {
            if (mObj is Element)
                return Add((Element)mObj);
            else if (mObj is Node)
                return Add((Node)mObj);
            else if (mObj is UserCoordinateSystemReference)
                return Add((UserCoordinateSystemReference)mObj);
            else if (mObj is Level)
                return Add((Level)mObj);
            else if (mObj is Family)
                return Add((Family)mObj);
            else if (mObj is Material)
                return Add((Material)mObj);
            else if (mObj is ModelObjectSetBase)
                return Add((ModelObjectSetBase)mObj);
            else if (mObj is LoadCase)
                return Add((LoadCase)mObj);
            else if (mObj is Load)
                return Add((Load)mObj);
            else return false;
        }

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
        /// Add a new user-defined coordinate system to this model, if it does not
        /// already exist within it
        /// </summary>
        /// <param name="cSystem">The user-defined coordinate system to be added.</param>
        /// <returns>True if the coordinate system could be added, 
        /// false if it already existed within the model.</returns>
        public bool Add(UserCoordinateSystemReference cSystem)
        {
            return CoordinateSystems.TryAdd(cSystem);
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
        public bool Add(Family property)
        {
            return Families.TryAdd(property);
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
        /// Add a new object set to this model, if it does not already exist within it
        /// </summary>
        /// <param name="set">The set to be added.</param>
        /// <returns>True if the material could be added, false if it had already
        /// been added to  the model.</returns>
        public bool Add(ModelObjectSetBase set)
        {
            return Sets.TryAdd(set);
        }

        /// <summary>
        /// Add a new load case to this model, if it does not already exist within it
        /// </summary>
        /// <param name="loadCase">The load case to be added.</param>
        /// <returns>True if the load case could be added, false if it had already
        /// been added to the model.</returns>
        public bool Add(LoadCase loadCase)
        {
            return LoadCases.TryAdd(loadCase);
        }

        /// <summary>
        /// Add a new load to this model, if it does not already exist within it
        /// </summary>
        /// <param name="load">The load to be added</param>
        /// <returns>True if the load could be added, false if it had already been added to the model.</returns>
        public bool Add(Load load)
        {
            return Loads.TryAdd(load);
        }

        /// <summary>
        /// Register a new object with this model for event handling
        /// </summary>
        /// <param name="unique"></param>
        protected void Register(ModelObject unique)
        {
            unique.PropertyChanged += HandlesObjectPropertyChanged;
            //if (unique is LoadCase)
            //{
            //    var lC = (LoadCase)unique;
            //    lC.Loads.CollectionChanged += HandlesInternalCollectionChanged;
            //}
        }

        /// <summary>
        /// Get the object with the specified GUID, if it can be found in this model.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ModelObject GetObject(Guid guid)
        {
            if (_Elements != null && _Elements.Contains(guid)) return _Elements[guid];
            else if (_Nodes != null && _Nodes.Contains(guid)) return _Nodes[guid];
            else if (_Loads != null && _Loads.Contains(guid)) return _Loads[guid];
            else if (_LoadCases != null && _LoadCases.Contains(guid)) return _LoadCases[guid];
            else if (_Families != null && _Families.Contains(guid)) return _Families[guid];
            else if (_Materials != null && _Materials.Contains(guid)) return _Materials[guid];
            else if (_Levels != null && _Levels.Contains(guid)) return _Levels[guid];
            else if (_CoordinateSystems != null && _CoordinateSystems.Contains(guid)) return _CoordinateSystems[guid];
            else if (_Sets != null && _Sets.Contains(guid)) return _Sets[guid];
            else return null;
        }

        /// <summary>
        /// Get the table from this model (if any) responsible for storing the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<ModelObject> GetTableFor(Type type)
        {
            if (typeof(Node).IsAssignableFrom(type)) return Nodes;
            else if (typeof(Element).IsAssignableFrom(type)) return Elements;
            else if (typeof(Family).IsAssignableFrom(type)) return Families;
            else if (typeof(Material).IsAssignableFrom(type)) return Materials;
            else if (typeof(Level).IsAssignableFrom(type)) return Levels;
            else if (typeof(LoadCase).IsAssignableFrom(type)) return LoadCases;
            else if (typeof(Load).IsAssignableFrom(type)) return Loads;
            else if (typeof(CoordinateSystemReference).IsAssignableFrom(type)) return CoordinateSystems;
            else if (typeof(ModelObjectSetBase).IsAssignableFrom(type)) return Sets;
            //Add any other new types here
            else return null;
        }

        /// <summary>
        /// Find and return an object in this model by it's type and numeric ID
        /// </summary>
        /// <param name="type"></param>
        /// <param name="numericID"></param>
        /// <returns></returns>
        public ModelObject GetByNumericID(Type type, long numericID)
        {
            var table = GetTableFor(type);
            return table?.GetByNumericID(numericID);
        }

        /// <summary>
        /// Find and return an object in this model of the specified type with the
        /// specifed numeric ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="numericID"></param>
        /// <returns></returns>
        public T GetByNumericID<T>(long numericID)
            where T : ModelObject
        {
            return GetByNumericID(typeof(T), numericID) as T;
        }

        /// <summary>
        /// Find and return an object in this model of the specified type with a
        /// compatible description.  Descriptions may be the object name, a string
        /// containing the numeric ID or just the numeric ID itself.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public ModelObject GetByDescription(Type type, string description)
        {
            var table = GetTableFor(type);
            ModelObject result = table.FindByName(description);
            if (result != null) return result;
            string numericIDString = description.TrimNonNumeric();
            if (!string.IsNullOrWhiteSpace(numericIDString))
            {
                long numericID = numericIDString.ToLong();
                result = table.GetByNumericID(numericID);
            }
            return result;
        }

        /// <summary>
        /// Generate or update nodes within this model to structurally represent
        /// the vertices of elements.
        /// </summary>
        /// <param name="options"></param>
        public void GenerateNodes(NodeGenerationParameters options)
        {
            Elements.GenerateNodes(options);

            if (options.DeleteUnusedNodes)
            {
                foreach (Node node in Nodes)
                {
                    if (!node.IsDeleted && node.GetConnectedElements().UndeletedCount() == 0) node.Delete();
                }
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
            if (_Families != null) _Families.CollectionChanged += HandlesInternalCollectionChanged;
            if (_Materials != null) _Materials.CollectionChanged += HandlesInternalCollectionChanged;
            if (_Levels != null) _Levels.CollectionChanged += HandlesInternalCollectionChanged;
            if (_CoordinateSystems != null) _CoordinateSystems.CollectionChanged += HandlesInternalCollectionChanged;
            if (_Sets != null) _Sets.CollectionChanged += HandlesInternalCollectionChanged;
            if (_LoadCases != null) _LoadCases.CollectionChanged += HandlesInternalCollectionChanged;

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

        /// <summary>
        /// Clean this model of all deleted objects.  This will result in the
        /// permenant removal of these objects from the model database and it will
        /// no longer be possible to successfully undelete them.
        /// </summary>
        public void CleanDeleted()
        {
            foreach (IList<ModelObject> table in AllTables)
            {
                table.RemoveDeleted();
            }
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

            if (sender is Node)
            {
                //If node moved, clear tree:
                if (e.PropertyName == "Position") Nodes.SpatialTree = null; //TODO: Review
            }
        }

        #endregion


    }
}
