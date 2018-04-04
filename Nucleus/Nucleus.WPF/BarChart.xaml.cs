using Nucleus.Base;
using Nucleus.Maths;
using Nucleus.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    /// Interaction logic for BarChart.xaml
    /// </summary>
    public partial class BarChart : UserControl
    {
        #region Properties

        /// <summary>
        /// Static callback function when data property changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static object OnSourceDataCoerced(DependencyObject d, object baseValue)
        {
            ((BarChart)d).WatchSource(baseValue);
            ((BarChart)d).Refresh();
            return baseValue;
        }

        /// <summary>
        /// Static callback function when data property changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSourceDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BarChart)d).Refresh();
        }

        /// <summary>
        /// Data dependency property
        /// </summary>
        public static DependencyProperty SourceDataProperty =
            DependencyProperty.Register("SourceData", typeof(INamedDataSetCollection), typeof(BarChart),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnSourceDataChanged), new CoerceValueCallback(OnSourceDataCoerced)));

        /// <summary>
        /// The data to display.  Should be a collection of NamedDataSets or derivative types
        /// </summary>
        public INamedDataSetCollection SourceData
        {
            get { return (INamedDataSetCollection)GetValue(SourceDataProperty); }
            set { SetValue(SourceDataProperty, value); }
        }

        /// <summary>
        /// Bar Brushes dependency property
        /// </summary>
        public static DependencyProperty BarBrushesProperty =
            DependencyProperty.Register("BarBrushes", typeof(IDictionary), typeof(BarChart),
                new FrameworkPropertyMetadata(new ObservablePairCollection<string, object>()));

        /// <summary>
        /// The set of colours or brushes used to paint the bar segments.
        /// The dictionary may contain WPF Brushes and Colors and Nucleus DisplayBrushes and Colours,
        /// keyed by a string.
        /// The dictionary may be automatically populated when required to display data keyed by
        /// values not already included in the dictionary.
        /// </summary>
        public IDictionary BarBrushes
        {
            get { return (IDictionary)GetValue(BarBrushesProperty); }
            set { SetValue(BarBrushesProperty, value); }
        }

        /// <summary>
        /// Shading dependency property
        /// </summary>
        public static DependencyProperty ShadingProperty =
            DependencyProperty.Register("Shading", typeof(double), typeof(BarChart),
                new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// The strength of shading on the bars
        /// </summary>
        public double Shading
        {
            get { return (double)GetValue(ShadingProperty); }
            set { SetValue(ShadingProperty, value); }
        }

        #endregion

        #region Constructor

        public BarChart()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }

        #endregion

        #region Methods

        private void WatchSource(object newValue)
        {
            INamedDataSetCollection dColl = SourceData;
            if (dColl != null && dColl is INotifyCollectionChanged)
            {
                INotifyCollectionChanged nColl = (INotifyCollectionChanged)dColl;
                nColl.CollectionChanged -= SourceData_CollectionChanged;
            }
            if (newValue != null && newValue is INotifyCollectionChanged)
            {
                INotifyCollectionChanged nColl = (INotifyCollectionChanged)newValue;
                nColl.CollectionChanged += SourceData_CollectionChanged;
            }
        }

        private void SourceData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Refresh and redraw the diagram
        /// </summary>
        public void Refresh()
        {
            //foreach (NamedDataSet dataSet in SourceData)
            //{
            //    Polygon pgon = new Polygon();
            //    Color color = ToWPF.Convert(dataSet.Colour.CapBrightness(ColourBrightnessCap));
            //    // TODO?
            //}
        }

        #endregion
    }
}
