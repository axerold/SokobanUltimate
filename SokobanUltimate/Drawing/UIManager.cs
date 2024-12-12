using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SokobanUltimate.GameLogic;
using SokobanUltimate.GameLogic.Entities;
using SokobanUltimate.GameLogic.Levels;

namespace SokobanUltimate.Drawing;

public class UIManager
{
    private SpriteFont _statusFont;
    private int _startXCoordinate;
    private int _startYCoordinate;
    private int _cellSize;

    public UIManager(SpriteFont font, int CellSize, int indent)
    {
        _statusFont = font;
        _cellSize = CellSize;
        _startXCoordinate = CellSize * (GameState.GetCurrentLevel().LevelWidth + indent);
        _startYCoordinate = CellSize * (GameState.GetCurrentLevel().LevelHeight + indent);
    }

    public void DrawUI(SpriteBatch batch)
    {
        var instruction = GameState.GetDrawingInstruction();
        if (instruction.DrawTime) DrawTime(batch, new Vector2(_startXCoordinate, 0));
        if (instruction.DrawStepsCounter) DrawStepsCount(batch, new Vector2(_startXCoordinate, _cellSize));
        if (instruction.DrawBoxDelivered) DrawBoxDelivered(batch, new Vector2(0, _startYCoordinate));
        if (instruction.DrawPauseMenu) DrawPauseMenu(batch);
        if (instruction.DrawWinScreen) DrawWinScreen(batch);
        if (instruction.DrawLossScreen) DrawLossScreen(batch);
    }

    private void DrawText(SpriteBatch batch, Vector2 position, string text)
    {
        batch.DrawString(_statusFont, text, position, Color.White);
    }

    private void DrawTime(SpriteBatch batch, Vector2 position)
    {
        batch.DrawString(_statusFont, GetTimeInMinutesAndSeconds(), position, Color.White);
    }

    private string GetTimeInMinutesAndSeconds()
    {
        var seconds = (int)GameState.LevelTimer;
        var minutes = seconds / 60;
        seconds %= 60;
        return minutes == 0 ? $"{seconds}" : $"{FormatCorrector(minutes)}:{FormatCorrector(seconds)}";
    }

    private static string FormatCorrector(int minutesOrSeconds) => minutesOrSeconds < 10
        ? $"0{minutesOrSeconds}"
        : $"{minutesOrSeconds}";
        
    private void DrawStepsCount(SpriteBatch batch, Vector2 position)
    {
        var text = $"{GameState.GetCurrentLevel().StepCounter} STEPS";
        batch.DrawString(_statusFont, text, position, Color.White);
    }

    private void DrawBoxDelivered(SpriteBatch batch, Vector2 position)
    {
        var collectorCells = ((Level)GameState.GetCurrentLevel()).GetCollectorsCells();
        var activatedCollectors = collectorCells.Count(cell => ((BoxCollector)cell.Landlord).BoxReceived);
        var text = $"BOX DELIVERED: {activatedCollectors} / {collectorCells.Count}";
        batch.DrawString(_statusFont, text, position, Color.White);
    }

    private void DrawPauseMenu(SpriteBatch batch)
    {
        const string text = "PAUSED";
        batch.DrawString(_statusFont, text, GetCenterPosition(text), Color.White);
    }

    private void DrawWinScreen(SpriteBatch batch)
    {
        var text = "    YOU WON     \n" +
                   $"ELAPSED TIME: {GetTimeInMinutesAndSeconds()}\n" +
                   $"STEPS MADE: {GameState.GetCurrentLevel().StepCounter}";
        batch.DrawString(_statusFont, text, GetCenterPosition(text), Color.White);    
    }

    private void DrawLossScreen(SpriteBatch batch)
    {
        const string text = $"     LEVEL IS OVER    \n" +
                            $" PRESS 'R' TO RESTART \n" +
                            $"PRESS 'Z' TO STEP BACK";
        batch.DrawString(_statusFont, text, GetCenterPosition(text), Color.White);
    }

    private Vector2 GetCenterPosition(string text)
    {
        var size = _statusFont.MeasureString(text);
        var position = new Vector2((GameState.GetCurrentLevel().LevelWidth * _cellSize - size.X) / 2,
            (GameState.GetCurrentLevel().LevelHeight * _cellSize - size.Y) / 2);
        return position;
    }

}