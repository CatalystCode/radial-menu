using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        // StartAngle
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(PieSlice),
                new PropertyMetadata(default(double), (s, e) => { Changed(s as PathBase); }));

        // Radius
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(PieSlice),
            new PropertyMetadata(DependencyProperty.UnsetValue, (s, e) => { Changed(s as PathBase); }));

        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(double), typeof(PieSlice),
            new PropertyMetadata(DependencyProperty.UnsetValue, (s, e) => { Changed(s as PathBase); }));
        #endregion

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        protected static void Changed(PathBase path)
        {
            if (path._isLoaded)
            {
                path.Redraw();
            }
        }

        protected bool _isLoaded;
        
        /// <summary>
        /// Specifies how to draw this path
        /// </summary>
        protected abstract void Redraw();

        /// <summary>
        /// Constructs a new PathBase instance
        /// </summary>
        public PathBase()
        {
            Loaded += (s, e) =>
            {
                _isLoaded = true;
                Redraw();
            };
        }
    }
}
