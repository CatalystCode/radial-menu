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
            button1.Label = "1";
            button1.Icon = "1";
            button1.OuterNormalColor = Colors.AliceBlue;

            RadialMenuButton button2 = new RadialMenuButton();
            button2.Label = "2";
            button2.Icon = "2";
            //button2.OuterNormalColor = Colors.Aqua;

            RadialMenuButton button3 = new RadialMenuButton();
            button3.Label = "3";
            button3.Icon = "3";
            //button3.OuterNormalColor = Colors.Azure;

            RadialMenuButton button4 = new RadialMenuButton();
            button4.Label = "4";
            button4.Icon = "4";
            button4.OuterNormalColor = Colors.Black;

            RadialMenuButton button5 = new RadialMenuButton();
            button5.Label = "5";
            button5.Icon = "5";
            //button5.OuterNormalColor = Colors.Gold;

            RadialMenuButton button6 = new RadialMenuButton();
            button6.Label = "6";
            button6.Icon = "6";
            //button6.OuterNormalColor = Colors.DarkOrchid;

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
