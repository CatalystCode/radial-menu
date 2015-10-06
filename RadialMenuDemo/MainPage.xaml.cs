﻿using Windows.UI.Xaml.Controls;
using RadialMenuControl.UserControl;
using RadialMenuControl.Components;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;
using System;
﻿using Windows.UI.Xaml.Input;

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
                Type = RadialMenuButton.ButtonType.Radio
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


            var button3 = new RadialMenuButton
            {
                Label = "Meter",
                Icon = "🍜",
                CustomMenu = new MeterSubMenu()
                {
                    MeterEndValue = 100,
                    MeterStartValue = 0,
                    TickInterval = 5,
                    MeterRadius = 50,
                    StartAngle = -90,
                    MeterPointerLength = 50,
                    RoundSelectValue = true
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

            InitializeComponent();
            MyRadialMenu.AddButton(button1);
            MyRadialMenu.AddButton(button2);
            MyRadialMenu.AddButton(button3);
            MyRadialMenu.AddButton(button4);
            MyRadialMenu.AddButton(button5);
            MyRadialMenu.AddButton(button6);

            MyRadialMenu.CenterButtonTappedEvent += RadialMenu_CenterButtonTappedEvent;

            LayoutRoot.DataContext = this;
            MyRadialMenu.PropertyChanged += RadialMenu_PropertyChanged;
        }
        
        private void MeterMenu_ValueSelected(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs args)
        {
            Debug.WriteLine("User selected value: " + (sender as MeterSubMenu).SelectedValue);
        }
        
        private static void RadialMenu_CenterButtonTappedEvent(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Debug.WriteLine("Hi from center button!");
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
