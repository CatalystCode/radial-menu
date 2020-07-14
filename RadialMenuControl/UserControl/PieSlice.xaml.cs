using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using RadialMenuControl.Components;
using Windows.Foundation;
using Windows.UI.Xaml.Media.Animation;

namespace RadialMenuControl.UserControl
{
    public sealed partial class PieSlice : Windows.UI.Xaml.Controls.UserControl
    {
        // Arcs
        public OuterPieSlicePath OuterArcElement => OuterPieSlicePath;

        // Inner Arc Colors
        public static readonly DependencyProperty InnerNormalColorProperty =
            DependencyProperty.Register("InnerNormalColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty InnerHoverColorProperty =
            DependencyProperty.Register("InnerHoverColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty InnerTappedColorProperty =
            DependencyProperty.Register("InnerTappedColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty InnerReleasedColorProperty =
            DependencyProperty.Register("InnerReleasedColor", typeof(Color), typeof(PieSlice), null);

        /// <summary>
        /// Hover color for the inner portion of the PieSlice
        /// </summary>
        public Color InnerHoverColor
        {
            get { return (Color)GetValue(InnerHoverColorProperty); }
            set { SetValue(InnerHoverColorProperty, value); }
        }

        /// <summary>
        /// Normal color for the inner portion of the PieSlice
        /// </summary>
        public Color InnerNormalColor
        {
            get { return (Color)GetValue(InnerNormalColorProperty); }
            set { SetValue(InnerNormalColorProperty, value); }
        }

        /// <summary>
        /// Tapped color for the inner portion of the PieSlice
        /// </summary>
        public Color InnerTappedColor
        {
            get { return (Color)GetValue(InnerTappedColorProperty); }
            set { SetValue(InnerTappedColorProperty, value); }
        }

        /// <summary>
        ///  Released color for the inner portion of the PieSlice
        /// </summary>
        public Color InnerReleasedColor
        {
            get { return (Color)GetValue(InnerReleasedColorProperty); }
            set { SetValue(InnerReleasedColorProperty, value); }
        }

        // Indication Arc
        public static readonly DependencyProperty IndicationArcStartPointProperty =
        DependencyProperty.Register("InidicationArcStartPoint", typeof(Point), typeof(PieSlice), null);
        public static readonly DependencyProperty IndicationArcEndPointProperty =
        DependencyProperty.Register("InidicationArcEndPoint", typeof(Point), typeof(PieSlice), null);
        public static readonly DependencyProperty IndicationArcSizeProperty =
        DependencyProperty.Register("InidicationArcSize", typeof(Size), typeof(PieSlice), null);
        public static readonly DependencyProperty IndicationArcIsLargeArcProperty =
        DependencyProperty.Register("IndicationArcIsLargeArc", typeof(bool), typeof(PieSlice), null);
        public static readonly DependencyProperty IndicationArcColorProperty =
        DependencyProperty.Register("IndicationArcColor", typeof(Color), typeof(PieSlice), null);
        public static readonly DependencyProperty IndicationArcSweepAngleProperty =
        DependencyProperty.Register("IndicationArcSweepAngle", typeof(double), typeof(PieSlice), null);
        public static readonly DependencyProperty IndicationArcStrokeThicknessProperty =
        DependencyProperty.Register("IndicationArcStrokeThickness", typeof(double), typeof(PieSlice), null);
        public static readonly DependencyProperty IndicationArcDistanceFromEdgeProperty =
        DependencyProperty.Register("IndicationArcDistanceFromEdge", typeof(double), typeof(PieSlice), null);
        public static readonly DependencyProperty UseIndicationArcsProperty =
            DependencyProperty.Register("UseIndicationArc", typeof(bool), typeof(PieSlice), null);
        public Point IndicationArcStartPoint
        {
            get { return (Point)GetValue(IndicationArcStartPointProperty); }
            set { SetValue(IndicationArcStartPointProperty, value); }
        }
        public Point IndicationArcEndPoint
        {
            get { return (Point)GetValue(IndicationArcEndPointProperty); }
            set { SetValue(IndicationArcEndPointProperty, value); }
        }
        public Size IndicationArcSize
        {
            get { return (Size)GetValue(IndicationArcSizeProperty); }
            set { SetValue(IndicationArcSizeProperty, value); }
        }
        public Color IndicationArcColor
        {
            get { return (Color)GetValue(IndicationArcColorProperty); }
            set { SetValue(IndicationArcColorProperty, value); }
        }
        public double IndicationArcSweepAngle
        {
            get { return (double)GetValue(IndicationArcSweepAngleProperty); }
            set { SetValue(IndicationArcSweepAngleProperty, value); }
        }
        public double IndicationArcStrokeThickness
        {
            get { return (double)GetValue(IndicationArcStrokeThicknessProperty); }
            set { SetValue(IndicationArcStrokeThicknessProperty, value); }
        }
        public bool IndicationArcIsLargeArc
        {
            get { return (bool)GetValue(IndicationArcIsLargeArcProperty); }
            set { SetValue(IndicationArcIsLargeArcProperty, value); }
        }
        public double IndicationArcDistanceFromEdge
        {
            get { return (double)GetValue(IndicationArcDistanceFromEdgeProperty); }
            set { SetValue(IndicationArcDistanceFromEdgeProperty, value); }
        }
        public bool UseIndicationArc
        {
            get { return (bool)GetValue(UseIndicationArcsProperty); }
            set { SetValue(UseIndicationArcsProperty, value); }
        }
        // Outer Arc Colors
        public static readonly DependencyProperty OuterNormalColorProperty =
            DependencyProperty.Register("OuterNormalColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty OuterHoverColorProperty =
            DependencyProperty.Register("OuterHoverColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty OuterTappedColorProperty =
            DependencyProperty.Register("OuterTappedColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty OuterDisabledColorProperty =
            DependencyProperty.Register("OuterDisabledColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty StrokeColorProperty =
            DependencyProperty.Register("StrokeColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(PieSlice), null);

        /// <summary>
        /// Hover color for the outer portion of the PieSlice
        /// </summary>
        public Color OuterHoverColor
        {
            get { return (Color)GetValue(OuterHoverColorProperty); }
            set { SetValue(OuterHoverColorProperty, value); }
        }

        /// <summary>
        /// Normal color for the outer portion of the PieSlice
        /// </summary>
        public Color OuterNormalColor
        {
            get { return (Color)GetValue(OuterNormalColorProperty); }
            set { SetValue(OuterNormalColorProperty, value); }
        }

        /// <summary>
        /// Disabled color for the outer portion of the PieSlice
        /// </summary>
        public Color OuterDisabledColor
        {
            get { return (Color)GetValue(OuterDisabledColorProperty); }
            set { SetValue(OuterDisabledColorProperty, value); }
        }

        /// <summary>
        /// Tapped color for the outer portion of the PieSlice
        /// </summary>
        public Color OuterTappedColor
        {
            get { return (Color)GetValue(OuterTappedColorProperty); }
            set { SetValue(OuterTappedColorProperty, value); }
        }

        // Stroke
        /// <summary>
        /// Color of the stroke around the PieSLice
        /// </summary>
        public Color StrokeColor
        {
            get { return (Color)GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }

        /// <summary>
        /// Thickness of the stroke around the PieSlice
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        // Angles & Radius
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(PieSlice), null);

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(PieSlice), null);

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(PieSlice), null);

        public static readonly DependencyProperty OuterArcThicknessProperty =
            DependencyProperty.Register("OuterThickness", typeof(double), typeof(PieSlice), null);

        /// <summary>
        /// Starting angle of this PieSlice (with 0 being the "north top")
        /// </summary>
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Angle (aka size) of this PieSlice
        /// </summary>
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        /// <summary>
        /// Radius of the (invisible) full circle used as drawing basis for this PieSlice
        /// </summary>
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        /// Thickness of the outer arc
        /// </summary>
        public double OuterArcThickness
        {
            get { return (double)GetValue(OuterArcThicknessProperty); }
            set { SetValue(OuterArcThicknessProperty, value); }
        }

        // Label
        public static readonly DependencyProperty IsLabelHiddenProperty =
            DependencyProperty.Register("IsLabelHidden", typeof(bool), typeof(PieSlice), null);

        public static readonly DependencyProperty InnerAccessKeyProperty =
            DependencyProperty.Register("InnerAccessKey", typeof(string), typeof(PieSlice), null);

        public static readonly DependencyProperty OuterAccessKeyProperty =
            DependencyProperty.Register("OuterAccessKey", typeof(string), typeof(PieSlice), null);

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(PieSlice), null);

        public static readonly DependencyProperty LabelColorProperty =
            DependencyProperty.Register("LabelColor", typeof(Brush), typeof(PieSlice), null);

        public static readonly DependencyProperty LabelSizeProperty =
            DependencyProperty.Register("LabelSize", typeof(int), typeof(PieSlice), null);

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(PieSlice), null);

        public static readonly DependencyProperty IconForegroundBrushProperty =
            DependencyProperty.Register("IconForegroundBrush", typeof(Brush), typeof(PieSlice), null);

        public static readonly DependencyProperty IconFontFamilyProperty =
            DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(PieSlice), null);

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(int), typeof(PieSlice), null);

        public static readonly DependencyProperty IconImageProperty =
            DependencyProperty.Register("IconImage", typeof(ImageSource), typeof(PieSlice), null);

        public static readonly DependencyProperty IconImageSideLengthProperty =
            DependencyProperty.Register("IconImageSideLength", typeof(ImageSource), typeof(PieSlice), null);

        public static readonly DependencyProperty CustomValueProperty =
           DependencyProperty.Register("CustomValue", typeof(string), typeof(PieSlice), null);

        /// <summary>
        /// Outer slice path access key
        /// </summary>
        public string OuterAccessKey
        {
            get { return (string)GetValue(OuterAccessKeyProperty); }
            set { SetValue(OuterAccessKeyProperty, value); }
        }

        /// <summary>
        /// Inner slice path access key
        /// </summary>
        public string InnerAccessKey
        {
            get { return (string)GetValue(InnerAccessKeyProperty); }
            set { SetValue(InnerAccessKeyProperty, value); }
        }

        /// <summary>
        /// Text label
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Label Color
        /// </summary>
        public Brush LabelColor
        {
            get { return (Brush)GetValue(LabelColorProperty); }
            set { SetValue(LabelColorProperty, value); }
        }

        /// <summary>
        /// Font size for the text label
        /// </summary>
        public int LabelSize
        {
            get { return (int)GetValue(LabelSizeProperty); }
            set { SetValue(LabelSizeProperty, value); }
        }

        /// <summary>
        /// Should the label be hidden?
        /// </summary>
        public bool IsLabelHidden
        {
            get { return (bool)GetValue(IsLabelHiddenProperty); }
            set { SetValue(IsLabelHiddenProperty, value); }
        }

        /// <summary>
        /// Icon (string-based), to be used with icon fonts like Segoe Symbol
        /// </summary>
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Foreground brush for the icon, allowing color change for text-based icons
        /// </summary>
        public Brush IconForegroundBrush
        {
            get { return (Brush)GetValue(IconForegroundBrushProperty); }
            set { SetValue(IconForegroundBrushProperty, value); }
        }

        /// <summary>
        /// Font family for the text-based icon
        /// </summary>
        public FontFamily IconFontFamily
        {
            get { return (FontFamily)GetValue(IconFontFamilyProperty); }
            set
            {
                IconTextElement.FontFamily = value;
                SetValue(IconFontFamilyProperty, value);
            }
        }

        /// <summary>
        /// Font size for the text-based icon
        /// </summary>
        public int IconSize
        {
            get { return (int)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        /// <summary>
        /// ImageSource for an image icon - if set, the text-based icon will not be displayed
        /// </summary>
        public ImageSource IconImage
        {
            get { return (ImageSource)GetValue(IconImageProperty); }
            set { SetValue(IconImageProperty, value); }
        }

        /// <summary>
        /// Length of the image-based icon
        /// </summary>
        public double IconImageSideLength
        {
            get { return (double)GetValue(IconImageSideLengthProperty); }
            set { SetValue(IconImageSideLengthProperty, value); }
        }

        /// <summary>
        /// Value for custom button
        /// </summary>
        public string CustomValue
        {
            get { return (string)GetValue(CustomValueProperty); }
            set {
                SetValue(CustomValueProperty, value);
                OriginalRadialMenuButton.Value = value;
            }
        }

        /// <summary>
        /// Visibility of the text block - determined by checking whether or not an IconImage is set
        /// </summary>
        public Visibility TextBlockVisibility => IconImage == null ? Visibility.Visible : Visibility.Collapsed;

        public static readonly DependencyProperty OriginalRadialMenuButtonProperty =
            DependencyProperty.Register("OriginalRadialMenuButton", typeof(RadialMenuButton), typeof(PieSlice), null);

        /// <summary>
        /// Reference to the original RadialMenuButton that was used to create this PieSLice
        /// </summary>
        public RadialMenuButton OriginalRadialMenuButton;

        /// <summary>
        /// Reference to the TextBox control for this PieSLice when the button type is custom
        /// </summary>
        private TextBox CustomTextBox;

        /// <summary>
        /// Delegate for a ChangeMenuRequest, asking the parent RadialMenu to change the menu to a submenu on a button
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="submenu">RadialMenu to change to</param>
        public delegate void ChangeMenuRequestHandler(object sender, MenuBase submenu);
        public event ChangeMenuRequestHandler ChangeMenuRequestEvent;

        /// <summary>
        /// A delegate for the ChangeSelected event, fired whenever a radio or toggle button changes its value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="slice"></param>
        public delegate void ChangeSelectedHandler(object sender, PieSlice slice);
        public event ChangeSelectedHandler ChangeSelectedEvent;

        // Tooltips
        private bool _areAccessKeyToolTipsVisible = false;
        /// <summary>
        /// Are little popups showing the access keys for the inner and outer arc visible?
        /// </summary>
        public bool AreAccessKeyToolTipsVisible
        {
            get { return _areAccessKeyToolTipsVisible; }
            set
            {
                _areAccessKeyToolTipsVisible = value;
                OuterAccessKeyPopup.IsOpen = OriginalRadialMenuButton.HasOuterArcAction && (OuterAccessKey != null) && value;
                InnerAccessKeyPopup.IsOpen = (InnerAccessKey != null) && value;
            }
        }

        private void OnChangeMenuRequest(object s, MenuBase sm)
        {
            ChangeMenuRequestEvent?.Invoke(s, sm);
        }

        public PieSlice()
        {
            InitializeComponent();
            DataContext = this;
            
            Loaded += OnLoaded;
        }

        private void SetupIndicationArc()
        {
            var indicationArcRadius = Radius - OuterArcThickness - IndicationArcDistanceFromEdge;
            IndicationArc.Size = new Size(indicationArcRadius, indicationArcRadius);
            double startAngle = (StartAngle + .10 * Angle) * (Math.PI / 180),
                    endAngle = (StartAngle + .90 * Angle) * (Math.PI / 180);
            double startX = Radius + indicationArcRadius * Math.Sin(startAngle),
                   startY = Radius - indicationArcRadius * Math.Cos(startAngle),
                   endX = Radius + indicationArcRadius * Math.Sin(endAngle),
                   endY = Radius - indicationArcRadius * Math.Cos(endAngle);
            IndicationArcPathFigure.StartPoint = new Point(startX, startY);
            IndicationArc.Point = new Point(endX, endY);
            IndicationArcPath.StrokeThickness = IndicationArcStrokeThickness;
            IndicationArcPath.Stroke = new SolidColorBrush(IndicationArcColor);
            // make all arcs initiall invisibile
            IndicationArcPath.Opacity = 0.0;
            
            var appearAnimation = new DoubleAnimation()
            {
                From = 0.0,
                To = 1.0,
            };
            appearAnimation.SetValue(Storyboard.TargetNameProperty, "IndicationArcPath");
            appearAnimation.SetValue(Storyboard.TargetPropertyProperty, "Opacity");
            InnerReleasedStoryBoard.Stop();
            InnerReleasedStoryBoard.Children.Add(appearAnimation);

        }
        /// <summary>
        /// Math and drawing operations for the path elements is handled in this OnLoaded event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (UseIndicationArc)
            {
                SetupIndicationArc();
            }
            
            // Setup outer arc
            OuterPieSlicePath.Radius = Radius;
            OuterPieSlicePath.StartAngle = StartAngle;
            OuterPieSlicePath.Angle = Angle;
            OuterPieSlicePath.Thickness = OuterArcThickness;
    
            var middleRadian = (Math.PI / 180) * (StartAngle + (Angle / 2));

            if (!OriginalRadialMenuButton.HasOuterArcAction)
            {
                OuterPieSlicePath.Fill = new SolidColorBrush(OuterDisabledColor);
            }
            else
            {
                OuterPieSlicePath.Fill = new SolidColorBrush(OuterNormalColor);
                OuterPieSlicePath.PointerPressed += outerPieSlicePath_PointerPressed;
                OuterPieSlicePath.PointerReleased += outerPieSlicePath_PointerReleased;
                OuterPieSlicePath.PointerEntered += outerPieSlicePath_PointerEntered;
                OuterPieSlicePath.PointerExited += outerPieSlicePath_PointerExited;

                // Setup Caret
                CaretRotateTransform.Angle = (StartAngle + (Angle / 2));
                CaretTranslate.X = (Radius-OuterArcThickness/2 + 3) * Math.Sin(middleRadian);
                CaretTranslate.Y = -(Radius-OuterArcThickness/2 + 3) * Math.Cos(middleRadian);
            }

            // Setup inner arc
            InnerPieSlicePath.Radius = Radius - OuterArcThickness;
            InnerPieSlicePath.StartAngle = StartAngle;
            InnerPieSlicePath.Angle = Angle;
            InnerPieSlicePath.Fill = new SolidColorBrush(InnerNormalColor);

            // Setup textbox for custom type RadialMenuButton
            if (OriginalRadialMenuButton.Type == RadialMenuButton.ButtonType.Custom) CreateCustomTextBox();

            // Stroke
            OuterPieSlicePath.StrokeThickness = StrokeThickness;
            OuterPieSlicePath.Stroke = new SolidColorBrush(StrokeColor);
            InnerPieSlicePath.StrokeThickness = StrokeThickness;
            InnerPieSlicePath.Stroke = new SolidColorBrush(StrokeColor);

            // Setup icon and text
            IconTranslate.X = ((Radius - OuterArcThickness) / 2 + 20) * Math.Sin(middleRadian);
            IconTranslate.Y = -((Radius - OuterArcThickness) / 2 + 20) * Math.Cos(middleRadian);

            // Setup Access Key Popups
            InnerAccessKeyPopup.HorizontalOffset = Radius;
            InnerAccessKeyPopup.VerticalOffset = Radius;
            OuterAccessKeyPopup.HorizontalOffset = Radius;
            OuterAccessKeyPopup.VerticalOffset = Radius;
            OuterAccessKeyPopupTranslate.X = (Radius - OuterArcThickness / 2 + 3) * Math.Sin(middleRadian);
            OuterAccessKeyPopupTranslate.Y = -(Radius - OuterArcThickness / 2 + 3) * Math.Cos(middleRadian);

            // Go to correct visual state
            UpdateSliceForToggle();
            UpdateSliceForRadio();
        }

        /// <summary>
        /// Creates a textbox to allow input of custom values for custom type RadialMenuButtons
        /// </summary>
        private void CreateCustomTextBox()
        {
            CustomTextBox = new TextBox
            {
                Name = "CustomTextBox",
                FontSize = LabelSize,
                Foreground = LabelColor,
                Margin = new Thickness(0, 67, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BorderThickness = new Thickness(0),
                TextAlignment = TextAlignment.Center,
                Background = new SolidColorBrush(Colors.Transparent),
                AcceptsReturn = false,
                Style = (Style)this.Resources["TransparentTextBox"]
            };

            CustomTextBox.Padding = new Thickness(0);

            CustomTextBox.GotFocus += (sender, args) => LabelTextElement.Opacity = 0;
            CustomTextBox.LostFocus += (sender, args) =>
            {
                OriginalRadialMenuButton.Value = ((TextBox)sender).Text;
                LabelTextElement.Opacity = 1;
                OriginalRadialMenuButton.OnValueChanged(args);

            };
            CustomTextBox.SetBinding(TextBox.TextProperty, new Windows.UI.Xaml.Data.Binding() { Source = this.CustomValue });

            TextLabelGrid.Children.Add(CustomTextBox);
        }

        /// <summary>
        /// Programmatically "click" the inner arc in the PieSlice
        /// </summary>
        public void ClickInner()
        {
            innerPieSlicePath_PointerPressed(this, new RoutedEventArgs() as PointerRoutedEventArgs);
            innerPieSlicePath_PointerReleased(this, new RoutedEventArgs() as PointerRoutedEventArgs);
        }

        /// <summary>
        /// Programmatically "click" the outer arc in the PieSlice
        /// </summary>
        public void ClickOuter()
        {
            outerPieSlicePath_PointerPressed(this, new RoutedEventArgs() as PointerRoutedEventArgs);
            outerPieSlicePath_PointerReleased(this, new RoutedEventArgs() as PointerRoutedEventArgs);
        }

        /// <summary>
        /// Event handler for a pointer press (mouse, touch, stylus) for on the outer arc of the PieSlice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void outerPieSlicePath_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "OuterPressed", true);

            // Check for Submenu & Custom Menu
            if (OriginalRadialMenuButton.Submenu != null)
            {
                OnChangeMenuRequest(OriginalRadialMenuButton, OriginalRadialMenuButton.Submenu);
            }
            else if (OriginalRadialMenuButton.CustomMenu != null)
            {
                OnChangeMenuRequest(OriginalRadialMenuButton, OriginalRadialMenuButton.CustomMenu);
            }

            OriginalRadialMenuButton.OnOuterArcPressed(e);
        }

        /// <summary>
        /// Event handler for a pointer relese (mouse, touch, stylus) on the outer arc of the PieSlice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void outerPieSlicePath_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "OuterHover", true);
            OriginalRadialMenuButton.OnOuterArcReleased(e);
        }

        /// <summary>
        /// Event handler for a pointer enter (mouse, touch, stylus) on the outer arc of the PieSlice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void outerPieSlicePath_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "OuterHover", true);
        }

        /// <summary>
        /// Event handler for a pointer exit (mouse, touch, stylus) on the outer arc of the PieSlice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void outerPieSlicePath_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "OuterNormal", true);
        }

        /// <summary>
        ///  Event handler for a pointer enter (mouse, touch, stylus) on the inner arc of the PieSlice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void innerPieSlicePath_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            switch (OriginalRadialMenuButton.Type)
            {
                case RadialMenuButton.ButtonType.Toggle:
                    VisualStateManager.GoToState(this, (OriginalRadialMenuButton.Value != null && ((bool)OriginalRadialMenuButton.Value)) ? "InnerReleased" : "InnerHover", true);
                    break;
                case RadialMenuButton.ButtonType.Radio:
                    VisualStateManager.GoToState(this, OriginalRadialMenuButton.MenuSelected ? "InnerReleased" : "InnerHover", true);
                    break;
                default:
                    VisualStateManager.GoToState(this, "InnerHover", true);
                    break;
            }
        }

        /// <summary>
        ///  Event handler for a pointer exit (mouse, touch, stylus) on the inner arc of the PieSlice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void innerPieSlicePath_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            switch (OriginalRadialMenuButton.Type)
            {
                case RadialMenuButton.ButtonType.Toggle:
                    UpdateSliceForToggle();
                    break;
                case RadialMenuButton.ButtonType.Radio:
                    UpdateSliceForRadio();
                    break;
                default:
                    VisualStateManager.GoToState(this, "InnerNormal", true);
                    break;
            }
        }

        /// <summary>
        ///  Event handler for a pointer press (mouse, touch, stylus) on the inner arc of the PieSlice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void innerPieSlicePath_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (OriginalRadialMenuButton.Type == RadialMenuButton.ButtonType.Custom)
            {
                CustomTextBox.Focus(FocusState.Keyboard);
                CustomTextBox.SelectAll();
                e.Handled = true;

            }
            else
            {
                VisualStateManager.GoToState(this, "InnerPressed", true);
                OriginalRadialMenuButton.OnInnerArcPressed(e);
                switch (OriginalRadialMenuButton.Type)
                {
                    case RadialMenuButton.ButtonType.Toggle:
                        VisualStateManager.GoToState(this,
                            (OriginalRadialMenuButton.Value != null && ((bool)OriginalRadialMenuButton.Value))
                                ? "InnerReleased"
                                : "InnerNormal", true);
                        break;
                    case RadialMenuButton.ButtonType.Radio:
                        VisualStateManager.GoToState(this, "InnerReleased", true);
                        // get all other menus to release now that this menu has been selected
                        ChangeSelectedEvent?.Invoke(sender, this);
                        break;
                    default:
                        VisualStateManager.GoToState(this, "InnerNormal", true);
                        break;
                }
            }
        }

        /// <summary>
        ///  Event handler for a pointer release (mouse, touch, stylus) on the inner arc of the PieSlice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void innerPieSlicePath_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            OriginalRadialMenuButton.OnInnerArcReleased(e);

            switch (OriginalRadialMenuButton.Type)
            {
                case RadialMenuButton.ButtonType.Toggle:
                    VisualStateManager.GoToState(this,
                        (OriginalRadialMenuButton.Value != null && ((bool) OriginalRadialMenuButton.Value))
                            ? "InnerReleased"
                            : "InnerNormal", true);
                    break;
                case RadialMenuButton.ButtonType.Radio:
                    VisualStateManager.GoToState(this, "InnerReleased", true);
                    // get all other menus to release now that this menu has been selected
                    ChangeSelectedEvent?.Invoke(sender, this);
                    break;
                default:
                    VisualStateManager.GoToState(this, "InnerNormal", true);
                    break;
            }
        }

        /// <summary>
        /// If the PieSlice has been generated by a "radio" RadialMenuButton, this method ensures the correct visual state
        /// </summary>
        public void UpdateSliceForRadio()
        {
            if (OriginalRadialMenuButton.Type != RadialMenuButton.ButtonType.Radio) return;
            VisualStateManager.GoToState(this, OriginalRadialMenuButton.MenuSelected ? "InnerReleased" : "InnerNormal", true);
        }

        /// <summary>
        /// If the PieSlice has been generated by a "toggle" RadialMenuButton, this method ensures the correct visual state
        /// </summary>
        public void UpdateSliceForToggle()
        {
            if (OriginalRadialMenuButton.Type != RadialMenuButton.ButtonType.Toggle) return;
            VisualStateManager.GoToState(this, (OriginalRadialMenuButton.Value != null && ((bool)OriginalRadialMenuButton.Value)) ? "InnerReleased" : "InnerNormal", true);
        }
    }
}
