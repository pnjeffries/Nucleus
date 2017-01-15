using FreeBuild.Extensions;
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
    /// Interaction logic for ViewScroller.xaml
    /// </summary>
    public partial class ViewScroller : UserControl
    {
        #region Fields

        /// <summary>
        /// Drag scrolling toggle
        /// </summary>
        private bool _DragScrolling = false;

        /// <summary>
        /// The starting drag-scrolling point
        /// </summary>
        private Point _DragScrollPoint;

        #endregion

        #region Properties

        /// <summary>
        /// ZoomLevel dependency property
        /// </summary>
        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register("ZoomLevel", typeof(double), typeof(ViewScroller),
                new PropertyMetadata(1.0));
            
        /// <summary>
        /// Gets or sets the zoom level of this view scroller
        /// </summary>
        public double ZoomLevel
        {
            get { return (double)GetValue(ZoomLevelProperty); }
            set
            {
                SetValue(ZoomLevelProperty, value);
                //NotifyPropertyChanged necessary? 
            }
        }

        #endregion

        #region Constructors

        public ViewScroller()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scroller = sender as ScrollViewer;
            e.Handled = true;
            double dZ = 1.1.Power(e.Delta/60);
            double oldValue = ZoomLevel;

            ZoomLevel *= dZ;

            double relZC = (ZoomLevel) / oldValue;

            Point cursorPos = Mouse.GetPosition(scroller);

            //scroller.ScrollToHorizontalOffset(scroller.HorizontalOffset + cursorPos.X - scroller.ViewportWidth * relZC / 2);
            //scroller.ScrollToVerticalOffset(scroller.VerticalOffset + cursorPos.Y - scroller.ViewportHeight * relZC / 2);

            scroller.ScrollToHorizontalOffset(scroller.HorizontalOffset * relZC - (1 - relZC) * cursorPos.X);
            scroller.ScrollToVerticalOffset(scroller.VerticalOffset * relZC - (1 - relZC) * cursorPos.Y);
        }

        private void ScrollViewer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                ScrollViewer scroller = sender as ScrollViewer;
                _DragScrolling = true;
                _DragScrollPoint = e.GetPosition(scroller);
                scroller.Cursor = Cursors.ScrollAll;
                Mouse.Capture(scroller);
            }
        }

        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (_DragScrolling)
            {
                ScrollViewer scroller = sender as ScrollViewer;
                Point posNow = e.GetPosition(scroller);
                scroller.ScrollToHorizontalOffset(scroller.HorizontalOffset - (posNow.X - _DragScrollPoint.X));
                scroller.ScrollToVerticalOffset(scroller.VerticalOffset - (posNow.Y - _DragScrollPoint.Y));
                _DragScrollPoint = posNow;
            }
        }

        private void ScrollViewer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right && _DragScrolling)
            {
                ScrollViewer scroller = sender as ScrollViewer;
                _DragScrolling = false;
                scroller.Cursor = null;
                scroller.ReleaseMouseCapture();
            }
        }

        #endregion
    }
}
