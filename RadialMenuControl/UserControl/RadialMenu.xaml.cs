namespace RadialMenuControl.UserControl
{
    using Components;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Windows.UI;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    public partial class RadialMenu : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Pie _outerWheel;

        private Color _edgeBackgroundColor = Colors.Black;
        public Color EdgeBackgroundColor
        {
            get { return _edgeBackgroundColor; }
            set { SetField(ref _edgeBackgroundColor, value); }
        }

        private Color _foregroundColor = Color.FromArgb(255, 128, 57, 123);
        public Color ForegroundColor
        {
            get { return _foregroundColor; }
            set { SetField(ref _foregroundColor, value); }
        }

        private Color _edgeHighlightColor = Colors.DarkSeaGreen;
        public Color EdgeHighlightColor
        {
            get { return _edgeHighlightColor; }
            set { SetField(ref _edgeHighlightColor, value); }
        }

        /// <summary>
        /// Background Fill for the Center Button
        /// </summary>
        private Brush _centerButtonBackgroundFill = new SolidColorBrush(Colors.WhiteSmoke);
        public Brush CenterButtonBackgroundFill
        {
            get { return _centerButtonBackgroundFill; }
            set { SetField(ref _centerButtonBackgroundFill, value); }
        }

        /// <summary>
        /// Border Brush for the Center Button
        /// </summary>
        private Brush _centerButtonBorder = new SolidColorBrush(Colors.Transparent);
        public Brush CenterButtonBorder
        {
            get { return _centerButtonBorder; }
            set { SetField(ref _centerButtonBorder, value); }
        }

        /// <summary>
        /// Content for the Center Button (using Segoe UI Symbol)
        /// </summary>
        private string _centerButtonIcon = "";
        public string CenterButtonIcon
        {
            get { return _centerButtonIcon; }
            set { SetField(ref _centerButtonIcon, value); }
        }

        private int _centerButtonSize = 60;
        public int CenterButtonSize
        {
            get { return _centerButtonSize; }
            set { SetField(ref _centerButtonSize, value); }
        }

        private double _diameter;
        public double Diameter
        {
            get { return _diameter; }
            set
            {
                SetField(ref _diameter, value);

                Height = _diameter;
                Width = _diameter;
                pie.Size = _diameter;
            }
        }

        private IList<RadialMenuButton> _menuButtons;
        public IList<RadialMenuButton> Slices
        {
            get { return _menuButtons; }
            set
            {
                SetField<IList<RadialMenuButton>>(ref _menuButtons, value);
            }
        }

        public void AddButton(RadialMenuButton button)
        {
            pie.Slices.Add(button);
        }

        private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!value.Equals(field))
            {
                field = value;
                var eventHandler = PropertyChanged;
                eventHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Events
        public delegate void CenterButtonTappedHandler(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e);
        public event CenterButtonTappedHandler CenterButtonTappedEvent;

        private void OnCenterButtonTapped(object s, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (CenterButtonTappedEvent != null)
            {
                CenterButtonTappedEvent(s, e);
            }
        }

        public RadialMenu()
        {
            InitializeComponent();
            layoutRoot.DataContext = this;
            centerButton.Tapped += OnCenterButtonTapped;
        }
    }
}
