﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using RadialMenuControl.Components;
using RadialMenuControl.Extensions;
using RadialMenuControl.Shims;
using RadialMenuControl.Themes;

namespace RadialMenuControl.UserControl
{
    /// <summary>
    ///     The base class for the Radial Menu Control - and also the user control. A RadialMenu contains
    ///     multiple RadialMenuButtons, which in turn can have sub-menues.
    /// </summary>
    /// <example>
    ///     <code>
    /// var myRadialMenu = new RadialMenu
    ///     {
    ///         CenterButtonIcon = "F",
    ///         Diameter = 300,
    ///         Buttons =
    ///         {
    ///             new RadialMenuButton
    ///             {
    ///                 Label = "I'm a button!"
    ///             },
    ///             new RadialMenuButton
    ///             {
    ///                 Label = "Me too!"
    ///             }
    ///         }
    ///     }
    /// </code>
    ///     <remarks>
    ///         See RadialMenuDemo for complex tutorials - the menu is quite flexible and powerful,
    ///         meaning that it can be used in a number of ways.
    ///     </remarks>
    /// </example>
    public partial class RadialMenu : MenuBase
    {
        // Events
        public delegate void CenterButtonTappedHandler(object sender, TappedRoutedEventArgs e);

        public static readonly DependencyProperty OuterArcThicknesssProperty =
            DependencyProperty.Register("OuterThickness", typeof (double), typeof (RadialMenu),
                new PropertyMetadata((double) 20));

        private IList<Pie> _previousPies = new List<Pie>();

        private double _startAngle = 22.5;
        public ObservableCollection<MenuBase> DisplayMenus = new ObservableCollection<MenuBase>();

        /// <summary>
        ///     Initializes the RadialMenu. The constructrer doesn't accept any parameters.
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
                PieCompositeTransform.CenterX = Diameter/2;
                PieCompositeTransform.CenterY = Diameter/2;
                CustomPieCompositeTransform.CenterX = Diameter/2;
                CustomPieCompositeTransform.CenterY = Diameter/2;
            };
            CenterButton.Style = Resources["RoundedCenterButton"] as Style;

            Pie.SourceRadialMenu = this;
            CenterButton.DataContext = this;
            CenterButton.Tapped += OnCenterButtonTapped;
        }

        private bool _isMenuChangeAnimated = true;
        /// <summary>
        ///     Is the transition between submenus animated?
        /// </summary>
        public bool IsMenuChangeAnimated
        {
            get { return _isMenuChangeAnimated; }
            set { SetField(ref _isMenuChangeAnimated, value); }
        }

        /// <summary>
        ///     A public reference to the Pie used but the RadialMenu. The Pie abstracts away all the Math that has to happen
        ///     for the RadialMenu to be fast, pretty, and flexible.
        /// </summary>
        public Pie Pie
        {
            get { return DesignPie; }
            private set { DesignPie = value; }
        }

        /// <summary>
        ///     Start Angle for the first button. 0 is "top north".
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

        /// <summary>
        ///     Default for the thickness of the outer arc, if not set on a RadialMenuButton
        /// </summary>
        public double OuterThickness
        {
            get { return (double) GetValue(OuterArcThicknesssProperty); }
            set { SetValue(OuterArcThicknesssProperty, value); }
        }

        /// <summary>
        ///     Previous pies (for back navigation)
        /// </summary>
        public IList<Pie> PreviousPies
        {
            get { return _previousPies; }
            set { SetField(ref _previousPies, value); }
        }

        /// <summary>
        ///     Previous center buttons (for back navigation)
        /// </summary>
        public Stack<CenterButtonShim> PreviousButtons
        {
            get { return PreviousCenterButtons; }
            set { SetField(ref PreviousCenterButtons, value); }
        }

        /// <summary>
        ///     RadialMenuButtons on this menu. You can add buttons either in XAML or in code-behind.
        ///     If you change the list, the menu will be redrawn.
        /// </summary>
        public IList<RadialMenuButton> Buttons
        {
            get { return Pie.Slices; }
            set { Pie.Slices = value; }
        }

        /// <summary>
        /// Fired when the center button has been tapped
        /// </summary>
        public event CenterButtonTappedHandler CenterButtonTapped;

        /// <summary>
        ///     Event handler for dependency properties, asking the pie to redraw
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private static void DependencyPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var instance = (RadialMenu) obj;
            instance.Pie.Draw();
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
        ///     Show or hide the outer wheel. This change is animated.
        /// </summary>
        /// <remarks>
        ///     If you want to change the animation, you can do so using the storyboards
        ///     OpenStoryboard and CloseStoryboard as defined in RadialMenu.xaml.
        /// </remarks>
        public async void TogglePie()
        {
            var floatingParent = FindParent<Floating>(this);
            var distance = Diameter/2 - CenterButton.ActualHeight/2;

            if (Pie.Visibility == Visibility.Visible)
            {
                BackgroundEllipse.Visibility = Visibility.Collapsed;

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
                Canvas.SetTop(CenterButton, Diameter/2 - CenterButton.ActualHeight/2);
                Canvas.SetLeft(CenterButton, Diameter/2 - CenterButton.ActualWidth/2);
                Pie.Visibility = Visibility.Visible;

                await OpenStoryboard.PlayAsync();

                BackgroundEllipse.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        ///     Adds a RadialMenuButton to the current pie
        /// </summary>
        /// <param name="button">RadialMenuButton to add to the current pie</param>
        /// <remarks>If you add a button to the currently visible Radial Menu, the menu will be redrawn.</remarks>
        public void AddButton(RadialMenuButton button)
        {
            Pie.Slices.Add(button);
        }

        /// <summary>
        ///     Event Handler for a center button tap, calling user-registered events and handling navigation (if enabled)
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="e">Event information</param>
        private void OnCenterButtonTapped(object s, TappedRoutedEventArgs e)
        {
            // If an event has been registered with the center button tap, call it
            CenterButtonTapped?.Invoke(this, e);

            if (PreviousPies.Count == 0)
            {
                TogglePie();
            }

            if (PreviousPies.Count <= 0 || !IsCenterButtonNavigationEnabled) return;

            // a custom menu may have changed dragability. Ensure we are draggable, of using Floating
            var floatingParent = FindParent<Floating>(this);
            if (floatingParent != null) floatingParent.ShouldManipulateChild = true;

            // If we have a previous pie, we're going back to it
            ChangePie(this, PreviousPies[PreviousPies.Count - 1], false);
            PreviousPies.RemoveAt(PreviousPies.Count - 1);

            // We don't necessarily have the same amount of pies and center buttons.
            // Users can create submenues that don't bring their own center button
            if (PreviousButtons.Count <= 0) return;
            ChangeCenterButton(this, PreviousButtons.Pop(), false);
        }

        /// <summary>
        ///     Change the whole radial menu, using a new menu object.
        /// </summary>
        /// <remarks>This method is used to facilitate the transition between a parent and a submenu.</remarks>
        /// <param name="s">Sending object</param>
        /// <param name="menu">Menu to change to</param>
        public void ChangeMenu(object s, MenuBase menu)
        {
            if (menu is RadialMenu)
            {
                var radialMenu = (RadialMenu) menu;
                ChangePie(s, radialMenu.Pie, true);
                ChangeCenterButton(s, Helpers.ButtonToShim(radialMenu.CenterButton, radialMenu.CenterButtonTapped),
                    true);
            }
            else
            {
                ChangeToCustomMenu(s, menu, true);
                ChangeCenterButton(s, Helpers.ButtonToShim(menu.CenterButton), true);
            }

            var floatingParent = FindParent<Floating>(this);
            if (floatingParent != null)
            {
                floatingParent.ShouldManipulateChild = menu.IsDraggable;
            }
        }

        /// <summary>
        ///     Clears the current pie, removing all currently displayed slices.
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
        ///     Change to custom MenuBase menu.
        /// </summary>
        /// <param name="s">Sending object</param>
        /// <param name="newSubMenu">The new submenu which will be placed in customRadialControlRoot Canvas</param>
        /// <param name="storePrevious">Should we store the previous pie (for back navigation)?</param>
        public async void ChangeToCustomMenu(object s, MenuBase newSubMenu, bool storePrevious)
        {
            BackgroundEllipse.Visibility = Visibility.Visible;
            if (IsMenuChangeAnimated) await PieExitForChangeStoryboard.PlayAsync();

            _clearPie(storePrevious);

            // Redraw
            Pie.Draw();
            Pie.UpdateLayout();

            newSubMenu.Diameter = Diameter;
            CustomRadialControlRoot.Children.Add(newSubMenu);

            // Check if we're in a MeterSubMenu
            if (newSubMenu is MeterSubMenu)
            {
                var newMeterSubMenu = newSubMenu as MeterSubMenu;
                var defaultBackground = (HasBackgroundEllipse)
                    ? new SolidColorBrush(BackgroundEllipseFill)
                    : new SolidColorBrush(InnerNormalColor);
                newMeterSubMenu.SetDefault(MeterSubMenu.BackgroundFillBrushProperty, defaultBackground);
                newMeterSubMenu.SetDefault(MeterSubMenu.OuterEdgeBrushProperty, new SolidColorBrush(OuterDisabledColor));
                newMeterSubMenu.SetDefault(MeterSubMenu.OuterEdgeThicknessProperty, OuterThickness);
            }

            newSubMenu.UpdateLayout();

            if (IsMenuChangeAnimated) await CustomPieEnterForChangeStoryboard.PlayAsync();
            BackgroundEllipse.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     Change the current pie by replacing its PieSlices with new ones. If storePrevious is set to true,
        ///     the current pie will be stored away and restored during back navigation.
        /// </summary>
        /// <remarks>This method is used to facilitate the transition between RadialMenus.</remarks>
        /// <param name="s">Sending object</param>
        /// <param name="newPie">Pie object to take RadialMenuButtons from</param>
        /// <param name="storePrevious">Should we store the previous pie (for back navigation)?</param>
        public async void ChangePie(object s, Pie newPie, bool storePrevious)
        {
            BackgroundEllipse.Visibility = Visibility.Visible;

            foreach (var ps in Pie.PieSlices)
            {
                ps.OuterArcElement.Visibility = Visibility.Collapsed;
            }

            // Play animations, depending on what's in the control
            if (CustomRadialControlRoot.Children.Count > 0)
            {
                if (IsMenuChangeAnimated) await CustomPieExitForChangeStoryboard.PlayAsync();
            }
            else
            {
                if (IsMenuChangeAnimated) await PieExitForChangeStoryboard.PlayAsync();
            }

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
            Pie.SelectedItem = newPie.SelectedItem ?? null;

            if (IsMenuChangeAnimated) await PieEnterForChangeStoryboard.PlayAsync();
            BackgroundEllipse.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     Change the center button using a CenterButtonShim object, which is used to store the current state of a
        ///     CenterButton in a leightweight way.
        /// </summary>
        /// <remarks>This method is used to facilitate transitions between submenus.</remarks>
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
                    CenterButtonTappedHandler = CenterButtonTapped,
                    Opacity = CenterButtonOpacity
                };

                PreviousButtons.Push(backupButton);
            }

            // Decorate the current button with new props
            // Center button style defaults to current style unless overridden
            CenterButtonBorder = newButton?.BorderBrush ?? CenterButtonBorder;
            CenterButtonBackgroundFill = newButton?.Background ?? CenterButtonBackgroundFill;
            CenterButtonIcon = (string) newButton?.Content ?? CenterButtonIcon;
            CenterButtonFontSize = newButton?.FontSize ?? CenterButtonFontSize;
            CenterButtonTapped = newButton?.CenterButtonTappedHandler ?? null;
            CenterButtonOpacity = newButton?.Opacity ?? CenterButtonOpacity;

            if (newButton != null && !double.IsNaN(newButton.Top ?? 0) && !double.IsNaN(newButton.Left ?? 0))
            {
                CenterButtonTop = newButton.Top ?? CenterButtonTop;
                CenterButtonLeft = newButton.Left ?? CenterButtonTop;
            }
        }

        /// <summary>
        ///     Hide all tooltips displaying set access keys for the currently visible RadialMenuButtons.
        /// </summary>
        /// <remarks>
        ///     RadialMenuButtons do not automatically generate AccessKeys - if you whish to enable keyboard navigation,
        ///     please consult the RadialMenuButton documentation and the "Access Keys" scenario documentation.
        /// </remarks>
        public void HideAccessKeyTooltips()
        {
            foreach (var ps in Pie.PieSlices)
            {
                ps.AreAccessKeyToolTipsVisible = false;
            }
        }

        /// <summary>
        ///     Show all tooltips displaying set access keys for the currently visible RadialMenuButtons.
        /// </summary>
        /// <remarks>
        ///     RadialMenuButtons do not automatically generate AccessKeys - if you whish to enable keyboard navigation,
        ///     please consult the RadialMenuButton documentation and the "Access Keys" scenario documentation.
        /// </remarks>
        public void ShowAccessKeyTooltips()
        {
            foreach (var ps in Pie.PieSlices)
            {
                ps.AreAccessKeyToolTipsVisible = true;
            }
        }

        /// <summary>
        ///     Programmatically "click" the inner arc in a RadialMenuButton.
        /// </summary>
        /// <remarks>
        ///     This method is useful when implementing keyboard navigation.
        ///     RadialMenuButtons do not automatically generate AccessKeys - if you whish to enable keyboard navigation,
        ///     please consult the RadialMenuButton documentation and the "Access Keys" scenario documentation.
        /// </remarks>
        /// <param name="rmb">The RadialMenuButton to programmatically click.</param>
        public void ClickInnerRadialMenuButton(RadialMenuButton rmb)
        {
            foreach (var ps in Pie.PieSlices.Where(ps => ps.OriginalRadialMenuButton == rmb))
            {
                ps.ClickInner();
            }
        }

        /// <summary>
        ///     Programmatically "click" the outer arc in a RadialMenuButton
        /// </summary>
        /// <remarks>
        ///     This method is useful when implementing keyboard navigation.
        ///     RadialMenuButtons do not automatically generate AccessKeys - if you whish to enable keyboard navigation,
        ///     please consult the RadialMenuButton documentation and the "Access Keys" scenario documentation.
        /// </remarks>
        /// <param name="rmb">The RadialMenuButton to programmatically click.</param>
        public void ClickOuterRadialMenuButton(RadialMenuButton rmb)
        {
            foreach (var ps in Pie.PieSlices.Where(ps => ps.OriginalRadialMenuButton == rmb))
            {
                ps.ClickOuter();
            }
        }

        #region Colors

        // Outer Arc Colors
        public static readonly DependencyProperty OuterNormalColorProperty =
            DependencyProperty.Register("OuterNormalColor", typeof (Color), typeof (RadialMenu),
                new PropertyMetadata(DefaultColors.OuterNormalColor, DependencyPropertyChanged));

        public static readonly DependencyProperty OuterDisabledColorProperty =
            DependencyProperty.Register("OuterDisabledColor", typeof (Color), typeof (RadialMenu),
                new PropertyMetadata(DefaultColors.OuterDisabledColor, DependencyPropertyChanged));

        public static readonly DependencyProperty OuterHoverColorProperty =
            DependencyProperty.Register("OuterHoverColor", typeof (Color), typeof (RadialMenu),
                new PropertyMetadata(DefaultColors.OuterHoverColor, DependencyPropertyChanged));

        public static readonly DependencyProperty OuterTappedColorProperty =
            DependencyProperty.Register("OuterTappedColor", typeof (Color), typeof (RadialMenu),
                new PropertyMetadata(DefaultColors.OuterTappedColor, DependencyPropertyChanged));

        public Color OuterHoverColor
        {
            get { return (Color) GetValue(OuterHoverColorProperty); }
            set { SetValue(OuterHoverColorProperty, value); }
        }

        public Color OuterNormalColor
        {
            get { return (Color) GetValue(OuterNormalColorProperty); }
            set { SetValue(OuterNormalColorProperty, value); }
        }

        public Color OuterDisabledColor
        {
            get { return (Color) GetValue(OuterDisabledColorProperty); }
            set
            {
                SetValue(OuterDisabledColorProperty, value);
                BackgroundEllipse.Stroke = new SolidColorBrush(value);
            }
        }

        public SolidColorBrush OuterDisabledBrush { get; set; }

        public Color OuterTappedColor
        {
            get { return (Color) GetValue(OuterTappedColorProperty); }
            set { SetValue(OuterTappedColorProperty, value); }
        }

        // Inner Arc Colors
        public static readonly DependencyProperty InnerNormalColorProperty =
            DependencyProperty.Register("InnerNormalColor", typeof (Color), typeof (RadialMenu),
                new PropertyMetadata(DefaultColors.InnerNormalColor, DependencyPropertyChanged));

        public static readonly DependencyProperty InnerHoverColorProperty =
            DependencyProperty.Register("InnerHoverColor", typeof (Color), typeof (RadialMenu),
                new PropertyMetadata(DefaultColors.InnerHoverColor, DependencyPropertyChanged));

        public static readonly DependencyProperty InnerTappedColorProperty =
            DependencyProperty.Register("InnerTappedColor", typeof (Color), typeof (RadialMenu),
                new PropertyMetadata(DefaultColors.InnerTappedColor, DependencyPropertyChanged));

        public static readonly DependencyProperty InnerReleasedColorProperty =
            DependencyProperty.Register("InnerReleasedColor", typeof (Color), typeof (RadialMenu),
                new PropertyMetadata(DefaultColors.InnerReleasedColor, DependencyPropertyChanged));

        public Color InnerHoverColor
        {
            get { return (Color) GetValue(InnerHoverColorProperty); }
            set { SetValue(InnerHoverColorProperty, value); }
        }

        public Color InnerNormalColor
        {
            get { return (Color) GetValue(InnerNormalColorProperty); }
            set
            {
                SetValue(InnerNormalColorProperty, value);
                BackgroundEllipse.Fill = new SolidColorBrush(value);
            }
        }

        public Color InnerTappedColor
        {
            get { return (Color) GetValue(InnerTappedColorProperty); }
            set { SetValue(InnerTappedColorProperty, value); }
        }

        public Color InnerReleasedColor
        {
            get { return (Color) GetValue(InnerReleasedColorProperty); }
            set { SetValue(InnerReleasedColorProperty, value); }
        }

        // Indication Arcs
        public static readonly DependencyProperty UseIndicationArcsProperty =
            DependencyProperty.Register("UseIndicationArcsProperty", typeof (bool), typeof (RadialMenu),
                new PropertyMetadata(true, DependencyPropertyChanged));

        public static readonly DependencyProperty IndicationArcColorProperty =
            DependencyProperty.Register("IndicationArcColor", typeof (Color), typeof (RadialMenu),
                new PropertyMetadata(DefaultColors.OuterTappedColor, DependencyPropertyChanged));

        public static readonly DependencyProperty IndicationArcStrokeThicknessProperty =
            DependencyProperty.Register("IndicationArcStrokeThickness", typeof (double), typeof (RadialMenu),
                new PropertyMetadata(3.0, DependencyPropertyChanged));

        public static readonly DependencyProperty IndicationArcDistanceFromEdgeProperty =
            DependencyProperty.Register("IndicationArcDistanceFromEdge", typeof (double), typeof (RadialMenu),
                new PropertyMetadata(10.0, DependencyPropertyChanged));

        /// <summary>
        ///     Distance from the inner part of the outer band of the menu to the indication arc
        /// </summary>
        public double IndicationArcDistanceFromEdge
        {
            get { return (double) GetValue(IndicationArcDistanceFromEdgeProperty); }
            set { SetValue(IndicationArcDistanceFromEdgeProperty, value); }
        }

        /// <summary>
        ///     If set to true, an idication arc will be used on all radial menu buttons
        /// </summary>
        public bool UseIndicationArcs
        {
            get { return (bool) GetValue(UseIndicationArcsProperty); }
            set { SetValue(UseIndicationArcsProperty, value); }
        }

        /// <summary>
        ///     Sets the color for the indication arc
        /// </summary>
        public Color IndicationArcColor
        {
            get { return (Color) GetValue(IndicationArcColorProperty); }
            set { SetValue(IndicationArcColorProperty, value); }
        }

        /// <summary>
        ///     The stroke thickness for the indication arc
        /// </summary>
        public double IndicationArcStrokeThickness
        {
            get { return (double) GetValue(IndicationArcStrokeThicknessProperty); }
            set { SetValue(IndicationArcStrokeThicknessProperty, value); }
        }

        // Background Ellipse
        public static readonly DependencyProperty HasBackgroundEllipseProperty =
            DependencyProperty.Register("HasBackgroundEllipse", typeof (bool), typeof (RadialMenu), null);

        public static readonly DependencyProperty BackgroundEllipseFillProperty =
            DependencyProperty.Register("BackgroundEllipseFill", typeof (Color), typeof (RadialMenu),
                new PropertyMetadata(Colors.Transparent));

        /// <summary>
        ///     Background Ellpise, drawn behind the whole menu. Ignored if set on a submenu.
        /// </summary>
        public bool HasBackgroundEllipse
        {
            get { return (bool) GetValue(HasBackgroundEllipseProperty); }
            set { SetValue(HasBackgroundEllipseProperty, value); }
        }

        /// <summary>
        ///     Fill color for the background ellpise
        /// </summary>
        public Color BackgroundEllipseFill
        {
            get { return (Color) GetValue(BackgroundEllipseFillProperty); }
            set { SetValue(BackgroundEllipseFillProperty, value); }
        }

        #endregion
    }
}