﻿namespace RadialMenuControl.Extensions
{
    using Windows.UI;

    public static class ColorExtensions
    {
        private const float DefaultCorrectionFactor = 0.1f;

        public static Color Lighten(this Color source, float correctionFactor = DefaultCorrectionFactor)
        {
            var red = (255 - source.R) * correctionFactor + source.R;
            var green = (255 - source.G) * correctionFactor + source.G;
            var blue = (255 - source.B) * correctionFactor + source.B;
            return Color.FromArgb(source.A, (byte)red, (byte)green, (byte)blue);
        }
    }
}
