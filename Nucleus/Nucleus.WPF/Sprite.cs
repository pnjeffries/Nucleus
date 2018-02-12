using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using Nucleus.Rendering;
using System.Windows.Media;
using SWM = System.Windows.Media;
using System.Windows;
using Nucleus.Geometry;

namespace Nucleus.WPF
{
    /// <summary>
    /// A WPF control which represents an animated 2D Sprite
    /// </summary>
    public class Sprite : Shape
    {
        #region Fields

        private Rect _Rectangle = Rect.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Called when the value of a visually-important sprite dependency property is changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Sprite)d).AnimationChanged();
        }

        /// <summary>
        /// Called when the value of a visually-important sprite dependency property is changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Sprite)d).VisualChanged();
        }

        /// <summary>
        /// Animation name Dependency Property
        /// </summary>
        public static DependencyProperty AnimationProperty =
            DependencyProperty.Register("Animation", typeof(string), typeof(Sprite),
                new FrameworkPropertyMetadata("Idle", FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback(OnAnimationChanged)));

        /// <summary>
        /// The name of the current animation
        /// </summary>
        public string Animation
        {
            get { return GetValue(AnimationProperty).ToString(); }
            set { SetValue(AnimationProperty, value); }
        }

        /// <summary>
        /// Animation progress dependency property
        /// </summary>
        public static DependencyProperty AnimationProgressProperty =
            DependencyProperty.Register("AnimationProgress", typeof(double), typeof(Sprite),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback(OnVisualChanged)));

        /// <summary>
        /// The progress of the current animation
        /// </summary>
        public double AnimationProgress
        {
            get { return (double)GetValue(AnimationProgressProperty); }
            set { SetValue(AnimationProgressProperty, value); }
        }

        /// <summary>
        /// Orientaion Dependency Property
        /// </summary>
        public static DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Angle), typeof(Sprite),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnVisualChanged)));

        /// <summary>
        /// The orientation of the sprite.
        /// </summary>
        public Angle Orientation
        {
            get { return (Angle)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }


        /// <summary>
        /// Private backing member variable for the SpriteData property
        /// </summary>
        private SpriteData _SpriteData = null;

        /// <summary>
        /// The sprite data which defines the animations of this Sprite
        /// </summary>
        public SpriteData SpriteData
        {
            get { return _SpriteData; }
            set { _SpriteData = value; }
        }

        public override System.Windows.Media.Geometry RenderedGeometry
        {
            get { return new RectangleGeometry(_Rectangle); }
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get { return new RectangleGeometry(_Rectangle); }
        }

        public override SWM.Transform GeometryTransform => SWM.Transform.Identity;

        #endregion

        #region Constructors

        public Sprite() { }

        static Sprite()
        {
            StretchProperty.OverrideMetadata(typeof(Sprite), new FrameworkPropertyMetadata(Stretch.Fill));
            WidthProperty.OverrideMetadata(typeof(Sprite), new FrameworkPropertyMetadata(1));
            HeightProperty.OverrideMetadata(typeof(Sprite), new FrameworkPropertyMetadata(1));
        }

        #endregion

        #region Methods

        protected void VisualChanged()
        {
            Fill = null;
        }

        protected void AnimationChanged()
        {
            Fill = null;
            AnimationProgress = 0;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Fill == null && SpriteData != null)
            {
                ToWPF.Convert(SpriteData.GetFrame(Animation, Orientation, AnimationProgress));
            }
            drawingContext.DrawRectangle(Fill, null, _Rectangle);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (Stretch == Stretch.UniformToFill)
            {
                double width = constraint.Width;
                double height = constraint.Height;

                if (Double.IsInfinity(width) && Double.IsInfinity(height))
                {
                    return new Size();
                }
                else if (Double.IsInfinity(width) || Double.IsInfinity(height))
                {
                    width = Math.Min(width, height);
                }
                else
                {
                    width = Math.Max(width, height);
                }

                return new Size(width, width);
            }

            return new Size();
        }


        protected override Size ArrangeOverride(Size finalSize)
        {


            _Rectangle = new Rect(0, 0, 
                Math.Max(0, finalSize.Width), Math.Max(0, finalSize.Height));

            switch (Stretch)
            {
                case Stretch.None:
                    _Rectangle.Width = _Rectangle.Height = 0;
                    break;

                case Stretch.Fill:
                    break;

                case Stretch.Uniform:
                    if (_Rectangle.Width > _Rectangle.Height)
                    {
                        _Rectangle.Width = _Rectangle.Height;
                    }
                    else
                    {
                        _Rectangle.Height = _Rectangle.Width;
                    }
                    break;

                case Stretch.UniformToFill:

                    // The minimal square that fills the final box
                    if (_Rectangle.Width < _Rectangle.Height)
                    {
                        _Rectangle.Width = _Rectangle.Height;
                    }
                    else  // _rect.Width >= _rect.Height
                    {
                        _Rectangle.Height = _Rectangle.Width;
                    }
                    break;
            }


            //ResetRenderedGeometry();

            return finalSize;
        }

        #endregion
    }
}
