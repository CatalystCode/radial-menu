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

    public sealed partial class ListMenu : UserControl
    {

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

        public event PropertyChangedEventHandler PropertyChanged;

        public ListMenu()
        {
            this.InitializeComponent();
            layoutRoot.DataContext = this;
        }
       
        private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!value.Equals(field))
            {
                field = value;
                var eventHandler = PropertyChanged;
                eventHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
