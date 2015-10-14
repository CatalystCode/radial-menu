## Radial Menu
A Radial Menu for Windows UWP Applications, as made popular by the first versions of the modern OneNote App for Windows. Create radial menus floating op top of your application. The control supports variable numbers of buttons, toggle & radio buttons, a selector for long lists, and a fancy metered menu for intuitive selection of numbers.

!['Fancy GIF showing the control'](https://i.imgur.com/JVcoOzU.gif)

## Usage
At the core, this control comes with three user controls: The first one is a "floating" control, enabling a child control to float on top of all other elements, which allows the user to move the control around on the screen. The second one is the RadialMenuControl itself, which is able to house a number of RadialMenuButtons. Should a button contain a submenu, the button then houses a RadialMenuControl - and so on. You can have a virtually unlimited number of submenus.

#### Adding the Control
You can instantiate the control either using XAML or in code-behind. In both cases, buttons are added by adding instances of `RadialMenuButton` to the `Buttons` property of your radial menu.

```C
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

#### Seamless Background
If you give RadialMenuButtons the same background color, you may have noticed that Windows will render a 1px thick seam between the individual buttons. Those seams show up when shapes fight for the same pixel.

To get around this issue, you can set the option `HasBackgroundEllipse` on the `RadialMenu` to `true`. In that mode, the menu will render a full ellipse behind your whole control. Since the ellipse is one shape, no seams will be visible. Obviously, for the ellipse to function as background, you will need to set the `InnerNormalColor` of your menu and your buttons to a near invisible color (we recommend `#02FFFFFF`).

```C
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

#### Access Keys
The control comes with a number of helper properties to enable access key behaviour. To ensure that the control doesn't mess with your application, it does not access any global properties, meaning that you will have capture `KeyDown` events yourself. The good news: Helper methods on the control make enabling access keys really easy.

To implement access keys, start with the RadialMenuButtons: Give your buttons two properties each, `InnerAccessKey` and `OuterAccessKey` (for instance `M` and `P`). To show little tooltips with the access key, call `ShowAccessKeyTooltips()` on the RadialMenu. To hide them, call `HideAccessKeyTooltips()`. In order to programmatically click either the inner or the outer portion of a RadialMenuButton, call `ClickInnerRadialMenuButton(RadialMenuButton)` or `ClickOuterRadialMenuButton(RadialMenuButton)`, To wire this all up in your application, consult the following example code:

Add a RadialMenuButton to your RadialMenu, passing in access keys:

```C
<RadialMenuButton x:Name="Pen1" Label="Pen" Icon="" IconFontFamily="Segoe MDL2 Assets" Type="Toggle" InnerAccessKey="K" OuterAccessKey="L" />
```

Then, wire up a KeyDown handler for your application. Depending on the key, we either display the little tooltips - or programmatically perform actions on buttons.
```C
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
```C
private void Melbourne_KeyDown(CoreWindow sender, KeyEventArgs args)
{
    if (args.VirtualKey == VirtualKey.Shift) MyRadialMenu.HideAccessKeyTooltips();
}
```

## License
Copyright (C) 2015 Microsoft, licensed MIT. Please check `LICENSE` for details.
