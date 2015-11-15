using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Neumann.HelpFrame
{
    /// <summary>
    /// Provides a half-transparent layer that is integrated on top of the page.
    /// </summary>
    public class HelpLayer : ContentControl
    {

        #region Private Fields

        private Panel _container;
        internal List<HelpFrame> _frames;
        private bool _isInitialized;
        internal bool _isHiding;
        private Brush _originalBackground;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        public HelpLayer()
        {
            this.DefaultStyleKey = typeof(HelpLayer);
            _frames = new List<HelpFrame>();
            this.PointerPressed += this.OnPointerPressed;
            this.ShowHalfTransparentBackground = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the UI element that controlls the display of the layer via attached properties.
        /// </summary>
        internal FrameworkElement HelpFrameDisplayController { get; set; }

        /// <summary>
        /// Gets/sets a value indicating if the background of the layer is shown half-transparent.
        /// </summary>
        internal bool ShowHalfTransparentBackground { get; set; }

        #endregion

        #region Overrides

        /// <summary>
        /// Is called after the layer is initialized.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _container = this.GetTemplateChild("PART_Container") as Panel;
            this.Visibility = Visibility.Collapsed;
            _originalBackground = this.Background;
        }

        #endregion

        #region Internal Methods

        #region Show

        /// <summary>
        /// Displays the layer.
        /// </summary>
        internal void Show()
        {
            if (_container != null && !_isInitialized)
            {
                // cache the frames
                foreach (var frame in _frames)
                {
                    _container.Children.Add(frame);
                }
                _isInitialized = true;
            }

            // show/hide the background of the layer
            if (this.ShowHalfTransparentBackground)
            {
                if (_originalBackground != null)
                {
                    this.Background = _originalBackground;
                }
                VisualStateManager.GoToState(this, "Attached", true);
            }
            else
            {
                this.Background = new SolidColorBrush(Colors.Transparent);
                VisualStateManager.GoToState(this, "Attached", true);
            }
        }

        #endregion

        #region Hide

        /// <summary>
        /// Hides the layer.
        /// </summary>
        internal void Hide()
        {
            _isHiding = true;
            var storyboard = VisualStateHelpers.GetStoryboard("Attached", "Detached", _container);
            if (storyboard != null)
            {
                storyboard.Completed += this.OnDetached;
            }
            VisualStateManager.GoToState(this, "Detached", true);
        }

        #endregion

        #region AttachToPage

        /// <summary>
        /// Attaches the layer to the root layout panel of the page.
        /// </summary>
        /// <param name="panel">Panel instance to attach to.</param>
        internal void AttachToPage(Panel panel)
        {
            if (panel == null) throw new ArgumentNullException("panel");
            this.SetValue(Grid.ColumnSpanProperty, 100);
            this.SetValue(Grid.RowSpanProperty, 100);
            panel.Children.Add(this);
        }

        #endregion

        #region DetachFromPage

        /// <summary>
        /// Detaches the layer from the root layout panel of the page.
        /// </summary>
        /// <param name="panel">Panel instance to detach from.</param>
        internal void DetachFromPage(Panel panel)
        {
            if (panel == null) throw new ArgumentNullException("panel");
            VisualStateManager.GoToState(this, "Detached", true);
            panel.Children.Remove(this);
        }

        #endregion

        #region AttachLayer

        /// <summary>
        /// Attaches the given frame to the layer.
        /// </summary>
        /// <param name="frame">HelpFrame instance to attach.</param>
        internal void AttachFrame(HelpFrame frame)
        {
            _frames.Add(frame);
        }

        #endregion

        #region DetachLayer

        /// <summary>
        /// Detaches the given frame from the layer.
        /// </summary>
        /// <param name="frame">HelpFrame instance to detach.</param>
        internal void DetachFrame(HelpFrame frame)
        {
            _frames.Remove(frame);
            if (_container != null)
            {
                _container.Children.Remove(frame);
            }
        }

        #endregion

        #region GetFrameFromElement

        /// <summary>
        /// Gets the associated HelpFrame instance from the given element.
        /// </summary>
        /// <param name="element">FrameworkElement instance.</param>
        /// <returns>Associated HelpFrame instance of the element or null.</returns>
        internal HelpFrame GetFrameFromElement(FrameworkElement element)
        {
            foreach (var frame in _frames)
            {
                if (frame._attachedElement.Equals(element))
                {
                    return frame;
                }
            }
            return null;
        }

        #endregion

        #endregion

        #region Event Handling

        /// <summary>
        /// Is called after the user clicks on the layer.
        /// </summary>
        /// <param name="sender">HelpLayer instance.</param>
        /// <param name="e">PointerRoutedEventArgs instance.</param>
        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // Hide the layer.
            this.Hide();
        }

        /// <summary>
        /// Is called after the detach animation is finished.
        /// </summary>
        /// <param name="sender">Storyboard instance.</param>
        /// <param name="e">Arguments.</param>
        private void OnDetached(object sender, object e)
        {
            // Hide layer and set ShowHelp attached property to false.
            this.Visibility = Visibility.Collapsed;
            if (this.HelpFrameDisplayController != null)
            {
                this.HelpFrameDisplayController.SetValue(HelpFrame.ShowHelpProperty, false);
            }
            _isHiding = false;
        }

        #endregion

    }
}
