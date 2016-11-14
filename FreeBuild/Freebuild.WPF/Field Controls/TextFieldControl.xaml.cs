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

namespace Freebuild.WPF
{
    /// <summary>
    /// Interaction logic for TextFieldControl.xaml
    /// </summary>
    public partial class TextFieldControl : FieldControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TextFieldControl() : base()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }
    }
}
