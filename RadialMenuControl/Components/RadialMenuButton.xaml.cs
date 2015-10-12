using System.Collections.Generic;

namespace RadialMenuControl.Components
{
    using UserControl;
    using System;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using System.Reflection;

    public partial class RadialMenuButton : Button
    {
        public enum ButtonType
        {
            Simple = 0,
            Radio,
            Toggle
        };
        
        # region properties
        // Label
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(RadialMenuButton), new PropertyMetadata(""));

        public static readonly DependencyProperty LabelSizeProperty =
            DependencyProperty.Register("LabelSize", typeof(int), typeof(RadialMenuButton), new PropertyMetadata(10));

        public static readonly DependencyProperty HideLabelProperty =
            DependencyProperty.Register("HideLabel", typeof(bool), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(RadialMenuButton), new PropertyMetadata(""));

        public static readonly DependencyProperty IconFontFamilyProperty =
            DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(RadialMenuButton), new PropertyMetadata(new FontFamily("Segoe UI")));

        public static readonly DependencyProperty IconForegroundBrushProperty =
            DependencyProperty.Register("IconForegroundBrush", typeof(Brush), typeof(PieSlice), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(int), typeof(RadialMenuButton), new PropertyMetadata(26));

        public static readonly DependencyProperty IconImageProperty =
            DependencyProperty.Register("IconImage", typeof(ImageSource), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty MenuSeletedProperty =
            DependencyProperty.Register("MenuSelected", typeof(bool), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register("ButtonTypeProperty", typeof(ButtonType), typeof(RadialMenuButton), new PropertyMetadata(ButtonType.Simple));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("ValueProperty", typeof(object), typeof(RadialMenuButton), null);

        public string Label
        {
            get { return (string)GetValue(LabelProperty) ?? ""; }
            set { SetValue(LabelProperty, value); }
        }
        public int LabelSize
        {
            get { return (int)GetValue(LabelSizeProperty); }
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

        public FontFamily IconFontFamily
        {
            get { return (FontFamily)GetValue(IconFontFamilyProperty); }
            set { SetValue(IconFontFamilyProperty, value); }
        }

        public int IconSize
        {
            get { return (int)GetValue(IconSizeProperty); }
            set { SetValue(IconProperty, value); }
        }

        public Brush IconForegroundBrush
        {
            get { return (Brush)GetValue(IconForegroundBrushProperty); }
            set { SetValue(IconForegroundBrushProperty, value); }
        }

        public ImageSource IconImage
        {
            get { return (ImageSource)GetValue(IconImageProperty); }
            set { SetValue(IconImageProperty, value); }
        }

        // Values & Button Type
        public bool MenuSelected
        {
            get { return (bool)GetValue(MenuSeletedProperty); }
            set { SetValue(MenuSeletedProperty, value); }
        }

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set
            {
                if (Type == ButtonType.Simple)
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
        
        // CustomMenu
        public static readonly DependencyProperty CustomMenuProperty =
            DependencyProperty.Register("Submenu", typeof(MenuBase), typeof(RadialMenuButton), null);
            
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

        public MenuBase CustomMenu
        {
            get { return (MenuBase)GetValue(CustomMenuProperty); }
            set { SetValue(CustomMenuProperty, value); }
        }
        #endregion properties

        #region events
        public delegate void InnerArcPressedEventHandler(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e);
        public event InnerArcPressedEventHandler InnerArcPressedEvent;

        public void OnInnerArcPressed(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            InnerArcPressedEvent?.Invoke(this, e);
        }

        public delegate void OuterArcPressedEventHandler(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e);
        public event OuterArcPressedEventHandler OuterArcPressedEvent;

        public void OnOuterArcPressed(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            OuterArcPressedEvent?.Invoke(this, e);
        }

        public delegate void InnerArcReleasedEventHandler(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e);
        public event InnerArcReleasedEventHandler InnerArcReleasedEvent;

        public void OnInnerArcReleased(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            InnerArcReleasedEvent?.Invoke(this, e);

            if (Type != ButtonType.Simple)
            {
                MenuSelected = true;
            }
            if (Type == ButtonType.Toggle)
            {
                Value = (Value != null && (bool)Value) ? false : true;
            }
        }

        public delegate void OuterArcReleasedEventHandler(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e);
        public event OuterArcReleasedEventHandler OuterArcReleasedEvent;

        public void OnOuterArcReleased(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            OuterArcReleasedEvent?.Invoke(this, e);
        }
        #endregion

        /// <summary>
        /// Does this radial menu button have events on the outer arc?
        /// </summary>
        /// <returns></returns>
        public bool HasOuterArcEvents()
        {
            return (OuterArcPressedEvent != null || OuterArcReleasedEvent != null);
        }

        /// <summary>
        /// Allows the use of key/value pairs to set the colors
        /// </summary>
        /// <param name="colors"></param>
        public void SetColors(Dictionary<string, Color> colors)
        {
            foreach (var colorVariable in colors)
            {
                if (this.GetType().GetProperty(colorVariable.Key) != null)
                {
                    var prop = this.GetType().GetProperty(colorVariable.Key);
                    prop.SetValue(this, colorVariable.Value);
                }
            }
        }

        // SubMenu
        public static readonly DependencyProperty SubmenuProperty =
            DependencyProperty.Register("Submenu", typeof(RadialMenu), typeof(RadialMenuButton), null);

        public RadialMenu Submenu
        {
            get { return (RadialMenu)GetValue(SubmenuProperty); }
            set { SetValue(SubmenuProperty, value); }
        }

        public RadialMenuButton()
        {
            InitializeComponent();
        }
    }
}

