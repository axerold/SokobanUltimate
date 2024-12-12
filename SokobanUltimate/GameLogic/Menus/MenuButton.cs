using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using SokobanUltimate.GameLogic.Actions;

namespace SokobanUltimate.GameLogic.Menus;

public class MenuButton
{
    public MenuButton(string text, string action, int x, int y, int width, int height)
    {
        Text = text;
        Action = action;
        Bounds = new Rectangle(x, y, width, height);
    }

    [JsonIgnore]
    public Rectangle Bounds { get; private set; }

    public string Text { get; }
    public string Action { get; }
    
    public int X { get; }
    public int Y { get; }
    
    public int Width { get; }
    public int Height { get; }
}