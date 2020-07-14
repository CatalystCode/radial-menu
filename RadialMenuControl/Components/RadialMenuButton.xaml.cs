using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using RadialMenuControl.UserControl;

namespace RadialMenuControl.Components
{
    public partial class RadialMenuButton : Button
    {
        public enum ButtonType
        {
            Simple = 0,
            Radio,
            Toggle,
            Custom
        };

        // SubMenu
        public static readonly DependencyProperty SubmenuProperty =
            DependencyProperty.Register("Submenu", typeof (RadialMenu), typeof (RadialMenuButton), null);

        public RadialMenuButton()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     A RadialMenu that is opened when the user presses the button
        /// </summary>
        public RadialMenu Submenu
        {
            get { return (RadialMenu) GetValue(SubmenuProperty); }
            set { SetValue(SubmenuProperty, value); }
        }

        /// <summary>
        ///     Allows the use of key/value pairs to set the colors
        /// </summary>
        /// <param name="colors">Dictionary containing colors in a key/value pair</param>
        public void SetColors(Dictionary<string, Color> colors)
        {
            foreach (var colorVariable in colors)
            {
                if (GetType().GetProperty(colorVariable.Key) != null)
                {
                    var prop = GetType().GetProperty(colorVariable.Key);
                    prop.SetValue(this, colorVariable.Value);
                }
            }
        }

        # region properties

        // Label
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof (string), typeof (RadialMenuButton), new PropertyMetadata(""));

        public static readonly DependencyProperty LabelColorProperty =
            DependencyProperty.Register("LabelColor", typeof(Brush), typeof(RadialMenuButton), 
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty LabelSizeProperty =
            DependencyProperty.Register("LabelSize", typeof (int), typeof (RadialMenuButton), new PropertyMetadata(10));

        public static readonly DependencyProperty IsLabelHiddenProperty =
            DependencyProperty.Register("IsLabelHidden", typeof (bool), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof (string), typeof (RadialMenuButton), new PropertyMetadata(""));

        public static readonly DependencyProperty IconFontFamilyProperty =
            DependencyProperty.Register("IconFontFamily", typeof (FontFamily), typeof (RadialMenuButton),
                new PropertyMetadata(new FontFamily("Segoe UI")));

        public static readonly DependencyProperty IconForegroundBrushProperty =
            DependencyProperty.Register("IconForegroundBrush", typeof (Brush), typeof (RadialMenuButton),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof (int), typeof (RadialMenuButton), new PropertyMetadata(26));

        public static readonly DependencyProperty IconImageProperty =
            DependencyProperty.Register("IconImage", typeof (ImageSource), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty MenuSeletedProperty =
            DependencyProperty.Register("MenuSelected", typeof (bool), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register("ButtonType", typeof (ButtonType), typeof (RadialMenuButton),
                new PropertyMetadata(ButtonType.Simple));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof (object), typeof (RadialMenuButton), null);

        /// <summary>
        ///     Label, displayed in the inner portion of the button
        /// </summary>
        public string Label
        {
            get { return (string) GetValue(LabelProperty) ?? ""; }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        ///     Font Size for the label
        /// </summary>
        public int LabelSize
        {
            get { return (int) GetValue(LabelSizeProperty); }
            set { SetValue(LabelSizeProperty, value); }
        }

        public Brush LabelColor
        {
            get { return (Brush)GetValue(LabelColorProperty); }
            set { SetValue(LabelColorProperty, value); }
        }

        /// <summary>
        ///     Should the label be hidden?
        /// </summary>
        public bool IsLabelHidden
        {
            get { return (bool) GetValue(IsLabelHiddenProperty); }
            set { SetValue(IsLabelHiddenProperty, value); }
        }

        /// <summary>
        ///     Text-based icon, displayed in a TextBox (usually used with icon fonts)
        /// </summary>
        public string Icon
        {
            get { return (string) GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        ///     Font for the text-based icon
        /// </summary>
        public FontFamily IconFontFamily
        {
            get { return (FontFamily) GetValue(IconFontFamilyProperty); }
            set { SetValue(IconFontFamilyProperty, value); }
        }

        /// <summary>
        ///     Font size for the text-based icon
        /// </summary>
        public int IconSize
        {
            get { return (int) GetValue(IconSizeProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        ///     ForegroundBrush for the text-based icon
        /// </summary>
        public Brush IconForegroundBrush
        {
            get { return (Brush) GetValue(IconForegroundBrushProperty); }
            set { SetValue(IconForegroundBrushProperty, value); }
        }

        /// <summary>
        ///     A ImageSource for the icon. If set, the text-based icon will be hidden.
        /// </summary>
        public ImageSource IconImage
        {
            get { return (ImageSource) GetValue(IconImageProperty); }
            set { SetValue(IconImageProperty, value); }
        }

        // Values & Button Type
        /// <summary>
        ///     If the button is a radio button and selected on behalf of the whole RadialMenu, this value will be true (false
        ///     otherwise)
        /// </summary>
        public bool MenuSelected
        {
            get { return (bool) GetValue(MenuSeletedProperty); }
            set { SetValue(MenuSeletedProperty, value); }
        }

        /// <summary>
        ///     Value of this button
        /// </summary>
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set
            {
                if (Type == ButtonType.Simple)
                {
                    throw new Exception("A button of type SIMPLE should not have any value.");
                }
                SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        ///     Button type, indicating the way users can interact with this button
        /// </summary>
        public ButtonType Type
        {
            get { return (ButtonType) GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }

        // Inner Arc Colors
        public static readonly DependencyProperty InnerNormalColorProperty =
            DependencyProperty.Register("InnerNormalColor", typeof (Color?), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty InnerHoverColorProperty =
            DependencyProperty.Register("InnerHoverColor", typeof (Color?), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty InnerTappedColorProperty =
            DependencyProperty.Register("InnerTappedColor", typeof (Color?), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty InnerReleasedColorProperty =
            DependencyProperty.Register("InnerReleasedColor", typeof (Color?), typeof (RadialMenuButton), null);

        /// <summary>
        ///     Hover color for the inner portion of the button
        /// </summary>
        public Color? InnerHoverColor
        {
            get { return (Color?) GetValue(InnerHoverColorProperty); }
            set { SetValue(InnerHoverColorProperty, value); }
        }

        /// <summary>
        ///     Normal color for the inner portion of the button
        /// </summary>
        public Color? InnerNormalColor
        {
            get { return (Color?) GetValue(InnerNormalColorProperty); }
            set { SetValue(InnerNormalColorProperty, value); }
        }

        /// <summary>
        ///     Tapped color for the inner portion of the button
        /// </summary>
        public Color? InnerTappedColor
        {
            get { return (Color?) GetValue(InnerTappedColorProperty); }
            set { SetValue(InnerTappedColorProperty, value); }
        }

        /// <summary>
        ///     Released color for the inner portion of the button
        /// </summary>
        public Color? InnerReleasedColor
        {
            get { return (Color?) GetValue(InnerReleasedColorProperty); }
            set { SetValue(InnerReleasedColorProperty, value); }
        }

        // Outer Arc Colors
        public static readonly DependencyProperty OuterNormalColorProperty =
            DependencyProperty.Register("OuterNormalColor", typeof (Color?), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty OuterDisabledColorProperty =
            DependencyProperty.Register("OuterDisabledColor", typeof (Color?), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty OuterHoverColorProperty =
            DependencyProperty.Register("OuterHoverColor", typeof (Color?), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty OuterTappedColorProperty =
            DependencyProperty.Register("OuterTappedColor", typeof (Color?), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty OuterThicknessProperty =
            DependencyProperty.Register("OuterThickness", typeof (double?), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty StrokeColorProperty =
            DependencyProperty.Register("StrokeColor", typeof (Color?), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof (double?), typeof (RadialMenuButton), null);

        // Indication Arc
        public static readonly DependencyProperty UseIndicationArcProperty =
            DependencyProperty.Register("UseIndicationArcProperty", typeof (bool?), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty IndicationArcColorProperty =
            DependencyProperty.Register("IndicationArcColor", typeof (Color?), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty IndicationArcStrokeThicknessProperty =
            DependencyProperty.Register("IndicationArcStrokeThickness", typeof (double?), typeof (RadialMenuButton),
                null);

        public static readonly DependencyProperty IndicationArcDistanceFromEdgeProperty =
            DependencyProperty.Register("IndicationArcDistanceFromEdge", typeof (double?), typeof (RadialMenuButton),
                null);

        /// <summary>
        ///     Distance from the inner part of the outer band to the indication arc
        /// </summary>
        public double? IndicationArcDistanceFromEdge
        {
            get { return (double?) GetValue(IndicationArcDistanceFromEdgeProperty); }
            set { SetValue(IndicationArcDistanceFromEdgeProperty, value); }
        }

        /// <summary>
        ///     When set to true, an indication arc will be placed on this button
        /// </summary>
        public bool? UseIndicationArc
        {
            get { return (bool?) GetValue(UseIndicationArcProperty); }
            set { SetValue(UseIndicationArcProperty, value); }
        }

        public Color? IndicationArcColor
        {
            get { return (Color?) GetValue(IndicationArcColorProperty); }
            set { SetValue(IndicationArcColorProperty, value); }
        }

        /// <summary>
        ///     The Stroke thickness of the indication arc
        /// </summary>
        public double? IndicationArcStrokeThickness
        {
            get { return (double?) GetValue(IndicationArcStrokeThicknessProperty); }
            set { SetValue(IndicationArcStrokeThicknessProperty, value); }
        }

        /// <summary>
        ///     Hover color for the outer portion of the button
        /// </summary>
        public Color? OuterHoverColor
        {
            get { return (Color?) GetValue(OuterHoverColorProperty); }
            set { SetValue(OuterHoverColorProperty, value); }
        }

        /// <summary>
        ///     Normal color for the outer portion of the button
        /// </summary>
        public Color? OuterNormalColor
        {
            get { return (Color?) GetValue(OuterNormalColorProperty); }
            set { SetValue(OuterNormalColorProperty, value); }
        }

        /// <summary>
        ///     Disabled color for the outer portion of the button
        /// </summary>
        public Color? OuterDisabledColor
        {
            get { return (Color?) GetValue(OuterDisabledColorProperty); }
            set { SetValue(OuterDisabledColorProperty, value); }
        }

        /// <summary>
        ///     Tapped color for the outer portion of the button
        /// </summary>
        public Color? OuterTappedColor
        {
            get { return (Color?) GetValue(OuterTappedColorProperty); }
            set { SetValue(OuterTappedColorProperty, value); }
        }

        // Stroke
        /// <summary>
        ///     Color of the stroke around the PieSLice
        /// </summary>
        public Color? StrokeColor
        {
            get { return (Color?) GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }

        /// <summary>
        ///     Thickness of the stroke around the PieSlice
        /// </summary>
        public double? StrokeThickness
        {
            get { return (double?) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        ///     Thickness of the outer arc, on the outer side of the button
        /// </summary>
        public double? OuterThickness
        {
            get { return (double?) GetValue(OuterThicknessProperty); }
            set { SetValue(OuterThicknessProperty, value); }
        }

        // CustomMenu
        public static readonly DependencyProperty CustomMenuProperty =
            DependencyProperty.Register("Submenu", typeof (MenuBase), typeof (RadialMenuButton), null);

        /// <summary>
        ///     CustomMenu behind the button
        /// </summary>
        public MenuBase CustomMenu
        {
            get { return (MenuBase) GetValue(CustomMenuProperty); }
            set { SetValue(CustomMenuProperty, value); }
        }

        // Access Keys
        /// <summary>
        ///     Outer slice path access key
        /// </summary>
        public string OuterAccessKey
        {
            get { return (string)GetValue(OuterAccessKeyProperty); }
            set { SetValue(OuterAccessKeyProperty, value); }
        }

        /// <summary>
        ///     Inner slice path access key
        /// </summary>
        public string InnerAccessKey
        {
            get { return (string)GetValue(InnerAccessKeyProperty); }
            set { SetValue(InnerAccessKeyProperty, value); }
        }

        #endregion properties

        #region events

        /// <summary>
        ///     Delegate for the ValueChangedEvent, fired whenever the value of this button is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void ValueChangedHandler(object sender, RoutedEventArgs args);

        public event ValueChangedHandler ValueChanged;

        public void OnValueChanged(RoutedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        public static readonly DependencyProperty InnerAccessKeyProperty =
            DependencyProperty.Register("InnerAccessKey", typeof (string), typeof (RadialMenuButton), null);

        public static readonly DependencyProperty OuterAccessKeyProperty =
            DependencyProperty.Register("OuterAccessKey", typeof (string), typeof (RadialMenuButton), null);

        /// <summary>
        ///     Does this radial menu button have events on the outer arc?
        /// </summary>
        /// <returns></returns>
        public bool HasOuterArcEvents => (OuterArcPressed != null || OuterArcReleased != null);

        /// <summary>
        ///     Does this radial menu button have actions on the outer arc?
        /// </summary>
        /// <returns></returns>
        public bool HasOuterArcAction => (Submenu != null || CustomMenu != null || HasOuterArcEvents);

        public delegate void InnerArcPressedEventHandler(object sender, PointerRoutedEventArgs e);

        /// <summary>
        ///     Invoked when the inner arc of the button has been pressed (mouse, touch, stylus)
        /// </summary>
        public event InnerArcPressedEventHandler InnerArcPressed;

        public void OnInnerArcPressed(PointerRoutedEventArgs e)
        {
            InnerArcPressed?.Invoke(this, e);

            if (Type != ButtonType.Simple)
            {
                MenuSelected = true;
            }
            if (Type == ButtonType.Toggle)
            {
                Value = (Value == null || !(bool) Value);
            }
        }

        public delegate void OuterArcPressedEventHandler(object sender, PointerRoutedEventArgs e);

        /// <summary>
        ///     Invoked when the outer arc of the button has been pressed (mouse, touch, stylus)
        /// </summary>
        public event OuterArcPressedEventHandler OuterArcPressed;

        public void OnOuterArcPressed(PointerRoutedEventArgs e)
        {
            OuterArcPressed?.Invoke(this, e);
        }

        public delegate void InnerArcReleasedEventHandler(object sender, PointerRoutedEventArgs e);

        /// <summary>
        ///     Invoked when the inner arc of the button has been released (mouse, touch, stylus)
        /// </summary>
        public event InnerArcReleasedEventHandler InnerArcReleased;

        public void OnInnerArcReleased(PointerRoutedEventArgs e)
        {
            InnerArcReleased?.Invoke(this, e);
        }

        public delegate void OuterArcReleasedEventHandler(object sender, PointerRoutedEventArgs e);

        /// <summary>
        ///     Invoked when the outer arc of the button has been pressed (mouse, touch, stylus)
        /// </summary>
        public event OuterArcReleasedEventHandler OuterArcReleased;

        public void OnOuterArcReleased(PointerRoutedEventArgs e)
        {
            OuterArcReleased?.Invoke(this, e);
        }

        #endregion
    }
}