using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SokobanUltimate.GameLogic.Menus;

public class Menu
{
    public string Title { get; }
    public List<MenuButton> Buttons { get; }

    public Menu(string title, List<MenuButton> buttons)
    {
        Title = title;
        Buttons = buttons;
    }

    public string CheckButtonPressed()
    {
        var mouseState = Mouse.GetState();
        var mousePosition = new Point(mouseState.X, mouseState.Y);

        return (from button in Buttons where button.Bounds.Contains(mousePosition) 
            where mouseState.LeftButton == ButtonState.Pressed select button.Action).FirstOrDefault();
    }
}