using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace RadialMenuControl.Shims
{
    public class CenterButtonShim
    {
        public Brush BorderBrush;
        public Brush Background;
        public object Content;
        public double FontSize;
    }

    public class Helpers
    {
        public static CenterButtonShim ButtonToShim(Button input)
        {
            return new CenterButtonShim
            {
                BorderBrush = input.BorderBrush,
                Background = input.Background,
                Content = input.Content,
                FontSize = input.FontSize
            };
        }
    }
}
