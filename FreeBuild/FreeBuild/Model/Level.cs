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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Object that represents a level in a model
    /// </summary>
    [Serializable]
    public class Level : ModelObject
    {
        #region Properties

        /// <summary>
        /// Private backing field for the Z property
        /// </summary>
        private double _Z = 0;

        /// <summary>
        /// The z-coordinate of the level
        /// </summary>
        public double Z
        {
            get { return _Z; }
            set { _Z = value; NotifyPropertyChanged("Z"); }
        }

        /// <summary>
        /// The name of this level.
        /// </summary>
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

        #endregion
    }
}
