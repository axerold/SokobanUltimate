using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Action
{
    public readonly CommandType CommandType;
    public readonly Vector2 DeltaVector;
    public readonly IEntity InteractedEntity;

    public Action(CommandType commandType, Vector2 vector2 = new(), IEntity entity = null)
    {
        CommandType = commandType;
        DeltaVector = vector2;
        InteractedEntity = entity;
    }
}