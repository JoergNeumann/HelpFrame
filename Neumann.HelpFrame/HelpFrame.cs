using System;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Neumann.HelpFrame
{
    /// <summary>
    /// Provides attached properties to show inline help on top of the page.
    /// </summary>
    public class HelpFrame : Control
    {

        #region Private Fields

        private Panel _container;
        private bool _applyingTemplate;
        internal UIElement _attachedElement;
        private FontFamily _originalFontFamily;
        private double _originalFontSize;
        private FontWeight _originalFontWeight;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        public HelpFrame()
        {
            this.DefaultStyleKey = typeof(HelpFrame);
            this.LayoutUpdated += this.OnLayoutUpdated;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Is called after the frame is initialized.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _container = this.GetTemplateChild("PART_Container") as Panel;
            this.ApplyContentTemplate();
        }

        #endregion

        #region Properties

        #region HelpText

        /// <summary>
        /// Gets/sets the help text of the attached element.
        /// </summary>
        public static readonly DependencyProperty HelpTextProperty =
            DependencyProperty.RegisterAttached("HelpText", typeof(string), typeof(HelpFrame),
            new PropertyMetadata(null, OnHelpTextPropertyChanged));

        private static void OnHelpTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element != null)
            {
                var parent = element.GetParent();
                if (parent == null)
                {
                    element.Loaded += OnElementLoaded;
                }
                else
                {
                    AttachFrame(element);
                }
            }
        }

        /// <summary>
        /// Gets the help text of the attached element.
        /// </summary>
        /// <param name="element">UIElement instance.</param>
        /// <returns>Help text.</returns>
        public static string GetHelpText(UIElement element)
        {
            return element.GetValue(HelpFrame.HelpTextProperty) as string;
        }

        /// <summary>
        /// Sets the help text of the attached element.
        /// </summary>
        /// <param name="element">UIElement instance.</param>
        /// <param name="helpText">Help text.</param>
        public static void SetHelpText(UIElement element, string helpText)
        {
            element.SetValue(HelpFrame.HelpTextProperty, helpText);
        }

        #endregion

        #region ShowHelp

        /// <summary>
        /// Gets/sets a value indicating if the help layer is displayed.
        /// </summary>
        public static readonly DependencyProperty ShowHelpProperty =
            DependencyProperty.RegisterAttached("ShowHelp", typeof(bool), typeof(HelpFrame),
            new PropertyMetadata(false, OnShowHelpPropertyChanged));

        private static void OnShowHelpPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignMode.DesignModeEnabled) return;
            var element = d as FrameworkElement;
            if (element != null)
            {
                var parent = element.GetParent();
                if (parent == null)
                {
                    element.Loaded += OnRootElementLoaded;
                }
                else
                {
                    ShowLayer(element);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating if the help layer is displayed.
        /// </summary>
        /// <param name="element">UIElement instance.</param>
        /// <returns>True if the help layer is displayed.</returns>
        public static bool GetShowHelp(UIElement element)
        {
            return (bool)element.GetValue(ShowHelpProperty);
        }

        /// <summary>
        /// Sets a value indicating if the help layer is displayed.
        /// </summary>
        /// <param name="element">UIElement instance.</param>
        /// <param name="value">True if the help layer is displayed.</param>
        public static void SetShowHelp(UIElement element, bool value)
        {
            element.SetValue(ShowHelpProperty, value);
        }

        #endregion

        #region Text

        /// <summary>
        /// Internal dependency property that contains the display text.
        /// </summary>
        private string Text { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }
        /// <summary>
        /// Internal dependency property that contains the display text.
        /// </summary>
        private static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HelpFrame), new PropertyMetadata(null));

        #endregion

        #region TopTemplate

        /// <summary>
        /// Gets/sets the DataTemplate instance to display the help text on top of the associated control.
        /// </summary>
        public DataTemplate TopTemplate { get { return (DataTemplate)GetValue(TopTemplateProperty); } set { SetValue(TopTemplateProperty, value); } }
        /// <summary>
        /// Gets/sets the DataTemplate instance to display the help text on top of the associated control.
        /// </summary>
        public static readonly DependencyProperty TopTemplateProperty =
            DependencyProperty.Register("TopTemplate", typeof(DataTemplate), typeof(HelpFrame),
            new PropertyMetadata(null));

        #endregion

        #region BottomTemplate

        /// <summary>
        /// Gets/sets the DataTemplate instance to display the help text on bottom of the associated control.
        /// </summary>
        public DataTemplate BottomTemplate { get { return (DataTemplate)GetValue(BottomTemplateProperty); } set { SetValue(BottomTemplateProperty, value); } }
        /// <summary>
        /// Gets/sets the DataTemplate instance to display the help text on bottom of the associated control.
        /// </summary>
        public static readonly DependencyProperty BottomTemplateProperty =
            DependencyProperty.Register("BottomTemplate", typeof(DataTemplate), typeof(HelpFrame),
            new PropertyMetadata(null));

        #endregion

        #region LeftTemplate

        /// <summary>
        /// Gets/sets the DataTemplate instance to display the help text on the left of the associated control.
        /// </summary>
        public DataTemplate LeftTemplate { get { return (DataTemplate)GetValue(LeftTemplateProperty); } set { SetValue(LeftTemplateProperty, value); } }
        /// <summary>
        /// Gets/sets the DataTemplate instance to display the help text on the left of the associated control.
        /// </summary>
        public static readonly DependencyProperty LeftTemplateProperty =
            DependencyProperty.Register("LeftTemplate", typeof(DataTemplate), typeof(HelpFrame),
            new PropertyMetadata(null));

        #endregion

        #region RightTemplate

        /// <summary>
        /// Gets/sets the DataTemplate instance to display the help text on the right of the associated control.
        /// </summary>
        public DataTemplate RightTemplate { get { return (DataTemplate)GetValue(RightTemplateProperty); } set { SetValue(RightTemplateProperty, value); } }
        /// <summary>
        /// Gets/sets the DataTemplate instance to display the help text on the right of the associated control.
        /// </summary>
        public static readonly DependencyProperty RightTemplateProperty =
            DependencyProperty.Register("RightTemplate", typeof(DataTemplate), typeof(HelpFrame),
            new PropertyMetadata(null));

        #endregion

        #region ContentTemplate

        /// <summary>
        /// Gets/sets internal DataTemplate instance to display the help text on the associated control.
        /// </summary>
        private DataTemplate ContentTemplate { get { return (DataTemplate)GetValue(ContentTemplateProperty); } set { SetValue(ContentTemplateProperty, value); } }
        /// <summary>
        /// Gets/sets internal DataTemplate instance to display the help text on the associated control.
        /// </summary>
        private static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(HelpFrame),
            new PropertyMetadata(null));

        #endregion

        #region Layer

        /// <summary>
        /// Gets/sets internal HelpLayer instance to attach to the root layout panel of the page.
        /// </summary>
        private static readonly DependencyProperty LayerProperty =
            DependencyProperty.RegisterAttached("Layer", typeof(HelpLayer), typeof(HelpFrame),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets internal HelpLayer instance to attach to the root layout panel of the page.
        /// </summary>
        /// <param name="element">UIElement instance.</param>
        /// <returns>HelpLayer instance.</returns>
        private static HelpLayer GetLayer(UIElement element)
        {
            return (HelpLayer)element.GetValue(LayerProperty);
        }

        /// <summary>
        /// Sets the internal HelpLayer instance to attach to the root layout panel of the page.
        /// </summary>
        /// <param name="element">UIElement element.</param>
        /// <param name="value">HelpLayer instance.</param>
        private static void SetLayer(UIElement element, HelpLayer value)
        {
            element.SetValue(LayerProperty, value);
        }

        #endregion

        #region ShowBackgroundLayer

        /// <summary>
        /// Gets/sets a value indicating if the help layer is displayed.
        /// </summary>
        public static readonly DependencyProperty ShowBackgroundLayerProperty =
            DependencyProperty.RegisterAttached("ShowBackgroundLayer", typeof(bool), typeof(HelpFrame),
            new PropertyMetadata(true, OnShowBackgroundLayerPropertyChanged));

        private static void OnShowBackgroundLayerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
        }

        /// <summary>
        /// Gets a value indicating if the help layer is displayed.
        /// </summary>
        /// <param name="element">UIElement element.</param>
        /// <returns>True to display the layer, otherwise false.</returns>
        public static bool GetShowBackgroundLayer(UIElement element)
        {
            return (bool)element.GetValue(ShowBackgroundLayerProperty);
        }

        /// <summary>
        /// Sets a value indicating if the help layer is displayed.
        /// </summary>
        /// <param name="element">UIElement element.</param>
        /// <param name="value">True to display the layer, otherwise false.</param>
        public static void SetShowBackgroundLayer(UIElement element, bool value)
        {
            element.SetValue(ShowBackgroundLayerProperty, value);
        }

        #endregion

        #region HelpFrameAlignment

        /// <summary>
        /// Gets/sets a value indicating the alignment of the help frame on top of the associated element.
        /// </summary>
        public static readonly DependencyProperty AlignmentProperty =
            DependencyProperty.RegisterAttached("Alignment", typeof(HelpFrameAlignment), typeof(HelpFrame),
            new PropertyMetadata(HelpFrameAlignment.TopLeft, OnAlignmentPropertyChanged));

        private static void OnAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element != null)
            {
                var root = TreeHelpers.GetRootPanel();
                if (root != null)
                {
                    var layer = GetLayer(root);
                    if (layer != null)
                    {
                        var frame = layer.GetFrameFromElement(element);
                        if (frame != null)
                        {
                            frame.ApplyContentTemplate();
                            frame.PositionFrame();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating the alignment of the help frame on top of the associated element.
        /// </summary>
        /// <param name="element">Associated element.</param>
        /// <returns>Value of the HelpFrameAlignment enumeration.</returns>
        public static HelpFrameAlignment GetAlignment(UIElement element)
        {
            return (HelpFrameAlignment)element.GetValue(AlignmentProperty);
        }

        /// <summary>
        /// Sets a value indicating the alignment of the help frame on top of the associated element.
        /// </summary>
        /// <param name="element">Associated element.</param>
        /// <param name="value">Value of the HelpFrameAlignment enumeration.</param>
        public static void SetAlignment(UIElement element, HelpFrameAlignment value)
        {
            element.SetValue(AlignmentProperty, value);
        }

        #endregion

        #region ContentHorizontalAlignment

        /// <summary>
        /// Gets/sets the internal horizontal alignment of the help layer.
        /// </summary>
        public HorizontalAlignment ContentHorizontalAlignment { get { return (HorizontalAlignment)GetValue(ContentHorizontalAlignmentProperty); } set { SetValue(ContentHorizontalAlignmentProperty, value); } }
        /// <summary>
        /// Gets/sets the internal horizontal alignment of the help layer.
        /// </summary>
        public static readonly DependencyProperty ContentHorizontalAlignmentProperty =
            DependencyProperty.Register("ContentHorizontalAlignment", typeof(HorizontalAlignment), typeof(HelpFrame),
            new PropertyMetadata(HorizontalAlignment.Left, ContentHorizontalAlignmentPropertyChanged));

        private static void ContentHorizontalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frame = d as HelpFrame;
            frame.ApplyContentTemplate();
            frame.PositionFrame();
        }

        #endregion

        #region ContentVerticalAlignment

        /// <summary>
        /// Gets/sets the internal vertical alignment of the help layer.
        /// </summary>
        public VerticalAlignment ContentVerticalAlignment { get { return (VerticalAlignment)GetValue(ContentVerticalAlignmentProperty); } set { SetValue(ContentVerticalAlignmentProperty, value); } }
        /// <summary>
        /// Gets/sets the internal vertical alignment of the help layer.
        /// </summary>
        public static readonly DependencyProperty ContentVerticalAlignmentProperty =
            DependencyProperty.Register("ContentVerticalAlignment", typeof(VerticalAlignment), typeof(HelpFrame),
            new PropertyMetadata(VerticalAlignment.Top, ContentVerticalAlignmentPropertyChanged));

        private static void ContentVerticalAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frame = d as HelpFrame;
            frame.ApplyContentTemplate();
            frame.PositionFrame();
        }

        #endregion

        #region DisplayStyle

        /// <summary>
        /// Gets/sets a value of the HelpFrameDisplayStyle enumeration, indicating the display style of the help frames.
        /// </summary>
        /// <remarks>This attached property should be set on page level.</remarks>
        public static readonly DependencyProperty DisplayStyleProperty =
            DependencyProperty.RegisterAttached("DisplayStyle", typeof(HelpFrameDisplayStyle), typeof(HelpFrame),
            new PropertyMetadata(HelpFrameDisplayStyle.Classic, OnDisplayStylePropertyChanged));

        private static void OnDisplayStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignMode.DesignModeEnabled) return;
            var element = d as FrameworkElement;
            if (element != null)
            {
                var root = TreeHelpers.GetRootPanel();
                if (root != null)
                {
                    var layer = GetLayer(root);
                    if (layer != null)
                    {
                        foreach (var frame in layer._frames)
                        {
                            frame.ApplyContentTemplate();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value of the HelpFrameDisplayStyle enumeration, indicating the display style of the help frames.
        /// </summary>
        /// <param name="obj">Page element.</param>
        /// <returns>A value of the HelpFrameDisplayStyle enumeration.</returns>
        public static HelpFrameDisplayStyle GetDisplayStyle(DependencyObject obj)
        {
            return (HelpFrameDisplayStyle)obj.GetValue(DisplayStyleProperty);
        }

        /// <summary>
        /// Sets a value of the HelpFrameDisplayStyle enumeration, indicating the display style of the help frames.
        /// </summary>
        /// <param name="obj">Page element.</param>
        /// <param name="value">A value of the HelpFrameDisplayStyle enumeration.</param>
        public static void SetDisplayStyle(DependencyObject obj, HelpFrameDisplayStyle value)
        {
            obj.SetValue(DisplayStyleProperty, value);
        }

        #endregion

        #region FrameMargin

        /// <summary>
        /// Gets/sets the margin of the help layer relative to the associated element.
        /// </summary>
        public static readonly DependencyProperty FrameMarginProperty =
            DependencyProperty.RegisterAttached("FrameMargin", typeof(Thickness), typeof(HelpFrame), new PropertyMetadata(new Thickness()));

        /// <summary>
        /// Gets the margin of the help layer relative to the associated element.
        /// </summary>
        /// <param name="obj">Associated element.</param>
        /// <returns>Thickness value.</returns>
        public static Thickness GetFrameMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(FrameMarginProperty);
        }

        /// <summary>
        /// Sets the margin of the help layer relative to the associated element.
        /// </summary>
        /// <param name="obj">Associated element.</param>
        /// <param name="value">Thickness value.</param>
        public static void SetFrameMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(FrameMarginProperty, value);
        }

        #endregion

        #endregion

        #region Private Methods

        #region Attach

        /// <summary>
        /// Attaches a HelpFrame instance to the layer for the given element.
        /// </summary>
        /// <param name="element">UIElement instance.</param>
        private void Attach(UIElement element)
        {
            _attachedElement = element;

            // Get the text that is associated with the element
            this.Text = GetHelpText(element);

            // get the HelpLayer
            var panel = TreeHelpers.GetRootPanel();
            var layer = GetLayer(panel);
            if (layer != null)
            {
                // set the initial position of the frame
                var pos = element.GetPosition(panel);
                this.Margin = new Thickness(pos.X, pos.Y, 0, 0);
                this.HorizontalAlignment = HorizontalAlignment.Left;
                this.VerticalAlignment = VerticalAlignment.Top;
                layer.AttachFrame(this);
            }
        }

        #endregion

        #region Detach

        /// <summary>
        /// Detaches the frame for the given element.
        /// </summary>
        /// <param name="element">UIElement.</param>
        private void Detach(UIElement element)
        {
            this.Text = null;
            var panel = TreeHelpers.GetRootPanel();
            var layer = GetLayer(panel);
            if (layer != null)
            {
                layer.DetachFrame(this);
            }
        }

        #endregion

        #region ShowLayer

        /// <summary>
        /// Displays the HelpLayer.
        /// </summary>
        /// <param name="element">Page element.</param>
        private static void ShowLayer(FrameworkElement element)
        {
            // get the HelpLayer instance
            var panel = TreeHelpers.GetRootPanel();
            if (panel != null)
            {
                var layer = GetLayer(panel);
                if (layer != null)
                {
                    if (element != null)
                    {
                        layer.HelpFrameDisplayController = element;
                    }

                    // show/hide the layer
                    if (GetShowHelp(element))
                    {
                        layer.Visibility = Visibility.Visible;
                        layer.ShowHalfTransparentBackground = GetShowBackgroundLayer(element);
                        layer.Show();
                    }
                    else
                    {
                        if (!layer._isHiding)
                        {
                            layer.Hide();
                        }
                    }
                }
            }
        }

        #endregion

        #region AttachFrame

        /// <summary>
        /// Attaches the frame to the given element.
        /// </summary>
        /// <param name="element">FrameworkElement instance.</param>
        private static void AttachFrame(FrameworkElement element)
        {
            // get the HelpLayer instance
            var panel = TreeHelpers.GetRootPanel();
            if (panel != null)
            {
                var layer = GetLayer(panel);
                if (layer == null)
                {
                    // create a new layer
                    layer = new HelpLayer();
                    SetLayer(panel, layer);
                    layer.AttachToPage(panel);
                }

                // create a new frame and attach it to the layer
                var frame = new HelpFrame();
                frame.Attach(element);
            }
        }

        #endregion

        #region ApplyContentTemplate

        /// <summary>
        /// Applies the DataTemplate to display on base of DisplayStyle and Alignment property values.
        /// </summary>
        private void ApplyContentTemplate()
        {
            if (_attachedElement == null || _applyingTemplate)
            {
                return;
            }

            _applyingTemplate = true;
            var root = TreeHelpers.GetRootPanel().Parent;
            var style = GetDisplayStyle(root);
            var sketchTemplateUri = new Uri("ms-appx:///Neumann.HelpFrame/SketchStyle.xaml", UriKind.Absolute);
            var popupTemplateUri = new Uri("ms-appx:///Neumann.HelpFrame/PopupStyle.xaml", UriKind.Absolute);

            var direction = (HelpFrameAlignment)_attachedElement.GetValue(AlignmentProperty);
            if (direction == HelpFrameAlignment.TopLeft ||
                direction == HelpFrameAlignment.TopRight ||
                direction == HelpFrameAlignment.TopCenter ||
                direction == HelpFrameAlignment.CenterCenter)
            {
                if (style == HelpFrameDisplayStyle.Sketch)
                {
                    this.ContentTemplate = ResourceHelpers.GetResourceFromDictionary<DataTemplate>(sketchTemplateUri, "TopSketchTemplate");
                }
                else if (style == HelpFrameDisplayStyle.Popup)
                {
                    this.ContentTemplate = ResourceHelpers.GetResourceFromDictionary<DataTemplate>(popupTemplateUri, "TopPopupTemplate");
                }
                else
                {
                    this.ContentTemplate = this.TopTemplate;
                }
            }
            else if (direction == HelpFrameAlignment.BottomLeft ||
                direction == HelpFrameAlignment.BottomRight ||
                direction == HelpFrameAlignment.BottomCenter)
            {
                if (style == HelpFrameDisplayStyle.Sketch)
                {
                    this.ContentTemplate = ResourceHelpers.GetResourceFromDictionary<DataTemplate>(sketchTemplateUri, "BottomSketchTemplate");
                }
                else if (style == HelpFrameDisplayStyle.Popup)
                {
                    this.ContentTemplate = ResourceHelpers.GetResourceFromDictionary<DataTemplate>(popupTemplateUri, "BottomPopupTemplate");
                }
                else
                {
                    this.ContentTemplate = this.BottomTemplate;
                }
            }
            else if (direction == HelpFrameAlignment.CenterLeft)
            {
                if (style == HelpFrameDisplayStyle.Sketch)
                {
                    this.ContentTemplate = ResourceHelpers.GetResourceFromDictionary<DataTemplate>(sketchTemplateUri, "LeftSketchTemplate");
                }
                else if (style == HelpFrameDisplayStyle.Popup)
                {
                    this.ContentTemplate = ResourceHelpers.GetResourceFromDictionary<DataTemplate>(popupTemplateUri, "LeftPopupTemplate");
                }
                else
                {
                    this.ContentTemplate = this.LeftTemplate;
                }
            }
            else if (direction == HelpFrameAlignment.CenterRight)
            {
                if (style == HelpFrameDisplayStyle.Sketch)
                {
                    this.ContentTemplate = ResourceHelpers.GetResourceFromDictionary<DataTemplate>(sketchTemplateUri, "RightSketchTemplate");
                }
                else if (style == HelpFrameDisplayStyle.Popup)
                {
                    this.ContentTemplate = ResourceHelpers.GetResourceFromDictionary<DataTemplate>(popupTemplateUri, "RightPopupTemplate");
                }
                else
                {
                    this.ContentTemplate = this.RightTemplate;
                }
            }

            if (style == HelpFrameDisplayStyle.Popup)
            {
                this.ContentHorizontalAlignment = HorizontalAlignment.Center;
                this.ContentVerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                this.ContentHorizontalAlignment = HorizontalAlignment.Left;
                this.ContentVerticalAlignment = VerticalAlignment.Bottom;
                if (style == HelpFrameDisplayStyle.Sketch)
                {
                    _originalFontFamily = this.FontFamily;
                    _originalFontSize = this.FontSize;
                    _originalFontWeight = this.FontWeight;
                    this.FontFamily = new FontFamily("Buxton Sketch");
                    this.FontSize = 24;
                    this.FontWeight = FontWeights.Normal;
                }
                else if (_originalFontFamily != null)
                {
                    this.FontFamily = _originalFontFamily;
                    this.FontSize = _originalFontSize;
                    this.FontWeight = _originalFontWeight;
                }
            }

            _applyingTemplate = false;
        }

        #endregion

        #region PositionFrame

        /// <summary>
        /// Positions the HelpFrame on base of the Alignment property value.
        /// </summary>
        protected virtual void PositionFrame()
        {
            if (_attachedElement == null)
            {
                return;
            }
            var elementSize = _attachedElement.RenderSize;
            var size = this.RenderSize;
            var direction = GetAlignment(_attachedElement);
            var point = new Point(0, 0);
            var offsetX = 0d;
            var offsetY = 0d;

            switch (direction)
            {
                case HelpFrameAlignment.TopLeft:
                    point = new Point(0, -size.Height);
                    break;
                case HelpFrameAlignment.TopRight:
                    point = new Point(elementSize.Width, -size.Height);
                    break;
                case HelpFrameAlignment.TopCenter:
                    point = new Point(elementSize.Width / 2, -size.Height);
                    break;

                case HelpFrameAlignment.BottomLeft:
                    point = new Point(0, elementSize.Height);
                    break;
                case HelpFrameAlignment.BottomRight:
                    point = new Point(elementSize.Width, elementSize.Height);
                    break;
                case HelpFrameAlignment.BottomCenter:
                    point = new Point(elementSize.Width / 2, elementSize.Height);
                    break;

                case HelpFrameAlignment.CenterLeft:
                    point = new Point(-size.Width, elementSize.Height / 2);
                    break;
                case HelpFrameAlignment.CenterRight:
                    point = new Point(elementSize.Width, elementSize.Height / 2);
                    break;
                case HelpFrameAlignment.CenterCenter:
                    point = new Point(elementSize.Width / 2, elementSize.Height / 2);
                    break;
            }

            if (direction == HelpFrameAlignment.CenterLeft ||
                direction == HelpFrameAlignment.CenterRight ||
                direction == HelpFrameAlignment.CenterCenter)
            {
                switch (this.ContentVerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        offsetY = 0;
                        break;
                    case VerticalAlignment.Bottom:
                        offsetY = -size.Height;
                        break;
                    case VerticalAlignment.Center:
                        offsetY = -size.Height / 2;
                        break;
                }
                if (point.Y + offsetY + this.Margin.Top < 0)
                {
                    offsetY = -(point.Y + this.Margin.Top) + 4;
                }
            }
            else
            {
                switch (this.ContentHorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        offsetX = 0;
                        break;
                    case HorizontalAlignment.Right:
                        offsetX = -size.Width;
                        break;
                    case HorizontalAlignment.Center:
                        offsetX = -size.Width / 2;
                        break;
                }
                if (point.X + offsetX + this.Margin.Left < 0)
                {
                    offsetX = -(point.X + this.Margin.Left) + 4;
                }
            }

            // set the margin
            var margin = GetFrameMargin(_attachedElement);
            offsetX += margin.Left;
            offsetY += margin.Top;
            offsetX -= margin.Right;
            offsetY -= margin.Bottom;

            // check for negative Y value
            if (point.Y + offsetY + this.Margin.Top < 0)
            {
                offsetY = -(point.Y + this.Margin.Top) + 10;
            }

            // apply transformation
            this.RenderTransformOrigin = new Point(0, 0);
            this.RenderTransform = new TranslateTransform()
            {
                X = point.X + offsetX,
                Y = point.Y + offsetY
            };
        }

        #endregion

        #endregion

        #region Event Handling

        private void OnLayoutUpdated(object sender, object e)
        {
            this.PositionFrame();
        }

        private static void OnRootElementLoaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            ShowLayer(element);
        }

        private static void OnElementLoaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element != null)
            {
                element.Loaded -= OnElementLoaded;
                AttachFrame(element);
            }
        }

        #endregion

    }

    #region HelpFrameDisplayStyle

    /// <summary>
    /// Contains values that indicate the display style of a HelpFrame element.
    /// </summary>
    public enum HelpFrameDisplayStyle : int
    {
        /// <summary>
        /// Shows the HelpFrame in classic style or uses a custom template if provided.
        /// </summary>
        Classic = 0,
        /// <summary>
        /// Show the HelpFrame in sketch style.
        /// </summary>
        Sketch = 1,
        /// <summary>
        /// Show the HelpFrame in popup style.
        /// </summary>
        Popup = 2
    }

    #endregion

    #region HelpFrameAlignment

    /// <summary>
    /// Contains values that indicate the alignment of a HelpFrame element.
    /// </summary>
    public enum HelpFrameAlignment
    {
        /// <summary>
        /// Positions the frame on top and left to the associated element.
        /// </summary>
        TopLeft,
        /// <summary>
        /// Positions the frame on top and center to the associated element.
        /// </summary>
        TopCenter,
        /// <summary>
        /// Positions the frame on top and right to the associated element.
        /// </summary>
        TopRight,
        /// <summary>
        /// Positions the frame on center and left to the associated element.
        /// </summary>
        CenterLeft,
        /// <summary>
        /// Positions the frame on center to the associated element.
        /// </summary>
        CenterCenter,
        /// <summary>
        /// Positions the frame on center and right to the associated element.
        /// </summary>
        CenterRight,
        /// <summary>
        /// Positions the frame on bottom and left to the associated element.
        /// </summary>
        BottomLeft,
        /// <summary>
        /// Positions the frame on bottom and center to the associated element.
        /// </summary>
        BottomCenter,
        /// <summary>
        /// Positions the frame on bottom and right to the associated element.
        /// </summary>
        BottomRight
    }

    #endregion

    #region HelpFrameHorizontalAlignment

    /// <summary>
    /// Contains values that indicate the internal horizontal alignment of a HelpFrame element.
    /// </summary>
    internal enum HelpFrameHorizontalAlignment
    {
        /// <summary>
        /// Alignment left.
        /// </summary>
        Left,
        /// <summary>
        /// Alignment center.
        /// </summary>
        Center,
        /// <summary>
        /// Alignment right.
        /// </summary>
        Right
    }

    #endregion

    #region HelpFrameVerticalAlignment

    /// <summary>
    /// Contains values that indicate the internal vertical alignment of a HelpFrame element.
    /// </summary>
    internal enum HelpFrameVerticalAlignment
    {
        /// <summary>
        /// Alignment top.
        /// </summary>
        Top,
        /// <summary>
        /// Alignment center.
        /// </summary>
        Center,
        /// <summary>
        /// Alignment bottom.
        /// </summary>
        Bottom
    }

    #endregion

}
