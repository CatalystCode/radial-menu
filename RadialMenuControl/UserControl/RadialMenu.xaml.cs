namespace RadialMenuControl.UserControl
{
    using Components;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
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

        private IList<Pie> _previousPies = new List<Pie>();
        public IList<Pie> previousPies
        {
            get { return _previousPies; }
            set
            {
                SetField<IList<Pie>>(ref _previousPies, value);
            }
        }

        private IList<Button> _previousCenterButtons = new List<Button>();
        public IList<Button> previousButtons
        {
            get { return _previousCenterButtons; }
            set
            {
                SetField<IList<Button>>(ref _previousCenterButtons, value);
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

            // Should we go back? 
            // TODO: This needs some configuration
            if (previousPies.Count > 0)
            {
                ChangePie(this, previousPies[previousPies.Count - 1], false);
                previousPies.RemoveAt(previousPies.Count - 1);

                // TODO: Assumption that previousPies & previousButtons are always the same,
                // which is probably not a great idea
                ChangeCenterButton(this, previousButtons[previousButtons.Count - 1], false);
                previousButtons.RemoveAt(previousButtons.Count - 1);
            }
        }

        public void ChangeMenu(object sender, RadialMenu menu)
        {
            ChangePie(sender, menu.pie, true);
            ChangeCenterButton(sender, menu.centerButton, true);
        }

        public void ChangePie(object sender, Pie newPie, bool storePrevious)
        {
            // Store the current pie
            // TODO: Make a lean class
            if (storePrevious)
            {
                Pie backupPie = new Pie();
                foreach (RadialMenuButton rmb in pie.Slices)
                {
                    backupPie.Slices.Add(rmb);
                }

                previousPies.Add(backupPie);
            }

            // Delete the current slices
            pie.Slices.Clear();

            // Add the new ones
            foreach (RadialMenuButton rmb in newPie.Slices)
            {
                pie.Slices.Add(rmb);
            }

            // Redraw
            pie.Draw();
            pie.UpdateLayout();
        }

        public void ChangeCenterButton(object sender, Button newButton, bool storePrevious)
        {
            // Store the current button
            // TODO: Make a lean class that has only these props - and store only these, not whole buttons
            if (storePrevious)
            {
                Button backupButton = new Button();
                backupButton.BorderBrush = CenterButtonBorder;
                backupButton.Background = CenterButtonBackgroundFill;
                backupButton.Content = CenterButtonIcon;
                backupButton.FontSize = CenterButtonFontSize;
                previousButtons.Add(backupButton);
            }

            CenterButtonBorder = newButton.BorderBrush;
            CenterButtonBackgroundFill = newButton.Background;
            CenterButtonIcon = (string)newButton.Content;
            CenterButtonFontSize = newButton.FontSize;
        }
             
        public RadialMenu()
        {
            InitializeComponent();
            pie._sourceRadialMenu = this;
            layoutRoot.DataContext = this;
            centerButton.Tapped += OnCenterButtonTapped;
        }
    }
}
