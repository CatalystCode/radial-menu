namespace RadialMenuControl.UserControl
{
    using Components;
    using Themes;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public partial class Pie : UserControl, INotifyPropertyChanged
    {
        private readonly ObservableCollection<PieSlice> _pieSlices = new ObservableCollection<PieSlice>();

        public event PropertyChangedEventHandler PropertyChanged;

        // Pass in the original RadialMenu
        public RadialMenu SourceRadialMenu;
        public static readonly DependencyProperty SourceRadialMenuProperty =
            DependencyProperty.Register("SourceRadialMenu", typeof(RadialMenu), typeof(Pie), null);

        private Color _backgroundColor = Colors.White;
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetField(ref _backgroundColor, value); }
        }

        private Color _backgroundHighlightColor = DefaultColors.BackgroundHighlightColor;
        public Color BackgroundHighlightColor
        {
            get { return _backgroundHighlightColor; }
            set { SetField(ref _backgroundHighlightColor, value); }
        }

        private Color _foregroundColor = DefaultColors.ForegroundColor;
        public Color ForegroundColor
        {
            get { return _foregroundColor; }
            set { SetField(ref _foregroundColor, value); }
        }

        private Color _highlightColor = DefaultColors.HighlightColor;
        public Color HighlightColor
        {
            get { return _highlightColor; }
            set { SetField(ref _highlightColor, value); }
        }

        private double _startAngle;
        public double StartAngle
        {
            get { return _startAngle; }
            set {
                SetField(ref _startAngle, value);
                Draw();
            }
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

        public string SelectedItemValue => _selectedItem?.Label;

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
                            LayoutContent.Children.Add(item);
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (PieSlice item in args.OldItems)
                        {
                            LayoutContent.Children.Remove(item);
                        }
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        LayoutContent.Children.Clear();
                        break;
                }
            };

            #if DEBUG
            // Ensure that we're drawing a circle, even if no buttons are present
            if ((Slices == null || Slices.Count == 0) && Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Slices.Add(new RadialMenuButton());
                Slices.Add(new RadialMenuButton());
            }
            #endif
        }

        public void Draw()
        {
            _pieSlices.Clear();
            var startAngle = StartAngle;

            // Draw PieSlices for each Slice Object
            foreach (var slice in Slices)
            {
                var sliceSize = 360.00 / Slices.Count;
                var pieSlice = new PieSlice
                {
                    StartAngle = startAngle,
                    Angle = sliceSize,
                    Radius = Size / 2,
                    Height = Height,
                    Width = Width,
                    // The defaults below use OneNote-like purple colors
                    InnerNormalColor = slice.InnerNormalColor,
                    InnerHoverColor = slice.InnerHoverColor,
                    InnerTappedColor = slice.InnerTappedColor,
                    InnerReleasedColor = slice.InnerReleasedColor,
                    OuterNormalColor = slice.OuterNormalColor,
                    OuterDisabledColor = slice.OuterDisabledColor,
                    OuterHoverColor = slice.OuterHoverColor,
                    OuterTappedColor = slice.OuterTappedColor,
                    // Label
                    IconSize = slice.IconSize,
                    Icon = slice.Icon,
                    IconImage = slice.IconImage,
                    IconImageSideLength = (Size / 2) * .25,
                    HideLabel = slice.HideLabel,
                    Label = slice.Label,
                    LabelSize = slice.LabelSize,
                    // Original Button
                    OriginalRadialMenuButton = slice
                };

                // Allow slice to call the change request method on the radial menu
                pieSlice.ChangeMenuRequestEvent += SourceRadialMenu.ChangeMenu;
                // Allow slice to call the change selected request to clear all other radio buttons
                pieSlice.ChangeSelectedEvent += PieSlice_ChangeSelectedEvent;
                _pieSlices.Add(pieSlice);
                startAngle += sliceSize;
            }
        }

        private void PieSlice_ChangeSelectedEvent(object sender, PieSlice slice)
        {
            foreach(PieSlice ps in _pieSlices)
            {
                // find any previously selected Radio button to de-select
                if (ps.OriginalRadialMenuButton.Type != RadialMenuButton.ButtonType.Radio ||
                    !ps.OriginalRadialMenuButton.MenuSelected || ps.StartAngle == slice.StartAngle) continue;
                ps.OriginalRadialMenuButton.MenuSelected = false;
                ps.UpdateSliceForRadio();
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

