using Microsoft.Xna.Framework;
using SokobanUltimate.GameLogic.Actions;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Entities;

public class Belt(Point location, Point direction, bool isActive = true)
    : IEntity, IReactive
{
    public Point Location { get; set; } = location;

    public Point Direction { get; set; } = direction;

    public bool IsActive { get; set; } = isActive;

    public Action OnAction(Action action) => new();
    
    public Action React(Action foreignAction, Action ownAction)
    {
        var targetCell = GameState.GetCellByLocation(foreignAction.TargetLocation);
        if (foreignAction.TargetLocation == Location && IsActive &&
            targetCell.Landlord is not Wall && targetCell.GetLastTenant() is not Box)
            return new Action(initiator: foreignAction.Initiator, location:
                foreignAction.TargetLocation + Direction, commandType: CommandType.MOVE);
        return foreignAction;
    }

    public Properties GetProperties()
    {
        throw new System.NotImplementedException();
    }

    public bool isDead() => false;
}