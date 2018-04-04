using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Nucleus.WPF
{
    /// <summary>
    /// A custom Viewbox that will only scale the vertical component of a control
    /// </summary>
    public class VerticalViewbox : Panel
    {
        private double _Scale;

        protected override Size MeasureOverride(Size availableSize)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                availableSize = new Size(200, 200);

            double height = 0;
            Size unlimitedSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
            foreach (UIElement child in Children)
            {
                child.Measure(unlimitedSize);
                height += child.DesiredSize.Height;
            }
            _Scale = availableSize.Height / height;

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Transform scaleTransform = new ScaleTransform(1, _Scale);
            double height = 0;
            foreach (UIElement child in Children)
            {
                child.RenderTransform = scaleTransform;
                child.Arrange(new Rect(new Point(0, _Scale * height), new Size(finalSize.Width, child.DesiredSize.Height)));
                height += child.DesiredSize.Height;
            }

            return finalSize;
        }
    }
}
