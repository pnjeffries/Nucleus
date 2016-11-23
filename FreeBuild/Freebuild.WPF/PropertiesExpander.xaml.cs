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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FreeBuild.WPF
{
    /// <summary>
    /// Interaction logic for SidebarExpanderControl.xaml
    /// </summary>
    [ContentProperty("BodyContent")]
    public partial class PropertiesExpander : UserControl
    {
        /// <summary>
        /// Header Icon Dependency Property
        /// </summary>
        public static readonly DependencyProperty HeaderIconProperty
            = DependencyProperty.Register("HeaderIcon", typeof(ImageSource), typeof(PropertiesExpander));

        /// <summary>
        /// The image source to be used as an icon in the header of this control
        /// </summary>
        public ImageSource HeaderIcon
        {
            get { return (ImageSource)GetValue(HeaderIconProperty); }
            set { SetValue(HeaderIconProperty, value); }
        }

        /// <summary>
        /// Content hosted in the header of the control dependency property
        /// </summary>
        public static readonly DependencyProperty HeaderContentProperty
            = DependencyProperty.Register("HeaderContent", typeof(object), typeof(PropertiesExpander));

        /// <summary>
        /// Content hosted in the header of the control
        /// </summary>
        public object HeaderContent
        {
            get { return GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        /// <summary>
        /// Content hosted in the expandable portion of the control dependency property
        /// </summary>
        public static readonly DependencyProperty BodyContentProperty
            = DependencyProperty.Register("BodyContent", typeof(object), typeof(PropertiesExpander));

        /// <summary>
        /// Content hosted in the expandable portion of the control
        /// </summary>
        public object BodyContent
        {
            get { return GetValue(BodyContentProperty); }
            set { SetValue(BodyContentProperty, value); }
        }

        public PropertiesExpander()
        {
            InitializeComponent();
            //LayoutRoot.DataContext = this;
        }
    }
}
