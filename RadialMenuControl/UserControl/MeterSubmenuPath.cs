using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace RadialMenuControl.UserControl
{
    public class TickPoint
    {
        public Point Point { get; set; }
        public Point LabelPoint { get; set; }
        public double Value { get; set; }
    }
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
        
        public double MeterRadius
        {
            get; set;
        }

        public double LabelOffset { get; set; }

        public IList<TickPoint> MeterTickPoints
        {
            get; set;
        }

        public double? TickLength { get; set; }

        public IList<MeterRangeInterval> Intervals { get; set; } 
        #endregion

        /// <summary>
        /// Constructs a new MeterSubmenuPath
        /// </summary>
        public MeterSubmenuPath() : base()
        {
            MeterTickPoints = new List<TickPoint>();
        }

        /// <summary>
        /// Draws the scale ticks, with an invisible circle with r=MeterRadius bisecting each tick
        /// </summary>
        /// <param name="tickLength">The length of each tick</param>
        /// <param name="group">The geometry group to add each tick to</param>
        /// <param name="startAngle">The angle to start drawing ticks at, relative to the negative Y axis</param>
        private void DrawScale(double tickLength, GeometryGroup group, double startAngle = 0.0)
        {
            MeterTickPoints?.Clear();
            if (Intervals == null)
            {
                return;
            }
            foreach (var interval in Intervals)
            {
                DrawInterval(interval, tickLength, group, startAngle);
                startAngle += (interval.EndDegree - interval.StartDegree)*(Math.PI/180);
            }
            

        }


        private void DrawInterval(MeterRangeInterval interval, double tickLength, GeometryGroup group,
            double startAngle = 0.0)
        {
            double startRad = interval.StartDegree*(Math.PI/180),
                endRad = interval.EndDegree*(Math.PI/180);
            double radianInterval = (endRad - startRad) * (interval.TickInterval / (interval.EndValue - interval.StartValue));
            uint tickCount = (uint)((endRad - startRad)/ radianInterval);

            
            for (var i = 0; i <= tickCount; i++)
            {
                PathGeometry pathGeometry = new PathGeometry();
                PathFigure figure = new PathFigure();

                // draw tick line
                double x1 = MeterRadius * Math.Sin(startAngle),
                       y1 = MeterRadius * Math.Cos(startAngle),
                       x2 = (MeterRadius + tickLength) * Math.Sin(startAngle),
                       y2 = (MeterRadius + tickLength) * Math.Cos(startAngle),
                       labelX = (MeterRadius + LabelOffset + (tickLength / 2)) * Math.Sin(startAngle),
                       labelY = (MeterRadius + LabelOffset + (tickLength / 2)) * Math.Cos(startAngle);


                figure.StartPoint = new Point(Radius + x1, Radius - y1);

                var line = new LineSegment
                {
                    Point = new Point(Radius + x2, Radius - y2)
                };
                MeterTickPoints?.Add(new TickPoint() {
                    // midway point in the tick - the point the tick crosses the meter circle
                    Point = new Point(Radius + (MeterRadius * Math.Sin(startAngle)), Radius - (MeterRadius * Math.Cos(startAngle))),
                    LabelPoint = new Point(Radius + labelX, Radius - labelY),
                    Value = i * interval.TickInterval + interval.StartValue
                });
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
        }
        /// <summary>
        /// Redraws this meter
        /// </summary>
        protected override void Redraw()
        {
            var group = new GeometryGroup();
            if (TickLength == null)
            {
                TickLength = 5;
            }
            DrawScale((double)TickLength, group, StartAngle);
            
            Data = group;
            InvalidateArrange();
        }
            
    }
}
