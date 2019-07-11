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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// An ARGB colour structure.
    /// Functionaly equivalent to the System.Media.Color structure,
    /// but implemented here to allow for future compatibility with
    /// Mono.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{ToString()}")]
    public struct Colour
        : IEquatable<Colour>
    {
        #region Constants
        /// <summary> An opaque solid black colour </summary>
        public static readonly Colour Black = new Colour(0, 0, 0);

        /// <summary> An opaque solid white colour </summary>
        public static readonly Colour White = new Colour(255, 255, 255);

        /// <summary> An opaque solid red colour </summary>
        public static readonly Colour Red = new Colour(255, 0, 0);

        /// <summary> An opaque solid green colour </summary>
        public static readonly Colour Green = new Colour(0, 255, 0);

        /// <summary> An opaque solid blue colour </summary>
        public static readonly Colour Blue = new Colour(0, 0, 255);

        /// <summary> An opaque solid yellow colour </summary>
        public static readonly Colour Yellow = new Colour(255, 255, 0);

        /// <summary> A transparent colour </summary>
        public static readonly Colour Transparent = new Colour(0, 0, 0, 0);

        /// <summary> The shade of blue used in the Ramboll logo </summary>
        public static readonly Colour RambollCyan = new Colour(0, 157, 244);

        /// <summary> The shade of dark grey used in the Ramboll standard palette </summary>
        public static readonly Colour RambollDarkGrey = new Colour(121, 119, 102);

        /// <summary> The shade of light blue used in the Ramboll standard palette </summary>
        public static readonly Colour RambollLightBlue = new Colour(167, 211, 245);

        /// <summary> The shade of light grey used in the Ramboll standard palette </summary>
        public static readonly Colour RambollLightGrey = new Colour(208, 207, 197);

        /// <summary> The shade of lime green used in the Ramboll standard palette </summary>
        public static readonly Colour RambollLimeGreen = new Colour(161, 191, 54);

        /// <summary> The shade of green used in the Ramboll standard palette </summary>
        public static readonly Colour RambollGreen = new Colour(92, 165, 81);

        /// <summary> The shade of magenta used in the Ramboll standard palette </summary>
        public static readonly Colour RambollMagenta = new Colour(196, 0, 121);

        /// <summary> The shade of magenta used in the Ramboll standard palette </summary>
        public static readonly Colour RambollWarmRed = new Colour(198, 52, 24);

        /// <summary> The shade of brown used as an addition to the Ramboll standard palette </summary>
        public static readonly Colour RambollBrown = new Colour(99, 26, 12);

        /// <summary> The shade of yellow used as an addition to the Ramboll standard palette </summary>
        public static readonly Colour RambollYellow = new Colour(255, 192, 0);

        /// <summary> The shade of orange used in the Salamander logo </summary>
        public static readonly Colour SalamanderOrange = new Colour(255, 97, 47);

        /// <summary>
        /// The standard Ramboll colour palette
        /// </summary>
        public static readonly Colour[] RambollPalette
            = new Colour[]{
                RambollCyan,
                RambollLightGrey,
                RambollLightBlue,
                RambollGreen,
                RambollLimeGreen,
                RambollMagenta,
                RambollWarmRed,
                RambollBrown,
                RambollYellow,
                RambollDarkGrey,
                Black

};

        /// <summary>
        /// The standard Ramboll colour palette (re-ordered)
        /// </summary>
        public static readonly Colour[] RambollPalette2
            = new Colour[]{
                RambollLightBlue,
                RambollLightGrey,
                RambollLimeGreen,
                RambollGreen,
                RambollMagenta,
                RambollWarmRed,
                RambollCyan,
                RambollDarkGrey,
                Black,
                RambollBrown,
                RambollYellow};


        #endregion

        #region Properties

        /// <summary>
        /// The alpha channel of the colour.
        /// Range from 0-255.
        /// </summary>
        public byte A { get; }

        /// <summary>
        /// The red channel of the colour.
        /// Range from 0-255.
        /// </summary>
        public byte R { get; }

        /// <summary>
        /// The green channel of the colour.
        /// Range from 0-255.
        /// </summary>
        public byte G { get; }

        /// <summary>
        /// The blue channel of the colour.
        /// Range from 0-255.
        /// </summary>
        public byte B { get; }

        /// <summary>
        /// Obtain a measure of the apparent brightness of the colour,
        /// ranging between 0-1.
        /// The colour components are weighted differently when calculating
        /// 'brightness' in this way; Red is weighted by 0.3, green by 0.59 
        /// and blue by 0.11 - the brightness is the unitized combination of
        /// all three values.
        /// </summary>
        public double Brightness
        {
            get { return (R / 255.0) * 0.3 + (G / 255.0) * 0.59 + (B / 255.0) * 0.11; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// A,R,G,B byte constructor
        /// </summary>
        /// <param name="a">The alpha channel.  0-255.</param>
        /// <param name="r">The red channel.  0-255.</param>
        /// <param name="g">The green channel.  0-255.</param>
        /// <param name="b">The blue channel.  0-255.</param>
        public Colour(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// R,G,B byte constructor
        /// </summary>
        /// <param name="r">The red channel.  0-255.</param>
        /// <param name="g">The green channel.  0-255.</param>
        /// <param name="b">The blue channel.  0-255.</param>
        public Colour(byte r, byte g, byte b) : this((byte)255, r, g, b) { }

        /// <summary>
        /// A, R, G, B integer constructor.
        /// Will automatically cap values that exceed 255.
        /// </summary>
        /// <param name="a">The alpha channel.  0-255.</param>
        /// <param name="r">The red channel.  0-255.</param>
        /// <param name="g">The green channel.  0-255.</param>
        /// <param name="b">The blue channel.  0-255.</param>
        public Colour(int a, int r, int g, int b)
            : this((byte)Math.Min(a, 255), (byte)Math.Min(r, 255), (byte)Math.Min(g, 255), (byte)Math.Min(b, 255)) { }

        /// <summary>
        /// R, G, B integer constructor.
        /// Will automatically cap values that exceed 255.
        /// </summary>
        /// <param name="r">The red channel.  0-255.</param>
        /// <param name="g">The green channel.  0-255.</param>
        /// <param name="b">The blue channel.  0-255.</param>
        public Colour(int r, int g, int b)
            : this((byte)255, (byte)Math.Min(r, 255), (byte)Math.Min(g, 255), (byte)Math.Min(b, 255)) { }

        /// <summary>
        /// A,R,G,B float constructor
        /// </summary>
        /// <param name="a">The alpha channel.  0-1.</param>
        /// <param name="r">The red channel.  0-1.</param>
        /// <param name="g">The green channel.  0-1.</param>
        /// <param name="b">The blue channel.  0-1.</param>
        public Colour(float a, float r, float g, float b) 
            : this((byte)(255 * a), (byte)(255 * r), (byte)(255 * g), (byte)(255 * b)) { }

        /// <summary>
        /// A,R,G,B float constructor
        /// </summary>
        /// <param name="r">The red channel.  0-1.</param>
        /// <param name="g">The green channel.  0-1.</param>
        /// <param name="b">The blue channel.  0-1.</param>
        public Colour(float r, float g, float b)
            : this((byte)255, (byte)(255 * r), (byte)(255 * g), (byte)(255 * b)) { }

       
        #endregion

        #region Methods

        /// <summary>
        /// If the brightness of the colour currently exceeds the specified
        /// value, reduce it to that value.
        /// </summary>
        /// <param name="brightness">The maximum brightness value,
        /// expressed as a value between 0-1.</param>
        /// <returns></returns>
        public Colour CapBrightness(double brightness)
        {
            double factor = 1;
            double currentBrightness = Brightness;
            if (currentBrightness > brightness)
            {
                factor = brightness / currentBrightness;
            }
            return new Colour(
                A,
                (byte)(R*factor), 
                (byte)(G*factor),
                (byte)(B*factor));
        }

        /// <summary>
        /// Adjust the components of this colour to the specified brightness
        /// </summary>
        /// <param name="brightness"></param>
        /// <returns></returns>
        public Colour SetBrightness(double brightness)
        {
            double currentBrightness = Brightness;
            if (currentBrightness == 0)
            {
                return new Colour(A,
                    (byte)brightness * 255,
                    (byte)brightness * 255,
                    (byte)brightness * 255);
            }
            double factor = brightness / currentBrightness;
            return new Colour(
                A,
                (byte)(R * factor),
                (byte)(G * factor),
                (byte)(B * factor));
        }

        /// <summary>
        /// Get the hash code for this colour
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return A * 16777216 + R *65536 + G*256 + B;
        }

        /// <summary>
        /// Is this colour equal to another object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Colour) return Equals((Colour)obj);
            else return false;
        }

        /// <summary>
        /// Is this colour equal to another?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Colour other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
        }

        /// <summary>
        /// Add another colour to this one and return the result
        /// </summary>
        /// <param name="other">The colour to add to this one</param>
        /// <returns></returns>
        public Colour Add(Colour other)
        {
            return new Colour(
                A + other.A, 
                R + other.R, 
                G + other.G, 
                B + other.B);
        }

        /// <summary>
        /// Subtract another colour from this one and return the result
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Colour Subtract(Colour other)
        {
            return new Colour(
                Math.Max(A - other.A,0), 
                Math.Max(R - other.R,0),
                Math.Max(G - other.G,0), 
                Math.Max(B - other.B,0));
        }

        /// <summary>
        /// Invert this colour, subtracting its RGB values from white
        /// but keeping the same alpha value.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Colour Invert(Colour other)
        {
            return new Colour(A, 255 - R, 255 - G, 255 - B);
        }

        /// <summary>
        /// Convert this colour to an array of bytes
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            return new byte[] { A, R, G, B };
        }

        /// <summary>
        /// Convert this colour to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "#" + BitConverter.ToString(ToByteArray()).Replace("-", "");
        }

        /// <summary>
        /// Convert this colour to an ARGB hex string
        /// </summary>
        /// <returns></returns>
        public string ToHex()
        {
            return A.ToString("X2") + R.ToString("X2") + G.ToString("X2") + B.ToString("X2");
        }

        /// <summary>
        /// Multiply all components of this colour by a scalar factor.
        /// Components will be clamped to 0-255 to prevent overflow.
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public Colour Scale(double scalar)
        {
            return new Colour(
                (int)(A * scalar),
                (int)(R * scalar),
                (int)(G * scalar),
                (int)(B * scalar));
        }

        /// <summary>
        /// Interpolate between this colour and another
        /// </summary>
        /// <param name="towards">The colour to interpolate towards</param>
        /// <param name="factor">The interpolation factor.  0 = this colour, 1 = the 'towards' colour</param>
        /// <returns>An interpolated colour</returns>
        public Colour Interpolate(Colour towards, double factor)
        {
            return new Colour(
                (byte)(A + (towards.A - A) * factor),
                (byte)(R + (towards.R - R) * factor),
                (byte)(G + (towards.G - G) * factor),
                (byte)(B + (towards.B - B) * factor));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Create a colour from hue, saturation and value
        /// </summary>
        /// <param name="h">Hue, range 0-360</param>
        /// <param name="s">Saturation, range 0-255</param>
        /// <param name="v">Value, range 0-255</param>
        /// <param name="a">Alpha value, range 0-255</param>
        /// <returns></returns>
        public static Colour FromHSV(double h, double s, double v, byte a = 255)
        {
            // Normalise values
            s /= 255;
            v /= 255;

            h = h % 360; // Clamp to 0-360 range
            //while (h < 0) h += 360;
            //while (h > 360) h -= 360;

            // Set up intermediate values
            double c = v * s;
            double x = c * (1 - Math.Abs((h / 60) % 2 - 1));
            double m = v - c;
            double r = m;
            double g = m;
            double b = m;

            // Adjust RGB values based on hue:
            if (h < 60)
            {
                r += c;
                g += x;
            }
            else if (h < 120)
            {
                r += x;
                g += c;
            }
            else if (h < 180)
            {
                g += c;
                b += x;
            }
            else if (h < 240)
            {
                g += x;
                b += c;
            }
            else if (h < 300)
            {
                r += x;
                b += c;
            }
            else
            {
                r += c;
                b += x;
            }

            return new Colour(a, (byte)(255 * r), (byte)(255 * g), (byte)(255 * b));
        }

        #endregion

        #region Operators

        public static bool operator ==(Colour a, Colour b)
            => a.Equals(b);

        public static bool operator !=(Colour a, Colour b)
            => !a.Equals(b);

        public static Colour operator +(Colour a, Colour b)
            => a.Add(b);

        public static Colour operator -(Colour a, Colour b)
            => a.Subtract(b);

        #endregion
    }
}
