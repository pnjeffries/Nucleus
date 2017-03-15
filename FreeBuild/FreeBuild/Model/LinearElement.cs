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

using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A linear element is one defined in terms of a set-out curve
    /// and a section that is swept along the curve to define the 3D
    /// solid geometry.
    /// Used to represent objects where one dimension is greater than
    /// the others and the overall geometry can be represented as an
    /// extrusion along a curve, such as Beams, Columns, Walls, etc.
    /// </summary>
    [Serializable]
    public class LinearElement : Element<Curve, SectionFamily>
    {
        #region Properties

        /// <summary>
        /// The node (if any) that the start of this element is connected to.
        /// This is a shortcut property to get or set the node attached to the
        /// start of this element's geometry curve.
        /// Note that FreeBuild element geometry is not dependent on nodes and
        /// so this property may have a null value.
        /// </summary>
        public Node StartNode
        {
            get
            {
                return Geometry?.Start?.Node;
            }
            set
            {
                if (Geometry != null && Geometry.Start != null) Geometry.Start.Node = value;
            }
        }

        /// <summary>
        /// The node (if any) that the end of this element is connected to.
        /// This is a shortcut property to get or set the node attached to the
        /// end of this element's geometry curve.
        /// Note that FreeBuild element geometry is not dependent on nodes and
        /// so this property may have a null value.
        /// </summary>
        public Node EndNode
        {
            get
            {
                return Geometry?.End?.Node;
            }
            set
            {
                if (Geometry != null && Geometry.End != null) Geometry.End.Node = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// For use in factory methods only.
        /// </summary>
        public LinearElement()
        {

        }

        /// <summary>
        /// Curve geometry constructor
        /// </summary>
        /// <param name="geometry"></param>
        public LinearElement(Curve geometry)
        {
            Geometry = geometry;
        }

        /// <summary>
        /// Curve, property constructor
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="property"></param>
        public LinearElement(Curve geometry, SectionFamily property) : this(geometry)
        {
            Family = property;
        }

        /// <summary>
        /// Start and end point constructor
        /// Creates an element set-out along a straight line between
        /// the start and end points.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public LinearElement(Vector startPoint, Vector endPoint)
        {
            Geometry = new Line(startPoint, endPoint);
        }

        /// <summary>
        /// Start and end point constructor
        /// Creates an element set-out along a straight line between
        /// the start and end points.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="property"></param>
        public LinearElement(Vector startPoint, Vector endPoint, SectionFamily property):this(startPoint, endPoint)
        {
            Family = property;
        }

        /// <summary>
        /// Start and end node constructor.
        /// Initialises a new element set-out along a straight line
        /// between the start and end nodes.
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        public LinearElement(Node startNode, Node endNode, SectionFamily property = null)
        {
            Geometry = new Line(startNode, endNode);
            Family = property;
        }

        #endregion

    }
}
