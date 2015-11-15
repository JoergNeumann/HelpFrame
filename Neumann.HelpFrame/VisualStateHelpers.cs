using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Neumann.HelpFrame
{
    /// <summary>
    /// Provides methods to get a Storyboard from a visual state or a visual state transition.
    /// </summary>
    public static class VisualStateHelpers
    {
        /// <summary>
        /// Gets the storyboard from a visual state with the given name from the given container.
        /// </summary>
        /// <param name="visualStateName">Name of the visual state.</param>
        /// <param name="container">FrameworkElement that includes the visual state.</param>
        /// <returns>Storyboard instance or null.</returns>
        public static Storyboard GetStoryboard(string visualStateName, FrameworkElement container)
        {
            var groups = VisualStateManager.GetVisualStateGroups(container);
            foreach (var group in groups)
            {
                foreach (var trans in group.Transitions)
                {
                    if (trans.From == "Attached" && trans.To == "Detached")
                    {
                        return trans.Storyboard;
                    }
                }
                foreach (var state in group.States)
                {
                    if (state.Name == visualStateName)
                    {
                        return state.Storyboard;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the storyboard from a visual state transition with the given names from the given container.
        /// </summary>
        /// <param name="transitionFromName">Name of the 'from' transition.</param>
        /// <param name="transitionToName">Name of the 'to' transition.</param>
        /// <param name="container">FrameworkElement that includes the visual state.</param>
        /// <returns>Storyboard instance or null.</returns>
        public static Storyboard GetStoryboard(string transitionFromName, string transitionToName, FrameworkElement container)
        {
            var groups = VisualStateManager.GetVisualStateGroups(container);
            foreach (var group in groups)
            {
                foreach (var trans in group.Transitions)
                {
                    if (trans.From == transitionFromName && trans.To == transitionToName)
                    {
                        return trans.Storyboard;
                    }
                }
            }
            return null;
        }
    }
}
