using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Actions;

public class Action(CommandType commandType = CommandType.IDLE, IEntity initiator = null, IntVector2 location = new())
{
    public readonly CommandType CommandType = commandType;
    public readonly IEntity Initiator = initiator;
    public readonly IntVector2 TargetLocation = location;
    public readonly IntVector2 StartLocation = initiator?.Location ?? new IntVector2();
}