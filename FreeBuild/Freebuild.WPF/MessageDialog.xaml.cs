using FreeBuild.Extensions;
using FreeBuild.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace FreeBuild.WPF
{
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : Window
    {
        #region Properties

        /// <summary>
        /// The value to be returned when this dialog closes
        /// </summary>
        public object ReturnValue { get; set; } = null;

        /// <summary>
        /// The value of the 'Do not show this message again' checkbox
        /// </summary>
        public bool DoNotShowAgain { get; set; } = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new MessageDialog with the specified title displaying the specified messages
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public MessageDialog(string title, params string[] messages)
        {
            InitializeComponent();

            this.Title = title;
            TextBox.Document.Blocks.Clear();
            foreach (string message in messages)
            {
                AddParagraph(message);
            }
        }

        /// <summary>
        /// Initialise a new MessageDialog with the specified title displaying the specified messages
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public MessageDialog(string title, bool showAgainCheckbox, params string[] messages)
        {
            InitializeComponent();

            this.Title = title;
            TextBox.Document.Blocks.Clear();
            foreach (string message in messages)
            {
                AddParagraph(message);
            }
            if (showAgainCheckbox)
            {
                ShowAgainCB.DataContext = this;
                ShowAgainCB.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Initialise a new MessageDialog with the specified title displaying the specified message
        /// and offering the specified set of options
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public MessageDialog(string title, string message, params UIOption[] options)
        {
            InitializeComponent();

            this.Title = title;
            TextBox.Document.Blocks.Clear();
            AddParagraph(message);

            if (options.Length > 0)
            {
                ButtonsPanel.Children.Clear();
                foreach (UIOption option in options)
                {
                    Button button = new Button();
                    button.Content = option.Text;
                    button.Tag = option.ReturnValue;
                    button.Click += OptionButton_Click;
                    ButtonsPanel.Children.Add(button);
                }
            }
        }

        /// <summary>
        /// Initialise a new MessageDialog with the specified title displaying data for the specified exception
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public MessageDialog(string title, Exception ex, string prefaceMessage = null)
        {
            InitializeComponent();

            this.Title = title;
            ErrorIcon.Visibility = Visibility.Visible;
            TextBox.Document.Blocks.Clear();
            if (prefaceMessage != null) TextBox.Document.Blocks.Add(new Paragraph(new Run(prefaceMessage)));
            Exception inEx = ex;
            while (inEx != null)
            {
                Run messageRun = new Run(inEx.Message);
                messageRun.FontWeight = FontWeights.Bold;
                TextBox.Document.Blocks.Add(new Paragraph(messageRun));
                TextBox.Document.Blocks.Add(new Paragraph(new Run(inEx.StackTrace)));
                inEx = inEx.InnerException;
            }
        }

        #endregion

        #region Methods

        private void AddParagraph(string message)
        {
            TextBox.Document.Blocks.Add(message.ToParagraph());
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ReturnValue = button.Tag;
            if (ReturnValue is bool) DialogResult = (bool)ReturnValue;
            Close();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Show a new MessageDialog window
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static void Show(string title, params string[] messages)
        {
            MessageDialog mD = new MessageDialog(title, messages);
            mD.ShowDialog();
        }

        /// <summary>
        /// Show a new MessageDialog window including a 'Do Not Show This Message Again' checkbox
        /// </summary>
        /// <param name="title"></param>
        /// <param name="dontShowAgain">A boolean value that can be used to output 'true' when the
        /// same message should not be shown again.</param>
        /// <param name="message"></param>
        public static void Show(string title, out bool dontShowAgain, params string[] messages)
        {
            MessageDialog mD = new MessageDialog(title, true, messages);
            mD.ShowDialog();
            dontShowAgain = mD.DoNotShowAgain;
        }

        /// <summary>
        /// Show a new MessageDialog exception window
        /// </summary>
        /// <param name="title"></param>
        /// <param name="exception"></param>
        public static void Show(string title, Exception exception, string prefaceMessage = null)
        {
            MessageDialog mD = new MessageDialog(title, exception, prefaceMessage);
            mD.ShowDialog();
        }

        /// <summary>
        /// Show a new MessageDialog window offering the specified set of options.
        /// Returns the ReturnValue of the selected option (if any).
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static object ShowOptions(string title, string message, params UIOption[] options)
        {
            MessageDialog mD = new MessageDialog(title, message, options);
            mD.ShowDialog();
            return mD.ReturnValue;
        }

        #endregion
    }
}
