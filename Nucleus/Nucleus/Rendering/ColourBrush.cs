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

namespace Nucleus.Rendering
{
    /// <summary>
    /// A brush that uses a single solid colour
    /// </summary>
    [Serializable]
    public class ColourBrush : DisplayBrush
    {
        #region Constants

        /// <summary>
        /// Get a default black colour brush
        /// </summary>
        public static ColourBrush Black { get { return new ColourBrush(Colour.Black); } }

        /// <summary>
        /// Get a default white colour brush
        /// </summary>
        public static ColourBrush White { get { return new ColourBrush(Colour.White); } }

        /// <summary>
        /// Get a default red colour brush
        /// </summary>
        public static ColourBrush Red { get { return new ColourBrush(Colour.Red); } }

        /// <summary>
        /// Get a default green colour brush
        /// </summary>
        public static ColourBrush Green { get { return new ColourBrush(Colour.Green); } }

        /// <summary>
        /// Get a default blue colour brush
        /// </summary>
        public static ColourBrush Blue { get { return new ColourBrush(Colour.Blue); } }

        /// <summary>
        /// Get a default transparent colour brush
        /// </summary>
        public static ColourBrush Transparent { get { return new ColourBrush(Colour.Transparent); } }

        /// <summary>
        /// Get a single-colour brush in Ramboll blue
        /// </summary>
        public static ColourBrush RambollBlue { get { return new ColourBrush(Colour.RambollCyan); } }

        /// <summary>
        /// Get a single-colour brush in Salamander orange
        /// </summary>
        public static ColourBrush SalamanderOrange { get { return new ColourBrush(Colour.SalamanderOrange); } }

        #endregion


        #region Properties

        private Colour _Colour = Colour.Black;

        /// <summary>
        /// The colour of the brush
        /// </summary>
        public Colour Colour
        {
            get { return _Colour; }
            set { _Colour = value; }
        }

        /// <summary>
        /// Get the base colour for this display brush - a single
        /// colour that can be used to represent this brush in cases where
        /// more complex shading is not available.
        /// For ColourBrushes this will return the Colour property.
        /// </summary>
        public override Colour BaseColour { get { return Colour; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Colour constructor
        /// </summary>
        /// <param name="colour"></param>
        public ColourBrush(Colour colour)
        {
            Colour = colour;
        }

        #endregion
    }
}
