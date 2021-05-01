﻿// Copyright (c) 2016 Paul Jeffries
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

using Nucleus.Geometry;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Object that represents a level in a model
    /// </summary>
    [Serializable]
    public class Level : ModelObject, IComparable<Level>
    {
        #region Properties

        /// <summary>
        /// Private backing field for the Z property
        /// </summary>
        private double _Z = 0;

        /// <summary>
        /// The z-coordinate of the level
        /// </summary>
        [AutoUI(400)]
        public double Z
        {
            get { return _Z; }
            set
            {
                _Z = value; NotifyPropertyChanged("Z");
                if (_Name == null) NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// The name of this level.
        /// </summary>
        [AutoUI(100)]
        public override string Name
        {
            get
            {
                if (_Name == null)
                {
                    string result = "Level ";
                    result += string.Format("{0:+0.00;-0.00}", Z);
                    return result;
                }
                return base.Name;
            }

            set
            {
                base.Name = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new level situated at Z = 0.
        /// </summary>
        public Level() { }

        /// <summary>
        /// Initialise a new level at the specified Z-level.
        /// </summary>
        /// <param name="z"></param>
        public Level(double z) : this()
        {
            Z = z;
        }

        /// <summary>
        /// Initialise a new level with the specified name at the specified Z-level.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="z"></param>
        public Level(string name, double z) : this(z)
        {
            Name = name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is this level higher than the other specified level
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAbove(Level other)
        {
            return Z > other.Z;
        }

        /// <summary>
        /// Is this level lower than the other specified level
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsBelow(Level other)
        {
            return Z < other.Z;
        }

        /// <summary>
        /// Is this level coincident with the other specified level
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsCoincident(Level other)
        {
            return Z == other.Z; //TODO: Tolerance?
        }

        /// <summary>
        /// IComparable implementation
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Level other)
        {
            return Z.CompareTo(other.Z);
        }

        /// <summary>
        /// Project the specified set of point vectors onto this level
        /// </summary>
        /// <param name="points">The initial set of points</param>
        /// <returns>A projected set of points on this level</returns>
        public Vector[] ProjectPoints(IList<Vector> points)
        {
            var result = new Vector[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                result[i] = points[i].WithZ(Z);
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj is Level level) return Z == level.Z;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Z.GetHashCode();
        }

        #endregion
    }
}
