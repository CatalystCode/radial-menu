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
            button1.Label = "Rainbow";
            button1.Icon = "🌈";

            RadialMenuButton button2 = new RadialMenuButton();
            button2.Label = "Party";
            button2.Icon = "🎉";

            RadialMenuButton button3 = new RadialMenuButton();
            button3.Label = "Ramen Time";
            button3.Icon = "🍜";

            RadialMenuButton button4 = new RadialMenuButton();
            button4.Label = "Surf's up";
            button4.Icon = "🏄";

            RadialMenuButton button5 = new RadialMenuButton();
            button5.Label = "Effin Dragons";
            button5.Icon = "🐉";

            RadialMenuButton button6 = new RadialMenuButton();
            button6.Label = "Pay Rent";
            button6.Icon = "💸";

            this.InitializeComponent();
            radialMenu.AddButton(button1);
            radialMenu.AddButton(button2);
            radialMenu.AddButton(button3);
            radialMenu.AddButton(button4);
            radialMenu.AddButton(button5);
            radialMenu.AddButton(button6);

            layoutRoot.DataContext = this;
            radialMenu.PropertyChanged += RadialMenu_PropertyChanged; ;
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
    }
}
