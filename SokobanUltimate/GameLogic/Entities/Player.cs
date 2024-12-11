using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Serilog;
using SokobanUltimate.Drawing;
using SokobanUltimate.GameLogic.Actions;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Entities;

public class Player : IEntity, IActive
{
    private IntVector2 _coordinates;
    private Action _lastAction;

    public Player(IntVector2 coordinates)
    {
        _coordinates = coordinates;
        LastDirection = AnimationManager.Directions[0];
    }

    public IntVector2 Location
    {
        get => _coordinates;
        set => _coordinates = value;
    }

    public IntVector2 LastDirection
    {
        get;
        set;
    }

    public Action LastAction
    {
        get => _lastAction ?? new Action();
        set => _lastAction = value;
    }

    public static readonly Dictionary<Keys, IntVector2> KeysToDirections = new()
    {
        { Keys.W, new IntVector2(0, -1) }, { Keys.S, new IntVector2(0, 1) },
        { Keys.A, new IntVector2(-1, 0) }, { Keys.D, new IntVector2(1, 0) }
    };

    public Action OnAction(Action action)
    {
        return new Action();
    }

    public Properties GetProperties() => new(this);

    public bool isDead() => false;
    
    public Action Act()
    {
        var keyboardState = Keyboard.GetState();
        foreach (var (key,direction) in KeysToDirections)
            if (keyboardState.IsKeyDown(key))
            {
                var newLocation = _coordinates + direction;
                LastDirection = direction;
                return new Action(CommandType.MOVE, this, newLocation);
            }

        return new Action(CommandType.IDLE, this, Location);
    }
}