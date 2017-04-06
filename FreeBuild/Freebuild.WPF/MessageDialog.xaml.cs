﻿using System;
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

namespace FreeBuild.WPF
{
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : Window
    {
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
                TextBox.Document.Blocks.Add(new Paragraph(new Run(message)));
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

        private void OK_Click(object sender, RoutedEventArgs e)
        {
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
        /// Show a new MessageDialog exception window
        /// </summary>
        /// <param name="title"></param>
        /// <param name="exception"></param>
        public static void Show(string title, Exception exception, string prefaceMessage = null)
        {
            MessageDialog mD = new MessageDialog(title, exception, prefaceMessage);
            mD.ShowDialog();
        }

        #endregion
    }
}