using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FreeBuild.OpenTK
{
    /// <summary>
    /// Interaction logic for RenderViewportControl.xaml
    /// </summary>
    public partial class RenderViewportControl : UserControl
    {
        #region Constructor

        public RenderViewportControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invalidate the render area
        /// </summary>
        public void Refresh()
        {
            RenderArea.Invalidate();
        }

        #endregion

        private void RenderControl_Load(object sender, EventArgs e)
        {

        }
    }
}
