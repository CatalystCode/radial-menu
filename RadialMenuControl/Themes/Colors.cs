﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace RadialMenuControl.Themes
{
    /// <summary>
    /// Common colors to use by default across control components
    /// </summary>
    public static class DefaultColors
    {
        public static Color InnerNormalColor = Color.FromArgb(255, 255, 255, 255),
                            InnerHoverColor = Color.FromArgb(255, 245, 236, 243),
                            InnerTappedColor = Color.FromArgb(255, 237, 234, 236),
                            OuterNormalColor = Color.FromArgb(255, 128, 57, 123),
                            OuterDisabledColor = Color.FromArgb(255, 237, 211, 236),
                            OuterHoverColor = Color.FromArgb(255, 155, 79, 150),
                            OuterTappedColor = Color.FromArgb(255, 104, 41, 100),
                            BackgroundHighlightColor = Color.FromArgb(255, 235, 235, 235),
                            ForegroundColor = Color.FromArgb(255, 241, 218, 234),
                            HighlightColor = Color.FromArgb(255, 128, 57, 123),
                            MeterSelectorColor = Colors.Green;

    }
}
