namespace RadialMenuControl.UserControl
{
    using RadialMenuControl.UserControl;
    using System;
    using System.Diagnostics;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    public sealed partial class PieSlice : UserControl
    {
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(PieSlice), null);

        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor", typeof(string), typeof(PieSlice), null);

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(PieSlice), null);

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(PieSlice), null);

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(PieSlice), null);

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty HighlightColorProperty =
            DependencyProperty.Register("HighlightColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty HideLabelProperty =
            DependencyProperty.Register("HideLabel", typeof(bool), typeof(PieSlice), null);

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value.ToUpperInvariant()); }
        }

        public Color ForegroundColor
        {
            get { return (Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public Color HighlightColor
        {
            get { return (Color)GetValue(HighlightColorProperty); }
            set { SetValue(HighlightColorProperty, value); }
        }

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public bool HideLabel
        {
            get { return (bool)GetValue(HideLabelProperty); }
            set { SetValue(HideLabelProperty, value); }
        }

        public PieSlice()
        {
            this.InitializeComponent();
            this.DataContext = this;

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // TODO: Check if we have a submenu, "grey out" if we don't
            outerPieSlicePath.Radius = this.Radius;
            outerPieSlicePath.StartAngle = this.StartAngle;
            outerPieSlicePath.Angle = this.Angle;
            outerPieSlicePath.Fill = new SolidColorBrush(this.ForegroundColor);

            innerPieSlicePath.Radius = this.Radius - 20;
            innerPieSlicePath.StartAngle = this.StartAngle;
            innerPieSlicePath.Angle = this.Angle;
            innerPieSlicePath.Fill = new SolidColorBrush(this.BackgroundColor);

            // Calculating a point in the "direction" of our button
            double middleRadian = (Math.PI / 180) * (this.StartAngle + (this.Angle / 2));
            //textBlockTranslate.X = -100 * Math.Cos(middleRadian);
            //textBlockTranslate.Y = 100 * Math.Sin(middleRadian);
        }

        private void outerPieSlicePath_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // TODO: Navigate to Submenu, if one exists
            Debug.Write("Slice Tapped");
        }

        private void outerPieSlicePath_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // TODO: Check if we have a submenu, otherwise don't highlight
            VisualStateManager.GoToState(this, "OuterMouseOver", true);
        }

        private void outerPieSlicePath_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "OuterNormal", true);
        }

        private void innerPieSlicePath_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void innerPieSlicePath_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "InnerMouseOver", true);
        }

        private void innerPieSlicePath_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "InnerNormal", true);
        }
    }
}
