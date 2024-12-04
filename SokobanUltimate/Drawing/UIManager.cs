using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SokobanUltimate.GameLogic;

namespace SokobanUltimate.Drawing;

public class UIManager
{
    private SpriteFont _statusFont;
    private int _startXCoordinate;
    private int _cellSize;
    private Vector2 Center;
    public UIManager(SpriteFont font, int CellSize, int indent)
    {
        _statusFont = font;
        _cellSize = CellSize;
        _startXCoordinate = CellSize * (GameState.GetCurrentLevel().LevelWidth + indent);
        var middleXCoordinate = CellSize * (GameState.GetCurrentLevel().LevelWidth / 2);
        var middleYCoordinate = CellSize * (GameState.GetCurrentLevel().LevelHeight / 2);
        Center = new Vector2(middleXCoordinate, middleYCoordinate);
    }

    public void DrawUI(SpriteBatch batch)
    {
        var instruction = GameState.GetDrawingInstruction();
        if (instruction.DrawTime) DrawTime(batch, new Vector2(_startXCoordinate, 0));
        if (instruction.DrawStepsCounter) DrawStepsCount(batch, new Vector2(_startXCoordinate, _cellSize));
        if (instruction.DrawPauseMenu) DrawPauseMenu(batch, Center);
        if (instruction.DrawWinScreen) DrawWinScreen(batch, Center);
        if (instruction.DrawLossScreen) DrawLossScreen(batch, Center);
    }

    private void DrawTime(SpriteBatch batch, Vector2 position)
    {
        var text = $"{(int)GameState.LevelTimer} s";
        batch.DrawString(_statusFont, text, position, Color.White);
    }

    private void DrawStepsCount(SpriteBatch batch, Vector2 position)
    {
        var text = $"{GameState.GetCurrentLevel().StepCounter} STEPS";
        batch.DrawString(_statusFont, text, position, Color.White);
    }

    private void DrawPauseMenu(SpriteBatch batch, Vector2 position)
    {
        const string text = "PAUSED";
        batch.DrawString(_statusFont, text, position, Color.White);
    }

    private void DrawWinScreen(SpriteBatch batch, Vector2 position)
    {
        var text = "    YOU WON     \n" +
                   $"ELAPSED TIME: {(int)GameState.LevelTimer}\n" +
                   $"STEPS MADE: {GameState.GetCurrentLevel().StepCounter}";
        batch.DrawString(_statusFont, text, position, Color.White);    
    }

    private void DrawLossScreen(SpriteBatch batch, Vector2 position)
    {
        const string text = $"       LEVEL IS OVER        \n" +
                            $"    PRESS 'R' TO RESTART    \n" +
                            $"PRESS BACKSPACE TO STEP BACK";
        batch.DrawString(_statusFont, text, position, Color.White);
    }

}