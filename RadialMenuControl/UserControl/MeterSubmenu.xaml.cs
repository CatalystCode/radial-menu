using System;
using System.Globalization;
using Windows.Foundation;

namespace RadialMenuControl.UserControl
{
    using Themes;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;

    public partial class MeterSubMenu : MenuBase
    {

        #region properties
        private bool _valueSelected;
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

        private double _tickInterval;
        public double TickInterval
        {
            get
            {
                return _tickInterval;
            }
            set
            {
                SetField(ref _tickInterval, value);
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

        
        private double _meterTextX;
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

        private Brush _outerEdgeBrush; 
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

        private Brush _backgroundFillBrush;
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

        private Brush _selectedValueBrush = new SolidColorBrush(DefaultColors.HighlightColor);
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

        private Brush _hoverValueBrush = new SolidColorBrush(DefaultColors.MeterSelectorColor);
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
        
        public Button CenterButton
        {
            get { return SubMenuCenterButton; }
            set { SetField(ref SubMenuCenterButton, value); }
        }

        public double OuterCircleRadius => Radius - 10;

        #endregion

        private void SetMeterPoint(Point point, bool setSelectedLine = false)
        {

            double angle;
            MeterLine.Point = ComputeMeterLinePoint(MeterPointerLength, point, out angle);


            var startAngleRad = StartAngle * (Math.PI / 180);
            angle -= startAngleRad;

            if (angle < 0)
            {
                angle += (2 * Math.PI);
            }
            var value = Math.Abs(((angle) / (2 * Math.PI)) * (MeterEndValue - MeterStartValue));

            MeterTextX = Diameter / 2 - (Title.ActualWidth / 2);
            MeterTextY = Diameter - (40 + Title.ActualHeight / 2);

            if (!_valueSelected || setSelectedLine)
            {
                SelectedValue = RoundSelectValue ? Math.Round(value) : value;
            }


            if (setSelectedLine)
            {
                _valueSelected = true;
                SelectedValueLine.Point = ComputeMeterLinePoint(MeterPointerLength, point, out angle);
            }
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
            Path.MeterStartValue = MeterStartValue;
            Path.MeterEndValue = MeterEndValue;
            Path.MeterRadius = MeterRadius;
            Path.TickInterval = TickInterval;
            Path.StartAngle = StartAngle * (Math.PI / 180);
            Path.LabelOffset = 10;
            Path.Draw();

            var count = MeterStartValue;

            foreach (var tick in Path.MeterTickPoints)
            {
                TextBlock tickLabel = new TextBlock
                {
                    Text = count.ToString(CultureInfo.CurrentCulture),
                    FontSize = 8
                };
                tickLabel.Measure(new Size());
                LayoutRoot.Children.Add(tickLabel);
                
                Canvas.SetTop(tickLabel, tick.Y - (tickLabel.ActualHeight / 2));
                Canvas.SetLeft(tickLabel, tick.X - (tickLabel.ActualWidth / 2));
                Canvas.SetZIndex(tickLabel, 100);
                count += TickInterval;
            }
        }
        public delegate void ValueSelectedHandler(object sender, TappedRoutedEventArgs args);
        public event ValueSelectedHandler ValueSelected;
        public MeterSubMenu()
        {
            InitializeComponent();
            DataContext = this;
            BackgroundFillBrush = new SolidColorBrush(DefaultColors.InnerNormalColor);
            OuterEdgeBrush = new SolidColorBrush(DefaultColors.HighlightColor);
            PointerMoved += (sender, args) =>
            {
                var point = args.GetCurrentPoint(sender as UIElement);
                SetMeterPoint(point.Position);
            };

            Tapped += (sender, args) =>
            {
                var point = args.GetPosition((sender as UIElement));
                SetMeterPoint(point, true);
                ValueSelected?.Invoke(this, args);
            };

            PropertyChanged += (sender, args) =>
            {
                if(args.PropertyName == "Diameter")
                {
                    Center = new Point(Diameter / 2, Diameter / 2);
                }
                
            };
            Loaded += (sender, e) =>
            {
                // point meter to left initially
                SetMeterPoint(new Point(0, Diameter / 2), true);
                Draw();
            };
        }
    }
}
