using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Action
{
    public readonly CommandType CommandType;
    public readonly IntVector2 DeltaVector;
    public readonly IEntity InteractedEntity;

    public Action(CommandType commandType, IntVector2 vector2 = new(), IEntity entity = null)
    {
        CommandType = commandType;
        DeltaVector = vector2;
        InteractedEntity = entity;
    }
}