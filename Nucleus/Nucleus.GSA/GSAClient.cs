using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interop.gsa_8_7;
using Nucleus.IO;
using Nucleus.Model;
using Nucleus.Geometry;
using Nucleus.Extensions;
using System.IO;

namespace Nucleus.GSA
{
    /// <summary>
    /// Controller class to drive GSA
    /// </summary>
    public class GSAClient : MessageRaiser, IApplicationClient
    {
        #region Properties

        /// <summary>
        /// Private backing field for GSA Property
        /// </summary>
        private ComAuto _GSA = null;

        /// <summary>
        /// The current GSA application
        /// </summary>
        public ComAuto GSA
        {
            get
            {
                if (_GSA == null)
                {
                    _GSA = new ComAuto();
                }
                return _GSA;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Open the GSA file at the specified filepath
        /// </summary>
        /// <param name="filePath">The filepath to open.  (Note that this can be expressed as a string)</param>
        /// <returns>True if the specified file could be opened, false if this was prevented in some way.</returns>
        public bool Open(FilePath filePath)
        {
            try
            {
                RaiseMessage("Opening GSA file '" + filePath + "'...");
                return GSA.Open(filePath) == 0;
            }
            catch (Exception ex)
            {
                RaiseMessage("Error: Opening GSA file '" + filePath + "' failed.  " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Open a new file in the target application
        /// </summary>
        /// <returns></returns>
        public bool New()
        {
            try
            {
                RaiseMessage("Creating new GSA model...");
                return GSA.NewFile() == 0;
            }
            catch (Exception ex)
            {
                RaiseMessage("Error: Creating new GSA model failed.  " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Save the currently open GSA file to the specified file location
        /// </summary>
        /// <param name="filePath">The filepath to save to</param>
        /// <returns></returns>
        public bool Save(FilePath filePath)
        {
            try
            {
                RaiseMessage("Writing GSA file to '" + filePath + "'...");
                switch (GSA.SaveAs(filePath))
                {
                    case 0:
                        RaiseMessage("...Complete.");
                        return true;
                    case 1:
                        RaiseMessage("...Failed.  No GSA file is open!");
                        return false;
                    case 2:
                        RaiseMessage("...Failed.  Invalid file extension!");
                        return false;
                    default:
                        RaiseMessage("...Failed!");
                        return false;
                }
            }
            catch (Exception ex)
            {
                RaiseMessage("Error: Writing GSA file to '" + filePath + "' failed.  " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Close the currently open file in the target application
        /// </summary>
        public void Close()
        {
            GSA.Close();
        }

        /// <summary>
        /// Issue a GWA command to GSA
        /// </summary>
        /// <param name="command">The command to be issued, in GWA format</param>
        /// <returns></returns>
        public dynamic GWACommand(string command)
        {
            return GSA.GwaCommand(command);
        }

        /// <summary>
        /// Issue a GWA command to GSA, assembling the full string automatically
        /// from the tokens provided
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public dynamic GWACommand(params string[] tokens)
        {
            var sb = new StringBuilder();
            foreach (string token in tokens)
            {
                if (sb.Length > 0) sb.Append("\t"); // Separator
                sb.Append(token);
            }
           return GWACommand(sb.ToString());
        }

        /// <summary>
        /// Issue a 'GET' GWA command to GSA.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GWAGet(string keyword, string id)
        {
            return GWACommand("GET", keyword, id);
        }

        /// <summary>
        /// Issue a 'GET_ALL' GWA command to GSA
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string GWAGetAll(string keyword)
        {
            return GWACommand("GET_ALL", keyword);
        }

        /// <summary>
        /// Read the currently open GSA model to populate a Nucleus model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool ReadModel(Model.Model model, GSAConversionContext context)
        {
            RaiseMessage("Reading data from GSA...");
            if (context.Options.Families)
            {
                ReadSections(model, context);
                ReadBuildUps(model, context);
            }
            if (context.Options.Nodes) ReadNodes(model, context);
            if (context.Options.LinearElements || context.Options.PanelElements) ReadElements(model, context);
            return true;
        }

        /// <summary>
        /// Read all sections from the currently open GSA file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        public void ReadSections(Model.Model model, GSAConversionContext context)
        {
            RaiseMessage("Reading sections...");
            ParseGWALines(GWAGetAll(Keywords.Section), model, context);
        }

        /// <summary>
        /// Read all 2d element properties from the currently open GSA file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        public void ReadBuildUps(Model.Model model, GSAConversionContext context)
        {
            RaiseMessage("Reading build-ups...");
            ParseGWALines(GWAGetAll(Keywords.BuildUp), model, context);
        }

        /// <summary>
        /// Read all nodes from the currently open GSA file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        public void ReadNodes(Model.Model model, GSAConversionContext context)
        {
            RaiseMessage("Reading nodes...");
            ParseGWALines(GWAGetAll(Keywords.Node), model, context);
        }

        /// <summary>
        /// Read all elements from the currently open GSA file
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        public void ReadElements(Model.Model model, GSAConversionContext context)
        {
            RaiseMessage("Reading elements...");
            ParseGWALines(GWAGetAll(Keywords.Element), model, context);
        }

        private void ParseGWALines(string gwa, Model.Model model, GSAConversionContext context)
        {
            GWAParser parser = new GWAParser();
            using (TextReader reader = new StringReader(gwa))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (context.Options.PrintAll) RaiseMessage(line);
                    parser.ParseGWALine(line, model, context);
                }
            }
        }

        public void ReadSets(Model.Model model, GSAConversionContext context)
        {
            RaiseMessage("Reading sets...");
            ParseGWALines(GWAGetAll(Keywords.List), model, context);
        }

        // Abortive attempt to use the new struct types:
        /*public void ReadNodes()
        {
            int[] ids;
            GSA.EntitiesInList("all", GsaEntity.NODE, out ids);
            if (ids != null)
            {
                GsaNode[] nodes;
                GSA.Nodes(ids, out nodes);
                foreach (GsaNode node in nodes)
                {
                    //TODO (?)
                }
            }

        }

        public void ReadElements()
        {
            int[] ids;
            GSA.EntitiesInList("all", GsaEntity.ELEMENT, out ids);
            if (ids != null)
            {
                GsaElement[] elements;
                GSA.Elements(ids, out elements);
                foreach (GsaElement element in elements)
                {
                    //TODO (?)
                }
            }
        }*/

        

        #endregion
    }
}
