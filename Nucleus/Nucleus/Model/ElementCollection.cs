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
using Nucleus.Extensions;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Generic base collection of elements
    /// </summary>
    /// <typeparam name="TElement">The sub-type of elements that this collection contains</typeparam>
    [Serializable]
    public abstract class ElementCollection<TElement, TSelf> : ModelObjectCollection<TElement>
        where TElement : Element
        where TSelf : ElementCollection<TElement,TSelf>, new()
    {
        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new empty ElementCollection
        /// </summary>
        public ElementCollection() : base() { }

        /// <summary>
        /// Owner constructor.  Initialises an empty ElementCollection with the specified owner
        /// </summary>
        /// <param name="model"></param>
        protected ElementCollection(Model model) : base(model) { }

        #endregion

        #region Methods

        /// <summary>
        /// Find and return the subset of elements in this collection that contain a reference to the
        /// specified node.
        /// </summary>
        /// <param name="node">The node to search for</param>
        /// <returns></returns>
        public TSelf AllWith(Node node)
        {
            var result = new TSelf();
            if (node != null)
            {
                foreach (TElement el in this)
                {
                    if (el.ContainsNode(node)) result.Add(el);
                }
            }
            return result;
        }

        /// <summary>
        /// Find and return the subset of elements in this collection that contain a reference to the
        /// specified property.
        /// </summary>
        /// <param name="property">The property to search for</param>
        /// <returns></returns>
        public TSelf AllWith(Family property)
        {
            var result = new TSelf();
            foreach (TElement el in this)
            {
                if (el.GetFamily() == property) result.Add(el);
            }
            return result;
        }

        /// <summary>
        /// Calculate the bounding box of the elements in this collection
        /// </summary>
        /// <returns></returns>
        public BoundingBox BoundingBox()
        {
            if (Count == 0) return null;
            else
            {
                return new BoundingBox(this);
            }
        }

        /// <summary>
        /// Generate or update nodes within this collection to structurally represent
        /// the vertices of elements.
        /// </summary>
        /// <param name="options"></param>
        public virtual void GenerateNodes(NodeGenerationParameters options)
        {
            foreach (Element element in this)
            {
                if (!element.IsDeleted) element.GenerateNodes(options);
            }
        }

        /// <summary>
        /// Get all of the nodes which belong to elements in this collection.
        /// </summary>
        /// <returns></returns>
        public NodeCollection GetNodes()
        {
            var result = new NodeCollection();
            foreach (Element element in this)
            {
                result.TryAddRange(element.Nodes);
            }
            return result;
        }

        /// <summary>
        /// Extract from this collection subcollections of elements which
        /// are connected together.
        /// </summary>
        /// <returns></returns>
        public IList<TSelf> ExtractConnectedSubstructures()
        {
            var result = new List<TSelf>();

            // Build pool of all elements to gradually eliminate
            var pool = new TSelf();
            foreach (var element in this) pool.Add(element);

            while (pool.Count > 0)
            {
                var current = new TSelf();
                ConnectedSubstructureSearch(pool.First(), pool, current);
                if (current.Count > 0)
                    result.Add(current);
            }
            return result;
        }

        /// <summary>
        /// Recursive search for connected elements
        /// </summary>
        /// <param name="element"></param>
        /// <param name="pool"></param>
        /// <param name="current"></param>
        private void ConnectedSubstructureSearch(TElement element, TSelf pool, TSelf current)
        {
            if (pool.Contains(element.GUID))
            {
                pool.Remove(element.GUID);
                current.Add(element);
                foreach (Node node in element.Nodes)
                {
                    foreach (Element connected in node.GetConnectedElements())
                    {
                        if (connected is TElement)
                            ConnectedSubstructureSearch((TElement)connected, pool, current);
                    }
                }
            }
        }

        /// <summary>
        /// Get the first element in this collection with an attached data component
        /// of the specified type.
        /// </summary>
        /// <typeparam name="TDataComponent">The type of attached data component.</typeparam>
        /// <param name="ignore">Ignore this element in the collection, even if it has the
        /// relevant component.</param>
        /// <returns></returns>
        public TElement FirstWithDataComponent<TDataComponent>(TElement ignore = null)
            where TDataComponent : class, IElementDataComponent
        {
            foreach (TElement el in this)
            {
                if (el != ignore && el.HasData<TDataComponent>())
                {
                    return el;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the first element in this collection with an attached data components
        /// of the specified types.
        /// </summary>
        /// <typeparam name="TDataComponent">The type of attached data component.</typeparam>
        /// <typeparam name="TDataComponent2">The second type of attached data component.</typeparam>
        /// <param name="ignore">Ignore this element in the collection, even if it has the
        /// relevant component.</param>
        /// <returns></returns>
        public TElement FirstWithDataComponents<TDataComponent, TDataComponent2>(TElement ignore = null)
            where TDataComponent : class, IElementDataComponent
        {
            foreach (TElement el in this)
            {
                if (el != ignore && el.HasData<TDataComponent>() && el.HasAttachedDataType<TDataComponent2>())
                {
                    return el;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the first element in this collection with an attached data components
        /// of the specified types.
        /// </summary>
        /// <typeparam name="TDataComponent">The type of attached data component.</typeparam>
        /// <typeparam name="TDataComponent2">The second type of attached data component.</typeparam>
        /// <typeparam name="TDataComponent3">The third type of attached data component.</typeparam>
        /// <param name="ignore">Ignore this element in the collection, even if it has the
        /// relevant component.</param>
        /// <returns></returns>
        public TElement FirstWithDataComponents<TDataComponent, TDataComponent2, TDataComponent3>(TElement ignore = null)
            where TDataComponent : class, IElementDataComponent
        {
            foreach (TElement el in this)
            {
                if (el != ignore && el.HasData<TDataComponent>() && el.HasAttachedDataType<TDataComponent2>() && el.HasAttachedDataType<TDataComponent3>())
                {
                    return el;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the last element in this collection with an attached data component
        /// of the specified type.
        /// </summary>
        /// <typeparam name="TDataComponent">The type of attached data component.</typeparam>
        /// <param name="ignore">Ignore this element in the collection, even if it has the
        /// relevant component.</param>
        /// <returns></returns>
        public TElement LastWithDataComponent<TDataComponent>(TElement ignore = null)
            where TDataComponent : class, IElementDataComponent
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                TElement el = this[i];
                if (el != ignore && el.HasData<TDataComponent>())
                {
                    return el;
                }
            }
            return null;
        }

        /// <summary>
        /// Get all elements in this collection with an attached data component
        /// of the specified type.
        /// </summary>
        /// <typeparam name="TDataComponent">The type of attached data component</typeparam>
        /// <param name="ignore">Ignore this element in the collection, even if it has the
        /// relevant component.</param>
        /// <returns></returns>
        public TSelf AllWithDataComponent<TDataComponent>(TElement ignore = null)
            where TDataComponent : class, IElementDataComponent
        {
            var result = new TSelf();
            foreach (TElement el in this)
            {
                if (el != ignore && el.HasData<TDataComponent>())
                {
                    result.Add(el);
                }
            }
            return result;
        }

        #endregion
    }

    /// <summary>
    /// A collection of elements
    /// </summary>
    [Serializable]
    public class ElementCollection : ElementCollection<Element, ElementCollection>
    {
        #region Properties

        /// <summary>
        /// Extract a collection of all the linear elements in this collection.
        /// A new collection will be generated each time this is called.
        /// </summary>
        public LinearElementCollection LinearElements
        {
            get
            {
                var result = new LinearElementCollection();
                foreach (Element element in this)
                {
                    if (element is LinearElement) result.Add((LinearElement)element);
                }
                return result;
            }
        }

        /// <summary>
        /// Extract a collection of all the panel elements in this collection.
        /// A new collection will be generated each time this is called.
        /// </summary>
        public PanelElementCollection PanelElements
        {
            get
            {
                var result = new PanelElementCollection();
                foreach (Element element in this)
                {
                    if (element is PanelElement) result.Add((PanelElement)element);
                }
                return result;
            }
        }

        /// <summary>
        /// Extract a collection of all the constraint elements in this collection.
        /// A new collection will be generated each time this is called.
        /// </summary>
        public ConstraintElementCollection ConstraintElemnts
        {
            get
            {
                var result = new ConstraintElementCollection();
                foreach (Element element in this)
                {
                    if (element is ConstraintElement) result.Add((ConstraintElement)element);
                }
                return result;
            }
        }

        /// <summary>
        /// Does this collection contain linear elements and only linear elements?
        /// </summary>
        public bool IsAllLinear
        {
            get
            {
                if (Count == 0) return false;
                return this.ContainsOnlyType(typeof(LinearElement));
            }
        }

        /// <summary>
        /// Does this collection contain panel elements and only panel elements?
        /// </summary>
        public bool IsAllPanels
        {
            get
            {
                if (Count == 0) return false;
                return this.ContainsOnlyType(typeof(PanelElement));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new empty ElementCollection
        /// </summary>
        public ElementCollection() : base() { }

        /// <summary>
        /// Initialise a new collection containing the specified single element
        /// </summary>
        /// <param name="element"></param>
        public ElementCollection(Element element) : base()
        {
            Add(element);
        }

        /// <summary>
        /// Initialise a new collection containing the same elements as another
        /// </summary>
        /// <param name="elements"></param>
        public ElementCollection(IEnumerable<Element> elements) : base()
        {
            AddRange(elements);
        }

        /// <summary>
        /// Owner constructor.  Initialises an empty ElementCollection with the specified owner
        /// </summary>
        /// <param name="model"></param>
        protected ElementCollection(Model model) : base(model) { }

        #endregion

        #region Methods

        /// <summary>
        /// Get the subset of this collection which has a recorded modification after the specified date and time
        /// </summary>
        /// <param name="since">The date and time to filter by</param>
        /// <returns></returns>
        public ElementCollection Modified(DateTime since)
        {
            return this.Modified<ElementCollection, Element>(since);
        }

        /// <summary>
        /// Get the subset of this collection which has an attached data component of the specified type
        /// </summary>
        /// <typeparam name="TData">The type of data component to check for</typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ElementCollection AllWithDataComponent<TData>()
            where TData : class
        {
            return this.AllWithDataComponent<ElementCollection, Element, TData>();
        }

        /// <summary>
        /// Get the merged element vertices for the elements in this collection
        /// </summary>
        /// <returns></returns>
        public IList<MultiElementVertex> GetMergedElementVertices()
        {
            var dictionary = new Dictionary<string, MultiElementVertex>();
            foreach (Element el in this)
            {
                var elVerts = el.ElementVertices;
                foreach (ElementVertex elVert in elVerts)
                {
                    if (dictionary.ContainsKey(elVert.Description)) dictionary[elVert.Description].Merge(elVert);
                    else dictionary.Add(elVert.Description, elVert.ToMultiElementVertex());
                }
            }
            return dictionary.Values.ToList();
        }

        #endregion

    }
}
