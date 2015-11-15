using System;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Neumann.HelpFrame
{
    /// <summary>
    /// Provides methods for getting the position of an element.
    /// </summary>
    public static class PositionHelper
    {
        /// <summary>
        /// Gets the position of the given element, relative to the given parent element.
        /// </summary>
        /// <param name="element">UIElement instance.</param>
        /// <param name="parent">Parent element.</param>
        /// <returns>Point structure.</returns>
        public static Point GetPosition(this UIElement element, UIElement parent)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            var ttv = element.TransformToVisual(parent);
            return ttv.TransformPoint(new Point(0, 0));
        }
    }
}
