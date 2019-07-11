using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// The category of a material.
    /// May be used to determine which design codes are
    /// applicable or to establish equivalent material
    /// properties in external packages.
    /// </summary>
    public enum MaterialCategory
    {
        /// <summary>
        /// The material category is not specified and
        /// may be a type of material not expressly covered
        /// by this enum.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The material is a type of steel.
        /// </summary>
        Steel = 100,

        /// <summary>
        /// The material is a type of aluminium.
        /// </summary>
        Aluminium = 150,

        /// <summary>
        /// The material is a type of concrete
        /// or reinforced concrete
        /// </summary>
        Concrete = 200,

        /// <summary>
        /// The material is a type of wood
        /// </summary>
        Wood = 300,

        /// <summary>
        /// The material is a type of glass
        /// </summary>
        Glass = 400,

    }
}
