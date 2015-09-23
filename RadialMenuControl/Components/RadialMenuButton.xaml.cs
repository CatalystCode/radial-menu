namespace RadialMenuControl.Components
{
    using System;
    using System.Collections.ObjectModel;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public partial class RadialMenuButton : Button
    {
                public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(RadialMenuButton), null);

        public static readonly DependencyProperty HideLabelProperty =
            DependencyProperty.Register("HideLabel", typeof(bool), typeof(RadialMenuButton), null);

        public string Label
        {
            get { return (string)GetValue(LabelProperty) ?? ""; }
            set { SetValue(LabelProperty, value.ToUpperInvariant()); }
        }

        public bool HideLabel
        {
            get { return (bool)GetValue(HideLabelProperty); }
            set { SetValue(HideLabelProperty, value); }
        }

        public RadialMenuButton()
        {
            InitializeComponent();
        }
    }
}
