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

namespace FreeBuild.WPF
{
    /// <summary>
    /// Interaction logic for XYZControl.xaml
    /// </summary>
    public partial class XYZControl : LabelledControl
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
            ((XYZControl)d).RaiseXValueChanged(d, e);
        }

        /// <summary>
        /// Value dependency property
        /// </summary>
        public static readonly DependencyProperty XValueProperty
            = DependencyProperty.Register("XValue", typeof(object), typeof(XYZControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
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
            ((XYZControl)d).RaiseYValueChanged(d, e);
        }

        /// <summary>
        /// Value dependency property
        /// </summary>
        public static readonly DependencyProperty YValueProperty
            = DependencyProperty.Register("YValue", typeof(object), typeof(XYZControl),
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
            ((XYZControl)d).RaiseZValueChanged(d, e);
        }

        /// <summary>
        /// Value dependency property
        /// </summary>
        public static readonly DependencyProperty ZValueProperty
            = DependencyProperty.Register("ZValue", typeof(object), typeof(XYZControl),
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


        public XYZControl()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }
    }
}
