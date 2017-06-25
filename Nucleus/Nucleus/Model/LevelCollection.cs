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

using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A collection of levels.
    /// </summary>
    [Serializable]
    public class LevelCollection : ModelObjectCollection<Level>
    {
        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new empty LevelCollection.
        /// </summary>
        public LevelCollection() : base() { }

        /// <summary>
        /// Owner constructor.  Initialises a new LevelCollection owned by the specified model.
        /// </summary>
        /// <param name="model">The model that owns the items in this collection</param>
        public LevelCollection(Model model) : base(model) { }

        #endregion

        #region Methods

        /// <summary>
        /// Search through this collection and find the next highest level after
        /// the one specified.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public Level NextLevelAbove(double z)
        {
            double minDistance = 0;
            Level result = null;
            foreach (Level testLevel in this)
            {
                double dist = testLevel.Z - z;
                if (dist > 0 && (result == null || dist < minDistance))
                {
                    result = testLevel;
                    minDistance = dist;
                }
            }
            return result;
        }

        /// <summary>
        /// Search through this collection and find the next lowest level after
        /// the one specified.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public Level NextLevelBelow(double z)
        {
            double minDistance = 0;
            Level result = null;
            foreach (Level testLevel in this)
            {
                double dist = z - testLevel.Z;
                if (dist > 0 && (result == null || dist < minDistance))
                {
                    result = testLevel;
                    minDistance = dist;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the level in this collection closest to the specified z-coordinate
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public Level NearestLevel(double z)
        {
            double minDistance = 0;
            Level result = null;
            foreach (Level level in this)
            {
                double dist = (level.Z - z).Abs();
                if (result == null || dist < minDistance)
                {
                    result = level;
                    minDistance = dist;
                }
            }
            return result;
        }

        /// <summary>
        /// Get the subset of this collection which has a recorded modification after the specified date and time
        /// </summary>
        /// <param name="since">The date and time to filter by</param>
        /// <returns></returns>
        public LevelCollection Modified(DateTime since)
        {
            return this.Modified<LevelCollection, Level>(since);
        }

        /// <summary>
        /// Get the subset of this collection which has an attached data component of the specified type
        /// </summary>
        /// <typeparam name="TData">The type of data component to check for</typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public LevelCollection AllWithDataComponent<TData>()
            where TData : class
        {
            return this.AllWithDataComponent<LevelCollection, Level, TData>();
        }

        #endregion
    }
}
