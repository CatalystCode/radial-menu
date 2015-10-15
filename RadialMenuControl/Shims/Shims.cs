using Windows.UI.Xaml.Media;
using RadialMenuControl.Components;
using RadialMenuControl.UserControl;

namespace RadialMenuControl.Shims
{
    /// <summary>
    /// A shim for the Center Button. Retains most things we care about
    /// a center button without using the actual button.
    /// </summary>
    public class CenterButtonShim
    {
        /// <summary>
        /// The border brush for the center button
        /// </summary>
        public Brush BorderBrush { get; set; }
        /// <summary>
        /// The background brush for the center button
        /// </summary>
        public Brush Background { get; set; }
        /// <summary>
        /// The Content for this center button
        /// </summary>
        public object Content { get; set; }
        /// <summary>
        /// The font size for this center button
        /// </summary>
        public double FontSize { get; set; }
        /// <summary>
        /// The Cavas Top value
        /// </summary>
        public double? Top { get; set; }
        /// <summary>
        /// The Canvas Left value
        /// </summary>
        public double? Left { get; set; }
        /// <summary>
        /// The Tapped handler for this center button
        /// </summary>
        public RadialMenu.CenterButtonTappedHandler CenterButtonTappedHandler { get; set; }
    }

    public class Helpers
    {
        /// <summary>
        /// Creates a Shim from a button
        /// </summary>
        /// <param name="input">The actual CenterButton</param>
        /// <param name="tappedHandler">The tapped handler for this button</param>
        /// <returns></returns>
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

        /// <summary>
        /// Given a Center Button returns a CenterButtonShim of that button
        /// </summary>
        /// <param name="input">The Actual CenterButton</param>
        /// <returns></returns>
        public static CenterButtonShim ButtonToShim(CenterButton input)
        {
            return ButtonToShim(input, null);
        }
    }
}
