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

namespace Nucleus.WPF
{
    /// <summary>
    /// Interaction logic for AutoUIDialog.xaml
    /// </summary>
    public partial class AutoUIDialog : Window
    {
        private AutoUIDialog(bool cancelable)
        {
            InitializeComponent();

            if (!cancelable) CancelButton.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Initialise a new AutoUIDialog with the specfied context object
        /// </summary>
        /// <param name="context"></param>
        public AutoUIDialog(object context, bool cancelable = false) : this(cancelable)
        {
            MainPanel.DataContext = context;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Create and show an auto-generated options dialog for the specified
        /// object.  Interface elements will be generated automatically for any AutoUI
        /// tagged properties on the object
        /// </summary>
        /// <param name="title"></param>
        /// <param name="context"></param>
        public static bool? Show(string title, object context, bool cancelable = false)
        {
            var dialog = new AutoUIDialog(context);
            dialog.Title = title;
            return dialog.ShowDialog();
        }

        
    }
}
