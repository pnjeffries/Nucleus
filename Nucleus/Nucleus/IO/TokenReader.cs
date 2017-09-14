using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// A TokenReader is a utility class that helps with tokenising and reading
    /// out strings sequentially.
    /// </summary>
    public class TokenReader
    {
        #region Properties

        /// <summary>
        /// Private backing field for Tokens property
        /// </summary>
        private string[] _Tokens;

        /// <summary>
        /// The full set of tokens in the string
        /// </summary>
        public string[] Tokens
        {
            get { return _Tokens; }
        }

        /// <summary>
        /// Private backing field for Index property
        /// </summary>
        private int _Index = 0;

        /// <summary>
        /// The index of the next token
        /// </summary>
        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new TokenReader set up to tokenise and read the
        /// specified string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        public TokenReader(string str, params char[] separator)
        {
            _Tokens = str.Split(separator);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the next token.  The position of the reader will
        /// then move along to the next token, such that repeated calls
        /// to Next() will return each token in sequence.
        /// </summary>
        /// <returns></returns>
        public string Next()
        {
            if (_Index < _Tokens.Length)
            {
                var result = _Tokens[_Index];
                Index++;
                return result;
            }
            else return null;
        }

        /// <summary>
        /// Get the next token, parsed to a double.  The position of the reader will
        /// then move along to the next token, such that repeated calls
        /// to Next___() will return each token in sequence.
        /// </summary>
        /// <returns></returns>
        public double NextDouble()
        {
            string next = Next();
            return double.Parse(next);
        }

        /// <summary>
        /// Get the next token, parsed to an integer.  The position of the reader will
        /// then move along to the next token, such that repeated calls
        /// to Next___() will return each token in sequence.
        /// </summary>
        /// <returns></returns>
        public int NextInt()
        {
            string next = Next();
            return int.Parse(next);
        }

        /// <summary>
        /// Get the next token, parsed to a boolean.  The position of the reader will
        /// then move along to the next token, such that repeated calls
        /// to Next___() will return each token in sequence.
        /// </summary>
        /// <returns></returns>
        public bool NextBool()
        {
            string next = Next();
            return bool.Parse(next); //TODO: Make more flexible?
        }

        /// <summary>
        /// Read the next three tokens and parse them as a 
        /// Vector's X, Y and Z coordinates.
        /// The position of the reader will
        /// then move along to the next token after these, such that repeated calls
        /// to Next___() will return each token in sequence.
        /// </summary>
        /// <returns></returns>
        public Vector Next3AsVector()
        {
            var result = Vector.FromTokensList(_Tokens, _Index);
            Index += 3;
            return result;
        }

        /// <summary>
        /// Read the next six tokens and parse them as a 
        /// Six-vector's X, Y, Z, XX, YY and ZZ coordinates.
        /// The position of the reader will
        /// then move along to the next token after these, such that repeated calls
        /// to Next___() will return each token in sequence.
        /// </summary>
        /// <returns></returns>
        public SixVector Next6AsSixVector()
        {
            var result = SixVector.FromTokensList(_Tokens, _Index);
            Index += 6;
            return result;
        }

        /// <summary>
        /// Read the next six tokens and parse them as a 
        /// Bool6D's X, Y, Z, XX, YY and ZZ boolean values.
        /// The position of the reader will
        /// then move along to the next token after these, such that repeated calls
        /// to Next___() will return each token in sequence.
        /// </summary>
        /// <returns></returns>
        public Bool6D Next6AsBool6D()
        {
            //TODO!
        }

        /// <summary>
        /// Is the next string equal to the specified value (ignoring case)?
        /// The position of the reader will
        /// then move along to the next token, such that repeated calls
        /// to Next___() will return each token in sequence.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool NextIs(string value)
        {
            string token = Next();
            return token.EqualsIgnoreCase(value);
        }

        /// <summary>
        /// Get the first token in the string
        /// </summary>
        /// <returns></returns>
        public string First()
        {
            if (_Tokens.Length > 0) return _Tokens[0];
            else return null;
        }

        /// <summary>
        /// Get the last token in the string
        /// </summary>
        /// <returns></returns>
        public string Last()
        {
            if (_Tokens.Length > 0) return _Tokens[_Tokens.Length - 1];
            else return null;
        }

        /// <summary>
        /// Skip the specified number of tokens
        /// </summary>
        /// <param name="number"></param>
        public void Skip(int number = 1)
        {
            Index += number;
        }

        /// <summary>
        /// Has the TokenReader reached the end of the string?
        /// </summary>
        /// <returns></returns>
        public bool AtEnd()
        {
            return _Index >= _Tokens.Length;
        }

        #endregion

    }
}
