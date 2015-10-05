using Windows.UI.Xaml.Controls;
using RadialMenuControl.UserControl;
using RadialMenuControl.Components;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System;

namespace RadialMenuDemo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            RadialMenuButton button1 = new RadialMenuButton();
            button1.Label = "Rainbow Simple";
            button1.Icon = "🌈";
            button1.Type = RadialMenuButton.ButtonType.SIMPLE;
            button1.InnerArcPressedEvent += Button1_InnerArcPressedEvent;
            button1.OuterArcReleasedEvent += Button1_OuterArcReleasedEvent;
            
            RadialMenu button1Submenu = new RadialMenu();
            button1Submenu.CenterButtonIcon = "";
            RadialMenuButton button11 = new RadialMenuButton { Label = "World", Icon = "🌍" };
            button11.Type = RadialMenuButton.ButtonType.RADIO;
            RadialMenuButton button12 = new RadialMenuButton { Label = "Sun", Icon = "🌞" };
            button12.Type = RadialMenuButton.ButtonType.RADIO;
            RadialMenuButton button13 = new RadialMenuButton { Label = "Canada!", Icon = "🍁" };
            button13.Type = RadialMenuButton.ButtonType.RADIO;
            RadialMenuButton button14 = new RadialMenuButton { Label = "Bento", Icon = "🍱" };
            button14.Type = RadialMenuButton.ButtonType.RADIO;
            button1Submenu.AddButton(button11);
            button1Submenu.AddButton(button12);
            button1Submenu.AddButton(button13);;
            button1Submenu.AddButton(button14);
            button1.Submenu = button1Submenu;



            RadialMenuButton button2 = new RadialMenuButton();
            button2.Label = "Stop";
            button2.IconImage = new BitmapImage(new Uri("ms-appx:///Assets/button_blue_stop.png"));
            button2.Type = RadialMenuButton.ButtonType.SIMPLE;

            RadialMenuButton button3 = new RadialMenuButton();
            button3.Label = "Ramen Time";
            button3.Icon = "🍜";
            button3.Type = RadialMenuButton.ButtonType.RADIO;

            RadialMenuButton button4 = new RadialMenuButton();
            button4.Label = "Surf's up";
            button4.Icon = "🏄";
            button4.Type = RadialMenuButton.ButtonType.RADIO;

            RadialMenuButton button5 = new RadialMenuButton();
            button5.Label = "Effin Dragons";
            button5.Icon = "🐉";
            button5.Type = RadialMenuButton.ButtonType.RADIO;

            RadialMenuButton button6 = new RadialMenuButton();
            button6.Label = "Pay Rent Toggle";
            button6.Icon = "💸";
            button6.Type = RadialMenuButton.ButtonType.TOGGLE;

            RadialMenuButton button7 = new RadialMenuButton();
            button7.Label = "Text Font Type";
            button7.Icon = "✎";
            button7.Type = RadialMenuButton.ButtonType.SIMPLE;

            ListMenu button7SubMenu = new ListMenu();
            RadialMenuButton button71 = new RadialMenuButton { Label = "Arial" };
            button71.Type = RadialMenuButton.ButtonType.LIST;
            RadialMenuButton button72 = new RadialMenuButton { Label = "Calibri" };
            button72.Type = RadialMenuButton.ButtonType.LIST;
            RadialMenuButton button73 = new RadialMenuButton { Label = "Cambria" };
            button73.Type = RadialMenuButton.ButtonType.LIST;
            RadialMenuButton button74 = new RadialMenuButton { Label = "Courier" };
            button74.Type = RadialMenuButton.ButtonType.LIST;
            RadialMenuButton button75 = new RadialMenuButton { Label = "Georgia" };
            button75.Type = RadialMenuButton.ButtonType.LIST;
            RadialMenuButton button76 = new RadialMenuButton { Label = "Helvetica" };
            button76.Type = RadialMenuButton.ButtonType.LIST;
            RadialMenuButton button77 = new RadialMenuButton { Label = "Tahoma" };
            button77.Type = RadialMenuButton.ButtonType.LIST;
            RadialMenuButton button78 = new RadialMenuButton { Label = "Times New Roman" };
            button78.Type = RadialMenuButton.ButtonType.LIST;
            RadialMenuButton button79 = new RadialMenuButton { Label = "Verdana" };
            button79.Type = RadialMenuButton.ButtonType.LIST;
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

            radialMenu.AddButton(button1);
            radialMenu.AddButton(button2);
            radialMenu.AddButton(button3);
            radialMenu.AddButton(button7);
            radialMenu.AddButton(button4);
            radialMenu.AddButton(button5);
            radialMenu.AddButton(button6);

            radialMenu.CenterButtonTappedEvent += RadialMenu_CenterButtonTappedEvent;

            layoutRoot.DataContext = this;
            radialMenu.PropertyChanged += RadialMenu_PropertyChanged;
        }

        private void RadialMenu_CenterButtonTappedEvent(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Debug.WriteLine("Hi from center button!");
        }

        private void Button1_OuterArcReleasedEvent(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Hi from button 1!");
        }

        private void Button1_InnerArcPressedEvent(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Hi from button 1!");
        }

        private void RadialMenu_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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

        private void radialMenu_CenterButtonPressedEvent(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }
    }
}
