using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Units
{
    /// <summary>
    /// Attribute to be applied to properties in order to specify the dimensions
    /// of the quantities they represent.
    /// </summary>
    [Serializable]
    public class DimensionAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// The type of dimension of the annotated property
        /// </summary>
        public DimensionTypes Type { get; } = DimensionTypes.Dimensionless;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        public DimensionAttribute(DimensionTypes type)
        {
            Type = type;
        }
    }
}
