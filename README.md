## Radial Menu
A Radial Menu for Windows UWP Applications, as made popular by the first versions of the modern OneNote App for Windows. Create radial menus floating op top of your application. The control supports variable numbers of buttons, toggle & radio buttons, a selector for long lists, and a fancy metered menu for intuitive selection of numbers.

!['Fancy GIF showing the control'](http://im.ezgif.com/tmp/ezgif-2737883079.gif)

## Usage
At the core, this control comes with three user controls: The first one is a "floating" control, enabling a child control to float on top of all other elements, which allows the user to move the control around on the screen. The second one is the RadialMenuControl itself, which is able to house a number of RadialMenuButtons. Should a button contain a submenu, the button then houses a RadialMenuControl - and so on. You can have a virtually unlimited number of submenus.

#### Adding the Control
You can instantiate the control either using XAML or in code-behind. In both cases, buttons are added by adding instances of `RadialMenuButton` to the `Buttons` property of your radial menu.

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

## License
Copyright (C) 2015 Microsoft, licensed MIT. Please check `LICENSE` for details.
