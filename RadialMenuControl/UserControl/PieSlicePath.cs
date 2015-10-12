namespace RadialMenuControl.UserControl
{
    using System;
    using System.Diagnostics;
    using Windows.Foundation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;

    public class PieSlicePath : PathBase
    {
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(PieSlice),
                new PropertyMetadata(DependencyProperty.UnsetValue, (s, e) => { Changed(s as PathBase); }));

        /// <summary>
        /// Angle for the path
        /// </summary>
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        /// <summary>
        /// Override for the path's redraw mehtod, controlling how the path is drawns
        /// </summary>
        protected override void Redraw()
        {
            // Reference:
            // http://blog.jerrynixon.com/2012/06/windows-8-animated-pie-slice.html
            
            Debug.Assert(GetValue(StartAngleProperty) != DependencyProperty.UnsetValue);
            Debug.Assert(GetValue(RadiusProperty) != DependencyProperty.UnsetValue);
            Debug.Assert(GetValue(AngleProperty) != DependencyProperty.UnsetValue);

            Width = Height = 2 * (Radius);
            var endAngle = StartAngle + Angle;

            // path container
            var figure = new PathFigure
            {
                StartPoint = new Point(Radius, Radius),
                IsClosed = true,
                IsFilled = true
            };

            //  start angle line
            var lineX = Radius + Math.Sin(StartAngle * Math.PI / 180) * Radius;
            var lineY = Radius - Math.Cos(StartAngle * Math.PI / 180) * Radius;
            var line = new LineSegment { Point = new Point(lineX, lineY) };
            figure.Segments.Add(line);

            // outer arc
            var arcX = Radius + Math.Sin(endAngle * Math.PI / 180) * Radius;
            var arcY = Radius - Math.Cos(endAngle * Math.PI / 180) * Radius;
            var arc = new ArcSegment
            {
                IsLargeArc = Angle >= 180.0,
                Point = new Point(arcX, arcY),
                Size = new Size(Radius, Radius),
                SweepDirection = SweepDirection.Clockwise,
            };

            figure.Segments.Add(arc);

            Data = new PathGeometry { Figures = { figure } };
            InvalidateArrange();
            
        }
    }
}
