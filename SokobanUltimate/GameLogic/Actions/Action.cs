using Microsoft.Xna.Framework;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Actions;

public class Action(CommandType commandType = CommandType.IDLE, IEntity initiator = null, Point location = new())
{
    public readonly CommandType CommandType = commandType;
    public readonly IEntity Initiator = initiator;
    public readonly Point TargetLocation = location;
    public readonly Point StartLocation = initiator?.Location ?? new Point();
}