using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Neumann.HelpFrame
{
    /// <summary>
    /// Provides methods to get elements from the visual tree.
    /// </summary>
    public static class TreeHelpers
    {
        /// <summary>
        /// Gets the root layout panel from the current page.
        /// </summary>
        /// <returns>Panel instance.</returns>
        public static Panel GetRootPanel()
        {
            var window = Window.Current;
            if (window != null)
            {
                var frame = window.Content as Frame;
                if (frame != null)
                {
                    var page = frame.Content as Page;
                    if (page != null)
                    {
                        return page.Content as Panel;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the current page.
        /// </summary>
        /// <returns>Page instance.</returns>
        public static Page GetCurrentPage()
        {
            var window = Window.Current;
            if (window != null)
            {
                var frame = window.Content as Frame;
                if (frame != null)
                {
                    return frame.Content as Page;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the parent of the given element in the visual tree.
        /// </summary>
        /// <param name="element">UIElement instance.</param>
        /// <returns>Parent UIElement instance.</returns>
        public static UIElement GetParent(this UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            var parent = VisualTreeHelper.GetParent(element);
            if (parent != null)
            {
                var parentElement = parent as UIElement;
                if (parentElement == null)
                {
                    parentElement = parentElement.GetParent();
                }
                return parentElement;
            }
            return null;
        }

        /// <summary>
        /// Gets the parent layout panel of the given element.
        /// </summary>
        /// <param name="element">UIElement instance.</param>
        /// <returns>Panel instance.</returns>
        public static Panel GetParentPanel(this UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            var parent = VisualTreeHelper.GetParent(element);
            if (parent != null)
            {
                var parentPanel = parent as Panel;
                if (parentPanel == null)
                {
                    parentPanel = GetParentPanel(parentPanel);
                }
                return parentPanel;
            }
            return null;
        }
    }
}
