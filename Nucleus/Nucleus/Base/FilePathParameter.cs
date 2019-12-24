using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// A named input or output parameter which holds a file path
    /// </summary>
    [Serializable]
    public class FilePathParameter : Parameter<FilePath>
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Open property
        /// </summary>
        private bool _Open = false;

        /// <summary>
        /// Is this filepath limited to existing files which are to be opened?
        /// </summary>
        public bool Open
        {
            get { return _Open; }
            set { ChangeProperty(ref _Open, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialise a new FilePath parameter with the specified name
        /// </summary>
        /// <param name="name"></param>
        public FilePathParameter(string name, bool open = false) : base(name)
        {
            _Open = open;
        }

        /// <summary>
        /// Initialse a new FilePath parameter with the specified name and value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public FilePathParameter(string name, FilePath value) : base(name, value) { }

        /// <summary>
        /// Initialise a new FilePath parameter with the specified name, group and value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <param name="value"></param>
        public FilePathParameter(string name, ParameterGroup group, FilePath value)
            : base(name, group, value) { }

        #endregion
    }
}
