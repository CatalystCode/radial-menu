using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
namespace RadialMenuControl.UserControl
{
    class MeterSubmenuPath : PathBase
    {
        
        #region properties
        public void Draw()
        {
            Redraw();
        }

        public double MeterStartValue
        {
            get; set;
        }

        public double MeterEndValue
        {
            get; set;
        }

        public double TickInterval
        {
            get; set;
        }
        
        public double MeterRadius
        {
            get; set;
        }
        private double _labelOffset;
        public double LabelOffset
        {
            get
            {
                return _labelOffset;
            }
            set
            {
                _labelOffset = value;
            }
        }
        public IList<Point> MeterTickPoints
        {
            get; set;
        }
        #endregion

        /// <summary>
        /// Constructs a new MeterSubmenuPath
        /// </summary>
        public MeterSubmenuPath() : base()
        {
            MeterTickPoints = new List<Point>();
        }

        /// <summary>
        /// Draws the scale ticks, with an invisible circle with r=MeterRadius bisecting each tick
        /// </summary>
        /// <param name="tickLength">The length of each tick</param>
        /// <param name="group">The geometry group to add each tick to</param>
        /// <param name="startAngle">The angle to start drawing ticks at, relative to the negative Y axis</param>
        private void DrawScale(double tickLength, GeometryGroup group, double startAngle = 0.0)
        {
            double radianInterval = 2 * Math.PI * (TickInterval / (MeterEndValue - MeterStartValue));
            uint tickCount = (uint)(2 * Math.PI / radianInterval);

            MeterTickPoints?.Clear();
            for (var i = 0; i < tickCount; i++)
            {
                PathGeometry pathGeometry = new PathGeometry();
                PathFigure figure = new PathFigure();

                double x1 = (MeterRadius - (tickLength / 2)) * Math.Sin(startAngle),
                       y1 = (MeterRadius - (tickLength / 2)) * Math.Cos(startAngle),
                       x2 = (MeterRadius + (tickLength / 2)) * Math.Sin(startAngle),
                       y2 = (MeterRadius + (tickLength / 2)) * Math.Cos(startAngle),
                       labelX = (MeterRadius + LabelOffset + (tickLength / 2)) * Math.Sin(startAngle),
                       labelY = (MeterRadius + LabelOffset + (tickLength / 2)) * Math.Cos(startAngle);


                figure.StartPoint = new Point(Radius + x1, Radius - y1);
                
                var line = new LineSegment
                {
                    Point = new Point(Radius + x2, Radius - y2)
                };
                MeterTickPoints.Add(new Point(Radius + labelX, Radius - labelY));
                figure.Segments.Add(line);
                
                pathGeometry.Figures.Add(figure);
                group.Children.Add(pathGeometry);
                startAngle += radianInterval;
            }

            var scale = new EllipseGeometry()
            {
                RadiusX = MeterRadius,
                RadiusY = MeterRadius,
                Center = new Point(Radius, Radius)
            };

           // group.Children.Add(scale);

        }

        /// <summary>
        /// Redraws this meter
        /// </summary>
        protected override void Redraw()
        {
            var group = new GeometryGroup();
            if (TickInterval > 0)
            {
                if ((MeterEndValue - MeterStartValue) % TickInterval != 0.0)
                {
                    throw new InvalidOperationException("Meter tick interval must divide evenly into MeterEndValue - MeterStartValue");
                }
                DrawScale(10.0, group, StartAngle);
            }
            
            Data = group;
            InvalidateArrange();
        }
            
    }
}
