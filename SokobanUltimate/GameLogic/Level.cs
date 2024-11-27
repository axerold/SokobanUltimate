using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Level : ILevel
{
    public readonly char[,] CharsInitialState;
    public readonly int LevelHeight;
    public readonly int LevelWidth;
    
    private Player _player;
    private IEntity[,] _currentState;
    private List<IEntity> _aliveEntities;
    private List<IEntity> _deadEntities;
    private List<BoxCollector> _collectors;

    public Level(char[,] charMap)
    {
        CharsInitialState = charMap;
        LevelHeight = charMap.GetLength(0);
        LevelWidth = charMap.GetLength(1);
        _currentState = new IEntity[LevelHeight, LevelWidth];
        SetUpEntitiesOnLevel();
    }
    
    public void Update()
    {
        throw new System.NotImplementedException();
    }

    public bool IsWin()
    {
        return _collectors.All(collector => collector.BoxReceived);
    }

    public bool IsLoss()
    {
        return _deadEntities.Any(entity => entity is Player or Box);
    }

    private void SetUpEntitiesOnLevel()
    {
        for (var i = 0; i < LevelHeight; i++)
        {
            for (var j = 0; j < LevelWidth; j++)
            {
                var entity = GetEntityBySymbol(new Vector2(j, i), CharsInitialState[i, j]);
                _currentState[i, j] = entity;
                
                if (entity is Player) _player = (Player)entity;
                if (entity is Player or Box) _aliveEntities.Add(entity);
                if (entity is BoxCollector) _collectors.Add((BoxCollector)entity);
            }
        }
    }

    private IEntity GetEntityBySymbol(Vector2 vector2, char symbol)
    {
        return symbol switch
        {
            'P' => new Player(),
            'W' => new Wall(),
            'B' => new Box(),
            'C' => new BoxCollector(),
            ' ' => new Space(),
            _ => throw new ArgumentException("Unexpected map symbol")
        };
    }
}