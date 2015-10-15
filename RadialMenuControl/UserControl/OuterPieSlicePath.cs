namespace RadialMenuControl.UserControl
{
    using System;
    using System.Diagnostics;
    using Windows.Foundation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;

    public class OuterPieSlicePath : PathBase
    {
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(PieSlice),
                new PropertyMetadata(DependencyProperty.UnsetValue, (s, e) => { Changed(s as PathBase); }));

        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(double), typeof(PieSlice),
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
        /// Thickness for the path
        /// </summary>
        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        /// <summary>
        /// Override for the path's redraw mehtod, controlling how the path is drawns
        /// </summary>
        protected override void Redraw()
        {
            Debug.Assert(GetValue(StartAngleProperty) != DependencyProperty.UnsetValue);
            Debug.Assert(GetValue(RadiusProperty) != DependencyProperty.UnsetValue);
            Debug.Assert(GetValue(AngleProperty) != DependencyProperty.UnsetValue);

            if (Radius == 0 || !(Thickness > 0)) return;

            Width = Height = 2 * (Radius);
            var endAngle = StartAngle + Angle;
            var smallRadius = Radius - Thickness;
           
            var startX = Radius + Math.Sin(StartAngle * Math.PI / 180) * Radius;
            var startY = Radius - Math.Cos(StartAngle * Math.PI / 180) * Radius;

            // path container
            var figure = new PathFigure
            {
                StartPoint = new Point(startX, startY),
                IsClosed = true,
                IsFilled = true
            };

            // outer arc
            var outerArcX = Radius + Math.Sin(endAngle * Math.PI / 180) * Radius;
            var outerArcY = Radius - Math.Cos(endAngle * Math.PI / 180) * Radius;
            var outerArc = new ArcSegment
            {
                IsLargeArc = Angle >= 180.0,
                Point = new Point(outerArcX, outerArcY),
                Size = new Size(Radius, Radius),
                SweepDirection = SweepDirection.Clockwise,
            };
            figure.Segments.Add(outerArc);

            // start angle line
            var lineX = smallRadius + Math.Sin(endAngle * Math.PI / 180) * smallRadius;
            var lineY = smallRadius - Math.Cos(endAngle * Math.PI / 180) * smallRadius;
            var line = new LineSegment { Point = new Point(lineX + Thickness, lineY + Thickness) };
            figure.Segments.Add(line);

            // inner arc
            var innerArcX = smallRadius + Math.Sin(StartAngle * Math.PI / 180) * smallRadius ;
            var innerArcY = smallRadius - Math.Cos(StartAngle * Math.PI / 180) * smallRadius;
            var innerArc = new ArcSegment
            {
                IsLargeArc = Angle >= 180.0,
                Point = new Point(innerArcX + Thickness, innerArcY + Thickness),
                Size = new Size(smallRadius, smallRadius),
                SweepDirection = SweepDirection.Counterclockwise,
            };
            figure.Segments.Add(innerArc);

            Data = new PathGeometry { Figures = { figure } };
            InvalidateArrange();
        }
    }
}
