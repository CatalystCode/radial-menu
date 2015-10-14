using System.Collections.Generic;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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

        private List<MeterRangeInterval> opacityMeterIntervals = new List<MeterRangeInterval>()
        {
            new MeterRangeInterval
            {
                StartValue = 0,
                EndValue = 50,
                TickInterval = 5,
                StartDegree = 0,
                EndDegree = 180
            },
            new MeterRangeInterval
            {
                StartValue = 50,
                EndValue = 100,
                TickInterval = 5,
                StartDegree = 180,
                EndDegree = 355
            },
        };

        private List<MeterRangeInterval> scaledMeterIntervals = new List<MeterRangeInterval>()
        {
            new MeterRangeInterval
            {
               StartValue = 5,
               EndValue = 11,
               TickInterval = 1,
               StartDegree = 0,
               EndDegree = 90
            },
            new MeterRangeInterval
            {
                StartValue = 11,
                EndValue = 12,
                TickInterval = 1,
                StartDegree = 90,
                EndDegree = 110
            },
            new MeterRangeInterval
            {
                StartValue = 12,
                EndValue = 28,
                TickInterval = 2,
                StartDegree = 110,
                EndDegree = 250
            },
            new MeterRangeInterval
            {
                StartValue = 28,
                EndValue = 36,
                TickInterval = 8,
                StartDegree = 250,
                EndDegree = 280
            },
            new MeterRangeInterval
            {
                StartValue = 36,
                EndValue = 48,
                TickInterval = 12,
                StartDegree = 280,
                EndDegree = 300
            },
            new MeterRangeInterval
            {
                StartValue = 48,
                EndValue = 72,
                TickInterval = 24,
                StartDegree = 300,
                EndDegree = 320
            }
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
                  
            Pen1StrokeMenu.Intervals = scaledMeterIntervals;
            Pen1OpacityMenu.Intervals = opacityMeterIntervals;
            Pen2StrokeMenu.Intervals = scaledMeterIntervals;
            Pen2OpacityMenu.Intervals = opacityMeterIntervals;

            CoreWindow.GetForCurrentThread().KeyDown += Melbourne_KeyDown;
            CoreWindow.GetForCurrentThread().KeyUp += Melbourne_KeyUp; ;
        }

        private void Melbourne_KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Shift:
                    MyRadialMenu.HideAccessKeyTooltips();
                    break;
                case VirtualKey.P:
                    MyRadialMenu.ClickInnerRadialMenuButton(Pan);
                    break;
                case VirtualKey.O:
                    MyRadialMenu.ClickOuterRadialMenuButton(Pan);
                    break;
                case VirtualKey.K:
                    MyRadialMenu.ClickInnerRadialMenuButton(Pen1);
                    break;
                case VirtualKey.L:
                    MyRadialMenu.ClickOuterRadialMenuButton(Pen1);
                    break;
            };
        }

        
        private void Melbourne_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Shift) MyRadialMenu.ShowAccessKeyTooltips();
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
