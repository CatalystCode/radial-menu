using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RadialMenuControl.Components;
using RadialMenuControl.UserControl;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RadialMenuDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Melbourne : Page
    {
        public Melbourne()
        {
            var buttonColors = new Dictionary<string, Color>()
            {
                { "OuterNormalColor", Color.FromArgb(255, 56, 55, 57) },
                { "OuterHoverColor", Color.FromArgb(255, 70, 102, 102) },
                { "OuterDisabledColor", Color.FromArgb(255, 96, 139, 139) },
                { "InnerNormalColor", Colors.White },
                { "InnerHoverColor", Color.FromArgb(255, 227, 235, 235) },
                { "InnerDisabledColor", Color.FromArgb(255, 227, 235, 235) },
                { "InnerReleasedColor", Color.FromArgb(255, 227, 235, 235) },
            };

            var eraser = new RadialMenuButton
            {
                Label = "Eraser",
                Icon = "",
                IconFontFamily = new FontFamily("Segoe MDL2 Assets"),
                Type = RadialMenuButton.ButtonType.Radio
            };

            var pan = new RadialMenuButton
            {
                Label = "Pan",
                Icon = "",
                IconFontFamily = new FontFamily("Segoe MDL2 Assets"),
                Type = RadialMenuButton.ButtonType.Radio,
                Submenu = new RadialMenu()
            };

            pan.Submenu.AddButton(new RadialMenuButton
            {
                Label = "Line"
            });
            pan.Submenu.AddButton(new RadialMenuButton
            {
                Label = "Select"
            });
            pan.Submenu.AddButton(new RadialMenuButton
            {
                Label = "Text"
            });

            var pen1 = new RadialMenuButton
            {
                Label = "Pen",
                Icon = "",
                IconFontFamily = new FontFamily("Segoe MDL2 Assets"),
                Type = RadialMenuButton.ButtonType.Radio
            };
            var pen2 = new RadialMenuButton
            {
                Label = "Pen",
                Icon = "",
                IconFontFamily = new FontFamily("Segoe MDL2 Assets"),
                Type = RadialMenuButton.ButtonType.Radio
            };
            var add = new RadialMenuButton
            {
                Label = "Add",
                Icon = "",
                IconFontFamily = new FontFamily("Segoe MDL2 Assets")
            };
            var insert = new RadialMenuButton
            {
                Label = "Insert",
                Icon = "",
                IconFontFamily = new FontFamily("Segoe MDL2 Assets")
            };
            var highlight = new RadialMenuButton
            {
                Label = "Highlight",
                Icon = "",
                IconFontFamily = new FontFamily("Segoe MDL2 Assets")
            };
            var undo = new RadialMenuButton
            {
                Label = "Undo",
                Icon = "",
                IconFontFamily = new FontFamily("Segoe MDL2 Assets")
            };

            this.InitializeComponent();

            MyRadialMenu.ButtonDefaultColors = buttonColors;
            MyRadialMenu.AddButton(eraser);
            MyRadialMenu.AddButton(pan);
            MyRadialMenu.AddButton(pen1);
            MyRadialMenu.AddButton(pen2);
            MyRadialMenu.AddButton(highlight);
            MyRadialMenu.AddButton(add);
            MyRadialMenu.AddButton(insert);
            MyRadialMenu.AddButton(undo);
        }
    }
}
