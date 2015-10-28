using RadialMenuControl.Components;

namespace RadialMenuControl.UserControl
{
    using System.Collections.Generic;
    using Themes;
    using Windows.UI;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;

    public partial class ListSubMenu : MenuBase
    {

        #region properties
        private object _selectedValue;
        
        /// <summary>
        /// The currently selected element on this list
        /// </summary>
        public object SelectedValue
        {
            set
            {
                SetField(ref _selectedValue, value);
            }
            get
            {
                return _selectedValue;
            }
        }
        private Brush _backgroundFillBrush = new SolidColorBrush(Colors.WhiteSmoke);
        /// <summary>
        /// The selected value for the 
        /// </summary>
        public Brush BackgroundFillBrush
        {
            set
            {
                SetField(ref _backgroundFillBrush, value);
            }
            get
            {
                return _backgroundFillBrush;
            }
        }

        private Brush _selectedValueBrush = new SolidColorBrush(DefaultColors.HighlightColor);
        /// <summary>
        /// The brush to use for the selected element on the list
        /// </summary>
        public Brush SelectedValueBrush
        {
            set
            {
                SetField(ref _selectedValueBrush, value);
            }
            get
            {
                return _selectedValueBrush;
            }
        }

        private List<RadialMenuButton> _listMenuItems = new List<RadialMenuButton>();
        public List<RadialMenuButton> ListMenuItems
        {
            set
            {
                SetField(ref _listMenuItems, value);
            }
            get
            {
                return _listMenuItems;
            }
        }

        private Brush _hoverValueBrush = new SolidColorBrush(DefaultColors.MeterSelectorColor);

        /// <summary>
        /// The brush to use when the pointer hovers over an element
        /// </summary>
        public Brush HoverValueBrush
        {
            set
            {
                SetField(ref _hoverValueBrush, value);
            }
            get
            {
                return _hoverValueBrush;
            }
        }
        
        /// <summary>
        /// The CenterButton of this control for back navigation
        /// </summary>
        public override CenterButton CenterButton
        {
            get { return SubMenuCenterButton; }
            set { SetField(ref SubMenuCenterButton, value); }
        }

        #endregion

        /// <summary>
        /// Draws the list menu
        /// </summary>
        public void Draw()
        {

            SubMenuListView.ItemsSource = null;

            List<string> items = new List<string>();
            int selectedIndex = 0;
            int count = 0;
            foreach(RadialMenuButton item in ListMenuItems)
            {
                items.Add(item.Label);
                if (item.MenuSelected)
                {
                    selectedIndex = count;
                }
                count++;
            }

            SubMenuListView.ItemsSource = items;
            SubMenuListView.SelectedIndex = selectedIndex;
            //SubMenuListView.SelectionChanged += SubMenuListView_SelectionChanged;
        }

        /// <summary>
        /// Event Handler for selection changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubMenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = (sender as ListView).SelectedValue as string;
            foreach (RadialMenuButton item in ListMenuItems)
            {
                if (item.Label == selectedItem)
                {
                    item.MenuSelected = true;
                    SelectedValue = item.Value;
                    break;
                }
            }
           
        }

        public delegate void ValueSelectedHandler(object sender, TappedRoutedEventArgs args);
        public event ValueSelectedHandler ValueSelected;
        /// <summary>
        /// Creates a ListSubMenu instance
        /// </summary>
        public ListSubMenu()
        {

            InitializeComponent();
            DataContext = this;
            BackgroundFillBrush = new SolidColorBrush(DefaultColors.InnerNormalColor);

            Tapped += (sender, args) =>
            {
                string selectedItem = SubMenuListView.SelectedValue as string;
                foreach (RadialMenuButton item in ListMenuItems)
                {
                    if (item.Label == selectedItem)
                    {
                        item.MenuSelected = true;
                        SelectedValue = item.Value;
                    }
                    else
                    {
                        item.MenuSelected = false;
                    }
                }
                ValueSelected?.Invoke(this, args);
            };

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Diameter")
                {
                   
                    CenterButton.Top = Diameter / 2 - CenterButton.Width / 2;
                    CenterButton.Left = Diameter / 2 - CenterButton.Width / 2;
                }


            };
            Loaded += (sender, e) =>
            {
                
                Draw();
               
            };
            
        }
    }
}
