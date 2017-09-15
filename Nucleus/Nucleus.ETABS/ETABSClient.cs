using Nucleus.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Base;
using ETABS2016;
using Nucleus.Conversion;

namespace Nucleus.ETABS
{
    public class ETABSClient : MessageRaiser, IApplicationClient
    {
        #region Properties

        /// <summary>
        /// Backing field for ETABS property
        /// </summary>
        private cOAPI _ETABS = null;

        /// <summary>
        /// The current ETABS application
        /// </summary>
        public cOAPI ETABS
        {
            get
            {
                if (_ETABS == null)
                {
                    RaiseMessage("Establishing ETABS link...");
                    cHelper helper = new Helper();
                    _ETABS = helper.CreateObjectProgID("CSI.ETABS.API.ETABSObject");
                    RaiseMessage("ETABS link established.");
                }
                return _ETABS;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Close the linked ETABS application
        /// </summary>
        public void Close()
        {
            ETABS.ApplicationExit(false);
        }

        /// <summary>
        /// Open a new ETABS Model
        /// </summary>
        /// <returns></returns>
        public bool New()
        {
            return ETABS.SapModel.InitializeNewModel(eUnits.N_m_C) == 0;
        }

        /// <summary>
        /// Open a saved ETABS Model
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool Open(FilePath filePath)
        {
            return ETABS.SapModel.File.OpenFile(filePath) == 0;
        }

        /// <summary>
        /// Save the current ETABS model to a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool Save(FilePath filePath)
        {
            return ETABS.SapModel.File.Save(filePath) == 0;
        }

        public bool WriteModelToEtabs(FilePath filePath, Model.Model model, ref ETABSIDMappingTable idMap, ETABSConversionOptions options)
        {
            return false;
        }


        #endregion


    }
}
