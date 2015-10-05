using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using RadialMenuControl.Components;
using RadialMenuControl.Extensions;
using RadialMenuControl.Shims;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;

namespace RadialMenuControl.UserControl
{
    public partial class RadialMenu : Windows.UI.Xaml.Controls.UserControl, INotifyPropertyChanged
    {
        public delegate void CenterButtonTappedHandler(object sender, TappedRoutedEventArgs e);

        private Brush _centerButtonBackgroundFill = new SolidColorBrush(Colors.WhiteSmoke);

        private Brush _centerButtonBorder = new SolidColorBrush(Colors.Transparent);

        /// <summary>
        ///     Font Size for the Center Button
        /// </summary>
        private double _centerButtonFontSize = 19;

        /// <summary>
        ///     Content for the Center Button (using Segoe UI Symbol)
        /// </summary>
        private string _centerButtonIcon = "";

        /// <summary>
        ///     Width/Height for the Center Button
        /// </summary>
        private int _centerButtonSize = 60;

        private double _diameter;

        private bool _isCenterButtonNavigationEnabled = true;

        private IList<CenterButtonShim> _previousCenterButtons = new List<CenterButtonShim>();

        private IList<Pie> _previousPies = new List<Pie>();


        /// <summary>
        ///     Start Angle
        /// </summary>
        private double _startAngle = 22.5;

        public RadialMenu()
        {
            InitializeComponent();
            Pie.SourceRadialMenu = this;
            LayoutRoot.DataContext = this;
            CenterButton.Tapped += OnCenterButtonTapped;
        }

        /// <summary>
        ///     Background Fill for the Center Button
        /// </summary>
        public Brush CenterButtonBackgroundFill
        {
            get { return _centerButtonBackgroundFill; }
            set { SetField(ref _centerButtonBackgroundFill, value); }
        }

        /// <summary>
        ///     Border Brush for the Center Button
        /// </summary>
        public Brush CenterButtonBorder
        {
            get { return _centerButtonBorder; }
            set { SetField(ref _centerButtonBorder, value); }
        }

        public string CenterButtonIcon
        {
            get { return _centerButtonIcon; }
            set { SetField(ref _centerButtonIcon, value); }
        }

        public int CenterButtonSize
        {
            get { return _centerButtonSize; }
            set { SetField(ref _centerButtonSize, value); }
        }

        public double CenterButtonFontSize
        {
            get { return _centerButtonFontSize; }
            set { SetField(ref _centerButtonFontSize, value); }
        }

        /// <summary>
        ///     If disabled, the center button won't automatically allow "back" navigation between submenue
        /// </summary>
        public bool IsCenterButtonNavigationEnabled
        {
            get { return _isCenterButtonNavigationEnabled; }
            set { SetField(ref _isCenterButtonNavigationEnabled, value); }
        }

        public double StartAngle
        {
            get { return _startAngle; }
            set
            {
                SetField(ref _startAngle, value);
                Pie.StartAngle = value;
            }
        }

        /// <summary>
        ///     Diameter of the whole control
        /// </summary>
        public double Diameter
        {
            get { return _diameter; }
            set
            {
                SetField(ref _diameter, value);

                Height = _diameter;
                Width = _diameter;
                Pie.Size = _diameter;
            }
        }

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
            get { return _previousCenterButtons; }
            set { SetField(ref _previousCenterButtons, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
            var distance = Diameter/2 - CenterButton.ActualHeight/2;

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
        ///     Helper function ensuring that we're not wasting resources when updating a field
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

        public event CenterButtonTappedHandler CenterButtonTappedEvent;

        /// <summary>
        ///     Event Handler for a center button tap, calling user-registered events and handling navigation (if enabled)
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="e">Event information</param>
        private void OnCenterButtonTapped(object s, TappedRoutedEventArgs e)
        {
            // If an event has been registered with the center button tap, call it
            CenterButtonTappedEvent?.Invoke(s, e);

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
        public void ChangeMenu(object s, Windows.UI.Xaml.Controls.UserControl menu)
        {
            // If submenu is a Radial menu
            if (menu.GetType() == typeof(RadialMenu))
            {
                ChangePie(s, ((RadialMenu)menu).Pie, true);
                ChangeCenterButton(s, Helpers.ButtonToShim(((RadialMenu)menu).CenterButton), true);
            }
            else // If submenu is a list menu
            {
                listmenu = (ListMenu)menu;
                IList<RadialMenuButton> ListItems = listmenu.ListItems;

                TogglePie();
                CenterButton.Visibility = Visibility.Collapsed;

                Style menuStyle = new Windows.UI.Xaml.Style { TargetType = typeof(MenuFlyoutPresenter) };
                menuStyle.Setters.Add(new Setter(WidthProperty, this.Width));
                menuStyle.Setters.Add(new Setter(MaxHeightProperty, this.Height / 2));
                menuStyle.Setters.Add(new Setter(MarginProperty, new Thickness(0, 150, 0, 0)));
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
            foreach (RadialMenuButton btn in listmenu.ListItems)
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
        ///     Change the current pie - aka update the current radial menu buttons
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="newPie">Pie object to take RadialMenuButtons from</param>
        /// <param name="storePrevious">Should we store the previous pie (for back navigation)?</param>
        public void ChangePie(object s, Pie newPie, bool storePrevious)
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
            CenterButtonIcon = (string) newButton.Content;
            CenterButtonFontSize = newButton.FontSize;
        }
    }
}