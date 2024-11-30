using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SokobanUltimate.GameLogic;

public class Player(IntVector2 coordinates) : IEntity, IActive
{
    public IntVector2 Coordinates
    {
        get => coordinates;
        set => coordinates = value;
    }

    public static readonly Dictionary<Keys, IntVector2> KeysToDirections = new()
    {
        { Keys.W, new IntVector2(0, -1) }, { Keys.S, new IntVector2(0, 1) },
        { Keys.A, new IntVector2(-1, 0) }, { Keys.D, new IntVector2(1, 0) }
    };

    public Action ActedBy(IEntity entity, Action action) => Level.IdleAction;
    
    public Properties GetProperties() => new(this);

    public bool isDead() => false;
    
    public Action Act()
    {
        var keyboardState = Keyboard.GetState();
        foreach (var keyToDirection in KeysToDirections)
            if (keyboardState.IsKeyDown(keyToDirection.Key))
                return new Action(CommandType.MOVE, keyToDirection.Value);
        return Level.IdleAction;
    }

    public Action Act(IEntity entity, Action action)
    {
        var finalAction = entity.ActedBy(this, action);
        GameState.ActionList.Add(new(this, finalAction));
        return finalAction;
    }
}