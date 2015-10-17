## Radial Menu
A Radial Menu for Windows UWP Applications, as made popular by the first versions of the modern OneNote App for Windows. Create radial menus floating op top of your application. The control supports variable numbers of buttons, toggle & radio buttons, a selector for long lists, and a fancy metered menu for intuitive selection of numbers.

!['Fancy GIF showing the control'](http://im.ezgif.com/tmp/ezgif-2737883079.gif)

## Usage
At the core, this control comes with three user controls: The first one is a "floating" control, enabling a child control to float on top of all other elements, which allows the user to move the control around on the screen. The second one is the RadialMenuControl itself, which is able to house a number of RadialMenuButtons. Should a button contain a submenu, the button then houses a RadialMenuControl - and so on. You can have a virtually unlimited number of submenus.

### Adding the Control
You can instantiate the control either using XAML or in code-behind. 

```c#
<Page xmlns:rm="using:RadialMenuControl.UserControl" xmlns:rmb="using:RadialMenuControl.Components">
    <rm:RadialMenu x:Name="MyRadialMenu" Diameter="300" StartAngle="-22.5" OuterArcThickness="20" CenterButtonBorder="Black" CenterButtonIcon="&#x1f369;">
    // ...
    </RadialMenu>
</Page>
```

### Adding Buttons
Now that you have a control like the one above, buttons are added by adding instances of `RadialMenuButton` to the `Buttons` property of your radial menu.

```c#
<Page xmlns:rm="using:RadialMenuControl.UserControl" xmlns:rmb="using:RadialMenuControl.Components">
    <rm:RadialMenu x:Name="MyRadialMenu" Diameter="300" StartAngle="-22.5" OuterArcThickness="20" CenterButtonBorder="Black" CenterButtonIcon="&#x1f369;">
        <RadialMenu.Buttons>
            <rmb:RadialMenuButton x:Name="Eraser" Label="Eraser" Icon="" IconFontFamily="Segoe MDL2 Assets" Type="Radio" />
            <rmb:RadialMenuButton x:Name="Pen" Label="Pen" Icon="" IconFontFamily="Segoe MDL2 Assets" Type="Radio" />
            <rmb:RadialMenuButton x:Name="Party" Label="Party" Icon="" IconFontFamily="Segoe MDL2 Assets" Type="Simple" />
        </RadialMenu.Buttons>
    </RadialMenu>
</Page>
```

#### Button Types
Now that you have created a control, let's get more familiar with all the different types of buttons you get out of the box.

##### Simple
A `Simple` type button is one that triggers an event, but it does not contain any value. To add a `Simple` type button, set `Type` to `"Simple"`.

```c#
<rmb:RadialMenuButton x:Name="Party" Label="Party" Icon="" IconFontFamily="Segoe MDL2 Assets" Type="Simple" />
```

##### Toggle
A `Toggle` type button contains a boolean value of either true or false. To add a `Toggle` type button, set `Type` to `"Toggle"`.

```c#
<rmb:RadialMenuButton x:Name="Bold" Label="Bold" Icon="" IconFontFamily="Segoe MDL2 Assets" Type="Toggle" />
```
When the button is clicked, its value is set to the opposite of the current value. By default the visual state of the button is bind to the current boolean value of the button. For example, if a toggle button is selected and its value is `true`, then the pie slice is set to the `InnerReleased` visual state to indicate it has been selected. If the button's value is `false`, then the pie slice is set to the `InnerNormal` visual state to indicate it is currently not selected. To modify this behavior, go to `PieSlice.xaml.cs`, find the `innerPieSlicePath_PointerReleased` event handler, then modify the following.
```c#
case RadialMenuButton.ButtonType.Toggle:
    VisualStateManager.GoToState(this,
        (OriginalRadialMenuButton.Value != null && ((bool) OriginalRadialMenuButton.Value))
            ? "InnerReleased"
            : "InnerNormal", true);
    // ...
```

##### Radio
A `Radio` type button can contain any value of any type that the application provides. To add a `Radio` type button, set `Type` to `"Radio"` and provide a value for the button.

```c#
<rmb:RadialMenuButton x:Name="RedColor" Label="Red" Icon="" IconFontFamily="Segoe MDL2 Assets" Type="Radio" Value="#..."/>
<rmb:RadialMenuButton x:Name="BlueColor" Label="Blue" Icon="" IconFontFamily="Segoe MDL2 Assets" Type="Radio" Value="#..."/>
<rmb:RadialMenuButton x:Name="YellowColor" Label="Yellow" Icon="" IconFontFamily="Segoe MDL2 Assets" Type="Radio" Value="#..."/>
```
In a given pie, all `Radio` buttons added to the pie belong to a `Radio` button set such that when one `Radio` button is selected, all other `Radio` buttons in the pie become deselected. In the example above, if the `Red` button is clicked, then the `RedColor` `RadialMenuButton`'s `MenuSelected` field is set to `true` and all other `Radio` buttons in the pie are deselected such that their `MenuSelected` fields are set to `false`. In turn, the `SelectedItem` field for the Pie is now set to the current selected `Radio` button. To change this behavior, in `Pie.xaml.cs`, in the `PieSlice_ChangeSelectedEvent` event handler, modify the following. 

```c#
if (slice != null && slice.OriginalRadialMenuButton.Type == RadialMenuButton.ButtonType.Radio)
{
    SelectedItem = slice.OriginalRadialMenuButton;
}

foreach (var ps in PieSlices.Where(ps => ps.OriginalRadialMenuButton.Type == RadialMenuButton.ButtonType.Radio && ps.OriginalRadialMenuButton.MenuSelected && ps.StartAngle != slice.StartAngle))
{
    ps.OriginalRadialMenuButton.MenuSelected = false;
    ps.UpdateSliceForRadio();
}
```

##### Custom
A `Custom` type button allows users to input custom values. To add a `Custom` type button, set `Type` to `"Custom"` and provide a default value for the input control.

```c#
<rmb:RadialMenuButton x:Name="Pen1CustomStrokeButton" IconImage="Icons/stroke.png" Label="Custom Stroke" Type="Custom" Value="5" />
```
When the pie slice is clicked, by default all the text in the TextBox is selected and the TextBox is focused to allow user to enter a value. To modify this behavior, in `PieSlice.xaml.cs`, find the `innerPieSlicePath_PointerPressed` event handler. 

```c#
private void innerPieSlicePath_PointerPressed(object sender, PointerRoutedEventArgs e)
{
    if (OriginalRadialMenuButton.Type == RadialMenuButton.ButtonType.Custom)
    {
        CustomTextBox.Focus(FocusState.Keyboard);
        CustomTextBox.SelectAll();
        e.Handled = true;
    }

    // ...
}
```
To get the custom value the user entered, you can catch the event in your application. Take the example from above, create an event handler for the `ValueChanged` event. 
```c#
Pen1CustomStrokeButton.ValueChanged += Pen1CustomStrokeButton_ValueChanged;

// ...

private void Pen1CustomStrokeButton_ValueChanged(object sender, RoutedEventArgs args)
{
    Debug.WriteLine("User updated value to: " + (sender as RadialMenuButton)?.Value);
}  
```

### Adding Radial Submenus
To add a `RadialMenu` submenu to a `RadialMenuButton`, you can add a `RadialMenuButton.Submenu` element to your button top level button. Then add a new `RadialMenu` with its own `CenterButton` properties, `StartAngle`, and its own `RadialMenuButton` elements to the submenu element. 

```c#
<rmb:RadialMenuButton x:Name="Pan" Label="Pan" Icon="" IconFontFamily="Segoe MDL2 Assets" Type="Radio" InnerAccessKey="P" OuterAccessKey="O">
    <rmb:RadialMenuButton.Submenu>
        <rm:RadialMenu x:Name="PanSubMenu" Diameter="300" StartAngle="-90" OuterArcThickness="20" CenterButtonBorder="Black" CenterButtonIcon="&#x1f369;"
            <rm:RadialMenu.Buttons>
                <rmb:RadialMenuButton Label="Line" Type="Radio" IconImage="Icons/Polygon-50.png" />
                <rmb:RadialMenuButton Label="Select" Type="Radio" IconImage="Icons/Cursor-50.png" />
                <rmb:RadialMenuButton Label="Text" Type="Radio" IconImage="Icons/Text Cursor-50.png" />
            </rm:RadialMenu.Buttons>
        </rm:RadialMenu>
    </rmb:RadialMenuButton.Submenu>
</rmb:RadialMenuButton>
```
### Adding Custom Submenus
In addition to adding a `RadialMenu` submenu, you can also add a custom submenu that is not a `RadialMenu` and is derived from the `MenuBase` class. For example, we have created `MeterSubMenu` and `ListSubMenu` based on the `MenuBase` class. In the example below, to add a `MeterSubMenu` to a `RadialMenuButton`, add the `RadialMenuButton.CustomMenu` element first, then add a `MeterSubMenu` with its properties. 

```c#
<rmb:RadialMenuButton x:Name="Pen1StrokeButton" IconImage="Icons/stroke.png" Label="Stroke">
    <rmb:RadialMenuButton.CustomMenu>
        <rm:MeterSubMenu x:Name="Pen1StrokeMenu" 
                          CenterButtonBorder="Black"
                          MeterEndValue="72" 
                          MeterStartValue="5" 
                          MeterRadius="80" 
                          StartAngle="-90" 
                          MeterPointerLength="80" 
                          RoundSelectValue="True" 
                          OuterEdgeBrush="#383739"
                          />
    </rmb:RadialMenuButton.CustomMenu>
</rmb:RadialMenuButton>
```

## Scenarios & Use cases
Below are write-ups of how to achieve various scenarios - the control is pretty powerful and flexible, so cramming all variations and options into one demo wouldn't have been less useful.

### Seamless Background
If you give RadialMenuButtons the same background color, you may have noticed that Windows will render a 1px thick seam between the individual buttons. Those seams show up when shapes fight for the same pixel.

To get around this issue, you can set the option `HasBackgroundEllipse` on the `RadialMenu` to `true`. In that mode, the menu will render a full ellipse behind your whole control. Since the ellipse is one shape, no seams will be visible. Obviously, for the ellipse to function as background, you will need to set the `InnerNormalColor` of your menu and your buttons to a near invisible color (we recommend `#02FFFFFF`).

```c#
<userControl:RadialMenu
    x:Name="MyRadialMenu"
    Diameter="300"
    StartAngle="-22.5"
    HasBackgroundEllipse="True"
    BackgroundEllipseFill="White"
    OuterArcThickness="20"
    CenterButtonBorder="Black"
    CenterButtonForeground="Black"
    CenterButtonIcon="&#x1f369;"
    InnerHoverColor="#E3EBEB"
    InnerNormalColor="#02FFFFFF" />
```

### Access Keys
The control comes with a number of helper properties to enable access key behaviour. To ensure that the control doesn't mess with your application, it does not access any global properties, meaning that you will have capture `KeyDown` events yourself. The good news: Helper methods on the control make enabling access keys really easy.

To implement access keys, start with the RadialMenuButtons: Give your buttons two properties each, `InnerAccessKey` and `OuterAccessKey` (for instance `M` and `P`). To show little tooltips with the access key, call `ShowAccessKeyTooltips()` on the RadialMenu. To hide them, call `HideAccessKeyTooltips()`. In order to programmatically click either the inner or the outer portion of a RadialMenuButton, call `ClickInnerRadialMenuButton(RadialMenuButton)` or `ClickOuterRadialMenuButton(RadialMenuButton)`, To wire this all up in your application, consult the following example code:

Add a RadialMenuButton to your RadialMenu, passing in access keys:

```c#
<RadialMenuButton x:Name="Pen1" Label="Pen" Icon="" IconFontFamily="Segoe MDL2 Assets" Type="Toggle" InnerAccessKey="K" OuterAccessKey="L" />
```

Then, wire up a KeyDown handler for your application. Depending on the key, we either display the little tooltips - or programmatically perform actions on buttons.
```c#
private void MyApp_KeyUp(CoreWindow sender, KeyEventArgs args)
{
    switch (args.VirtualKey)
    {
        case VirtualKey.Shift:
            MyRadialMenu.HideAccessKeyTooltips();
            break;
        case VirtualKey.P:
            MyRadialMenu.ClickInnerRadialMenuButton(Pan);
            break;
        case VirtualKey.O:
            MyRadialMenu.ClickOuterRadialMenuButton(Pan);
            break;
    };
}
```

As soon as the user lifts the `Shift` key again, we hide the tooltips:
```c#
private void Melbourne_KeyDown(CoreWindow sender, KeyEventArgs args)
{
    if (args.VirtualKey == VirtualKey.Shift) MyRadialMenu.HideAccessKeyTooltips();
}
```

### Adding a Third Arc to Buttons
Let's assume you don't like the buttons we gave you - and you'd rather have buttons with a third arc (right now, you have an inner and an outer arc). Scenarios like that aren't configurable with the current code, but easy to implement yourself. You will have to touch two classes: `RadialMenuButton`, which is the control you use from your application, and `PieSlice`, which is the class we instantiate using a given button.

##### Add A Third Arc to the RadialMenuButton
In `RadialMenuButton.xaml.cs`, go and find all the dependency properties that impact the inner arc - and merely copy those, slightly changing the name. In this example, we're using `MoreInner` instead of `Inner`. Add the following dependency properties to `RadialMenuButton.xaml.cs`:

```
// More Inner Arc Colors
public static readonly DependencyProperty MoreInnerNormalColorProperty =
    DependencyProperty.Register("MoreInnerNormalColor", typeof(Color?), typeof(RadialMenuButton), null);

public static readonly DependencyProperty MoreInnerHoverColorProperty =
    DependencyProperty.Register("MoreInnerHoverColor", typeof(Color?), typeof(RadialMenuButton), null);

public static readonly DependencyProperty MoreInnerTappedColorProperty =
    DependencyProperty.Register("MoreInnerTappedColor", typeof(Color?), typeof(RadialMenuButton), null);

public static readonly DependencyProperty MoreInnerReleasedColorProperty =
    DependencyProperty.Register("MoreInnerReleasedColor", typeof(Color?), typeof(RadialMenuButton), null);

/// <summary>
/// Hover color for the inner portion of the button
/// </summary>
public Color? MoreInnerHoverColor
{
    get { return (Color?)GetValue(MoreInnerHoverColorProperty); }
    set { SetValue(MoreInnerHoverColorProperty, value); }
}

/// <summary>
/// Normal color for the inner portion of the button
/// </summary>
public Color? MoreInnerNormalColor
{
    get { return (Color?)GetValue(MoreInnerNormalColorProperty); }
    set { SetValue(MoreInnerNormalColorProperty, value); }
}

/// <summary>
/// Tapped color for the inner portion of the button
/// </summary>
public Color? MoreInnerTappedColor
{
    get { return (Color?)GetValue(MoreInnerTappedColorProperty); }
    set { SetValue(MoreInnerTappedColorProperty, value); }
}

/// <summary>
/// Released color for the inner portion of the button
/// </summary>
public Color? MoreInnerReleasedColor
{
    get { return (Color?)GetValue(MoreInnerReleasedColorProperty); }
    set { SetValue(MoreInnerReleasedColorProperty, value); }
}
```

Next, we need to add an event handler & delegate for the `Pressed` event - in case you want to somehow handle that the more inner arc has been pressed.

```c#
public delegate void MoreInnerArcPressedEventHandler(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e);
/// <summary>
/// Invoked when the inner arc of the button has been pressed (mouse, touch, stylus)
/// </summary>
public event MoreInnerArcPressedEventHandler MoreInnerArcPressedEvent;

public void OnMoreInnerArcPressed(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
{
    MoreInnerArcPressedEvent?.Invoke(this, e);
}
```

Now that the button has this information, we need to make sure we render it properly. Whenever the menu is given `RadialMenuButtons`, it uses `PieSlices` to actually render objects. Right now, a `PieSlice` consists of two Path objects (`InnerPieSlicePath` and `OuterPieSlicePath`), as well as icon and label objects.

A quick word how the two pieces are drawn: The outer arc element is "thick arc", drawn using two `LineSegments` and two `ArcSegments`. The inner arc is using only three points - one arc (right below the oute arc) and two lines to the center of the control.

To create a third arc (`MoreInnerArc`) that is able to sit on top of the `InnerArc`, we can simply reuse `InnerArc` with a smaller radius. However, nothing keeps you from creating a custom class with a custom `Redraw()` method.

To add the third arc, open up `PieSlice.xaml`. Find the section with the two arcs and add the third arc below them:

```c#
<userControl:OuterPieSlicePath x:Name="OuterPieSlicePath" />
<userControl:InnerPieSlicePath x:Name="InnerPieSlicePath" />
<userControl:InnerPieSlicePath x:Name="MoreInnerPieSlicePath" />
```

Then, open up `PieSlice.xaml.cs`. Here, we need also need to add the above defined color dependency properties. You can simply reuse what you already added to `RadialMenuButton` - just copy in the same dependency properties. Then, go and find the `OnLoaded` method, in which we also configure and setup the two other arcs. Take a look at how we setup the other arcs and configure the third arc to your needs. In the example below, we're configuring the the third arc to have a radius that is 50px smaller than the `InnerArc`.

```c#
private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
{
    // ...

    MoreInnerPieSlicePath.Radius = Radius - OuterArcThickness - 50;
    MoreInnerPieSlicePath.StartAngle = StartAngle;
    MoreInnerPieSlicePath.Angle = Angle;
    MoreInnerPieSlicePath.Fill = new SolidColorBrush(MoreInnerNormalColor);

    // ...
}
```

Make sure to also setup event handlers. You can do that either in code-behind or in XAML, as long as you call the original method on RadialMenuButton (which is referenced in `OriginalRadialMenuButton`). If you want your third inner arc to be animated and have hover/pressed states, go checkout `PieSlice.xaml` - a bunch of visual states are defined and you can easily add additional ones.

```c#
private void innerPieSlicePath_PointerPressed(object sender, PointerRoutedEventArgs e)
{
    OriginalRadialMenuButton.OnMoreInnerArcPressed(e);

    // The line below only works if you previously defined a "MoreInnerPressed" visual state
    // in PieSlice.xaml
    // VisualStateManager.GoToState(this, "MoreInnerPressed", true);

}
```

Well done! Now you have successfully created a third arc inside your button :metal:! You will probably notice that the icons and labels on the inner arc may now be behind your more inner arc - obviously, you can configure those to your liking, too.

### Creating Custom Buttons 
Let's say you need to create custom buttons that are not yet available in the RadialMenu control for your application. You can follow the steps below and use the custom TextBox input button we have created as a guide. In order to add a new custom button, you will need to modify three classes: `RadialMenuButton`, which is the control you use from your application, `PieSlice`, which is the class we instantiate using a given button, and `Pie`, which is the class we instantiate to create all the slices in a pie. 

##### Add a New Button Type for Custom RadialMenuButton
In `RadialMenuButton.xaml.cs`, find the `ButtonType` enum. By adding `MyType` as one of the `ButtonType` enum values, we are able to specify the button type of the `RadialMenuButton` control from our application to instantiate the necessary controls in `PieSlice`. 

```c#
public enum ButtonType
{
    Simple = 0,
    Radio,
    Toggle,
    Custom,
    MyType
};
```
##### Add Controls to the Pie Slice
In `PieSlice.xaml.cs`, from `OnLoad`, add code like the following to check if the application requests for a `MyType` button type `RadialMenuButton`. If so, then programmatically add the necessary controls to `PieSlice`.
```c#
// Setup controls for `MyType` button type RadialMenuButton
if (OriginalRadialMenuButton.Type == RadialMenuButton.ButtonType.MyType) CreateMyTypeControls();
```
Now you can add the necessary controls to `PieSlice` in the `CreateMyTypeControls` function. When done, add these controls to the `TextLabelGrid` to render them. This is probably where you will need to spend some time to ensure your new controls look right in `PieSlice` by configuring its Style, Margin, and Alignment. 

```c#
private void CreateMyTypeControls()
{

    // Your custom controls here ...
    CustomControl = new SomeControl
    {
        Name = "CustomControl",
        FontSize = LabelSize,
        Margin = new Thickness(0, 67, 0, 0),
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
        // ... add more properties to your control

    TextLabelGrid.Children.Add(...);
}
```
##### Interact with Custom Controls When Pie Slice is Pressed
Now that we have the custom controls on our pie slice, in case you need to allow users to interact with the controls, we can enable the controls to focus when the inner pie slice is pressed. In `PieSlice.xaml.cs`, find the `innerPieSlicePath_PointerPressed` event handler. The following ensures your custom control now has focus.

```c#
private void innerPieSlicePath_PointerPressed(object sender, PointerRoutedEventArgs e)
{
    if (OriginalRadialMenuButton.Type == RadialMenuButton.ButtonType.MyType)
    {
        // YourControl.Focus(FocusState.Keyboard);
        e.Handled = true;
    }

    // ...
}
```

##### Pass Value to Custom Control in Pie Slice
For the MyType button custom controls, what if I want to set a default value? Sure you can. First we need to create a dependency property for the value of this control in `PieSlice.xaml.cs`.

```c#
public static readonly DependencyProperty CustomControlValueProperty =
    DependencyProperty.Register("CustomControlValue", typeof(string), typeof(PieSlice), null);
```
Then in `CreateMyTypeControls`, we need to programmatically bind the `CustomControlValue` Dependency property to your custom control's `Text` or `Value` property.

```c#
YourCustomControl.SetBinding(YourCustomControl.TextProperty, new Windows.UI.Xaml.Data.Binding() { Source = this.CustomControlValue });
```
To pass in a value for the dependency property 'CustomControlValueProperty', in 'Pie.xaml.cs', from the `Draw` function, if the slice RadialMenuButton is of type `MyType`, we set the `CustomControlValue` property of this pie slice to the value specified from your application.

```c#
if (slice.Type == RadialMenuButton.ButtonType.MyType)
{
    pieSlice.CustomControlValue = (string)slice.Value;
}
```
Hey! Look at that. You just created your own custom button! Good job! :clap::clap: Now go and create more awesome looking radial menus.  

## License
Copyright (C) 2015 Microsoft, licensed MIT. Please check `LICENSE` for details.
