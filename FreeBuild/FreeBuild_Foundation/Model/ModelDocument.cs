using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A document which contains and stores a model
    /// </summary>
    [Serializable]
    public class ModelDocument : Document
    {
        /// <summary>
        /// The model contained within this document
        /// </summary>
        public Model Model { get; protected set; }
    }
}
