using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Neumann.HelpFrame
{
    /// <summary>
    /// Provides a balloon surface.
    /// </summary>
    public class Balloon : ContentControl
    {

        #region Constructors

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        public Balloon()
        {
            this.DefaultStyleKey = typeof(Balloon);
            this.SizeChanged += this.OnSizeChanged;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Is called after the control is initialized.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Content is string && this.ContentTemplate == null)
            {
                this.ContentTemplate = this.TextTemplate;
            }
        }

        /// <summary>
        /// Is called after the value of the Content property is changed.
        /// </summary>
        /// <param name="oldContent">Old content.</param>
        /// <param name="newContent">New content.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (newContent is string && this.ContentTemplate == null)
            {
                this.ContentTemplate = this.TextTemplate;
            }
        }

        #endregion

        #region Properties

        #region Geometry

        /// <summary>
        /// Gets/sets the internal geometry of the control.
        /// </summary>
        public object Geometry { get { return (object)GetValue(GeometryProperty); } set { SetValue(GeometryProperty, value); } }
        /// <summary>
        /// Gets/sets the internal geometry of the control.
        /// </summary>
        public static readonly DependencyProperty GeometryProperty =
            DependencyProperty.Register("Geometry", typeof(object), typeof(Balloon), new PropertyMetadata(null));

        #endregion

        #region TextTemplate

        /// <summary>
        /// Gets/sets the DataTemplate instance that is used for showing text content.
        /// </summary>
        public DataTemplate TextTemplate { get { return (DataTemplate)GetValue(TextTemplateProperty); } set { SetValue(TextTemplateProperty, value); } }
        /// <summary>
        /// Gets/sets the DataTemplate instance that is used for showing text content.
        /// </summary>
        public static readonly DependencyProperty TextTemplateProperty =
            DependencyProperty.Register("TextTemplate", typeof(DataTemplate), typeof(Balloon),
            new PropertyMetadata(null));

        #endregion

        #region ArrowDirection

        /// <summary>
        /// Gets/sets a value of the BobbleDirection enumeration that indicates the position of the control.
        /// </summary>
        public BobbleDirection ArrowDirection { get { return (BobbleDirection)GetValue(ArrowDirectionProperty); } set { SetValue(ArrowDirectionProperty, value); } }
        /// <summary>
        /// Gets/sets a value of the BobbleDirection enumeration that indicates the position of the control.
        /// </summary>
        public static readonly DependencyProperty ArrowDirectionProperty =
            DependencyProperty.Register("ArrowDirection", typeof(BobbleDirection), typeof(Balloon),
            new PropertyMetadata(BobbleDirection.Down, OnArrowDirectionPropertyChanged));

        private static void OnArrowDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Balloon)d).CreateGeometry();
        }

        #endregion

        #region CornerRadius

        /// <summary>
        /// Gets/sets the corner radius of the control.
        /// </summary>
        public CornerRadius CornerRadius { get { return (CornerRadius)GetValue(CornerRadiusProperty); } set { SetValue(CornerRadiusProperty, value); } }
        /// <summary>
        /// Gets/sets the corner radius of the control.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Balloon),
            new PropertyMetadata(new CornerRadius(0), OnCornerRadiusPropertyChanged));

        private static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Balloon)d).CreateGeometry();
        }

        #endregion

        #region OffsetX

        /// <summary>
        /// Gets/sets the internal X offset inside the control.
        /// </summary>
        private double OffsetX { get { return (double)GetValue(OffsetXProperty); } set { SetValue(OffsetXProperty, value); } }
        /// <summary>
        /// Gets/sets the internal X offset inside the control.
        /// </summary>
        private static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.Register("OffsetX", typeof(double), typeof(Balloon), new PropertyMetadata(0d));

        #endregion

        #region OffsetY

        /// <summary>
        /// Gets/sets the internal Y offset inside the control.
        /// </summary>
        private double OffsetY { get { return (double)GetValue(OffsetYProperty); } set { SetValue(OffsetYProperty, value); } }
        /// <summary>
        /// Gets/sets the internal Y offset inside the control.
        /// </summary>
        private static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.Register("OffsetY", typeof(double), typeof(Balloon), new PropertyMetadata(0d));

        #endregion

        #endregion

        #region Private Methods

        #region CreateArc

        private ArcSegment CreateArc(CornerPosition position, double start, double width, double height)
        {
            ArcSegment arc = null;
            switch (position)
            {
                case CornerPosition.TopLeft:
                    arc = new ArcSegment
                    {
                        Size = new Size(this.CornerRadius.TopLeft, this.CornerRadius.TopLeft),
                        Point = new Point(this.CornerRadius.TopLeft, start),
                        RotationAngle = 0,
                        IsLargeArc = false,
                        SweepDirection = SweepDirection.Clockwise
                    };
                    break;
                case CornerPosition.TopRight:
                    arc = new ArcSegment
                    {
                        Size = new Size(this.CornerRadius.TopRight, this.CornerRadius.TopRight),
                        Point = new Point(width, start + this.CornerRadius.TopRight),
                        RotationAngle = 0,
                        IsLargeArc = false,
                        SweepDirection = SweepDirection.Clockwise
                    };
                    break;
                case CornerPosition.BottomRight:
                    arc = new ArcSegment
                    {
                        Size = new Size(this.CornerRadius.BottomRight, this.CornerRadius.BottomRight),
                        Point = new Point(width - this.CornerRadius.BottomRight, height),
                        RotationAngle = 0,
                        IsLargeArc = false,
                        SweepDirection = SweepDirection.Clockwise
                    };
                    break;
                case CornerPosition.BottomLeft:
                    arc = new ArcSegment
                    {
                        Size = new Size(this.CornerRadius.BottomLeft, this.CornerRadius.BottomLeft),
                        Point = new Point(start, height - this.CornerRadius.BottomLeft),
                        RotationAngle = 0,
                        IsLargeArc = false,
                        SweepDirection = SweepDirection.Clockwise
                    };
                    break;
            }
            return arc;
        }

        #endregion

        #region CreateGeometry

        private void CreateGeometry()
        {
            var size = this.RenderSize;
            var thickness = this.BorderThickness.Left;
            var width = size.Width - (thickness / 2);
            var height = size.Height - 10 - (thickness / 2);
            var start = thickness / 2;

            var pf = new PathFigure();
            pf.IsClosed = true;
            pf.StartPoint = new Point(start, start);
            switch (this.ArrowDirection)
            {
                case BobbleDirection.Down:
                    this.OffsetX = 0;
                    this.OffsetY = -5;

                    // TopLeft
                    if (this.CornerRadius.TopLeft == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(start, start) });
                    }
                    else
                    {
                        pf.StartPoint = new Point(start, this.CornerRadius.TopLeft);
                        pf.Segments.Add(this.CreateArc(CornerPosition.TopLeft, start, width, height));
                    }

                    // TopRight
                    if (this.CornerRadius.TopRight == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, start) });
                    }
                    else
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width - this.CornerRadius.TopRight, start) });
                        pf.Segments.Add(this.CreateArc(CornerPosition.TopRight, start, width, height));
                    }

                    // BottomRight
                    if (this.CornerRadius.BottomRight == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, height) });
                    }
                    else
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, height - this.CornerRadius.BottomRight) });
                        pf.Segments.Add(this.CreateArc(CornerPosition.BottomRight, start, width, height));
                    }

                    // Arrow
                    pf.Segments.Add(new LineSegment { Point = new Point((width / 2) + 10, height) });
                    pf.Segments.Add(new LineSegment { Point = new Point((width / 2), height + 10) });
                    pf.Segments.Add(new LineSegment { Point = new Point((width / 2) - 10, height) });

                    // BottomLeft
                    if (this.CornerRadius.BottomLeft == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(start, height) });
                    }
                    else
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(start + this.CornerRadius.BottomLeft, height) });
                        pf.Segments.Add(this.CreateArc(CornerPosition.BottomLeft, start, width, height));
                    }
                    break;

                case BobbleDirection.Up:
                    height += 10;
                    this.OffsetX = 0;
                    this.OffsetY = 5;
                    pf.StartPoint = new Point(start, 10);

                    // TopLeft
                    if (this.CornerRadius.TopLeft == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(start, 10) });
                    }
                    else
                    {
                        pf.StartPoint = new Point(start, this.CornerRadius.TopLeft + 10);
                        pf.Segments.Add(this.CreateArc(CornerPosition.TopLeft, 10, width, height));
                    }

                    // Arrow
                    pf.Segments.Add(new LineSegment { Point = new Point((width / 2) - 10, 10) });
                    pf.Segments.Add(new LineSegment { Point = new Point((width / 2), start) });
                    pf.Segments.Add(new LineSegment { Point = new Point((width / 2) + 10, 10) });

                    // TopRight
                    if (this.CornerRadius.TopRight == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, 10) });
                    }
                    else
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width - this.CornerRadius.TopRight, 10) });
                        pf.Segments.Add(this.CreateArc(CornerPosition.TopRight, start + 10, width, height));
                    }

                    // BottomRight
                    if (this.CornerRadius.BottomRight == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, height) });
                    }
                    else
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, height - this.CornerRadius.BottomRight) });
                        pf.Segments.Add(this.CreateArc(CornerPosition.BottomRight, start, width, height));
                    }

                    // BottomLeft
                    if (this.CornerRadius.BottomLeft == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(start, height) });
                    }
                    else
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(start + this.CornerRadius.BottomLeft, height) });
                        pf.Segments.Add(this.CreateArc(CornerPosition.BottomLeft, start, width, height));
                    }
                    break;
                case BobbleDirection.Right:
                    height += 10;
                    width -= 10;
                    this.OffsetX = -5;
                    this.OffsetY = 0;
                    pf.StartPoint = new Point(start, start);

                    // TopLeft
                    if (this.CornerRadius.TopLeft == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, start) });
                    }
                    else
                    {
                        pf.StartPoint = new Point(start, this.CornerRadius.TopLeft);
                        pf.Segments.Add(this.CreateArc(CornerPosition.TopLeft, start, width, height - 10));
                    }

                    // TopRight
                    if (this.CornerRadius.TopRight == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, start) });
                    }
                    else
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width - this.CornerRadius.TopRight, start) });
                        pf.Segments.Add(this.CreateArc(CornerPosition.TopRight, start, width, height));
                    }

                    // Arrow
                    pf.Segments.Add(new LineSegment { Point = new Point(width, (height / 2) - 10) });
                    pf.Segments.Add(new LineSegment { Point = new Point(width + 10, (height / 2)) });
                    pf.Segments.Add(new LineSegment { Point = new Point(width, (height / 2) + 10) });

                    // BottomRight
                    if (this.CornerRadius.TopRight == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, height) });
                    }
                    else
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, height - this.CornerRadius.BottomRight) });
                        pf.Segments.Add(this.CreateArc(CornerPosition.BottomRight, start, width, height));
                    }

                    // BottomLeft
                    if (this.CornerRadius.BottomLeft == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(start, height) });
                    }
                    else
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(start + this.CornerRadius.BottomLeft, height) });
                        pf.Segments.Add(this.CreateArc(CornerPosition.BottomLeft, start, width, height));
                    }
                    break;
                case BobbleDirection.Left:
                    height += 10;
                    this.OffsetX = 5;
                    this.OffsetY = 0;

                    // TopLeft
                    if (this.CornerRadius.TopLeft == 0)
                    {
                        pf.StartPoint = new Point(10, start);
                    }
                    else
                    {
                        pf.StartPoint = new Point(10, this.CornerRadius.TopLeft);
                        var arc = this.CreateArc(CornerPosition.TopLeft, start, width, height);
                        arc.Point = new Point(this.CornerRadius.TopLeft + 10, start);
                        pf.Segments.Add(arc);
                    }

                    // TopRight
                    if (this.CornerRadius.TopRight == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, start) });
                    }
                    else
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width - this.CornerRadius.TopRight, start) });
                        pf.Segments.Add(this.CreateArc(CornerPosition.TopRight, start, width, height));
                    }

                    // BottomRight
                    if (this.CornerRadius.TopRight == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, height) });
                    }
                    else
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(width, height - this.CornerRadius.BottomRight) });
                        pf.Segments.Add(this.CreateArc(CornerPosition.BottomRight, start, width, height));
                    }


                    // BottomLeft
                    if (this.CornerRadius.BottomLeft == 0)
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(10, height) });
                    }
                    else
                    {
                        pf.Segments.Add(new LineSegment { Point = new Point(start + 10 + this.CornerRadius.BottomLeft, height) });
                        pf.Segments.Add(this.CreateArc(CornerPosition.BottomLeft, start + 10, width, height));
                    }

                    pf.Segments.Add(new LineSegment { Point = new Point(10, (height / 2) + 10) });
                    pf.Segments.Add(new LineSegment { Point = new Point(0, (height / 2)) });
                    pf.Segments.Add(new LineSegment { Point = new Point(10, (height / 2) - 10) });
                    break;
            }

            var pg = new PathGeometry();
            pg.Figures.Add(pf);

            var p = new Path();
            p.Data = pg;
            p.StrokeThickness = thickness;
            p.Stroke = this.BorderBrush;
            p.Fill = this.Background;
            p.Width = size.Width;
            p.Height = size.Height;

            this.Geometry = p;
        }

        #endregion

        #endregion

        #region Event Handling

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.CreateGeometry();
        }

        #endregion

        #region CornerPosition

        enum CornerPosition
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        #endregion

    }

    #region BobbleDirection

    /// <summary>
    /// Contains values that indicates the position of the arrow on a Balloon control.
    /// </summary>
    public enum BobbleDirection
    {
        /// <summary>
        /// Arrow down.
        /// </summary>
        Down,
        /// <summary>
        /// Arrow up.
        /// </summary>
        Up,
        /// <summary>
        /// Arrow left.
        /// </summary>
        Left,
        /// <summary>
        /// Arrow right.
        /// </summary>
        Right
    }

    #endregion

}
