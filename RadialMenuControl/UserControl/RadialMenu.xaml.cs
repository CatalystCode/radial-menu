namespace RadialMenuControl.UserControl
{
    using Components;
    using Shims;
    using Extensions;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Automation.Peers;
    using Windows.UI.Xaml.Automation.Provider;

    public partial class RadialMenu : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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


        private bool _isCenterButtonNavigationEnabled = true;
        /// <summary>
        /// If disabled, the center button won't automatically allow "back" navigation between submenue 
        /// </summary>
        public bool IsCenterButtonNavigationEnabled
        {
            get { return _isCenterButtonNavigationEnabled; }
            set { SetField(ref _isCenterButtonNavigationEnabled, value); }
        }


        /// <summary>
        /// Start Angle
        /// </summary>
        private double _startAngle = 22.5;
        public double StartAngle
        {
            get { return _startAngle; }
            set {
                SetField(ref _startAngle, value);
                pie.StartAngle = value;
            }
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
                pie.Size = _diameter;
            }
        }

        private IList<Pie> _previousPies = new List<Pie>();
        /// <summary>
        /// Storage for previous pies (for back navigation)
        /// </summary>
        public IList<Pie> previousPies
        {
            get { return _previousPies; }
            set
            {
                SetField<IList<Pie>>(ref _previousPies, value);
            }
        }

        private IList<CenterButtonShim> _previousCenterButtons = new List<CenterButtonShim>();
        /// <summary>
        ///  Storage for previous center buttons (for back navigation)
        /// </summary>
        public IList<CenterButtonShim> previousButtons
        {
            get { return _previousCenterButtons; }
            set
            {
                SetField<IList<CenterButtonShim>>(ref _previousCenterButtons, value);
            }
        }

        /// <summary>
        /// Find a parent element by type!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
            {
                return null;
            }
        
            T parent = parentObject as T;

            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }

    /// <summary>
    /// Show or hide the outer wheel
    /// </summary>
    public async void TogglePie()
        {
            Floating floatingParent = FindParent<Floating>(this);
            double distance = Diameter / 2 - centerButton.ActualHeight / 2;

            if (pie.Visibility == Visibility.Visible)
            {
                await HidePieStoryboard.PlayAsync();
                pie.Visibility = Visibility.Collapsed;
                Width = centerButton.ActualWidth;
                Height = centerButton.ActualHeight;

                // Check if we're floating
                if (floatingParent != null)
                {
                    floatingParent.ManipulateControlPosition(distance, distance);
                }
            }
            else
            {
                pie.Visibility = Visibility.Visible;
                await ShowPieStoryboard.PlayAsync();
                Width = Diameter;
                Height = Diameter;

                // Check if we're floating
                if (floatingParent != null)
                {
                    floatingParent.ManipulateControlPosition(-distance, -distance);
                }
            }
        }

        /// <summary>
        /// Add a RadialMenuButton to the current pie
        /// </summary>
        /// <param name="button">RadialMenuButton to add to the current pie</param>
        public void AddButton(RadialMenuButton button)
        {
            pie.Slices.Add(button);
        }

        /// <summary>
        /// Helper function ensuring that we're not wasting resources when updating a field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">Field to use</param>
        /// <param name="value">Value to use</param>
        /// <param name="propertyName">Name of the property</param>
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

        /// <summary>
        /// Event Handler for a center button tap, calling user-registered events and handling navigation (if enabled)
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="e">Event information</param>
        private void OnCenterButtonTapped(object s, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // If an event has been registered with the center button tap, call it
            if (CenterButtonTappedEvent != null)
            {
                CenterButtonTappedEvent(s, e);
            }

            if (previousPies.Count == 0)
            {
                TogglePie();
            }

            if (previousPies.Count > 0 && IsCenterButtonNavigationEnabled)
            {
                // If we have a previous pie, we're going back to it
                ChangePie(this, previousPies[previousPies.Count - 1], false);
                previousPies.RemoveAt(previousPies.Count - 1);

                // We don't necessarily have the same amount of pies and center buttons.
                // Users can create submenues that don't bring their own center button
                if (previousButtons.Count > 0)
                {
                    ChangeCenterButton(this, previousButtons[previousButtons.Count - 1], false);
                    previousButtons.RemoveAt(previousButtons.Count - 1);
                }
            }
        }

        /// <summary>
        /// Change the whole radial menu, using a new menu object
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="menu">Menu to change to</param>
        public void ChangeMenu(object s, UserControl menu)
        {
            // If submenu is a Radial menu
            if(menu.GetType() == typeof(RadialMenu))
            {
                ChangePie(s, ((RadialMenu)menu).pie, true);
                ChangeCenterButton(s, Helpers.ButtonToShim(((RadialMenu)menu).centerButton), true);
            }
            else // If submenu is a list menu
            {
                listmenu = (ListMenu)menu;
                IList<RadialMenuButton> ListItems = listmenu.ListItems;

                TogglePie();
                centerButton.Visibility = Visibility.Collapsed;

                Style menuStyle = new Windows.UI.Xaml.Style { TargetType = typeof(MenuFlyoutPresenter) };
                menuStyle.Setters.Add(new Setter(WidthProperty, this.Width));
                menuStyle.Setters.Add(new Setter(MaxHeightProperty, this.Height/2));
                menuStyle.Setters.Add(new Setter(MarginProperty, new Thickness(0, 150, 0, 0)));
                MenuFlyout flyoutMenu = new MenuFlyout();
                flyoutMenu.MenuFlyoutPresenterStyle = menuStyle;

                ToggleMenuFlyoutItem toggle = new ToggleMenuFlyoutItem();
                toggle.Text = "◀ Back";
                toggle.Click += ToggleMenu_Click;
                flyoutMenu.Items.Add(toggle);

                flyoutMenu.Items.Add(new MenuFlyoutSeparator());
                foreach(RadialMenuButton btn in ListItems)
                {
                    MenuFlyoutItem menuItem = new MenuFlyoutItem();
                    menuItem.Text = btn.Label;
                    menuItem.Margin = new Thickness(0, 0, 50, 0);
                    flyoutMenu.Items.Add(menuItem);
                    if (btn.MenuSelected)
                    {
                        menuItem.Background = new SolidColorBrush (Colors.Gray);   
                    }
                    menuItem.Click += MenuItem_Click;
                }
                RadialMenuButton pieButton = (RadialMenuButton)s;
                centerButton.Flyout = flyoutMenu;
                flyoutMenu.ShowAt(centerButton);

                ButtonAutomationPeer peer = new ButtonAutomationPeer(centerButton);
                IInvokeProvider invokeProv =  peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            }
            
        }

        private void ToggleMenu_Click(object sender, RoutedEventArgs e)
        {
            centerButton.Visibility = Visibility.Visible;
            TogglePie();
            centerButton.Flyout = null;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem selectedItem = (MenuFlyoutItem)sender;
            foreach(RadialMenuButton btn in listmenu.ListItems)
            {
                if(btn.Label == selectedItem.Text)
                {
                    btn.MenuSelected = true;

                }
                else if(btn.MenuSelected)
                {
                    btn.MenuSelected = false;
                }
            }
            centerButton.Visibility = Visibility.Visible;
            TogglePie();
            centerButton.Flyout = null;
        }


        /// <summary>
        /// Change the current pie - aka update the current radial menu buttons
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="newPie">Pie object to take RadialMenuButtons from</param>
        /// <param name="storePrevious">Should we store the previous pie (for back navigation)?</param>
        public void ChangePie(object s, Pie newPie, bool storePrevious)
        {
            // Store the current pie
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

        /// <summary>
        /// Change the center button using a CenterButtonShim object
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="newButton">CenterButtonShim object to take properties for the new button from</param>
        /// <param name="storePrevious">Should we store the previous center button (for back navigation?)</param>
        public void ChangeCenterButton(object s, CenterButtonShim newButton, bool storePrevious)
        {
            // Store the current button
            if (storePrevious)
            {
                CenterButtonShim backupButton = new CenterButtonShim
                {
                    BorderBrush = CenterButtonBorder,
                    Background = CenterButtonBackgroundFill,
                    Content = CenterButtonIcon,
                    FontSize = CenterButtonFontSize
                };

                previousButtons.Add(backupButton);
            }

            // Decorate the current button with new props
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
