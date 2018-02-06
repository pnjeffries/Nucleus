using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using Nucleus.Rendering;
using System.Windows.Media;
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

        public static DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Angle), typeof(Sprite),
                new PropertyMetadata(0));

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

        public override Transform GeometryTransform => Transform.Identity;

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

        #region Method

        protected override void OnRender(DrawingContext drawingContext)
        {
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
