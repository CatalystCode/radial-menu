namespace RadialMenuControl.UserControl
{
    using RadialMenuControl.UserControl;
    using RadialMenuControl.Components;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Automation.Peers;
    using Windows.UI.Xaml.Automation.Provider;

    public sealed partial class ListSubMenu : MenuBase
    {
        public static readonly DependencyProperty ListHeightProperty =
            DependencyProperty.Register("ListHeightProperty", typeof(double), typeof(ListSubMenu), null);

        public double ListHeight
        {
            get { return (double)GetValue(ListHeightProperty); }
            set { SetValue(ListHeightProperty, value); }
        }
        public static readonly DependencyProperty ListMarginProperty =
            DependencyProperty.Register("ListMarginProperty", typeof(Thickness), typeof(ListSubMenu), null);

        public Thickness ListMargin
        {
            get { return (Thickness)GetValue(ListMarginProperty); }
            set { SetValue(ListMarginProperty, value); }
        }
        private IList<RadialMenuButton> _listItems = new List<RadialMenuButton>();
        public IList<RadialMenuButton> ListItems
        {
            get { return _listItems; }
            set
            {
                SetField(ref _listItems, value);
            }
        }

        public void AddButton(RadialMenuButton button)
        {
            _listItems.Add(button);
        }
        
        public ListSubMenu()
        {
            this.InitializeComponent();
            LayoutRoot.DataContext = this;
        }
       
    }
}
