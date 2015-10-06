using Windows.UI.Xaml.Input;

namespace RadialMenuControl.UserControl
{
    using Components;
    using Shims;
    using Extensions;
    using System.Collections.Generic;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;
    using System.Collections.ObjectModel;
    using Windows.UI.Xaml.Controls;
    using Windows.UI;
    using Windows.UI.Xaml.Automation.Peers;
    using Windows.UI.Xaml.Automation.Provider;

    public partial class RadialMenu : MenuBase
    {
        public ObservableCollection<MenuBase> DisplayMenus = new ObservableCollection<MenuBase>();

        // Events
        public delegate void CenterButtonTappedHandler(object sender, TappedRoutedEventArgs e);
        public event CenterButtonTappedHandler CenterButtonTappedEvent;

        /// <summary>
        /// Start Angle
        /// </summary>
        private double _startAngle = 22.5;
        public double StartAngle
        {
            get { return _startAngle; }
            set
            {
                SetField(ref _startAngle, value);
                Pie.StartAngle = value;
            }
        }

        private IList<Pie> _previousPies = new List<Pie>();

        /// <summary>
        ///     Storage for previous pies (for back navigation)
        /// </summary>
        public IList<Pie> PreviousPies
        {
            get { return _previousPies; }
            set { SetField(ref _previousPies, value); }
        }

        /// <summary>
        ///     Storage for previous center buttons (for back navigation)
        /// </summary>
        public IList<CenterButtonShim> PreviousButtons
        {
            get { return PreviousCenterButtons; }
            set { SetField(ref PreviousCenterButtons, value); }
        }

        /// <summary>
        ///     Find a parent element by type!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
            {
                return null;
            }

            var parent = parentObject as T;
            return parent ?? FindParent<T>(parentObject);
        }

        /// <summary>
        ///     Show or hide the outer wheel
        /// </summary>
        public async void TogglePie()
        {
            var floatingParent = FindParent<Floating>(this);
            var distance = Diameter / 2 - CenterButton.ActualHeight / 2;

            if (Pie.Visibility == Visibility.Visible)
            {
                await HidePieStoryboard.PlayAsync();
                Pie.Visibility = Visibility.Collapsed;
                Width = CenterButton.ActualWidth;
                Height = CenterButton.ActualHeight;

                // Check if we're floating
                floatingParent?.ManipulateControlPosition(distance, distance);
            }
            else
            {
                Pie.Visibility = Visibility.Visible;
                await ShowPieStoryboard.PlayAsync();
                Width = Diameter;
                Height = Diameter;

                // Check if we're floating
                floatingParent?.ManipulateControlPosition(-distance, -distance);
            }
        }

        /// <summary>
        ///     Add a RadialMenuButton to the current pie
        /// </summary>
        /// <param name="button">RadialMenuButton to add to the current pie</param>
        public void AddButton(RadialMenuButton button)
        {
            Pie.Slices.Add(button);
        }

        /// <summary>
        /// Event Handler for a center button tap, calling user-registered events and handling navigation (if enabled)
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="e">Event information</param>
        private void OnCenterButtonTapped(object s, TappedRoutedEventArgs e)
        {
            // If an event has been registered with the center button tap, call it
            CenterButtonTappedEvent?.Invoke(this, e);

            if (PreviousPies.Count == 0)
            {
                TogglePie();
            }

            if (PreviousPies.Count <= 0 || !IsCenterButtonNavigationEnabled) return;
            // If we have a previous pie, we're going back to it
            ChangePie(this, PreviousPies[PreviousPies.Count - 1], false);
            PreviousPies.RemoveAt(PreviousPies.Count - 1);

            // We don't necessarily have the same amount of pies and center buttons.
            // Users can create submenues that don't bring their own center button
            if (PreviousButtons.Count <= 0) return;
            ChangeCenterButton(this, PreviousButtons[PreviousButtons.Count - 1], false);
            PreviousButtons.RemoveAt(PreviousButtons.Count - 1);
        }

        /// <summary>
        ///     Change the whole radial menu, using a new menu object
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="menu">Menu to change to</param>
        public void ChangeMenu(object s, MenuBase menu)
        {
            if (menu is RadialMenu)
            {
                var radialMenu = (RadialMenu) menu;
                ChangePie(s, radialMenu.Pie, true);
                ChangeCenterButton(s, Helpers.ButtonToShim(radialMenu.CenterButton), true);
            }
            else if (menu is MeterSubMenu)
            {
                ChangeToCustomMenu(s, ((MeterSubMenu) menu), true);
                ChangeCenterButton(s, Helpers.ButtonToShim(((MeterSubMenu) menu).CenterButton), true);
            }
            else if (menu is ListSubMenu)
            {
                ChangeToCustomMenu(s, ((ListSubMenu)menu), true);
            }
        }

        /// <summary>
        /// Clears the current pie
        /// </summary>
        /// <param name="storePrevious">Should we store the previous pie (for back navigation)?</param>
        private void _clearPie(bool storePrevious)
        {
            // Store the current pie
            if (storePrevious)
            {
                var backupPie = new Pie();
                foreach (var rmb in Pie.Slices)
                {
                    backupPie.Slices.Add(rmb);
                }

                PreviousPies.Add(backupPie);
            }

            // Delete the current slices
            Pie.Slices.Clear();
            CustomRadialControlRoot.Children.Clear();
        }

        /// <summary>
        /// Change to custom MenuBase menu.
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="newSubMenu">The new submenu which will be placed in customRadialControlRoot Canvas</param>
        /// <param name="storePrevious">Should we store the previous pie (for back navigation)?</param>
        public void ChangeToCustomMenu(object s, MenuBase newSubMenu, bool storePrevious)
        {
           
            if(newSubMenu is ListSubMenu)
            {
                CenterButton.Visibility = Visibility.Collapsed;
                TogglePie();

                listSubMenu = (ListSubMenu)newSubMenu;
                IList<RadialMenuButton> ListItems = listSubMenu.ListItems;

                Style menuStyle = new Windows.UI.Xaml.Style { TargetType = typeof(MenuFlyoutPresenter) };
                menuStyle.Setters.Add(new Setter(WidthProperty, this.Width));
                menuStyle.Setters.Add(new Setter(MaxHeightProperty, listSubMenu.ListHeight));
                menuStyle.Setters.Add(new Setter(MarginProperty, listSubMenu.ListMargin));
                MenuFlyout flyoutMenu = new MenuFlyout();
                flyoutMenu.MenuFlyoutPresenterStyle = menuStyle;

                ToggleMenuFlyoutItem toggle = new ToggleMenuFlyoutItem();
                toggle.Text = "◀ Back";
                toggle.Click += ToggleMenu_Click;
                flyoutMenu.Items.Add(toggle);

                flyoutMenu.Items.Add(new MenuFlyoutSeparator());
                foreach (RadialMenuButton btn in ListItems)
                {
                    MenuFlyoutItem menuItem = new MenuFlyoutItem();
                    menuItem.Text = btn.Label;
                    menuItem.Margin = new Thickness(0, 0, 50, 0);
                    flyoutMenu.Items.Add(menuItem);
                    if (btn.MenuSelected)
                    {
                        menuItem.Background = new SolidColorBrush(Colors.Gray);
                    }
                    menuItem.Click += MenuItem_Click;
                }
                RadialMenuButton pieButton = (RadialMenuButton)s;
                CenterButton.Flyout = flyoutMenu;
                flyoutMenu.ShowAt(CenterButton);

                ButtonAutomationPeer peer = new ButtonAutomationPeer(CenterButton);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            }
            else
            {
                _clearPie(storePrevious);
                // Redraw
                Pie.Draw();
                Pie.UpdateLayout();
                // TODO use just an auxilary canvas and add custom controls to that
                newSubMenu.Diameter = Diameter;
                CustomRadialControlRoot.Children.Add(newSubMenu);
                newSubMenu.UpdateLayout();
            }
        }

        private void ToggleMenu_Click(object sender, RoutedEventArgs e)
        {
            CenterButton.Visibility = Visibility.Visible;
            TogglePie();
            CenterButton.Flyout = null;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem selectedItem = (MenuFlyoutItem)sender;
            foreach (RadialMenuButton btn in listSubMenu.ListItems)
            {
                if (btn.Label == selectedItem.Text)
                {
                    btn.MenuSelected = true;

                }
                else if (btn.MenuSelected)
                {
                    btn.MenuSelected = false;
                }
            }
            CenterButton.Visibility = Visibility.Visible;
            TogglePie();
            CenterButton.Flyout = null;
        }
        /// <summary>
        /// Change the current pie - aka update the current radial menu buttons
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="newPie">Pie object to take RadialMenuButtons from</param>
        /// <param name="storePrevious">Should we store the previous pie (for back navigation)?</param>
        public void ChangePie(object s, Pie newPie, bool storePrevious)
        {
            _clearPie(storePrevious);

            // Add the new ones
            foreach (var rmb in newPie.Slices)
            {
                Pie.Slices.Add(rmb);
            }

            // Redraw
            Pie.Draw();
            Pie.UpdateLayout();
        }

        /// <summary>
        ///     Change the center button using a CenterButtonShim object
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="newButton">CenterButtonShim object to take properties for the new button from</param>
        /// <param name="storePrevious">Should we store the previous center button (for back navigation?)</param>
        public void ChangeCenterButton(object s, CenterButtonShim newButton, bool storePrevious)
        {
            // Store the current button
            if (storePrevious)
            {
                var backupButton = new CenterButtonShim
                {
                    BorderBrush = CenterButtonBorder,
                    Background = CenterButtonBackgroundFill,
                    Content = CenterButtonIcon,
                    FontSize = CenterButtonFontSize
                };

                PreviousButtons.Add(backupButton);
            }

            // Decorate the current button with new props
            CenterButtonBorder = newButton.BorderBrush;
            CenterButtonBackgroundFill = newButton.Background;
            CenterButtonIcon = (string)newButton.Content;
            CenterButtonFontSize = newButton.FontSize;
        }

        /// <summary>
        /// Initializes the Center Button, since we want to share with other classes
        /// </summary>
        public RadialMenu()
        {
            InitializeComponent();
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Diameter")
                {
                    Pie.Size = Diameter;
                }

            };
            
            CenterButton.Style = Resources["RoundedCenterButton"] as Style;

            Pie.SourceRadialMenu = this;
            LayoutRoot.DataContext = this;
            CenterButton.Tapped += OnCenterButtonTapped;
            
        }
    }
}