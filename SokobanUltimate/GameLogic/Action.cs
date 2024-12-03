using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Action(CommandType commandType = CommandType.IDLE, IEntity initiator = null, IntVector2 location = new())
{
    public readonly CommandType CommandType = commandType;
    public readonly IEntity Initiator = initiator;
    public readonly IntVector2 TargetLocation = location;
}