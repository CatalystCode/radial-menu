using Windows.UI.Xaml.Controls;
using RadialMenuControl.UserControl;
using RadialMenuControl.Components;
using System.Diagnostics;

namespace RadialMenuDemo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            RadialMenuButton button1 = new RadialMenuButton();
            button1.Label = "1";

            RadialMenuButton button2 = new RadialMenuButton();
            button2.Label = "2";

            RadialMenuButton button3 = new RadialMenuButton();
            button3.Label = "3";

            RadialMenuButton button4 = new RadialMenuButton();
            button4.Label = "4";

            RadialMenuButton button5 = new RadialMenuButton();
            button5.Label = "5";

            RadialMenuButton button6 = new RadialMenuButton();
            button6.Label = "6";

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
