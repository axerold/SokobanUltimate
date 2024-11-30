using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Box(IntVector2 coordinates) : IEntity
{
    public IntVector2 Coordinates
    {
        get => coordinates;
        set => coordinates = value;
    }

    private Dictionary<IntVector2, IEntity> neighbors;

    public Dictionary<IntVector2, IEntity> Neighbors
    {
        set
        {
            foreach (var directionNeighbor in value)
            {
                if (directionNeighbor.Value is not null
                    && directionNeighbor.Value.Coordinates != coordinates + directionNeighbor.Key)
                    throw new ValidationException("Incorrect neighborhood");
            }
            neighbors = value;
        }
    }
    
    public Action ActedBy(IEntity entity, Action action)
    {
        var actualAction = Level.IdleAction;
        if (entity is Player)
        {
            var neighbor = neighbors[action.DeltaVector];
            actualAction = neighbor is null ? Level.IdleAction : neighbor.ActedBy(this, action);
        }
        GameState.ActionList.Add(new(this, actualAction));
        return actualAction;
    }

    public Properties GetProperties() => new(this);

    public bool isDead()
    {
        var deadEndCount = neighbors.Count(
            directionNeighbor => directionNeighbor.Value is null or Wall or Box);
        return deadEndCount >= 2;
    }
}