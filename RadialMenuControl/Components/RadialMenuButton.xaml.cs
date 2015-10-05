namespace RadialMenuControl.Components
{
    using UserControl;
    using System;
    using System.Collections.ObjectModel;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    public partial class RadialMenuButton : Button
    {
        public enum ButtonType
        {
            SIMPLE = 0,
            RADIO,
            TOGGLE,
            LIST
        };

        // Label
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty LabelSizeProperty =
            DependencyProperty.Register("LabelSize", typeof(int?), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty HideLabelProperty =
            DependencyProperty.Register("HideLabel", typeof(bool), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(int?), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty IconImageProperty =
            DependencyProperty.Register("IconImage", typeof(ImageSource), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty MenuSeletedProperty =
            DependencyProperty.Register("MenuSelected", typeof(bool), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register("ButtonTypeProperty", typeof(ButtonType), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("ValueProperty", typeof(bool), typeof(RadialMenuButton), null);

        public string Label
        {
            get { return (string)GetValue(LabelProperty) ?? ""; }
            set { SetValue(LabelProperty, value); }
        }
        public int? LabelSize
        {
            get { return (int?)GetValue(LabelSizeProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public bool HideLabel
        {
            get { return (bool)GetValue(HideLabelProperty); }
            set { SetValue(HideLabelProperty, value); }
        }

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public int? IconSize
        {
            get { return (int?)GetValue(IconSizeProperty); }
            set { SetValue(IconProperty, value); }
        }

        public bool MenuSelected
        {
            get { return (bool)GetValue(MenuSeletedProperty); }
            set { SetValue(MenuSeletedProperty, value); }
        }

        public object Value {
            get { return (object)GetValue(ValueProperty); }
            set {
                if(this.Type == ButtonType.SIMPLE)
                {
                    throw new Exception("A button of type SIMPLE should not have any value.");
                }
                else
                {
                    SetValue(ValueProperty, value);
                }
            }
        }


        public ButtonType Type
        {
            get { return (ButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }

        public ImageSource IconImage
        {
            get { return (ImageSource)GetValue(IconImageProperty); }
            set { SetValue(IconImageProperty, value); }
        }
        
        // Inner Arc Colors
        public static readonly DependencyProperty InnerNormalColorProperty =
            DependencyProperty.Register("InnerNormalColor", typeof(Color?), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty InnerHoverColorProperty =
            DependencyProperty.Register("InnerHoverColor", typeof(Color?), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty InnerTappedColorProperty =
            DependencyProperty.Register("InnerTappedColor", typeof(Color?), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty InnerReleasedColorProperty =
            DependencyProperty.Register("InnerReleasedColor", typeof(Color?), typeof(RadialMenuButton), null);

        public Color? InnerHoverColor
        {
            get { return (Color?)GetValue(InnerHoverColorProperty); }
            set { SetValue(InnerHoverColorProperty, value); }
        }

        public Color? InnerNormalColor
        {
            get { return (Color?)GetValue(InnerNormalColorProperty); }
            set { SetValue(InnerNormalColorProperty, value); }
        }

        public Color? InnerTappedColor
        {
            get { return (Color?)GetValue(InnerTappedColorProperty); }
            set { SetValue(InnerTappedColorProperty, value); }
        }
        public Color? InnerReleasedColor
        {
            get { return (Color?)GetValue(InnerReleasedColorProperty); }
            set { SetValue(InnerReleasedColorProperty, value); }
        }

        // Outer Arc Colors
        public static readonly DependencyProperty OuterNormalColorProperty =
            DependencyProperty.Register("OuterNormalColor", typeof(Color?), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty OuterDisabledColorProperty =
            DependencyProperty.Register("OuterDisabledColor", typeof(Color?), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty OuterHoverColorProperty =
            DependencyProperty.Register("OuterHoverColor", typeof(Color?), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty OuterTappedColorProperty =
            DependencyProperty.Register("OuterTappedColor", typeof(Color?), typeof(RadialMenuButton), null);

        public Color? OuterHoverColor
        {
            get { return (Color?)GetValue(OuterHoverColorProperty); }
            set { SetValue(OuterHoverColorProperty, value); }
        }

        public Color? OuterNormalColor
        {
            get { return (Color?)GetValue(OuterNormalColorProperty); }
            set { SetValue(OuterNormalColorProperty, value); }
        }

        public Color? OuterDisabledColor
        {
            get { return (Color?)GetValue(OuterDisabledColorProperty); }
            set { SetValue(OuterDisabledColorProperty, value); }
        }

        public Color? OuterTappedColor
        {
            get { return (Color?)GetValue(OuterTappedColorProperty); }
            set { SetValue(OuterTappedColorProperty, value); }
        }

        // Events
        public delegate void InnerArcPressedEventHandler(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e);
        public event InnerArcPressedEventHandler InnerArcPressedEvent;

        public void OnInnerArcPressed(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (InnerArcPressedEvent != null)
            {
                InnerArcPressedEvent(this, e);
            }
        }

        public delegate void OuterArcPressedEventHandler(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e);
        public event OuterArcPressedEventHandler OuterArcPressedEvent;

        public void OnOuterArcPressed(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (OuterArcPressedEvent != null)
            {
                OuterArcPressedEvent(this, e);
            }
        }

        public delegate void InnerArcReleasedEventHandler(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e);
        public event InnerArcReleasedEventHandler InnerArcReleasedEvent;

        public void OnInnerArcReleased(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (InnerArcReleasedEvent != null)
            {
                InnerArcReleasedEvent(this, e);
            }

            if (this.Type != ButtonType.SIMPLE)
            {
                this.MenuSelected = true;
            }
            if(this.Type == ButtonType.TOGGLE)
            {
                this.Value = (bool)this.Value ? false : true;
            }
        }

        public delegate void OuterArcReleasedEventHandler(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e);
        public event OuterArcReleasedEventHandler OuterArcReleasedEvent;

        public void OnOuterArcReleased(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (OuterArcReleasedEvent != null)
            {
                OuterArcReleasedEvent(this, e);
            }
        }

        /// <summary>
        /// Does this radial menu button have events on the outer arc?
        /// </summary>
        /// <returns></returns>
        public bool HasOuterArcEvents()
        {
            return (OuterArcPressedEvent != null || OuterArcReleasedEvent != null);
        }

        // SubMenu can be a RadialMenu or a ListMenu
        public static readonly DependencyProperty SubmenuProperty =
            DependencyProperty.Register("Submenu", typeof(UserControl), typeof(RadialMenuButton), null);

        public UserControl Submenu
        {
            get { return (UserControl)GetValue(SubmenuProperty); }
            set { SetValue(SubmenuProperty, value); }
        }

        public RadialMenuButton()
        {
            InitializeComponent();
        }
    }
}
