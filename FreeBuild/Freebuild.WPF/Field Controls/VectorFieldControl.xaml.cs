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
using FB = FreeBuild.Geometry;

namespace FreeBuild.WPF
{
    /// <summary>
    /// Interaction logic for VectorFieldControl.xaml
    /// </summary>
    public partial class VectorFieldControl : FieldControl
    {
        #region Properties

        /// <summary>
        /// The X-component value
        /// </summary>
        public double XValue
        {
            get { return ((FB.Vector)Value).X; }
            set { Value = ((FB.Vector)Value).WithX(value); }
        }

        /// <summary>
        /// The X-component value
        /// </summary>
        public double YValue
        {
            get { return ((FB.Vector)Value).Y; }
            set { Value = ((FB.Vector)Value).WithY(value); }
        }

        /// <summary>
        /// The X-component value
        /// </summary>
        public double ZValue
        {
            get { return ((FB.Vector)Value).Z; }
            set { Value = ((FB.Vector)Value).WithZ(value); }
        }

        #endregion

        #region Constructors

        public VectorFieldControl()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }

        #endregion
    }
}
