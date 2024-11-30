using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Level : ILevel
{
    private readonly string[] _charsInitialState;
    private readonly int _levelHeight;
    private readonly int _levelWidth;
    public static Action IdleAction = new(CommandType.IDLE);

    private static readonly List<IntVector2> Directions =
    [
        new(1, 0), new(-1, 0),
        new(0, 1), new(0, -1)
    ];
    
    private Player _player;
    
    private IEntity[,] _currentState;
    private List<IEntity> _aliveEntities;
    private List<IEntity> _deadEntities;
    private List<BoxCollector> _collectors;

    public Level(string charMap)
    {
        _charsInitialState = charMap.Split("\r\n");
        _levelHeight = _charsInitialState.Length;
        _levelWidth = _charsInitialState[0].Length;
        _currentState = new IEntity[_levelHeight, _levelWidth];
        SetUpEntitiesOnLevel();
    }
    
    public void Update()
    {
        ProcessAction(_player.Act());
        foreach (var entityAction in GameState.ActionList)
        {
            PerformAction(entityAction);        
        }

        foreach (var entityAction in GameState.ActionList)
        {
            if (entityAction.Key is Box box) UpdateBoxNeighbors(box);
        }
        
        GameState.ActionList.Clear();
        
        /*
         *
         *
         *
         * */
    }

    public bool IsWin()
    {
        return _collectors.All(collector => collector.BoxReceived);
    }

    public bool IsLoss()
    {
        return _aliveEntities.Any(entity => entity.isDead());
    }

    public bool OutOfBounds(IntVector2 coordinates)
        => 0 > coordinates.X || coordinates.X >= _levelWidth || 0 > coordinates.Y || coordinates.Y >= _levelHeight;

    public IEntity[,] GetCurrentState() => _currentState;
    public int GetLevelHeight() => _levelHeight;
    public int GetLevelWidth() => _levelWidth;

    private void SetUpEntitiesOnLevel()
    {
        _aliveEntities = [];
        _deadEntities = [];
        _collectors = [];
        
        for (var i = 0; i < _levelHeight; i++)
        {
            for (var j = 0; j < _levelWidth; j++)
            {
                var entity = GetEntityBySymbol(new IntVector2(j, i), _charsInitialState[i][j]);
                _currentState[i, j] = entity;
                
                if (entity is Player) _player = (Player)entity;
                if (entity is Player or Box) _aliveEntities.Add(entity);
                if (entity is BoxCollector) _collectors.Add((BoxCollector)entity);
            }
        }

        foreach (var box in _aliveEntities.OfType<Box>())
        {
            UpdateBoxNeighbors(box);
        }
    }

    private void UpdateBoxNeighbors(Box box)
    {
        var directionNeighbors = new Dictionary<IntVector2, IEntity>();
        foreach (var direction in Directions)
        {
            var newPosition = box.Coordinates + direction;
            directionNeighbors[direction] =
                OutOfBounds(newPosition) ? null : _currentState[newPosition.Y, newPosition.X];
        }

        box.Neighbors = directionNeighbors;
    }

    private static IEntity GetEntityBySymbol(IntVector2 coordinates, char symbol)
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

    private void ProcessAction(Action action)
    {
        var nextPosition = _player.Coordinates + action.DeltaVector;
        if (action.CommandType is CommandType.IDLE || OutOfBounds(nextPosition)) return;
        var nextPositionEntity = _currentState[nextPosition.Y, nextPosition.X];
        _player.Act(nextPositionEntity, action);
    }

    private void PerformAction(KeyValuePair<IEntity, Action> entityAction)
    {
        if (entityAction.Value.CommandType is not CommandType.MOVE) return;
        var oldPosition = entityAction.Key.Coordinates;
        var newPosition = oldPosition + entityAction.Value.DeltaVector;
        entityAction.Key.Coordinates = newPosition;
        _currentState[oldPosition.Y, oldPosition.X] = new Space(oldPosition);
        _currentState[newPosition.Y, newPosition.X] = entityAction.Key;
    }
}