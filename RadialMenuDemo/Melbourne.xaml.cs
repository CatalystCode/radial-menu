using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography.Core;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using RadialMenuControl.Components;
using RadialMenuControl.Extensions;
using RadialMenuControl.UserControl;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RadialMenuDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Melbourne : Page
    {
        private readonly Dictionary<string, Color> _buttonColors = new Dictionary<string, Color>()
        {
            {"OuterNormalColor", Color.FromArgb(255, 56, 55, 57)},
            {"OuterHoverColor", Color.FromArgb(255, 70, 102, 102)},
            {"OuterDisabledColor", Color.FromArgb(255, 96, 139, 139)},
            {"InnerNormalColor", Colors.White},
            {"InnerHoverColor", Color.FromArgb(255, 227, 235, 235)},
            {"InnerDisabledColor", Color.FromArgb(255, 227, 235, 235)},
            {"InnerReleasedColor", Color.FromArgb(255, 227, 235, 235)},
        };

        private RadialMenuButton CreateColorRadialMenuButton(Color sourceColor)
        {
            return new RadialMenuButton
            {
                InnerNormalColor = sourceColor,
                InnerHoverColor = sourceColor.Lighten(),
                InnerReleasedColor = sourceColor,
                InnerTappedColor = sourceColor,
                OuterNormalColor = _buttonColors["OuterNormalColor"],
                OuterHoverColor = _buttonColors["OuterHoverColor"],
                OuterDisabledColor = _buttonColors["OuterDisabledColor"]
            };
        }

        private RadialMenuButton CreateColorRadialMenuButtonWithSubMenu(Color sourceColor, double subMenuButtonCount)
        {
            var colorButton = CreateColorRadialMenuButton(sourceColor);
            colorButton.Submenu = new RadialMenu();

            for (var i = 0; i < subMenuButtonCount; i++)
            {
                var lightenFactor = (float) i/10;
                colorButton.Submenu.AddButton(CreateColorRadialMenuButton(sourceColor.Lighten(lightenFactor)));
            }

            return colorButton;
        }

        public Melbourne()
        {
            this.InitializeComponent();

            Pen1Submenu.AddButton(CreateColorRadialMenuButtonWithSubMenu(Colors.Black, 10));
            Pen1Submenu.AddButton(CreateColorRadialMenuButtonWithSubMenu(Colors.Red, 10));
            Pen1Submenu.AddButton(CreateColorRadialMenuButtonWithSubMenu(Colors.Blue, 10));
            Pen1Submenu.AddButton(CreateColorRadialMenuButtonWithSubMenu(Colors.Green, 10));
            Pen1Submenu.AddButton(CreateColorRadialMenuButtonWithSubMenu(Colors.Yellow, 10));

            Pen2Submenu.AddButton(CreateColorRadialMenuButtonWithSubMenu(Colors.Black, 10));
            Pen2Submenu.AddButton(CreateColorRadialMenuButtonWithSubMenu(Colors.Red, 10));
            Pen2Submenu.AddButton(CreateColorRadialMenuButtonWithSubMenu(Colors.Blue, 10));
            Pen2Submenu.AddButton(CreateColorRadialMenuButtonWithSubMenu(Colors.Green, 10));
            Pen2Submenu.AddButton(CreateColorRadialMenuButtonWithSubMenu(Colors.Yellow, 10));
                  
            MyRadialMenu.ButtonDefaultColors = _buttonColors;
        }

        private void HighlightRadialMenu_OnCenterButtonTappedEvent(object sender, TappedRoutedEventArgs e)
        {
            var sendingMenu = sender as RadialMenu;
            if (sendingMenu != null && sendingMenu.Pie.SelectedItem != null)
            {
                Highlight.Label = sendingMenu.Pie.SelectedItem.Label;
                Highlight.IconImage = sendingMenu.Pie.SelectedItem.IconImage;
            }
        }
    }
}
