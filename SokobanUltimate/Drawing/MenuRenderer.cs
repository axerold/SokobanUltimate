using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;
using SokobanUltimate.GameLogic.Menus;

namespace SokobanUltimate.Drawing;

public class MenuRenderer
{
    private SpriteFont _font;

    public MenuRenderer(SpriteFont font)
    {
        _font = font;
    }

    public void Render(SpriteBatch batch, TextureManager manager)
    {
        var title = "SOKOBAN Ultimate";
        var titleSize = _font.MeasureString(title);
        var titlePosition = new Vector2((800 - titleSize.X) / 2, 100);
        batch.DrawString(_font, title, titlePosition, Color.White);
        
        foreach (var button in MenuManager.CurrentMenu.Buttons)
        {
            batch.Draw(manager.GetTextureForUI("button"), button.Bounds, Color.White);
            var textSize = _font.MeasureString(button.Text);
            var textPosition = new Vector2(button.Bounds.X + (button.Bounds.Width - textSize.X) / 2,
                button.Bounds.Y + (button.Bounds.Height - textSize.Y) / 2);
            
            batch.DrawString(_font, button.Text, textPosition, Color.Black);
        }
    }
}