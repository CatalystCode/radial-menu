using Windows.UI.Xaml.Controls;
using RadialMenuControl.UserControl;
using RadialMenuControl.Components;
using System.Diagnostics;
using Windows.UI;

namespace RadialMenuDemo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            RadialMenuButton button1 = new RadialMenuButton();
            button1.Label = "Rainbow Simple";
            button1.Icon = "🌈";
            button1.Type = RadialMenuButton.ButtonType.SIMPLE;
            button1.InnerArcPressedEvent += Button1_InnerArcPressedEvent;
            button1.OuterArcReleasedEvent += Button1_OuterArcReleasedEvent;

            button1.Submenu = new RadialMenu();
            button1.Submenu.CenterButtonIcon = "";
            RadialMenuButton button11 = new RadialMenuButton { Label = "World", Icon = "🌍" };
            button11.Type = RadialMenuButton.ButtonType.RADIO;
            RadialMenuButton button12 = new RadialMenuButton { Label = "Sun", Icon = "🌞" };
            button12.Type = RadialMenuButton.ButtonType.RADIO;
            RadialMenuButton button13 = new RadialMenuButton { Label = "Canada!", Icon = "🍁" };
            button13.Type = RadialMenuButton.ButtonType.RADIO;
            RadialMenuButton button14 = new RadialMenuButton { Label = "Bento", Icon = "🍱" };
            button14.Type = RadialMenuButton.ButtonType.RADIO;
            button1.Submenu.AddButton(button11);
            button1.Submenu.AddButton(button12);
            button1.Submenu.AddButton(button13);;
            button1.Submenu.AddButton(button14);


            RadialMenuButton button2 = new RadialMenuButton();
            button2.Label = "Party Simple";
            button2.Icon = "🎉";
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

            this.InitializeComponent();
            radialMenu.AddButton(button1);
            radialMenu.AddButton(button2);
            radialMenu.AddButton(button3);
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
