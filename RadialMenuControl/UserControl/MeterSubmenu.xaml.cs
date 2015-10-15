using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.ApplicationModel.Store;
using Windows.Devices.PointOfService;
using Windows.Foundation;
using Windows.UI.Notifications;
using RadialMenuControl.Components;

namespace RadialMenuControl.UserControl
{
    using Themes;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// Describes a value range on the meter
    /// </summary>
    public class MeterRangeInterval
    {
        public double StartDegree { get; set; }
        public double EndDegree { get; set; }
        public double StartValue { get; set; }
        public double EndValue { get; set; }
        public double TickInterval { get; set; }
    }

    public partial class MeterSubMenu : MenuBase
    {
        #region dependency_properties
        public static readonly DependencyProperty MeterStartValueProperty =
            DependencyProperty.Register("MeterStartValue", typeof(double), typeof(MeterSubMenu), new PropertyMetadata(0.0, DependencyPropertyChanged));
        public static readonly DependencyProperty MeterStartPointProperty =
            DependencyProperty.Register("MeterStartPoint", typeof(Point), typeof(MeterSubMenu), new PropertyMetadata(new Point(0,0), DependencyPropertyChanged));
        public static readonly DependencyProperty MeterEndValueProperty =
            DependencyProperty.Register("MeterEndValue", typeof(double), typeof(MeterSubMenu), new PropertyMetadata(100, DependencyPropertyChanged));
        public static readonly DependencyProperty MeterPointerLengthProperty =
            DependencyProperty.Register("MeterPointerLength", typeof(double), typeof(MeterSubMenu), new PropertyMetadata(5.0, DependencyPropertyChanged));
        public static readonly DependencyProperty MeterRadiusProperty =
            DependencyProperty.Register("MeterRadius", typeof(double), typeof(MeterSubMenu), new PropertyMetadata(50.0, DependencyPropertyChanged));
        public static readonly DependencyProperty MeterEndPointProperty =
            DependencyProperty.Register("MeterEndPoint", typeof(Point), typeof(MeterSubMenu), new PropertyMetadata(new Point(50, 50), DependencyPropertyChanged));
        public static readonly DependencyProperty MeterTextXProperty =
            DependencyProperty.Register("MeterTextX", typeof(double), typeof(MeterSubMenu), new PropertyMetadata(30, DependencyPropertyChanged));
        public static readonly DependencyProperty MeterTextYProperty =
            DependencyProperty.Register("MeterTextY", typeof(double), typeof(MeterSubMenu), new PropertyMetadata(30, DependencyPropertyChanged));
        public static readonly DependencyProperty OuterEdgeBrushProperty =
            DependencyProperty.Register("OuterEdgeBrush", typeof(Brush), typeof(MeterSubMenu), new PropertyMetadata(new SolidColorBrush(DefaultColors.HighlightColor), DependencyPropertyChanged));
        public static readonly DependencyProperty OuterEdgeThicknessProperty =
            DependencyProperty.Register("OuterEdgeThickness", typeof(Double), typeof(MeterSubMenu), new PropertyMetadata((double)20, DependencyPropertyChanged));
        public static readonly DependencyProperty BackgroundFillBrushProperty =
            DependencyProperty.Register("BackgroundFillBrush", typeof(Brush), typeof(MeterSubMenu), new PropertyMetadata(new SolidColorBrush(DefaultColors.InnerNormalColor), DependencyPropertyChanged));
        public static readonly DependencyProperty HoverValueBrushProperty =
            DependencyProperty.Register("HoverValueBrush", typeof(Brush), typeof(MeterSubMenu), new PropertyMetadata(new SolidColorBrush(DefaultColors.HighlightColor), DependencyPropertyChanged));
        public static readonly DependencyProperty SelectedValueBrushProperty =
            DependencyProperty.Register("SelectedValueBrush", typeof(Brush), typeof(MeterSubMenu), new PropertyMetadata(new SolidColorBrush(DefaultColors.MeterSelectorColor), DependencyPropertyChanged));
        public static readonly DependencyProperty SelectedValueTextBrushProperty =
            DependencyProperty.Register("SelectedValueTextBrush", typeof(Brush), typeof(MeterSubMenu), new PropertyMetadata(new SolidColorBrush(DefaultColors.HighlightColor), DependencyPropertyChanged));
        public static readonly DependencyProperty LockedValueProperty =
                    DependencyProperty.Register("LockedValue", typeof(double), typeof(MeterSubMenu), new PropertyMetadata((double)0, DependencyPropertyChanged));
        public static readonly DependencyProperty MeterLineBrushProperty =
            DependencyProperty.Register("MeterLineBrush", typeof(Brush), typeof(MeterSubMenu), new PropertyMetadata(new SolidColorBrush(DefaultColors.MeterLineColor), DependencyPropertyChanged));
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(MeterSubMenu), new PropertyMetadata(-90.0, DependencyPropertyChanged));
        public static readonly DependencyProperty TickLengthProperty =
            DependencyProperty.Register("TickLength", typeof(double), typeof(MeterSubMenu), new PropertyMetadata(new SolidColorBrush(DefaultColors.OuterTappedColor), DependencyPropertyChanged));
        public static readonly DependencyProperty RoundSelectValueProperty =
            DependencyProperty.Register("RoundSelectValue", typeof(bool), typeof(MeterSubMenu), new PropertyMetadata(new SolidColorBrush(DefaultColors.OuterTappedColor), DependencyPropertyChanged));
        #endregion
        
        /// <summary>
        /// Handler for changed dependency values
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void DependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var meterMenu = d as MeterSubMenu;

            if (e.Property == LockedValueProperty)
            {
                meterMenu._setMeterPointForValue((double)e.NewValue);
            }
            
            if (meterMenu._isLoaded)
            {
                meterMenu?.Draw();
            }
            
        }

        #region properties
        private bool _isLoaded = false;
        private bool _isPressed = false;
        /// <summary>
        /// Starting value of the meter. If set to 5, the first selectable value will be 5.
        /// </summary>
        public double MeterStartValue
        {
            get { return (double)GetValue(MeterStartValueProperty); }
            set { SetValue(MeterStartValueProperty, value); }
        }
        
        /// <summary>
        /// Ending value of the meter. If set to 5, the last selectable value will be 5.
        /// </summary>
        public double MeterEndValue
        {
            get { return (double)GetValue(MeterEndValueProperty); }
            set { SetValue(MeterEndValueProperty, value); }
        }
        
        /// <summary>
        /// Length of the pointer (the line drawn from the center of the menu towards the potential selection)
        /// </summary>
        public double MeterPointerLength
        {
            get { return (double)GetValue(MeterPointerLengthProperty); }
            set { SetValue(MeterPointerLengthProperty, value); }
            
        }
        
        /// <summary>
        /// Radius of the whole meter. If bigger, the user will have an easier time making fine-grained selections.
        /// </summary>
        public double MeterRadius
        {
            get { return (double)GetValue(MeterRadiusProperty); }
            set { SetValue(MeterRadiusProperty, value); }
        }
        
        /// <summary>
        /// Shortcut for the meter size as size.
        /// </summary>
        public Size MeterSize => new Size(MeterRadius, MeterRadius);
        
        /// <summary>
        /// Ending point for the meter
        /// </summary>
        public Point MeterEndPoint
        {
            get { return (Point)GetValue(MeterEndPointProperty); }
            set { SetValue(MeterEndPointProperty, value); }
        }
        
        /// <summary>
        /// Starting point for the meter
        /// </summary>
        public Point MeterStartPoint
        {
            get { return (Point)GetValue(MeterStartPointProperty); }
            set { SetValue(MeterStartPointProperty, value); }
        }

        /// <summary>
        /// The meter sweep angle, based on the number of Intervals provided
        /// </summary>
        public double MeterArcSweepAngle
        {
            get { return Intervals.Sum(i => (i.EndDegree - i.StartDegree)); }
        }
        
        /// <summary>
        /// The X position of the value indicator
        /// </summary>
        public double MeterTextX
        {
            get { return (double)GetValue(MeterTextXProperty); }
            set { SetValue(MeterTextXProperty, value); }
        }
        
        /// <summary>
        /// The Y position of the value indicator
        /// </summary>
        public double MeterTextY
        {
            get { return (double)GetValue(MeterTextYProperty); }
            set { SetValue(MeterTextYProperty, value); }
        }

        private double _selectedValue = 0;
        /// <summary>
        /// The value the user is currently hovering over
        /// </summary>
        public double SelectedValue
        {
            get { return _selectedValue; }
            set { SetField(ref _selectedValue, value); }
        }

        /// <summary>
        /// The value the user has selected
        /// </summary>
        public double LockedValue
        {
            set { SetValue(LockedValueProperty, value); }
            get { return (double)GetValue(LockedValueProperty); }
        }
        
        /// <summary>
        /// Color for the outer edge of the meter (analog to the outer arc on a RadialMenuButton)
        /// </summary>
        public Brush OuterEdgeBrush
        {
            set { SetValue(OuterEdgeBrushProperty, value); }
            get { return (Brush)GetValue(OuterEdgeBrushProperty); }
        }

        /// <summary>
        /// Thickness of the outer edge
        /// </summary>
        public Double OuterEdgeThickness
        {
            set { SetValue(OuterEdgeThicknessProperty, value); }
            get { return (Double)GetValue(OuterEdgeThicknessProperty); }
        }

        /// <summary>
        /// Color for the background of the meter
        /// </summary>
        public Brush BackgroundFillBrush
        {
            set { SetValue(BackgroundFillBrushProperty, value); }
            get { return (Brush)GetValue(BackgroundFillBrushProperty); }
        }

        /// <summary>
        /// Color for the pointer for the potentially selected value.
        /// </summary>
        public Brush HoverValueColor
        {
            set { SetValue(HoverValueBrushProperty, value); }
            get { return (Brush)GetValue(HoverValueBrushProperty); }
        }
        
        /// <summary>
        /// Color for the pointer for the selected value
        /// </summary>
        public Brush SelectedValueColor
        {
            set { SetValue(SelectedValueBrushProperty, value); }
            get { return (Brush)GetValue(SelectedValueBrushProperty); }
        }

        /// <summary>
        /// Color for the text label for the selected value
        /// </summary>
        public Brush SelectedValueTextColor
        {
            set { SetValue(SelectedValueTextBrushProperty, value); }
            get { return (Brush)GetValue(SelectedValueBrushProperty); }
        }

        /// <summary>
        /// Color for the meter line
        /// </summary>
        public Brush MeterLineColor
        {
            set { SetValue(MeterLineBrushProperty, value); }
            get { return (Brush)GetValue(MeterLineBrushProperty); }
        }
        
        /// <summary>
        /// Start Angle
        /// </summary>
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }
        
        /// <summary>
        /// The length of the incremental ticks on the raidal meter
        /// </summary>
        public double TickLength
        {
            get { return (double)GetValue(TickLengthProperty); }
            set { SetValue(TickLengthProperty, value); }
        }
  
        
        /// <summary>
        /// When true, selected value event will return the number selected
        /// rounded to the nearest integer number
        /// </summary>
        public bool RoundSelectValue
        {
            get { return (bool)GetValue(RoundSelectValueProperty); }
            set { SetValue(RoundSelectValueProperty, value); }
        }

        /// <summary>
        /// Radius of the whole control
        /// </summary>
        public double Radius => Diameter / 2;

        private Point _center;
        /// <summary>
        /// Center point of the meter
        /// </summary>
        public Point Center
        {
            get { return _center; }
            set { SetField(ref _center, value); }
        }
        
        /// <summary>
        /// Center button for this meter
        /// </summary>
        public override CenterButton CenterButton
        {
            get { return SubMenuCenterButton; }
            set { SetField(ref SubMenuCenterButton, value); }
        }

        /// <summary>
        /// When true, selected value event will return the number selected
        /// rounded to the nearest integer number
        /// </summary>
        public bool MeterIsLargeArc
        {
            get { return Intervals.Sum(i => (i.EndDegree + i.StartDegree)) > 180; }
        }

        /// <summary>
        /// Radius of the outer circle
        /// </summary>
        public double OuterCircleRadius => Radius - 10;

        /// <summary>
        /// A list containing defined intervals, allowing you to set custom intervals for the meter. 
        /// The upper half of the meter could contain values between 0 and 10, while the lower half
        /// could contain values between 10 and 50.
        /// </summary>
        public IList<MeterRangeInterval> Intervals = new List<MeterRangeInterval>();
        #endregion

        /// <summary>
        /// Sets the projection end point for the meter lines
        /// </summary>
        /// <param name="point"></param>
        /// <param name="setSelectedLine"></param>
        private void SetMeterPoint(Point point, bool checkRange = false, bool setSelectedLine = false)
        {
            double angle;
            var newPoint = ComputeMeterLinePoint(MeterPointerLength, point, out angle);

            var startAngleRad = StartAngle*(Math.PI/180);
            angle -= startAngleRad;

            if (angle < 0) angle += (2*Math.PI);

            MeterTextX = Diameter/2 - (SelectedValueBlock.ActualWidth/2);
            MeterTextY = Diameter - (40 + SelectedValueBlock.ActualHeight/2);

            var inRange = ComputeSelectedValue(angle);

            if (!inRange && checkRange) return;

            MeterLine.Point = newPoint;

            if (setSelectedLine) SelectedValueLine.Point = ComputeMeterLinePoint(MeterPointerLength, point, out angle);
        }

        /// <summary>
        /// Sets the locked meter line to a specified value
        /// </summary>
        /// <param name="selectedValue"></param>
        private void _setMeterPointForValue(double selectedValue)
        {
            var angle = ComputeAngle(selectedValue);

            double xDelta = Math.Sin((double)angle) * Radius,
                   yDelta = Math.Cos((double)angle) * Radius;
            var point = new Point(Center.X + xDelta, Center.Y - yDelta);
            SelectedValue = selectedValue;
            SetMeterPoint(point, true, true);
            
        }

        /// <summary>
        /// Computes the selected value by the user. Returns false if no value selected
        /// </summary>
        /// <param name="angle"></param>
        private bool ComputeSelectedValue(double angle)
        {
            var selected = false;
            
            // Scan each meter interval segment and calculate selected value
            foreach (var interval in Intervals)
            {
                double startRad = interval.StartDegree*(Math.PI/180),
                    endRad = interval.EndDegree*(Math.PI/180);

                if (Math.Round(angle, 5) >= Math.Round(startRad, 5) && Math.Round(angle, 5) <= Math.Round(endRad, 5))
                {
                    var value = Math.Abs(((angle - startRad) / (endRad - startRad)) * (interval.StartValue - interval.EndValue)) + interval.StartValue;
                    SelectedValue = RoundSelectValue ? Math.Round(value) : value;
                    selected = true;
                    break;

                }
            }

            return selected;
        }

        /// <summary>
        /// Computes and angle based on a selected value
        /// </summary>
        /// <param name="angle"></param>
        private double ComputeAngle(double selectedValue)
        {   
            // Scan each meter interval segment and calculate selected value
            foreach (var interval in Intervals)
            {
                double startRad = (interval.StartDegree + StartAngle) * (Math.PI / 180),
                       endRad = (interval.EndDegree + StartAngle) * (Math.PI / 180),
                       startVal = interval.StartValue,
                       endVal = interval.EndValue;

                // check if value falls within this interval
                if (selectedValue >= startVal && selectedValue <= endVal)
                {
                    // now that we're in a uniform interval on the meter
                    // the proportion of the value inside the interval is
                    // equal to the proprtion of the angle from interval.StartDegree
                    var proportion = (selectedValue - startVal) / (endVal - startVal);

                    return proportion * (endRad - startRad) + startRad;
                }
            }

            return 0;
        }

        /// <summary>
        /// Projects point from center to target point along a specified radius
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="pointerPoint"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private Point ComputeMeterLinePoint(double radius, Point pointerPoint, out double angle)
        {
            var theta = Math.Atan((pointerPoint.X - Center.X) / (Center.Y - pointerPoint.Y));

            if (pointerPoint.Y > Center.Y)
            {
                theta += Math.PI;
            }
            var x = radius * Math.Sin(theta);
            var y = theta == 0 ? radius : x / Math.Tan(theta);
            angle = theta;
            return new Point(x + Center.X, Center.Y - y);
        }

        /// <summary>
        /// Draws the meter, running all necessary math operations and making sure that pixels appear
        /// </summary>
        public void Draw()
        {
            Path.Radius = Diameter / 2;
            Path.Intervals = Intervals;
            Path.MeterStartValue = MeterStartValue;
            Path.MeterEndValue = MeterEndValue;
            Path.MeterRadius = MeterRadius;
            // All dep props are checked for null
            Path.StartAngle = (double)(StartAngle * (Math.PI / 180));
            Path.LabelOffset = 10;
            Path.Draw();

            // Set the meter arc
            if (Path.MeterTickPoints.Count > 0)
            {
                MeterStartPoint = Path.MeterTickPoints[0].Point;
                MeterEndPoint = Path.MeterTickPoints[Path.MeterTickPoints.Count - 1].Point;
            }
            
            foreach (var tick in Path.MeterTickPoints)
            {
                var tickLabel = new TextBlock
                {
                    Text = tick.Value.ToString(CultureInfo.CurrentCulture),
                    FontSize = 8
                };
                tickLabel.Measure(new Size());
                LayoutRoot.Children.Add(tickLabel);
                
                Canvas.SetTop(tickLabel, tick.LabelPoint.Y - (tickLabel.ActualHeight / 2));
                Canvas.SetLeft(tickLabel, tick.LabelPoint.X - (tickLabel.ActualWidth / 2));
                Canvas.SetZIndex(tickLabel, 100);
            }
        }

        /// <summary>
        /// Set default properties on this instance, if a setting has otherwise not been set
        /// </summary>
        /// <param name="prop">Property to set</param>
        /// <param name="defaultSetting">Default setting to apply</param>
        public void SetDefault(DependencyProperty prop, object defaultSetting)
        {
            if (ReadLocalValue(prop) == DependencyProperty.UnsetValue)
            {
                if (defaultSetting is Brush)
                {
                    var defaultBrush = defaultSetting as Brush;
                    SetValue(prop, defaultBrush);
                } else if (defaultSetting is double)
                {
                    var defaultDouble = (double)defaultSetting;
                    SetValue(prop, defaultDouble);
                }
            }
        }

        /// <summary>
        /// Delegate for the ValueSelectedEvent, fired whenever the user selects a value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void ValueSelectedHandler(object sender, PointerRoutedEventArgs args);
        public event ValueSelectedHandler ValueSelected;

        public MeterSubMenu()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;

            IsDraggable = false;
            PointerPressed += (sender, args) =>
            {
                _isPressed = true;
                MeterLinePath.Visibility = Visibility.Visible;
                SelectedValueTextColor = MeterLineColor;
                args.Handled = true;
            };

            PointerReleased += (sender, args) =>
            {
                _isPressed = false;
                // hide the selection meter line
                MeterLinePath.Visibility = Visibility.Collapsed;
                // set the locked selection meter line
                // set the locked on value
                LockedValue = SelectedValue;
                // modify the display text color
                SelectedValueTextColor = SelectedValueColor;
                var point = args.GetCurrentPoint(sender as UIElement);
                SetMeterPoint(point.Position, true, true);
                ValueSelected?.Invoke(this, args);
                
            };
            PointerMoved += (sender, args) =>
            {
                if (!_isPressed)
                {
                    return;
                }

                var point = args.GetCurrentPoint(sender as UIElement);
                SetMeterPoint(point.Position, true, false);
                // indicate event handled
                args.Handled = true;
            };

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName != "Diameter")
                {
                    return;
                }

                Center = new Point(Diameter / 2, Diameter / 2);
                CenterButton.Top = Diameter / 2 - CenterButton.Width / 2;
                CenterButton.Left = Diameter / 2 - CenterButton.Width / 2;

            };
 
            Loaded += (sender, e) =>
            {
                // point meter to left initially
                SetMeterPoint(new Point(0, Diameter / 2), false, true);
                _setMeterPointForValue(LockedValue);
                _isLoaded = true;
                Draw();
            };
        }
    }
}
