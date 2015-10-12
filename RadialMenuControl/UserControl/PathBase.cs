using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
namespace RadialMenuControl.UserControl
{
    /// <summary>
    /// Baseclass for Custom Paths drawn within a Menu. Conatains basic tings like angles and radius
    /// </summary>
    public abstract class PathBase : Path
    {
        #region dependency_properties
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(PieSlice),
                new PropertyMetadata(default(double), (s, e) => { Changed(s as PathBase); }));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(PieSlice),
            new PropertyMetadata(DependencyProperty.UnsetValue, (s, e) => { Changed(s as PathBase); }));

        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(double), typeof(PieSlice),
            new PropertyMetadata(DependencyProperty.UnsetValue, (s, e) => { Changed(s as PathBase); }));
        #endregion

        /// <summary>
        /// Starting angle for this path object
        /// </summary>
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Radius for this path object
        /// </summary>
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        /// Thickness of this path object
        /// </summary>
        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        /// <summary>
        /// Helper method, called when a path has changed and needs to be redrawn
        /// </summary>
        /// <param name="path"></param>
        protected static void Changed(PathBase path)
        {
            if (path.IsLoaded)
            {
                path.Redraw();
            }
        }

        /// <summary>
        /// Has the path already been loaded?
        /// </summary>
        protected bool IsLoaded;
        
        /// <summary>
        /// Specifies how to draw this path
        /// </summary>
        protected abstract void Redraw();

        /// <summary>
        /// Constructs a new PathBase instance
        /// </summary>
        protected PathBase()
        {
            Loaded += (s, e) =>
            {
                IsLoaded = true;
                Redraw();
            };
        }
    }
}
