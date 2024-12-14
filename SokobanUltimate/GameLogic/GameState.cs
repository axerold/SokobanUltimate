using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Serilog;
using SokobanUltimate.Control;
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
        if (!Timer(gameTime)) return;
        var queries = KeyboardManager.ObtainKeyboardQueries();
        if (!ProcessQueries(queries) && State is LevelState.Running)
            _currentLevel.Update(queries);
        
        ChangeState();
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
    
    public static Cell GetCellByLocation(Point location) => _currentLevel.Cells[location.Y, location.X];

    private void ChangeState()
    {
        if (_currentLevel.StepCounter > _previousStepCounter)
        {
            if (_currentLevel.IsWin())
                State = LevelState.Win;
            if (_currentLevel.IsLoss())
                State = LevelState.Loss;
            _previousStepCounter = _currentLevel.StepCounter;
        }
    }

    private bool ProcessQueries(List<Query> queries)
    {
        if (queries?.Count != 1) return false;
        switch (queries[0].Command)
        {
            case "restart":
                if (State is not LevelState.Paused)
                    Restart();
                return true;
            case "undo":
                Undo();
                return true;
            case "switch":
                if (queries[0].Info == "pause")
                    SwitchPause();
                return true;
        }

        return false;
    }

    private bool Timer(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _cooldownTimer -= deltaTime;
        if (State is LevelState.Running)
            LevelTimer += deltaTime;
        if (!(_cooldownTimer <= 0.0f)) return false;
        _cooldownTimer = MoveCoolDown;
        return true;
    }

    private void Restart()
    {
        RestartActivated = true;
        _previousStepCounter = 0;
        LoadLevel(currentLevelMap);    
    }

    private void Undo()
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

    private static void SwitchPause()
    {
        switch (State)
        {
            case LevelState.Running:
                State = LevelState.Paused;
                break;
            case LevelState.Paused:
                State = LevelState.Running;
                break;
        }    
    }
}