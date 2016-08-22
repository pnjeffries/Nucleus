using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of model documents
    /// </summary>
    [Serializable]
    public class ModelDocumentCollection : UniquesCollection<ModelDocument>
    {
    }
}
