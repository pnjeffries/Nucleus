using FreeBuild.Extensions;
using FreeBuild.Geometry;
using FreeBuild.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FB = FreeBuild.Geometry;

namespace FreeBuild.WPF
{
    /// <summary>
    /// A stack panel that automatically generates a UI interface for members of its
    /// datacontext marked with an AutoUI attribute
    /// </summary>
    public class AutoUIStackPanel : StackPanel
    {
        #region Properties

        public static readonly DependencyProperty MyDataContextProperty =
        DependencyProperty.Register("MyDataContext",
                                    typeof(Object),
                                    typeof(AutoUIStackPanel),
                                    new PropertyMetadata(MyDataContextChanged));

        private static void MyDataContextChanged(
            object sender,
            DependencyPropertyChangedEventArgs e)
        {
            AutoUIStackPanel myControl = (AutoUIStackPanel)sender;
            myControl.Refresh();
        }

        public object MyDataContext
        {
            get { return GetValue(MyDataContextProperty); }
            set { SetValue(MyDataContextProperty, value); }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public AutoUIStackPanel() : base()
        {
           Initialise();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialise this panel
        /// </summary>
        protected void Initialise()
        {
            SetBinding(MyDataContextProperty, new Binding());
        }

        /// <summary>
        /// Called when the DataContext is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoPropertiesPanel_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Refresh the fields displayed by this panel
        /// </summary>
        public void Refresh()
        {
            Children.Clear();
            if (DataContext != null)
            {
                Children.GenerateControlsFor(DataContext);
            }
        }       

        #endregion

    }
}
