using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using SokobanUltimate.Control;
using SokobanUltimate.GameLogic.Actions;
using SokobanUltimate.GameLogic.Entities;
using SokobanUltimate.GameLogic.Interfaces;
using Action = SokobanUltimate.GameLogic.Actions.Action;
using Point = Microsoft.Xna.Framework.Point;

namespace SokobanUltimate.GameLogic.Levels;

public class Level : ILevel
{
    private readonly string[] _charsInitialState;
    public int LevelHeight { get; }
    public int LevelWidth { get; }
    
    public int StepCounter { get; private set; }

    public static readonly Dictionary<string, Point> Directions = new()
    {
        {"right", new Point(1, 0)},{"down", new Point(0, 1)},
        {"left", new Point(-1, 0)}, {"up", new Point(0, -1)}
    };
    
    private Player _player;
    public Cell[,] Cells { get; private set; }
    private List<Action> _actionList = [];
    
    private List<IEntity> _aliveEntities = [];
    private List<IEntity> _deadEntities = [];
    private List<Cell> _collectorsCells = [];

    public Level(string charMap)
    {
        _charsInitialState = charMap.Split("\r\n");
        LevelHeight = _charsInitialState.Length;
        LevelWidth = _charsInitialState[0].Length;
        InitializeCells();
    }
    
    public void Update(List<Query> queries)
    {
        PerformActions(queries);
    }

    public bool IsWin()
    {
        return _collectorsCells.All(collectorCell => ((BoxCollector)collectorCell.Landlord).BoxReceived);
    }

    public bool IsLoss()
    {
        return _deadEntities.Any(entity => entity is Box or Player);
    }

    public List<Action> GetActionList() => _actionList;

    public void RestoreBoxes()
    {
        if (GameState.State is LevelState.Running)
            _deadEntities.Clear();
    }
    
    public void UndoTurn()
    {
        var queue = GameState.GetLastTurnActions(this);
        if (queue is null) return;
        while (queue.Count > 0)
        {
            var action = queue.Dequeue();
            if (action.Initiator is Player player)
            {
                player.LastAction = action;
                player.LastDirection = action.TargetLocation - action.StartLocation;
            }

            if (action.CommandType is CommandType.MOVE)
            {
                Log.Debug("{CP}, {PP}", action.Initiator.Location, action.StartLocation);
                Move(action.Initiator, action.StartLocation, true);
            }
        }
    }

    public List<Cell> GetCollectorsCells() => _collectorsCells;

    private bool OutOfBounds(Point coordinates)
        => 0 > coordinates.X || coordinates.X >= LevelWidth || 0 > coordinates.Y || coordinates.Y >= LevelHeight;

    private void InitializeCells()
    {
        Cells = new Cell[LevelHeight, LevelWidth];
        for (var i = 0; i < LevelHeight; i++)
        {
            for (var j = 0; j < LevelWidth; j++)
            {
                var location = new Point(j, i);
                var entity = GetEntityBySymbol(location, _charsInitialState[i][j]);
                Cells[i, j] = new Cell(entity, location);
                switch (entity)
                {
                    case Player player:
                        _player = player;
                        break;
                    case Box:
                        _aliveEntities.Add(entity);
                        break;
                    case BoxCollector:
                        _collectorsCells.Add(Cells[i, j]);
                        break;
                }
            }
        }
    }
    
    private static IEntity GetEntityBySymbol(Point coordinates, char symbol)
    {
        return symbol switch
        {
            'P' => new Player(coordinates),
            'W' => new Wall(coordinates),
            'B' => new Box(coordinates),
            'C' => new BoxCollector(coordinates),
            ' ' => new Space(coordinates),
            _ => throw new ArgumentException("Unexpected map symbol")
        };
    }

    private Action ProcessAction(List<Query> queries)
    {
        var idleAction = new Action(CommandType.IDLE, _player, _player.Location);
        if (queries is null || queries.Count > 1)
            return idleAction;
        var query = queries[0];
        
        if (query.Command == "move" && Directions.TryGetValue(query.Info, out var targetLocation))
        {
            return new Action(CommandType.MOVE, _player, _player.Location + targetLocation);
        }

        return idleAction;

    }

    private void PerformActions(List<Query> queries)
    {
        var playerAction = ProcessAction(queries);
        if (playerAction is null) return;
        _actionList.Add(playerAction);
        var targetCell = Cells[playerAction.TargetLocation.Y, playerAction.TargetLocation.X];
        foreach (var tenant in targetCell.Tenants)
        {
            _actionList.Add(tenant.OnAction(playerAction));
            if (tenant is IReactive reactive) 
                _actionList[0] = reactive.React(playerAction, _actionList.Last());
        }
        
        _player.LastAction = _actionList[0];
        for (var i = _actionList.Count - 1; i >= 0; i--)
        {
            if (_actionList[i].CommandType is CommandType.MOVE)
                Move(_actionList[i].Initiator, _actionList[i].TargetLocation);
        }
        
        if (_actionList[0].CommandType is CommandType.MOVE)
            GameState.UpdateHistory();
        _actionList.Clear();
    }

    private void Move(IEntity entity, Point targetLocation, bool isRewind = false)
    {
        if (Cell.IsLandlord(entity) || entity.Location == targetLocation || OutOfBounds(targetLocation)) 
            return;
        
        var currentCell = Cells[entity.Location.Y, entity.Location.X];
        var targetCell = Cells[targetLocation.Y, targetLocation.X];
        if (targetCell.Landlord is Wall) 
            return;
        
        if (entity is Player)
        {
            switch (isRewind)
            {
                case true:
                    StepCounter--;
                    break;
                case false:
                    StepCounter++;
                    break;
            }

            _player.LastDirection = targetLocation - _player.Location;
        }
        currentCell.RemoveTenant(entity);
        targetCell.AddTenant(entity);
        entity.Location = targetLocation;

        if (targetCell.Landlord is BoxCollector targetCollector)
            targetCollector.BoxReceived = true;
        if (currentCell.Landlord is BoxCollector currentCollector)
            currentCollector.BoxReceived = false;
        if (entity is not Box || !entity.isDead()) return;
        _deadEntities.Add(entity);
        _aliveEntities.Remove(entity);
    }
}