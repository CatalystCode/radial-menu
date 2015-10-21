using RadialMenuControl.UserControl;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace HelloRadialMenu
{
    public sealed partial class MainPage : Page
    {
        private List<MeterRangeInterval> PizzaIntervals = new List<MeterRangeInterval>()
        {
            new MeterRangeInterval
            {
               StartValue = 5,
               EndValue = 20,
               TickInterval = 1,
               StartDegree = 0,
               EndDegree = 220
            },
            new MeterRangeInterval
            {
                StartValue = 20,
                EndValue = 34,
                TickInterval = 2,
                StartDegree = 220,
                EndDegree = 330
            }
        };

        private void SetupPizzaMeter()
        {

            var pizzaGauge = new MeterSubMenu()
            {
                MeterEndValue = 34,
                MeterStartValue = 5,
                MeterRadius = 70,
                StartAngle = -90,
                MeterPointerLength = 70,
                RoundSelectValue = true,
                Intervals = PizzaIntervals
            };

            pizzaGauge.ValueSelected += (s, e) =>
            {
                var selectedValue = (s as MeterSubMenu).SelectedValue;
                Debug.WriteLine("Value changed to " + selectedValue);
            };

            PizzaButton.CustomMenu = pizzaGauge;
        }

        public MainPage()
        {
            this.InitializeComponent();

            SetupPizzaMeter();
        }

        private void RadialMenuButton_InnerArcReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Get cooking!");
        }

        private void RadialMenu_CenterButtonTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var sendingMenu = sender as RadialMenu;
            if (sendingMenu != null && sendingMenu.Pie.SelectedItem != null)
            {
                FoodButton.Label = sendingMenu.Pie.SelectedItem.Label;
                FoodButton.Icon = sendingMenu.Pie.SelectedItem.Icon;
            }
        }
    }
}
