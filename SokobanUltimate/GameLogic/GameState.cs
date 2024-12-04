using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SokobanUltimate.GameLogic;

public class GameState
{
    private static ILevel _currentLevel;

    private float cooldownTimer;
    private float moveCoolDown = 0.12f;
    public static float LevelTimer { get; private set; }
    private static LevelState _state;

    public void LoadLevel(string charLevelMap)
    {
        _currentLevel = new Level(charLevelMap);
        LevelTimer = 0.0f;
        _state = LevelState.Running;
    }

    public void UpdateLevel(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        cooldownTimer -= deltaTime;
        if (_state is LevelState.Running)
            LevelTimer += deltaTime;
        if (!(cooldownTimer <= 0.0f)) return;
        cooldownTimer = moveCoolDown;
        switch (_state)
        {
            case LevelState.Running:
                _currentLevel.Update();
                break;
            case LevelState.Paused:
                //TODO
                break;
            case LevelState.Win:
                //TODO
                break;
            case LevelState.Loss:
                //TODO
                break;
        }
        
        CheckState();
    }
    
    public static DrawingInstruction GetDrawingInstruction()
    {
        return _state switch
        {
            LevelState.Running => new DrawingInstruction(drawTime: true, drawStepsCounter: true),
            LevelState.Paused => new DrawingInstruction(drawTime: true, drawStepsCounter: true, drawPauseMenu: true),
            LevelState.Win => new DrawingInstruction(drawWinScreen: true),
            LevelState.Loss => new DrawingInstruction(drawLossScreen: true),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void CheckState()
    {
        if (_currentLevel.IsWin())
            _state = LevelState.Win;
        if (_currentLevel.IsLoss())
            _state = LevelState.Loss;
        
        if (_state is LevelState.Running && Keyboard.GetState().IsKeyDown(Keys.Escape))
            _state = LevelState.Paused;
        else if (_state is LevelState.Paused && Keyboard.GetState().IsKeyDown(Keys.Escape))
            _state = LevelState.Running;
    }

    public static ILevel GetCurrentLevel() => _currentLevel;
}