using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.Foundation;
using RadialMenuControl.Components;

namespace RadialMenuControl.UserControl
{
    using Themes;
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

        #region properties
        private double _meterStartValue;
        public double MeterStartValue
        {
            get
            {
                return _meterStartValue;
            }
            set
            {
                SetField(ref _meterStartValue, value);
            }
        }
        
        private double _meterEndValue;
        public double MeterEndValue
        {
            get
            {
                return _meterEndValue;
            }
            set
            {
                SetField(ref _meterEndValue, value);
            }
        }
        
        private double _meterPointerLength;
        public double MeterPointerLength
        {
            set
            {
                SetField(ref _meterPointerLength, value);
            }
            get
            {
                return _meterPointerLength;
            }
        }

        private double _meterRadius;
        public double MeterRadius
        {
            set
            {
                SetField(ref _meterRadius, value);
            }
            get
            {
                return _meterRadius;
            }
        }
        
        public Size MeterSize => new Size(MeterRadius, MeterRadius);

        private Point _meterEndPoint;
        public Point MeterEndPoint
        {
            set { SetField(ref _meterEndPoint, value); }
            get { return _meterEndPoint; }
            
        }

        private Point _meterStartPoint;
        public Point MeterStartPoint
        {
            set { SetField(ref _meterStartPoint, value); }
            get { return _meterStartPoint; }

        }

        /// <summary>
        /// The meter sweep angle, based on the number of Intervals provided
        /// </summary>
        public double MeterArcSweepAngle
        {
            get
            {
                return Intervals.Sum(i => (i.EndDegree - i.StartDegree));
            }
        }
        private double _meterTextX;

        /// <summary>
        /// The X position of the value indicator
        /// </summary>
        public double MeterTextX
        {
            set
            {
                SetField(ref _meterTextX, value);
            }
            get
            {
                return _meterTextX;
            }
        }

        private double _meterTextY;
        public double MeterTextY
        {
            set
            {
                SetField(ref _meterTextY, value);
            }
            get
            {
                return _meterTextY;
            }
        }
        /// <summary>
        /// The value the user is currently hovering over
        /// </summary>
        private double _selectedValue;
        public double SelectedValue
        {
            set
            {
                SetField(ref _selectedValue, value);
            }
            get
            {
                return _selectedValue;
            }
        }

        /// <summary>
        /// The value the user has selected
        /// </summary>
        private double _lockedValue;
        public double LockedValue
        {
            set
            {
                SetField(ref _lockedValue, value);
            }
            get
            {
                return _lockedValue;
            }
        }

        private Brush _outerEdgeBrush = new SolidColorBrush(DefaultColors.HighlightColor);
        public Brush OuterEdgeBrush
        {
            set
            {
                SetField(ref _outerEdgeBrush, value);
            }
            get
            {
                return _outerEdgeBrush;
            }
        }

        private Brush _backgroundFillBrush = new SolidColorBrush(DefaultColors.InnerNormalColor);
        public Brush BackgroundFillBrush
        {
            set
            {
                SetField(ref _backgroundFillBrush, value);
            }
            get
            {
                return _backgroundFillBrush;
            }
        }

        private Brush _selectedValueBrush = new SolidColorBrush(DefaultColors.MeterSelectorColor);
        public Brush SelectedValueBrush
        {
            set
            {
                SetField(ref _selectedValueBrush, value);
            }
            get
            {
                return _selectedValueBrush;
            }
        }

        private Brush _selectedValueTextBrush = new SolidColorBrush(DefaultColors.HighlightColor);
        public Brush SelectedValueTextBrush
        {
            set
            {
                SetField(ref _selectedValueTextBrush, value);
            }
            get
            {
                return _selectedValueTextBrush;
            }
        }

        private Brush _hoverValueBrush = new SolidColorBrush(DefaultColors.HighlightColor);
        public Brush HoverValueBrush
        {
            set
            {
                SetField(ref _hoverValueBrush, value);
            }
            get
            {
                return _hoverValueBrush;
            }
        }

        private Brush _meterLineBrush = new SolidColorBrush(DefaultColors.MeterLineColor);
        public Brush MeterLineBrush
        {
            set
            {
                SetField(ref _meterLineBrush, value);
            }
            get
            {
                return _meterLineBrush;
            }
        }

        /// <summary>
        /// Start Angle
        /// </summary>
        private double _startAngle = 22.5;
        public double StartAngle
        {
            get { return _startAngle; }
            set
            {
                SetField(ref _startAngle, value);
            }
        }

        /// <summary>
        /// The length of the incremental ticks on the raidal meter
        /// </summary>
        private double _tickLength = 5;
        public double TickLength
        {
            get { return _tickLength; }
            set
            {
                SetField(ref _tickLength, value);
            }
        }

        /// <summary>
        /// When true, selected value event will return the number selected
        /// rounded to the nearest integer number
        /// </summary>
        private bool _roundSelectValue = true;
        public bool RoundSelectValue
        {
            get { return _roundSelectValue; }
            set
            {
                SetField(ref _roundSelectValue, value);
            }
        }

        /// <summary>
        /// Radius of the whole control
        /// </summary>
        public double Radius => Diameter / 2;

        private Point _center;
        public Point Center
        {
            get { return _center; }
            set { SetField(ref _center, value); }
        }
        
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

        public double OuterCircleRadius => Radius - 10;

        public IList<MeterRangeInterval> Intervals = new List<MeterRangeInterval>();

        #endregion

        /// <summary>
        /// Sets the projecction end point for the meter lines
        /// </summary>
        /// <param name="point"></param>
        /// <param name="setSelectedLine"></param>
        private void SetMeterPoint(Point point, bool checkRange = false, bool setSelectedLine = false)
        {

            double angle;
            var newPoint = ComputeMeterLinePoint(MeterPointerLength, point, out angle);


            var startAngleRad = StartAngle * (Math.PI / 180);
            angle -= startAngleRad;

            if (angle < 0)
            {
                angle += (2 * Math.PI);
            }

            MeterTextX = Diameter / 2 - (SelectedValueBlock.ActualWidth / 2);
            MeterTextY = Diameter - (40 + SelectedValueBlock.ActualHeight / 2);

            var inRange = ComputeSelectedValue(angle);

            if (!inRange && checkRange)
            {
                return;
            }
            MeterLine.Point = newPoint;

            if (setSelectedLine)
            {
                SelectedValueLine.Point = ComputeMeterLinePoint(MeterPointerLength, point, out angle);
            }
        }

        /// <summary>
        /// Computes the selected value by the user. Returns false if no value selected
        /// </summary>
        /// <param name="angle"></param>
        private bool ComputeSelectedValue(double angle)
        {
            bool selected = false;
            // Scan each meter interval segment and calculate selected value
            foreach (var interval in Intervals)
            {
                double startRad = interval.StartDegree*(Math.PI/180),
                    endRad = interval.EndDegree*(Math.PI/180);

                if (angle > startRad &&
                    angle < endRad)
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
            var y = x / Math.Tan(theta);
            angle = theta;
            return new Point(x + Center.X, Center.Y - y);
        }

        public void Draw()
        {
            Path.Radius = Diameter / 2;
            Path.Intervals = Intervals;
            Path.MeterStartValue = MeterStartValue;
            Path.MeterEndValue = MeterEndValue;
            Path.MeterRadius = MeterRadius;
            Path.StartAngle = StartAngle * (Math.PI / 180);
            Path.LabelOffset = 10;
            Path.Draw();

            // set the meter arc

            MeterStartPoint = Path.MeterTickPoints[0].Point;
            MeterEndPoint = Path.MeterTickPoints[Path.MeterTickPoints.Count - 1].Point;

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
        public delegate void ValueSelectedHandler(object sender, TappedRoutedEventArgs args);
        public event ValueSelectedHandler ValueSelected;
  
        public MeterSubMenu()
        {
            InitializeComponent();
            DataContext = this;
 
            PointerMoved += (sender, args) =>
            {
                var point = args.GetCurrentPoint(sender as UIElement);
                SetMeterPoint(point.Position, true, false);
            };

            PointerExited += (sender, args) =>
            {
                MeterLinePath.Visibility = Visibility.Collapsed;
                SelectedValue = LockedValue;
                SelectedValueTextBrush = SelectedValueBrush;
            };

            PointerEntered += (sender, args) =>
            {
                MeterLinePath.Visibility = Visibility.Visible;
                SelectedValueTextBrush = MeterLineBrush;
            };

            Tapped += (sender, args) =>
            {
                var point = args.GetPosition((sender as UIElement));
                SetMeterPoint(point, true, true);
                LockedValue = SelectedValue;
                ValueSelected?.Invoke(this, args);
            };

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Diameter")
                {
                    Center = new Point(Diameter / 2, Diameter / 2);
                    CenterButton.Top = Diameter / 2 - CenterButton.Width / 2;
                    CenterButton.Left = Diameter / 2 - CenterButton.Width / 2;
                }
                
            };
 
            Loaded += (sender, e) =>
            {
                // point meter to left initially
                SetMeterPoint(new Point(0, Diameter / 2), false, true);
                SelectedValue = Intervals.Count > 0 ? Intervals[0].StartValue : 0;
                LockedValue = SelectedValue;
                Draw();
            };
        }
    }
}
