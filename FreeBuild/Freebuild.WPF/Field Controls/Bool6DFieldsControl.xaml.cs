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
    /// Interaction logic for Bool6DFieldsControl.xaml
    /// </summary>
    public partial class Bool6DFieldsControl : LabelledControl
    {
        /// <summary>
        /// ValueChanged event
        /// </summary>
        public event PropertyChangedCallback XValueChanged;

        /// <summary>
        /// Raise a ValueChanged event on this control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public void RaiseXValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertyChangedCallback handler = XValueChanged;
            if (handler != null) handler(d, e);
        }

        /// <summary>
        /// Static callback function to raise a ValueChanged event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnXValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Bool6DFieldsControl)d).RaiseXValueChanged(d, e);
        }

        /// <summary>
        /// Value dependency property
        /// </summary>
        public static readonly DependencyProperty XValueProperty
            = DependencyProperty.Register("XValue", typeof(object), typeof(Bool6DFieldsControl),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnXValueChanged)));

        /// <summary>
        /// The value displayed in the textbox and slider
        /// </summary>
        public object XValue
        {
            get { return GetValue(XValueProperty); }
            set { SetValue(XValueProperty, value); }
        }

        /// <summary>
        /// ValueChanged event
        /// </summary>
        public event PropertyChangedCallback YValueChanged;

        /// <summary>
        /// Raise a ValueChanged event on this control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public void RaiseYValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertyChangedCallback handler = YValueChanged;
            if (handler != null) handler(d, e);
        }

        /// <summary>
        /// Static callback function to raise a ValueChanged event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnYValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Bool6DFieldsControl)d).RaiseYValueChanged(d, e);
        }

        /// <summary>
        /// Value dependency property
        /// </summary>
        public static readonly DependencyProperty YValueProperty
            = DependencyProperty.Register("YValue", typeof(object), typeof(Bool6DFieldsControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnYValueChanged)));

        /// <summary>
        /// The value displayed in the textbox and slider
        /// </summary>
        public object YValue
        {
            get { return GetValue(YValueProperty); }
            set { SetValue(YValueProperty, value); }
        }

        /// <summary>
        /// ValueChanged event
        /// </summary>
        public event PropertyChangedCallback ZValueChanged;

        /// <summary>
        /// Raise a ValueChanged event on this control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public void RaiseZValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertyChangedCallback handler = ZValueChanged;
            if (handler != null) handler(d, e);
        }

        /// <summary>
        /// Static callback function to raise a ValueChanged event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnZValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Bool6DFieldsControl)d).RaiseZValueChanged(d, e);
        }

        /// <summary>
        /// Value dependency property
        /// </summary>
        public static readonly DependencyProperty ZValueProperty
            = DependencyProperty.Register("ZValue", typeof(object), typeof(Bool6DFieldsControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnYValueChanged)));

        /// <summary>
        /// The value displayed in the textbox and slider
        /// </summary>
        public object ZValue
        {
            get { return GetValue(ZValueProperty); }
            set { SetValue(ZValueProperty, value); }
        }

        /// <summary>
        /// ValueChanged event
        /// </summary>
        public event PropertyChangedCallback XXValueChanged;

        /// <summary>
        /// Raise a ValueChanged event on this control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public void RaiseXXValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertyChangedCallback handler = XXValueChanged;
            if (handler != null) handler(d, e);
        }

        /// <summary>
        /// Static callback function to raise a ValueChanged event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnXXValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Bool6DFieldsControl)d).RaiseXXValueChanged(d, e);
        }

        /// <summary>
        /// Value dependency property
        /// </summary>
        public static readonly DependencyProperty XXValueProperty
            = DependencyProperty.Register("XXValue", typeof(object), typeof(Bool6DFieldsControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnXXValueChanged)));

        /// <summary>
        /// The value displayed in the textbox and slider
        /// </summary>
        public object XXValue
        {
            get { return GetValue(XXValueProperty); }
            set { SetValue(XXValueProperty, value); }
        }

        /// <summary>
        /// ValueChanged event
        /// </summary>
        public event PropertyChangedCallback YYValueChanged;

        /// <summary>
        /// Raise a ValueChanged event on this control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public void RaiseYYValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertyChangedCallback handler = YYValueChanged;
            if (handler != null) handler(d, e);
        }

        /// <summary>
        /// Static callback function to raise a ValueChanged event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnYYValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Bool6DFieldsControl)d).RaiseYYValueChanged(d, e);
        }

        /// <summary>
        /// Value dependency property
        /// </summary>
        public static readonly DependencyProperty YYValueProperty
            = DependencyProperty.Register("YYValue", typeof(object), typeof(Bool6DFieldsControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnYValueChanged)));

        /// <summary>
        /// The value displayed in the textbox and slider
        /// </summary>
        public object YYValue
        {
            get { return GetValue(YYValueProperty); }
            set { SetValue(YYValueProperty, value); }
        }

        /// <summary>
        /// ValueChanged event
        /// </summary>
        public event PropertyChangedCallback ZZValueChanged;

        /// <summary>
        /// Raise a ValueChanged event on this control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public void RaiseZZValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertyChangedCallback handler = ZZValueChanged;
            if (handler != null) handler(d, e);
        }

        /// <summary>
        /// Static callback function to raise a ValueChanged event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnZZValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Bool6DFieldsControl)d).RaiseZZValueChanged(d, e);
        }

        /// <summary>
        /// Value dependency property
        /// </summary>
        public static readonly DependencyProperty ZZValueProperty
            = DependencyProperty.Register("ZZValue", typeof(object), typeof(Bool6DFieldsControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnYValueChanged)));

        /// <summary>
        /// The value displayed in the textbox and slider
        /// </summary>
        public object ZZValue
        {
            get { return GetValue(ZZValueProperty); }
            set { SetValue(ZZValueProperty, value); }
        }



        public Bool6DFieldsControl()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }
    }
}
