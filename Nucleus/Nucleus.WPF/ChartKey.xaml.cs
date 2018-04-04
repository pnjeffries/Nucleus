using System;
using System.Collections;
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

namespace Nucleus.WPF
{
    /// <summary>
    /// Interaction logic for ChartKey.xaml
    /// </summary>
    public partial class ChartKey : UserControl
    {
        /// <summary>
        /// Bar Brushes dependency property
        /// </summary>
        public static DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IDictionary), typeof(ChartKey),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// The set of colours or brushes used to paint the bar segments.
        /// The dictionary may contain WPF Brushes and Colors and Nucleus DisplayBrushes and Colours,
        /// keyed by a string.
        /// The dictionary may be automatically populated when required to display data keyed by
        /// values not already included in the dictionary.
        /// </summary>
        public IDictionary ItemsSource
        {
            get { return (IDictionary)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public ChartKey()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }
    }
}
