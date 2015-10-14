## Radial Menu
A Radial Menu for Windows UWP Applications, as made popular by the first versions of the modern OneNote App for Windows. Create radial menus floating op top of your application. The control supports variable numbers of buttons, toggle & radio buttons, a selector for long lists, and a fancy metered menu for intuitive selection of numbers.

!['Fancy GIF showing the control'](https://i.imgur.com/JVcoOzU.gif)

## Usage
At the core, this control comes with three user controls: The first one is a "floating" control, enabling a child control to float on top of all other elements, which allows the user to move the control around on the screen. The second one is the RadialMenuControl itself, which is able to house a number of RadialMenuButtons. Should a button contain a submenu, the button then houses a RadialMenuControl - and so on. You can have a virtually unlimited number of submenus.

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
