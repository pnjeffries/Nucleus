using Nucleus.Alerts;
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
using System.Windows.Shapes;

namespace Nucleus.WPF
{
    /// <summary>
    /// Interaction logic for AlertsLogDialog.xaml
    /// </summary>
    public partial class AlertsLogDialog : Window
    {
        public AlertsLogDialog(AlertLog log)
        {
            InitializeComponent();

            this.DataContext = log;
        }
    }
}
