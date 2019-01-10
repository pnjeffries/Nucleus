using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Conversion
{
    /// <summary>
    /// Conversion options for whole-model writing
    /// </summary>
    [Serializable]
    public abstract class ModelConversionOptions : ConversionOptions
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
            set { _UpdateSince = value; NotifyPropertyChanged("UpdateSince"); }
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
        /// Private backing field for Levels property
        /// </summary>
        private bool _Levels = true;

        /// <summary>
        /// Read/Write Levels?
        /// </summary>
        [AutoUI(10,
            ToolTip = "Read/Write Level data?")]
        public bool Levels
        {
            get { return _Levels; }
            set { ChangeProperty(ref _Levels, value, "Levels"); }
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
        [AutoUI(13, ToolTip = "Read/Write Linear Element data?")]
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
        [AutoUI(14, ToolTip = "Read/Write Panel Element data?")]
        public bool PanelElements
        {
            get { return _PanelElements; }
            set { ChangeProperty(ref _PanelElements, value, "PanelElements"); }
        }

        /// <summary>
        /// Private backing field for Constraints property
        /// </summary>
        private bool _Constraints = true;

        /// <summary>
        /// Read/Write Rigid Constraints?
        /// </summary>
        [AutoUI(15, ToolTip = "Read/Write Nodal Rigid Constraints?")]
        public bool Constraints
        {
            get { return _Constraints; }
            set { ChangeProperty(ref _Constraints, value, "Constraints"); }
        }

        /// <summary>
        /// Private backing field for Sets property
        /// </summary>
        private bool _Sets = true;

        /// <summary>
        /// Read/Write Sets?
        /// </summary>
        [AutoUI(17, ToolTip = "Read/Write Saved Selection Sets?")]
        public bool Sets
        {
            get { return _Sets; }
            set { ChangeProperty(ref _Sets, value, "Sets"); }
        }

        /// <summary>
        /// Private backing field for Loading property
        /// </summary>
        private bool _Loading = true;

        /// <summary>
        /// Read/Write Loading Data?
        /// </summary>
        [AutoUI(20, ToolTip = "Read/Write Loading Data?")]
        public bool Loading
        {
            get { return _Loading; }
            set { ChangeProperty(ref _Loading, value, "Loading"); }
        }
    }
}
