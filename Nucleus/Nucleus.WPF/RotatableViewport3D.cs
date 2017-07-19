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

        private Point _StartScreenPosition;

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


        //private FrameworkElement _TrackPad = null;

        /// <summary>
        /// The FrameworkElement the mouse events of which will be tracked and used to control
        /// the camera
        /// </summary>
        public FrameworkElement TrackPad
        {
            get { return (FrameworkElement)GetValue(TrackPadProperty); }
            set { SetValue(TrackPadProperty, value); }
        }

        private ScaleTransform3D _CameraZoom = new ScaleTransform3D();
        private AxisAngleRotation3D _CameraRotation = new AxisAngleRotation3D();
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
                    _CameraTransform.Children.Add(new RotateTransform3D(_CameraRotation));
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
                oldValue.MouseDown -= _TrackPad_MouseDown;
                oldValue.MouseUp -= _TrackPad_MouseUp;
                oldValue.MouseMove -= _TrackPad_MouseMove;
            }

            if (newValue != null)
            {
                // Add event handlers
                newValue.MouseDown += _TrackPad_MouseDown;
                newValue.MouseUp += _TrackPad_MouseUp;
                newValue.MouseMove += _TrackPad_MouseMove;
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
                Quaternion orientation = new Quaternion(_CameraRotation.Axis, _CameraRotation.Angle);

                orientation *= movement;

                _CameraRotation.Axis = orientation.Axis;
                _CameraRotation.Angle = orientation.Angle;

                
            }
        }

        #endregion

        #region Event Handlers

        private void _TrackPad_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point screenPosition = e.GetPosition(TrackPad);
                Vector3D position3D = ScreenTo3D(screenPosition);


                RotateView(position3D);

                _StartScreenPosition = screenPosition;
                _Start3DPosition = position3D;
            }
            //TODO: Zooming
        }

        private void _TrackPad_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Release the mouse capture
            Mouse.Capture(TrackPad, CaptureMode.None);
        }

        private void _TrackPad_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Mouse.Capture(TrackPad, CaptureMode.Element);
            // Store the starting position:
            _StartScreenPosition = e.GetPosition(TrackPad);
            _Start3DPosition = ScreenTo3D(_StartScreenPosition);
        }

        #endregion
    }
}
