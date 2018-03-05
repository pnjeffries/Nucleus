using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFE = BriefFiniteElementNet;

namespace Nucleus.BriefFE
{
    /// <summary>
    /// A manager class to analyse a model using BriefFE
    /// </summary>
    public class BriefFEClient
    {
        public void InitialiseAnalysisModel(Model.Model model)
        {
            // Create model:
            var feModel = new BFE.Model();

            // Create nodes:
            foreach (var node in model.Nodes)
            {
                feModel.Nodes.Add(ToBFE.Convert(node));
            }

            // Create sections:
            foreach (var family in model.Families)
            {
                BFE.SectionGenerator.
            }

            // Create elements:
            foreach (var element in model.Elements)
            {
                if (element is LinearElement)
                {

                }


            }
        }
    }
}
