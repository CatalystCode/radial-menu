using Windows.UI.Xaml.Controls;
using RadialMenuControl.UserControl;
using RadialMenuControl.Components;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;
using System;
﻿using System.Collections.Generic;
﻿using Windows.UI;
﻿using Windows.UI.Xaml.Input;
﻿using Windows.UI.Xaml.Media;

namespace RadialMenuDemo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            var button1 = new RadialMenuButton
            {
                Label = "Melbourne",
                Icon = "🌏",
                Type = RadialMenuButton.ButtonType.Simple

            };

            button1.InnerArcPressedEvent += Button1_InnerArcPressedEvent;

            button1.Submenu = new RadialMenu {CenterButtonIcon = ""};

            var button11 = new RadialMenuButton
            {
                Label = "Radio #1",
                Icon = "🌍",
                Type = RadialMenuButton.ButtonType.Radio
            };
            var button12 = new RadialMenuButton
            {
                Label = "Radio #2",
                Icon = "🌞",
                Type = RadialMenuButton.ButtonType.Radio
            };
            var button13 = new RadialMenuButton
            {
                Label = "Radio #3",
                Icon = "🍁",
                Type = RadialMenuButton.ButtonType.Radio
            };
            var button14 = new RadialMenuButton
            {
                Label = "Radio #4",
                Icon = "🍱",
                Type = RadialMenuButton.ButtonType.Radio,
                Submenu = new RadialMenu()
            };

            button1.Submenu.AddButton(button11);
            button1.Submenu.AddButton(button12);
            button1.Submenu.AddButton(button13);
            button1.Submenu.AddButton(button14);

            var button2 = new RadialMenuButton
            {
                Label = "Image",
                IconImage = new BitmapImage(new Uri("ms-appx:///Assets/button_blue_stop.png")),
                Type = RadialMenuButton.ButtonType.Simple
            };

            List<MeterRangeInterval> fontRanges = new List<MeterRangeInterval>();
            fontRanges.Add((new MeterRangeInterval
            {
               StartValue = 5,
               EndValue = 11,
               TickInterval = 1,
               StartDegree = 0,
               EndDegree = 90
            }));
            fontRanges.Add((new MeterRangeInterval
            {
                StartValue = 11,
                EndValue = 12,
                TickInterval = 1,
                StartDegree = 90,
                EndDegree = 110
            }));
            fontRanges.Add((new MeterRangeInterval
            {
                StartValue = 12,
                EndValue = 28,
                TickInterval = 2,
                StartDegree = 110,
                EndDegree = 250
            }));
            fontRanges.Add((new MeterRangeInterval
            {
                StartValue = 28,
                EndValue = 36,
                TickInterval = 8,
                StartDegree = 250,
                EndDegree = 280
            }));
            fontRanges.Add((new MeterRangeInterval
            {
                StartValue = 36,
                EndValue = 48,
                TickInterval = 12,
                StartDegree = 280,
                EndDegree = 300
            }));
            fontRanges.Add((new MeterRangeInterval
            {
                StartValue = 48,
                EndValue = 72,
                TickInterval = 24,
                StartDegree = 300,
                EndDegree = 320
            }));
            var button3 = new RadialMenuButton
            {
                Label = "Meter",
                Icon = "🍜",
                CustomMenu = new MeterSubMenu()
                {
                    MeterEndValue = 72,
                    MeterStartValue = 5,
                    MeterRadius = 70,
                    StartAngle = -90,
                    MeterPointerLength = 70,
                    RoundSelectValue = true,
                    OuterEdgeBrush = new SolidColorBrush(Colors.DarkGreen),
                    Intervals =  fontRanges
                }
            };

            (button3.CustomMenu as MeterSubMenu).ValueSelected += MeterMenu_ValueSelected;
            button3.CustomMenu.CenterButtonIcon = "";

            var button4 = new RadialMenuButton
            {
                Label = "Radio",
                Icon = "🐙",
                Type = RadialMenuButton.ButtonType.Radio
            };

            var button5 = new RadialMenuButton
            {
                Label = "Radio #1",
                Icon = "🐉",
                Type = RadialMenuButton.ButtonType.Radio
            };

            var button6 = new RadialMenuButton
            {
                Label = "Toggle #1",
                Icon = "🎉",
                Type = RadialMenuButton.ButtonType.Toggle
            };

            var button7 = new RadialMenuButton
            {
                Label = "Toggle #2",
                Icon = "💸",
                Type = RadialMenuButton.ButtonType.Toggle
            };

            var button8 = new RadialMenuButton
            {
                Label = "List",
                Icon = "💸"
            };
            button8.CustomMenu = new ListSubMenu();
            (button8.CustomMenu as ListSubMenu).ValueSelected += ListSubMenu_ValueSelected;
            button8.CustomMenu.CenterButtonIcon = "";
            List<RadialMenuButton> listMenuItems = new List<RadialMenuButton>();
            var button81 = new RadialMenuButton
            {
                Label = "Arial",
                Type = RadialMenuButton.ButtonType.Radio,
                Value = "Arial"
            };
            var button82 = new RadialMenuButton
            {
                Label = "Calibri",
                Type = RadialMenuButton.ButtonType.Radio,
                Value = "Calibri"
            };
            var button83 = new RadialMenuButton
            {
                Label = "Courier",
                Type = RadialMenuButton.ButtonType.Radio,
                Value = "Courier"
            };
            var button84 = new RadialMenuButton
            {
                Label = "Times New Roman",
                Type = RadialMenuButton.ButtonType.Radio,
                Value = "Times New Roman"
            };
            listMenuItems.Add(button81);
            listMenuItems.Add(button82);
            listMenuItems.Add(button83);
            listMenuItems.Add(button84);
            (button8.CustomMenu as ListSubMenu).ListMenuItems = listMenuItems;

            InitializeComponent();
            MyRadialMenu.AddButton(button1);
            MyRadialMenu.AddButton(button2);
            MyRadialMenu.AddButton(button3);
            MyRadialMenu.AddButton(button4);
            MyRadialMenu.AddButton(button5);
            MyRadialMenu.AddButton(button6);
            MyRadialMenu.AddButton(button8);

            LayoutRoot.DataContext = this;
            MyRadialMenu.PropertyChanged += RadialMenu_PropertyChanged;
        }

        private void ListSubMenu_ValueSelected(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs args)
        {
            Debug.WriteLine("User selected value: " + (sender as ListSubMenu).SelectedValue);
        }
        private void MeterMenu_ValueSelected(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs args)
        {
            Debug.WriteLine("User selected value: " + (sender as MeterSubMenu).SelectedValue);
        }

        private void Button1_InnerArcPressedEvent(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (Melbourne));
        }

        private static void RadialMenu_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedItemValue":
                    {
                        Debug.WriteLine("Hello!");
                        break;
                    }
            }
        }
    }
}
