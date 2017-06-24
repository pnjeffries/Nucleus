using FreeBuild.Conversion;
using FreeBuild.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Robot
{
    /// <summary>
    /// A set of options to be used when writing from FreeBuild to Robot 
    /// </summary>
    public class RobotConversionOptions : ConversionOptions
    {
        /// <summary>
        /// Private backing field for UpdateSince property
        /// </summary>
        private DateTime _UpdateSince = DateTime.MinValue;

        /// <summary>
        /// Gets or sets the time of the prior update/read.
        /// If specified, only those objects which have been modified since this time
        /// will be updated here.
        /// </summary>
        public DateTime UpdateSince
        {
            get { return _UpdateSince; }
            set { _UpdateSince = value;  NotifyPropertyChanged("UpdateSince"); }
        }

        /// <summary>
        /// Private backing field for update property
        /// </summary>
        private bool _Update = false;

        /// <summary>
        /// Should this be updated since the last modfications, rather than being a full rewrite of the
        /// file.
        /// </summary>
        [AutoUI(1, Label = "Update", ToolTip = "Update an existing file (if possible) instead of writing out a new one")]
        public bool Update
        {
            get { return _Update; }
            set { ChangeProperty(ref _Update, value, "Update"); }
        }

        /// <summary>
        /// Private backing field for Nodes property
        /// </summary>
        private bool _Nodes = true;

        /// <summary>
        /// Read/Write Nodes?
        /// </summary>
        [AutoUI(11, 
            ToolTip = "Read/Write Node data?  Note that even if this is off nodes may still " +
            "be created if necessary for Elements")]
        public bool Nodes
        {
            get { return _Nodes; }
            set { ChangeProperty(ref _Nodes, value, "Nodes"); }
        }

        /// <summary>
        /// Private backing field for Families property
        /// </summary>
        private bool _Families = true;

        /// <summary>
        /// Read/Write Families?
        /// </summary>
        [AutoUI(12, ToolTip = "Read/Write Section & Build-Up Families?")]
        public bool Families
        {
            get { return _Families; }
            set { ChangeProperty(ref _Families, value, "Families"); }
        }

        /// <summary>
        /// Private backing field for LinearElements property
        /// </summary>
        private bool _LinearElements = true;

        /// <summary>
        /// Read/Write Linear Elements?
        /// </summary>
        [AutoUI(12, ToolTip = "Read/Write Linear Element data?")]
        public bool LinearElements
        {
            get { return _LinearElements; }
            set { ChangeProperty(ref _LinearElements, value, "LinearElements"); }
        }

        /// <summary>
        /// Private backing field for PanelElements property
        /// </summary>
        private bool _PanelElements = true;

        /// <summary>
        /// Read/Write Panel Elements?
        /// </summary>
        [AutoUI(13, ToolTip = "Read/Write Panel Element data?")]
        public bool PanelElements
        {
            get { return _PanelElements; }
            set { ChangeProperty(ref _PanelElements, value, "PanelElements"); }
        }

        

        #region Constructors

        /// <summary>
        /// Initialise a new RobotConversionOptions with the default values
        /// </summary>
        public RobotConversionOptions()
        {

        }

        /// <summary>
        /// Initialise a new RobotConversionOptions set to update objects
        /// modified since the specified date
        /// </summary>
        /// <param name="updateSince"></param>
        public RobotConversionOptions(DateTime updateSince)
        {
            UpdateSince = updateSince;
        }

        #endregion
    }
}
