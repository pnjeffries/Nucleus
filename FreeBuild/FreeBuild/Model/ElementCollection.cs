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
        public void GenerateNodes(NodeGenerationParameters options)
        {
            foreach (Element element in this)
            {
                if (!element.IsDeleted) element.GenerateNodes(options);
            }
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

        #endregion

    }
}
