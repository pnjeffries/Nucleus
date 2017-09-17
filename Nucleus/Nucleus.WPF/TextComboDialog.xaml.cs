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
    /// Interaction logic for TextComboDialog.xaml
    /// </summary>
    public partial class TextComboDialog : Window
    {
        #region Properties

        /// <summary>
        /// Private backing field for Text property
        /// </summary>
        private string _Text;

        /// <summary>
        /// The text entered into the dialog
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        /// <summary>
        /// Private backing field for Suggestions property
        /// </summary>
        private IList<string> _Suggestions = null;

        /// <summary>
        /// The suggested text values that the user can select via the combobox
        /// </summary>
        public IList<string> Suggestions
        {
            get { return _Suggestions; }
            set { _Suggestions = value; }
        }


        #endregion

        #region Constructors

        public TextComboDialog()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;

            Loaded += (sender, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        public TextComboDialog(IList<string> suggestions) : this()
        {
            Suggestions = suggestions;
        }

        public TextComboDialog(string title, IList<string> suggestions) : this(suggestions)
        {
            Title = title;
        }

        #endregion

        #region Event Handlers

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

        #endregion

        #region Static Methods

        /// <summary>
        /// Show a TextComboDialog
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="suggestions"></param>
        /// <returns></returns>
        public static bool? Show(string title, ref string text, IList<string> suggestions = null)
        {
            var dialog = new TextComboDialog(title, suggestions);
            dialog.Text = text;
            var result = dialog.ShowDialog();
            text = dialog.Text;
            return result;
        }


        #endregion

        private void Combo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                Close();
            }
        }
    }
}
