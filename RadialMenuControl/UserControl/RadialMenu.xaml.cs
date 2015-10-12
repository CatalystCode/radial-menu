using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace RadialMenuControl.UserControl
{
    using Components;
    using Shims;
    using Extensions;
    using Themes;
    using System.Collections.Generic;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Input;
    using System.Collections.ObjectModel;

    public partial class RadialMenu : MenuBase
    {
        public ObservableCollection<MenuBase> DisplayMenus = new ObservableCollection<MenuBase>();

        // Events
        public delegate void CenterButtonTappedHandler(object sender, TappedRoutedEventArgs e);
        public event CenterButtonTappedHandler CenterButtonTappedEvent;

        private bool _isMenuChangeAnimated = true;
        /// <summary>
        /// Is the transition between submenus animated?
        /// </summary>
        public bool IsMenuChangeAnimated
        {
            get { return _isMenuChangeAnimated; }
            set { SetField(ref _isMenuChangeAnimated, value); }
        }

        /// <summary>
        /// A public reference to the Pie used but the RadialMenu. The Pie abstracts away all the Math that has to happen
        /// for the RadialMenu to be fast, pretty, and flexible.
        /// </summary>
        public Pie Pie
        {
            get { return DesignPie; }
            private set { DesignPie = value; }
        }

        private double _startAngle = 22.5;
        /// <summary>
        /// Start Angle
        /// </summary>
        public double StartAngle
        {
            get { return _startAngle; }
            set
            {
                SetField(ref _startAngle, value);
                Pie.StartAngle = value;
            }
        }

        private Dictionary<string, Color> _buttonDefaultColors = new Dictionary<string, Color>()
        {
            { "OuterNormalColor", DefaultColors.OuterNormalColor },
            { "OuterHoverColor", DefaultColors.OuterHoverColor },
            { "OuterDisabledColor", DefaultColors.OuterDisabledColor },
            { "OuterTappedColor", DefaultColors.OuterTappedColor },
            { "InnerNormalColor", DefaultColors.InnerNormalColor },
            { "InnerHoverColor", DefaultColors.InnerHoverColor },
            { "InnerTappedColor", DefaultColors.InnerTappedColor },
            { "InnerReleasedColor", DefaultColors.InnerReleasedColor },
        };
        /// <summary>
        /// A dictionary containing the default colors - useful if you want to declare the default colors for the whole RadialMenu
        /// (instead of declaring colors on each RadialMenuButton)
        /// </summary>
        public Dictionary<string, Color> ButtonDefaultColors
        {
            get { return _buttonDefaultColors; }
            set
            {
                foreach (var colorPair in value.Where(colorPair => _buttonDefaultColors.ContainsKey(colorPair.Key)))
                {
                    _buttonDefaultColors[colorPair.Key] = colorPair.Value;
                }
            }
        }

        private TimeSpan _pieAnimationTimeSpan = TimeSpan.FromSeconds(0.3);
        /// <summary>
        /// Timespan for animations
        /// </summary>
        public TimeSpan PieAnimationTimeSpan
        {
            get { return _pieAnimationTimeSpan; }
            set
            {
                SetField(ref _pieAnimationTimeSpan, value);
                SetupStoryboards();
            }
        }

        private IList<Pie> _previousPies = new List<Pie>();
    
        /// <summary>
        ///  Previous pies (for back navigation)
        /// </summary>
        public IList<Pie> PreviousPies
        {
            get { return _previousPies; }
            set { SetField(ref _previousPies, value); }
        }

        /// <summary>
        /// Previous center buttons (for back navigation)
        /// </summary>
        public Stack<CenterButtonShim> PreviousButtons
        {
            get { return PreviousCenterButtons; }
            set { SetField(ref PreviousCenterButtons, value); }
        }

        /// <summary>
        ///  Find a parent element by type!
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
        /// Show or hide the outer wheel
        /// </summary>
        public async void TogglePie()
        {
            var floatingParent = FindParent<Floating>(this);
            var distance = Diameter / 2 - CenterButton.ActualHeight / 2;

            if (Pie.Visibility == Visibility.Visible)
            {
                await CloseStoryboard.PlayAsync();

                Pie.Visibility = Visibility.Collapsed;
                Width = CenterButton.ActualWidth;
                Height = CenterButton.ActualHeight;
                // Check if we're floating
                floatingParent?.ManipulateControlPosition(distance, distance, Width, Height);
                Canvas.SetTop(CenterButton, 0);
                Canvas.SetLeft(CenterButton, 0);
            }
            else
            {
                Width = Diameter;
                Height = Diameter;
                // Check if we're floating
                floatingParent?.ManipulateControlPosition(-distance, -distance, Width, Height);
                Canvas.SetTop(CenterButton, Diameter / 2 - CenterButton.ActualHeight / 2);
                Canvas.SetLeft(CenterButton, Diameter / 2 - CenterButton.ActualWidth / 2);
                Pie.Visibility = Visibility.Visible;

                await OpenStoryboard.PlayAsync();
            }
        }

        /// <summary>
        /// RadialMenuButtons on this menu
        /// </summary>
        public IList<RadialMenuButton> Buttons
        {
            get { return Pie.Slices; }
            set { Pie.Slices = value; }
        }

        /// <summary>
        ///  Add a RadialMenuButton to the current pie
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
            ChangeCenterButton(this, PreviousButtons.Pop(), false);
        }

        /// <summary>
        /// Change the whole radial menu, using a new menu object
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="menu">Menu to change to</param>
        public void ChangeMenu(object s, MenuBase menu)
        {
            if (menu is RadialMenu)
            {
                var radialMenu = (RadialMenu) menu;
                ChangePie(s, radialMenu.Pie, true);
                ChangeCenterButton(s, Helpers.ButtonToShim(radialMenu.CenterButton, radialMenu.CenterButtonTappedEvent), true);
            }
            else
            {
                ChangeToCustomMenu(s, menu, true);
                ChangeCenterButton(s, Helpers.ButtonToShim(menu.CenterButton), true);
            }
        }

        /// <summary>
        /// Clears the current pie, removing all currently displayed slices
        /// </summary>
        /// <param name="storePrevious">Should we store the pie (for back navigation)?</param>
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

                backupPie.SelectedItem = Pie.SelectedItem;
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
            _clearPie(storePrevious);
            // Redraw
            Pie.Draw();
            Pie.UpdateLayout();
            newSubMenu.Diameter = Diameter;
            CustomRadialControlRoot.Children.Add(newSubMenu);
            newSubMenu.UpdateLayout();
        }

        /// <summary>
        /// Change the current pie - aka update the current radial menu buttons
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="newPie">Pie object to take RadialMenuButtons from</param>
        /// <param name="storePrevious">Should we store the previous pie (for back navigation)?</param>
        public async void ChangePie(object s, Pie newPie, bool storePrevious)
        {
            await PieExitForChangeStoryboard.PlayAsync();

            _clearPie(storePrevious);
            // Add the new ones
            foreach (var rmb in newPie.Slices)
            {
                Pie.Slices.Add(rmb);
            }

            // Redraw
            Pie.Draw();
            Pie.UpdateLayout();

            // Ensure that we remember what the last selected item was
            if (newPie.SelectedItem != null)
            {
                Pie.SelectedItem = newPie.SelectedItem;
            }
            else
            {
                Pie.SelectedItem = null;
            }

            await PieEnterForChangeStoryboard.PlayAsync();
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
                var backupButton = new CenterButtonShim
                {
                    BorderBrush = CenterButtonBorder,
                    Background = CenterButtonBackgroundFill,
                    Content = CenterButtonIcon,
                    FontSize = CenterButtonFontSize,
                    Top = CenterButtonTop,
                    Left = CenterButtonLeft,
                    CenterButtonTappedHandler = CenterButtonTappedEvent
                };

                PreviousButtons.Push(backupButton);
            }

            // Decorate the current button with new props
            // Center button style defaults to current style unless overridden
            CenterButtonBorder = newButton?.BorderBrush ?? CenterButtonBorder;
            CenterButtonBackgroundFill = newButton?.Background ?? CenterButtonBackgroundFill;
            CenterButtonIcon = (string) newButton?.Content ?? CenterButtonIcon;
            CenterButtonFontSize = newButton?.FontSize ?? CenterButtonFontSize;
            CenterButtonTappedEvent = newButton?.CenterButtonTappedHandler ?? null;

            if (newButton != null && !double.IsNaN(newButton.Top ?? 0) && !double.IsNaN(newButton.Left ?? 0))
            {
                CenterButtonTop = newButton.Top ?? CenterButtonTop;
                CenterButtonLeft = newButton.Left ?? CenterButtonTop;
            }
            
        }

        /// <summary>
        /// Initializes our storyboard animations
        /// </summary>
        private void SetupStoryboards()
        {
            PieOpenOpacityAnimation.Duration = PieAnimationTimeSpan;
            PieOpenRotateAnimation.Duration = PieAnimationTimeSpan;
            PieOpenScaleXAnimation.Duration = PieAnimationTimeSpan;
            PieOpenScaleYAnimation.Duration = PieAnimationTimeSpan;
            PieCloseOpacityAnimation.Duration = PieAnimationTimeSpan;
            PieCloseRotateAnimation.Duration = PieAnimationTimeSpan;
            PieCloseScaleXAnimation.Duration = PieAnimationTimeSpan;
            PieCloseScaleYAnimation.Duration = PieAnimationTimeSpan;

            Storyboard.SetTarget(PieOpenOpacityAnimation, DesignPie);
            Storyboard.SetTargetProperty(PieOpenOpacityAnimation, "Opacity");
            Storyboard.SetTarget(PieOpenRotateAnimation, PieCompositeTransform);
            Storyboard.SetTargetProperty(PieOpenRotateAnimation, "Rotation");
            Storyboard.SetTarget(PieOpenScaleXAnimation, PieCompositeTransform);
            Storyboard.SetTargetProperty(PieOpenScaleXAnimation, "ScaleX");
            Storyboard.SetTarget(PieOpenScaleYAnimation, PieCompositeTransform);
            Storyboard.SetTargetProperty(PieOpenScaleYAnimation, "ScaleY");

            Storyboard.SetTarget(PieCloseOpacityAnimation, DesignPie);
            Storyboard.SetTargetProperty(PieCloseOpacityAnimation, "Opacity");
            Storyboard.SetTarget(PieCloseRotateAnimation, PieCompositeTransform);
            Storyboard.SetTargetProperty(PieCloseRotateAnimation, "Rotation");
            Storyboard.SetTarget(PieCloseScaleXAnimation, PieCompositeTransform);
            Storyboard.SetTargetProperty(PieCloseScaleXAnimation, "ScaleX");
            Storyboard.SetTarget(PieCloseScaleYAnimation, PieCompositeTransform);
            Storyboard.SetTargetProperty(PieCloseScaleYAnimation, "ScaleY");
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

            Loaded += (sender, args) =>
            {
                CenterButtonTop = Diameter/2 - (CenterButton.ActualWidth/2);
                CenterButtonLeft = Diameter/2 - (CenterButton.ActualHeight/2);
            };
            CenterButton.Style = Resources["RoundedCenterButton"] as Style;

            Pie.SourceRadialMenu = this;
            LayoutRoot.DataContext = this;
            CenterButton.Tapped += OnCenterButtonTapped;

            SetupStoryboards();
        }
    }
}