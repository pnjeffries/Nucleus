using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of BuildUpLayer objects
    /// </summary>
    [Serializable]
    public class BuildUpLayerCollection : UniquesCollection<BuildUpLayer>
    {
        #region Properties

        /// <summary>
        /// Get the total overall thickness of the build-up
        /// </summary>
        public double TotalThickness
        {
            get
            {
                double result = 0.0;
                foreach (BuildUpLayer layer in this)
                {
                    result += layer.Thickness;
                }
                return result;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new blank Build-Up Layer Collection
        /// </summary>
        public BuildUpLayerCollection() : base()
        {

        }

        /// <summary>
        /// Initialise a new BuildUpLayerCollection containing the specified layer
        /// </summary>
        /// <param name="layer"></param>
        public BuildUpLayerCollection(BuildUpLayer layer) : base()
        {
            Add(layer);
        }

        #endregion


    }
}
