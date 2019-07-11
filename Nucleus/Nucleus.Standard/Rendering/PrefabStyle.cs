using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// A data component that represents the visual styling of an element
    /// by reference to a predefined visual model identified by a string key.
    /// </summary>
    [Serializable]
    public class PrefabStyle : AvatarStyle, IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the PrefabKey property
        /// </summary>
        private string _PrefabKey = null;

        /// <summary>
        /// The key by which the pre-defined visual style is referenced.  Typically its name.
        /// </summary>
        public string PrefabKey
        {
            get { return _PrefabKey; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an uninitialised Prefab style
        /// </summary>
        public PrefabStyle() { }

        /// <summary>
        /// Creates a prefab style referencing the specified prefab
        /// </summary>
        /// <param name="prefabKey"></param>
        public PrefabStyle(string prefabKey)
        {
            _PrefabKey = prefabKey;
        }

        #endregion
    }
}
