namespace RadialMenuControl.UserControl
{
    using Components;
    using Shims;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Windows.UI;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// The base class for any custom menus in the radial menu control
    /// </summary>
    public class MenuBase : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// The property changed event for this menu
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private Brush _centerButtonForeground = new SolidColorBrush(Colors.Black);
        /// <summary>
        /// Background Fill for the Center Button
        /// </summary>
        public Brush CenterButtonForeground
        {
            get { return _centerButtonForeground; }
            set { SetField(ref _centerButtonForeground, value); }
        }

        private Brush _centerButtonBackgroundFill = new SolidColorBrush(Colors.WhiteSmoke);
        /// <summary>
        /// Background Fill for the Center Button
        /// </summary>
        public Brush CenterButtonBackgroundFill
        {
            get { return _centerButtonBackgroundFill; }
            set { SetField(ref _centerButtonBackgroundFill, value); }
        }

        private Brush _centerButtonBorder = new SolidColorBrush(Colors.Transparent);
        /// <summary>
        /// Border Brush for the Center Button
        /// </summary>
        public Brush CenterButtonBorder
        {
            get { return _centerButtonBorder; }
            set { SetField(ref _centerButtonBorder, value); }
        }

        private bool _isDraggable = true;
        /// <summary>
        /// Indicates if this Menu should be draggable
        /// </summary>
        public bool IsDraggable
        {
            get { return _isDraggable; }
            set { SetField(ref _isDraggable, value); }
        }

        private double _centerButtonBorderThickness = 2;
        /// <summary>
        /// Border Brush for the Center Button
        /// </summary>
        public double CenterButtonBorderThickness
        {
            get { return _centerButtonBorderThickness; }
            set { SetField(ref _centerButtonBorderThickness, value); }
        }

        /// <summary>
        /// Content for the Center Button (using Segoe UI Symbol)
        /// </summary>
        private string _centerButtonIcon = "";
        public string CenterButtonIcon
        {
            get { return _centerButtonIcon; }
            set { SetField(ref _centerButtonIcon, value); }
        }

        /// <summary>
        /// Width/Height for the Center Button
        /// </summary>
        private int _centerButtonSize = 60;
        public int CenterButtonSize
        {
            get { return _centerButtonSize; }
            set { SetField(ref _centerButtonSize, value); }
        }

        /// <summary>
        /// Font Size for the Center Button
        /// </summary>
        private double _centerButtonFontSize = 19;
        public double CenterButtonFontSize
        {
            get { return _centerButtonFontSize; }
            set { SetField(ref _centerButtonFontSize, value); }
        }

        /// <summary>
        /// Font Size for the Center Button
        /// </summary>
        private double _centerButtonTop = 19;
        public double CenterButtonTop
        {
            get { return _centerButtonTop; }
            set { SetField(ref _centerButtonTop, value); }
        }
        /// <summary>
        /// Font Size for the Center Button
        /// </summary>
        private double _centerButtonLeft = 19;
        public double CenterButtonLeft
        {
            get { return _centerButtonLeft; }
            set { SetField(ref _centerButtonLeft, value); }
        }

        public virtual CenterButton CenterButton { get; set; }

        private bool _isCenterButtonNavigationEnabled = true;

        protected Stack<CenterButtonShim> PreviousCenterButtons = new Stack<CenterButtonShim>();

        /// <summary>
        /// If disabled, the center button won't automatically allow "back" navigation between submenue 
        /// </summary>
        public bool IsCenterButtonNavigationEnabled
        {
            get { return _isCenterButtonNavigationEnabled; }
            set { SetField(ref _isCenterButtonNavigationEnabled, value); }
        }

        private double _diameter;
        /// <summary>
        /// Diameter of the whole control
        /// </summary>
        public double Diameter
        {
            get { return _diameter; }
            set
            {
                SetField(ref _diameter, value);

                Height = _diameter;
                Width = _diameter;
            }
        }

        /// <summary>
        /// Helper function ensuring that we're not wasting resources when updating a field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">Field to use</param>
        /// <param name="value">Value to use</param>
        /// <param name="propertyName">Name of the property</param>
        protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!value.Equals(field))
            {
                field = value;
                var eventHandler = PropertyChanged;
                eventHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
