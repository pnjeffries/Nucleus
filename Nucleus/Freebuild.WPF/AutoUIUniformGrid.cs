using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Nucleus.WPF
{
    /// <summary>
    /// A uniform grid that automatically populates itself for members of its datacontext
    /// marked with an AutoUIAttribute
    /// </summary>
    public class AutoUIUniformGrid : UniformGrid
    {
        #region Properties

        public static readonly DependencyProperty MyDataContextProperty =
        DependencyProperty.Register("MyDataContext",
                                    typeof(Object),
                                    typeof(AutoUIUniformGrid),
                                    new PropertyMetadata(MyDataContextChanged));

        private static void MyDataContextChanged(
            object sender,
            DependencyPropertyChangedEventArgs e)
        {
            AutoUIUniformGrid myControl = (AutoUIUniformGrid)sender;
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
        public AutoUIUniformGrid() : base()
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
