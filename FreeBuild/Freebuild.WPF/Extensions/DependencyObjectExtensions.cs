using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FreeBuild.WPF
{
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Get a child UIElement by its Uid
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static UIElement FindByUid(this DependencyObject parent, string uid)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            if (count == 0) return null;

            for (int i = 0; i < count; i++)
            {
                var el = VisualTreeHelper.GetChild(parent, i) as UIElement;
                if (el == null) continue;

                if (el.Uid == uid) return el;

                el = el.FindByUid(uid);
                if (el != null) return el;
            }
            return null;
        }

        /// <summary>
        /// Get a child UIElement by its Uid
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static T FindByType<T>(this DependencyObject parent)
            where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            if (count == 0) return null;

            for (int i = 0; i < count; i++)
            {
                var el = VisualTreeHelper.GetChild(parent, i);
                if (el == null) continue;

                if (el is T) return (T)el;

                el = el.FindByType<T>();
                if (el != null) return (T)el;
            }
            return null;
        }
    }
}
