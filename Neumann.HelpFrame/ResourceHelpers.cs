using System;
using Windows.UI.Xaml;

namespace Neumann.HelpFrame
{
    /// <summary>
    /// Provides methods for finding resouces inside a project.
    /// </summary>
    public static class ResourceHelpers
    {
        /// <summary>
        /// Gets a resource with the given key.
        /// </summary>
        /// <param name="element">UIElement on which the search should be begin.</param>
        /// <param name="key">Key of the resource.</param>
        /// <returns>Resource or null.</returns>
        public static object FindResource(this FrameworkElement element, object key)
        {
            if (element.Resources.ContainsKey(key))
                return element.Resources[key];
            else
            {
                if (element.Parent != null && element.Parent is FrameworkElement)
                    return ((FrameworkElement)element.Parent).FindResource(key);
                else
                {
                    if (Application.Current.Resources.ContainsKey(key))
                        return Application.Current.Resources[key];
                }
            }
            return null;
        }

        /// <summary>
        /// Gets a resource with the given key from the resource dictionary file with the given URI.
        /// </summary>
        /// <typeparam name="T">Type of the resource.</typeparam>
        /// <param name="dictionaryUri">URI to the resource dictionary file.</param>
        /// <param name="resourceKey">Key of the resource.</param>
        /// <returns>Resource or null.</returns>
        public static T GetResourceFromDictionary<T>(Uri dictionaryUri, string resourceKey)
        {
            var dictionary = new ResourceDictionary() { Source = dictionaryUri };
            if (dictionary != null)
            {
                return (T)dictionary[resourceKey];
            }
            return default(T);
        }
    }
}
