using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Serilog;
using SokobanUltimate.Drawing;
using SokobanUltimate.GameLogic.Interfaces;
using SokobanUltimate.GameLogic.Levels;
using Action = SokobanUltimate.GameLogic.Actions.Action;


namespace SokobanUltimate.GameLogic;

public class GameState
{
    private static ILevel _currentLevel;
    private static string currentLevelMap;
    private static Stack<Queue<Action>> TurnHistory = new();

    private float _cooldownTimer;
    public static readonly float MoveCoolDown = 0.12f;
    private int _previousStepCounter;
    public static float LevelTimer { get; private set; }
    public static LevelState State { get; private set; }

    public static bool RestartActivated { get; set; }

    public static void LoadLevel(string charLevelMap)
    {
        _currentLevel = new Level(charLevelMap);
        currentLevelMap = charLevelMap;
        LevelTimer = 0.0f;
        State = LevelState.Running;
        TurnHistory.Clear();
    }

    public void UpdateLevel(GameTime gameTime)
    {
        if (_currentLevel is null) return;
        
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _cooldownTimer -= deltaTime;
        if (State is LevelState.Running)
            LevelTimer += deltaTime;
        if (!(_cooldownTimer <= 0.0f)) return;
        _cooldownTimer = MoveCoolDown;
        switch (State)
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
        return State switch
        {
            LevelState.Running => new DrawingInstruction(drawTime: true, drawStepsCounter: true, drawBoxDelivered: true),
            LevelState.Paused => new DrawingInstruction(drawTime: true, drawStepsCounter: true, drawBoxDelivered: true,
                drawPauseMenu: true),
            LevelState.Win => new DrawingInstruction(drawWinScreen: true),
            LevelState.Loss => new DrawingInstruction(drawLossScreen: true),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public static ILevel GetCurrentLevel() => _currentLevel;

    public static void UpdateHistory()
    {
        TurnHistory.Push(new Queue<Action>(((Level)_currentLevel).GetActionList()));
    }

    public static Queue<Action> GetLastTurnActions(Level level)
    {
        if (level != _currentLevel) return null;
        TurnHistory.TryPop(out var lastTurn);
        return lastTurn;
    }
    
    public static Cell GetCellByLocation(IntVector2 location) => _currentLevel.Cells[location.Y, location.X];

    private void CheckState()
    {
        if (_currentLevel.StepCounter > _previousStepCounter)
        {
            if (_currentLevel.IsWin())
                State = LevelState.Win;
            if (_currentLevel.IsLoss())
                State = LevelState.Loss;
            _previousStepCounter = _currentLevel.StepCounter;
        }

        if (State is LevelState.Running && Keyboard.GetState().IsKeyDown(Keys.Escape))
            State = LevelState.Paused;
        else if (State is LevelState.Paused && Keyboard.GetState().IsKeyDown(Keys.Escape))
            State = LevelState.Running;

        if (State is not LevelState.Paused && Keyboard.GetState().IsKeyDown(Keys.R))
        {
            RestartActivated = true;
            LoadLevel(currentLevelMap);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Z))
        {
            switch (State)
            {
                case LevelState.Running:
                case LevelState.Win:
                    break;
                case LevelState.Paused:
                    return;
                case LevelState.Loss:
                    State = LevelState.Running;
                    if (_currentLevel is Level level)
                    {
                        level.RestoreBoxes();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            ((Level)_currentLevel).UndoTurn();
            _previousStepCounter = _currentLevel.StepCounter;
        }
    }
}