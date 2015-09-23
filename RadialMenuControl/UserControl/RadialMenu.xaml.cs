namespace RadialMenuControl.UserControl
{
    using Components;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Windows.UI;
    using Windows.UI.Xaml.Controls;

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

        private Color _foregroundColor = Colors.White;
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

        public RadialMenu()
        {
            InitializeComponent();
            layoutRoot.DataContext = this;
        }
    }
}
