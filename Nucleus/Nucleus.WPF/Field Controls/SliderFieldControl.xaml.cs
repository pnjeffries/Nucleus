using Nucleus.Geometry;
using Nucleus.WPF.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for SliderFieldControl.xaml
    /// </summary>
    public partial class SliderFieldControl : FieldControl
    {
        #region Properties

        /// <summary>
        /// Minumum dependency property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty
            = DependencyProperty.Register("Minimum", typeof(double), typeof(SliderFieldControl), new PropertyMetadata(0.0));

        /// <summary>
        /// The minimum value of the slider
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Maximum dependency property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty
            = DependencyProperty.Register("Maximum", typeof(double), typeof(SliderFieldControl), new PropertyMetadata(100.0));

        /// <summary>
        /// The maximum value of the slider
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Tick frequency dependency property
        /// </summary>
        public static readonly DependencyProperty TickFrequencyProperty
            = DependencyProperty.Register("TickFrequency", typeof(double), typeof(SliderFieldControl), new PropertyMetadata(0.1));

        /// <summary>
        /// The slider tick frequency
        /// </summary>
        public double TickFrequency
        {
            get { return (double)GetValue(TickFrequencyProperty); }
            set { SetValue(TickFrequencyProperty, value); }
        }

        /// <summary>
        /// IsSnapToTickEnabled dependency property
        /// </summary>
        public static readonly DependencyProperty IsSnapToTickEnabledProperty
            = DependencyProperty.Register("IsSnapToTickEnabled", typeof(bool), typeof(SliderFieldControl), new PropertyMetadata(true));

        /// <summary>
        /// Should the slider snap to the nearest tick?
        /// </summary>
        public bool IsSnapToTickEnabled
        {
            get { return (bool)GetValue(IsSnapToTickEnabledProperty); }
            set { SetValue(IsSnapToTickEnabledProperty, value); }
        }

        /// <summary>
        /// Tick placement dependency property
        /// </summary>
        public static readonly DependencyProperty TickPlacementProperty
            = DependencyProperty.Register("TickPlacement", typeof(TickPlacement), typeof(SliderFieldControl), new PropertyMetadata(TickPlacement.None));

        /// <summary>
        /// Slider tick placement
        /// </summary>
        public TickPlacement TickPlacement
        {
            get { return (TickPlacement)GetValue(TickPlacementProperty); }
            set { SetValue(TickPlacementProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SliderFieldControl()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }

        /// <summary>
        /// Minimum, maximum constructor
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public SliderFieldControl(double minimum, double maximum) : this()
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Minimum, maximum, tick frequency constructor
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="tickFrequency"></param>
        public SliderFieldControl(double minimum, double maximum, double tickFrequency) : this(minimum, maximum)
        {
            TickFrequency = tickFrequency;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set up this control to display the specified object property
        /// </summary>
        /// <param name="property"></param>
        public override void AdaptTo(PropertyInfo property)
        {
            base.AdaptTo(property);
            if (property.PropertyType == typeof(Angle))
            {
                var binding = new Binding(property.Name);
                binding.Converter = new RadiansDegreesConverter();
                SetBinding(ValueProperty, binding);
                Units = "°";
                Maximum = 360;
                Minimum = 0;
            }
            else if (property.PropertyType == typeof(int))
            {
                Maximum = 10;
                Minimum = 0;
                TickFrequency = 1;
                IsSnapToTickEnabled = true;
                var binding = new Binding(property.Name);
                binding.Converter = new IntConverter();
                SetBinding(ValueProperty, binding);
            }
        }

        #endregion

    }
}
