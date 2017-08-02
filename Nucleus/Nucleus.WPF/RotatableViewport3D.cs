using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Nucleus.WPF
{
    /// <summary>
    /// An extended Viewport3D that can be rotated via mouse interaction with a separate
    /// trackpad object (such as a transparent border over the viewport)
    /// </summary>
    public class RotatableViewport3D : Viewport3D
    {
        #region Fields

        private Point _StartScreenPosition = new Point(double.NaN, double.NaN);

        private Vector3D _Start3DPosition = new Vector3D(0, 0, 1);

        #endregion

        #region Properties

        private static void OnTrackPadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RotatableViewport3D)d).RegisterTrackPad((FrameworkElement)e.OldValue, (FrameworkElement)e.NewValue);
        }

        public static readonly DependencyProperty TrackPadProperty =
            DependencyProperty.Register("TrackPad", typeof(FrameworkElement), typeof(RotatableViewport3D),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnTrackPadChanged)));

        /// <summary>
        /// The FrameworkElement the mouse events of which will be tracked and used to control
        /// the camera
        /// </summary>
        public FrameworkElement TrackPad
        {
            get { return (FrameworkElement)GetValue(TrackPadProperty); }
            set { SetValue(TrackPadProperty, value); }
        }

        /*public static readonly DependencyProperty FocalPointProperty =
            DependencyProperty.Register("FocalPoint", typeof(Vector3D), typeof(RotatableViewport3D),
                new FrameworkPropertyMetadata(new Vector3D(), FrameworkPropertyMetadataOptions.None));*/

        private TranslateTransform3D _CameraPosition = new TranslateTransform3D(0, 0, 0);

        /// <summary>
        /// The point on which the camera is focussed
        /// </summary>
        public Vector3D FocalPoint
        {
            get { return new Vector3D(_CameraPosition.OffsetX, _CameraPosition.OffsetY, _CameraPosition.OffsetZ); }
            set
            {
                _CameraPosition.OffsetX = value.X;
                _CameraPosition.OffsetY = value.Y;
                _CameraPosition.OffsetZ = value.Z;
            }
        }

        private ScaleTransform3D _CameraZoom = new ScaleTransform3D(1.7,1.7,1.7);
        private AxisAngleRotation3D _CameraElevation = new AxisAngleRotation3D(new Vector3D(1,0,0), 35);
        private AxisAngleRotation3D _CameraRotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);

        private Transform3DGroup _CameraTransform = null;

        /// <summary>
        /// The transformation which is to be applied to the camera.
        /// Bind the transform of the viewport camera to this property.
        /// </summary>
        public Transform3DGroup CameraTransform
        {
            get
            {
                if (_CameraTransform == null)
                {
                    _CameraTransform = new Transform3DGroup();
                    _CameraTransform.Children.Add(_CameraZoom);
                    _CameraTransform.Children.Add(new RotateTransform3D(_CameraElevation));
                    _CameraTransform.Children.Add(new RotateTransform3D(_CameraRotation));
                    _CameraTransform.Children.Add(_CameraPosition);
                }
                return _CameraTransform;
            }
        }

        #endregion

        #region Methods

        public void RegisterTrackPad(FrameworkElement oldValue, FrameworkElement newValue)
        {
            if (oldValue != null)
            {
                // Remove event handlers
                oldValue.PreviewMouseDown -= _TrackPad_MouseDown;
                oldValue.MouseUp -= _TrackPad_MouseUp;
                oldValue.MouseMove -= _TrackPad_MouseMove;
                oldValue.MouseWheel -= _TrackPad_MouseWheel;
            }

            if (newValue != null)
            {
                // Add event handlers
                newValue.PreviewMouseDown += _TrackPad_MouseDown;
                newValue.MouseUp += _TrackPad_MouseUp;
                newValue.MouseMove += _TrackPad_MouseMove;
                newValue.MouseWheel += _TrackPad_MouseWheel;
            }
        }

        /// <summary>
        /// Transform screen coodinates to spherical 3D coordinates
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Vector3D ScreenTo3D(Point point)
        {
            double x = point.X / (TrackPad.ActualWidth / 2.0) - 1.0;
            double y = 1.0 - point.Y / (TrackPad.ActualHeight/2.0);
            double z = 1.0 - x.Squared() - y.Squared();
            if (z > 0) z = Math.Sqrt(z);
            else z = 0;

            return new Vector3D(x, y, z);
        }

        private void RotateView(Vector3D toPosition)
        {
            if (_Start3DPosition != toPosition)
            {
                // Determine the axis and angle of rotation
                Vector3D axis = Vector3D.CrossProduct(_Start3DPosition, toPosition);
                double angle = Vector3D.AngleBetween(_Start3DPosition, toPosition);
                Quaternion movement = new Quaternion(axis, -angle);

                // Get the current orientation
                Quaternion orientation = new Quaternion(_CameraElevation.Axis, _CameraElevation.Angle);

                orientation *= movement;

                _CameraElevation.Axis = orientation.Axis;
                _CameraElevation.Angle = orientation.Angle;

            }
        }

        private void RotateView(double dX, double dY)
        {
            _CameraRotation.Angle += dX * 180;
            _CameraElevation.Angle += dY * 180;
        }

        private void ZoomView(double dZ)
        {
            double scale = Math.Exp(dZ / 1000);

            _CameraZoom.ScaleX *= scale;
            _CameraZoom.ScaleY *= scale;
            _CameraZoom.ScaleZ *= scale;
        }

        private void PanView(double dX, double dY)
        {
            Vector3D xVector = new Vector3D(1, 0, 0);
            xVector = _CameraTransform.Transform(xVector);
            Vector3D yVector = new Vector3D(0, 1, 0);
            yVector = _CameraTransform.Transform(yVector);

            _CameraPosition.OffsetX += (xVector.X * dX) + (yVector.X * dY);
            _CameraPosition.OffsetY += (xVector.Y * dX) + (yVector.Y * dY);
            _CameraPosition.OffsetZ += (xVector.Z * dX) + (yVector.Z * dY);
        }

        #endregion

        #region Event Handlers

        private void _TrackPad_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed && !double.IsNaN(_StartScreenPosition.X))
            {
                Point screenPosition = e.GetPosition(this);
                Vector3D position3D = ScreenTo3D(screenPosition);

                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    ZoomView((screenPosition.Y - _StartScreenPosition.Y) * 10);
                }
                else if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    PanView(-90 * (screenPosition.X - _StartScreenPosition.X) / ActualWidth, 90 * (screenPosition.Y - _StartScreenPosition.Y) / ActualHeight);
                }
                else
                {
                    RotateView(-(screenPosition.X - _StartScreenPosition.X) / ActualWidth, -(screenPosition.Y - _StartScreenPosition.Y) / ActualHeight);
                    //RotateView(position3D);
                }

                _StartScreenPosition = screenPosition;
                _Start3DPosition = position3D;
            }
            //else e.Handled = false;
            //TODO: Zooming
        }

        private void _TrackPad_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Release the mouse capture
            if (e.ChangedButton == MouseButton.Right)
            {
                Mouse.Capture(TrackPad, CaptureMode.None);
                _StartScreenPosition = new Point(double.NaN, double.NaN);
            }
                
            //else e.Handled = false;
        }

        private void _TrackPad_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                Mouse.Capture(TrackPad, CaptureMode.Element);
                // Store the starting position:
                _StartScreenPosition = e.GetPosition(this);
                _Start3DPosition = ScreenTo3D(_StartScreenPosition);
            }
            //else e.Handled = false;
        }

        private void _TrackPad_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ZoomView(-e.Delta);
        }

        #endregion
    }
}
