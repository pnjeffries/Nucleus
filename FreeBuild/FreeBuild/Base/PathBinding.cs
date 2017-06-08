using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// A binding that represents a connection to a particular member on an object.
    /// Equivalent to WPF Binding class
    /// </summary>
    [Serializable]
    public class PathBinding : NotifyPropertyChangedBase
    {
        #region Properties

        /// <summary>
        /// Private backing field for Path property
        /// </summary>
        private string _Path;

        /// <summary>
        /// The path of the property or method to bind to
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set { ChangeProperty(ref _Path, value, "Path"); }
        }

        /// <summary>
        /// Private backing field for Source property
        /// </summary>
        private object _Source;

        /// <summary>
        /// The source object
        /// </summary>
        public object Source
        {
            get { return _Source; }
            set { ChangeProperty(ref _Source, value, "Source"); }
        }

        
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new PathBinding with the specified path
        /// </summary>
        /// <param name="path"></param>
        public PathBinding(string path)
        {
            Path = path;
        }

        /// <summary>
        /// Initialse a new PathBinding with the specified path and source object
        /// </summary>
        /// <param name="path"></param>
        /// <param name="source"></param>
        public PathBinding(string path, object source) : this(path)
        {
            Source = source;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the return value of the bound member on the source object
        /// </summary>
        public object Value(object target = null)
        {
            if (_Source != null) return _Source.GetFromPath(Path);
            else return null;
        }

        #endregion
    }
}
