namespace RadialMenuControl.UserControl
{
    using Components;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    public partial class Pie : UserControl, INotifyPropertyChanged
    {
        private readonly ObservableCollection<PieSlice> _pieSlices = new ObservableCollection<PieSlice>();

        public event PropertyChangedEventHandler PropertyChanged;

        // Pass in the original RadialMenuButton
        public RadialMenu _sourceRadialMenu;
        public static readonly DependencyProperty _sourceRadialMenuProperty =
            DependencyProperty.Register("_sourceRadialMenu", typeof(RadialMenu), typeof(Pie), null);

        private Color _backgroundColor = Colors.White;
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetField(ref _backgroundColor, value); }
        }

        private Color _backgroundHighlightColor = Color.FromArgb(255, 235, 235, 235);
        public Color BackgroundHighlightColor
        {
            get { return _backgroundHighlightColor; }
            set { SetField(ref _backgroundHighlightColor, value); }
        }

        private Color _foregroundColor = Color.FromArgb(255, 241, 218, 234);
        public Color ForegroundColor
        {
            get { return _foregroundColor; }
            set { SetField(ref _foregroundColor, value); }
        }

        private Color _highlightColor = Color.FromArgb(255, 128, 57, 123);
        public Color HighlightColor
        {
            get { return _highlightColor; }
            set { SetField(ref _highlightColor, value); }
        }

        private double _angle;
        public double Angle
        {
            get { return _angle; }
            set { SetField(ref _angle, value); }
        }

        private double _size;
        public double Size
        {
            get { return _size; }
            set
            {
                SetField(ref _size, value);

                Height = _size;
                Width = _size;
            }
        }

        private IList<RadialMenuButton> _slices = new List<RadialMenuButton>();
        public IList<RadialMenuButton> Slices
        {
            get { return _slices; }
            set
            {
                SetField(ref _slices, value);
                Draw();
            }
        }

        public string SelectedItemValue
        {
            get { return _selectedItem?.Label; }
        }

        private PieSlice _selectedItem;
        private PieSlice SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value != null)
                {
                    SetField(ref _selectedItem, value);

                    var eventHandler = PropertyChanged;
                    eventHandler?.Invoke(this, new PropertyChangedEventArgs("SelectedItemValue"));
                }
            }
        }

        public Pie()
        {
            InitializeComponent();
            DataContext = this;

            Loaded += (sender, args) =>
            {
                Draw();
                SelectedItem = _pieSlices.FirstOrDefault();

                if (SelectedItem != null)
                {
                    Angle = 360 - SelectedItem.Angle / 4;
                }
            };

            _pieSlices.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (PieSlice item in args.NewItems)
                        {
                            layoutContent.Children.Add(item);
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (PieSlice item in args.OldItems)
                        {
                            layoutContent.Children.Remove(item);
                        }
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        layoutContent.Children.Clear();
                        break;
                }
            };
        }

        public void Draw()
        {
            _pieSlices.Clear();
            var startAngle = 22.5;

            for (int i = 0; i < Slices.Count; i++)
            {
                var sliceSize = 360 / Slices.Count;
                var pieSlice = new PieSlice
                {
                    StartAngle = startAngle,
                    Angle = sliceSize,
                    Radius = Size / 2,
                    Height = Height,
                    Width = Width,
                    // The defaults below use OneNote-like purple colors
                    InnerNormalColor = Slices[i].InnerNormalColor ?? Color.FromArgb(255, 255, 255, 255),
                    InnerHoverColor = Slices[i].InnerHoverColor ?? Color.FromArgb(255, 245, 236, 243),
                    InnerTappedColor = Slices[i].InnerTappedColor ?? Color.FromArgb(255, 237, 234, 236),
                    OuterNormalColor = Slices[i].OuterNormalColor ?? Color.FromArgb(255, 128, 57, 123),
                    OuterDisabledColor = Slices[i].OuterDisabledColor ?? Color.FromArgb(255, 237, 211, 236),
                    OuterHoverColor = Slices[i].OuterHoverColor ?? Color.FromArgb(255, 155, 79, 150),
                    OuterTappedColor = Slices[i].OuterTappedColor ?? Color.FromArgb(255, 104, 41, 100),
                    // Label
                    IconSize = Slices[i].IconSize ?? 26,
                    Icon = Slices[i].Icon ?? "",
                    HideLabel = Slices[i].HideLabel,
                    Label = Slices[i].Label ?? "",
                    LabelSize = Slices[i].LabelSize ?? 10,
                    // Original Button
                    _radialMenuButton = Slices[i]
                };

                // Allow slice to call the change request method on the radial menu
                pieSlice.ChangeMenuRequestEvent += _sourceRadialMenu.ChangeMenu;

                _pieSlices.Add(pieSlice);
                startAngle += sliceSize;
            }
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
    }
}
