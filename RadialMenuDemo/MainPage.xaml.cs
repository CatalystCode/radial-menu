using Windows.UI.Xaml.Controls;
using RadialMenuControl.UserControl;
using RadialMenuControl.Components;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;
using System;

namespace RadialMenuDemo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            var button1 = new RadialMenuButton
            {
                Label = "Rainbow Simple",
                Icon = "🌈",
                Type = RadialMenuButton.ButtonType.Simple
            };
            button1.InnerArcPressedEvent += Button1_InnerArcPressedEvent;
            button1.OuterArcReleasedEvent += Button1_OuterArcReleasedEvent;

            RadialMenu button1Submenu = new RadialMenu {CenterButtonIcon = ""};

            var button11 = new RadialMenuButton
            {
                Label = "World",
                Icon = "🌍",
                Type = RadialMenuButton.ButtonType.Radio
            };
            var button12 = new RadialMenuButton
            {
                Label = "Sun",
                Icon = "🌞",
                Type = RadialMenuButton.ButtonType.Radio
            };
            var button13 = new RadialMenuButton
            {
                Label = "Canada!",
                Icon = "🍁",
                Type = RadialMenuButton.ButtonType.Radio
            };
            var button14 = new RadialMenuButton
            {
                Label = "Bento",
                Icon = "🍱",
                Type = RadialMenuButton.ButtonType.Radio
            };
            
            button1Submenu.AddButton(button11);
            button1Submenu.AddButton(button12);
            button1Submenu.AddButton(button13);
            button1Submenu.AddButton(button14);
            button1.Submenu = button1Submenu;

            var button2 = new RadialMenuButton
            {
                Label = "Stop",
                IconImage = new BitmapImage(new Uri("ms-appx:///Assets/button_blue_stop.png")),
                Type = RadialMenuButton.ButtonType.Simple
            };

            var button3 = new RadialMenuButton
            {
                Label = "Ramen Time",
                Icon = "🍜",
                Type = RadialMenuButton.ButtonType.Radio
            };

            var button4 = new RadialMenuButton
            {
                Label = "Surf's up",
                Icon = "🏄",
                Type = RadialMenuButton.ButtonType.Radio
            };

            var button5 = new RadialMenuButton
            {
                Label = "Effin Dragons",
                Icon = "🐉",
                Type = RadialMenuButton.ButtonType.Radio
            };

            var button6 = new RadialMenuButton
            {
                Label = "Pay Rent Toggle",
                Icon = "💸",
                Type = RadialMenuButton.ButtonType.Toggle
            };
            var button7 = new RadialMenuButton
            {
                Label = "Text Font Type",
                Icon = "✎",
                Type = RadialMenuButton.ButtonType.Simple
            };

            ListMenu button7SubMenu = new ListMenu();
            var button71 = new RadialMenuButton
            {
                Label = "Arial",
                Type = RadialMenuButton.ButtonType.List
            };
            var button72 = new RadialMenuButton
            {
                Label = "Calibri",
                Type = RadialMenuButton.ButtonType.List
            };
            var button73 = new RadialMenuButton
            {
                Label = "Cambria",
                Type = RadialMenuButton.ButtonType.List
            };
            var button74 = new RadialMenuButton
            {
                Label = "Courier",
                Type = RadialMenuButton.ButtonType.List
            };
            var button75 = new RadialMenuButton
            {
                Label = "Georgia",
                Type = RadialMenuButton.ButtonType.List
            };
            var button76 = new RadialMenuButton
            {
                Label = "Helvetica",
                Type = RadialMenuButton.ButtonType.List
            };
            var button77 = new RadialMenuButton
            {
                Label = "Tahoma",
                Type = RadialMenuButton.ButtonType.List
            };
            var button78 = new RadialMenuButton
            {
                Label = "Times New Roman",
                Type = RadialMenuButton.ButtonType.List
            };
            var button79 = new RadialMenuButton
            {
                Label = "Verdana",
                Type = RadialMenuButton.ButtonType.List
            };
            
            button7SubMenu.AddButton(button71);
            button7SubMenu.AddButton(button72);
            button7SubMenu.AddButton(button73);
            button7SubMenu.AddButton(button74);
            button7SubMenu.AddButton(button75);
            button7SubMenu.AddButton(button76);
            button7SubMenu.AddButton(button77);
            button7SubMenu.AddButton(button78);
            button7SubMenu.AddButton(button79);
            button7.Submenu = button7SubMenu;

            InitializeComponent();
            MyRadialMenu.AddButton(button1);
            MyRadialMenu.AddButton(button2);
            MyRadialMenu.AddButton(button3);
            MyRadialMenu.AddButton(button7);
            MyRadialMenu.AddButton(button4);
            MyRadialMenu.AddButton(button5);
            MyRadialMenu.AddButton(button6);

            MyRadialMenu.CenterButtonTappedEvent += RadialMenu_CenterButtonTappedEvent;

            LayoutRoot.DataContext = this;
            MyRadialMenu.PropertyChanged += RadialMenu_PropertyChanged;
        }

        private static void RadialMenu_CenterButtonTappedEvent(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Debug.WriteLine("Hi from center button!");
        }

        private static void Button1_OuterArcReleasedEvent(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Hi from button 1!");
        }

        private static void Button1_InnerArcPressedEvent(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Hi from button 1!");
        }

        private static void RadialMenu_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var rotaryWheel = (RadialMenu)sender;
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
