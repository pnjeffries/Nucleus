using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A data component representing a key item which may be used to unlock a Door
    /// </summary>
    [Serializable]
    public class Key : Unique, IElementDataComponent
    {
        private string _KeyCode;

        /// <summary>
        /// The keycode of this key.  Doors with a matching keycode
        /// may be unlocked by this key.
        /// </summary>
        public string KeyCode
        {
            get { return _KeyCode; }
        }

        public Key(string keyCode)
        {
            _KeyCode = keyCode;
        }
    }
}
