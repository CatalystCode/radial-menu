using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using RadialMenuControl.Components;
using RadialMenuControl.UserControl;

namespace RadialMenuControl.Shims
{
    public class CenterButtonShim
    {
        public Brush BorderBrush { get; set; }
        public Brush Background { get; set; }
        public object Content { get; set; }
        public double FontSize { get; set; }
        public double? Top { get; set; }
        public double? Left { get; set; }
        public RadialMenu.CenterButtonTappedHandler CenterButtonTappedHandler { get; set; }
    }

    public class Helpers
    {
        public static CenterButtonShim ButtonToShim(CenterButton input, RadialMenu.CenterButtonTappedHandler tappedHandler)
        {
            return new CenterButtonShim
            {
                BorderBrush = input.BorderBrush,
                Background = input.Background,
                Content = input.Content,
                FontSize = input.FontSize,
                Top = input.Top,
                Left = input.Left,
                CenterButtonTappedHandler = tappedHandler
            };
        }

        public static CenterButtonShim ButtonToShim(CenterButton input)
        {
            return ButtonToShim(input, null);
        }
    }
}
