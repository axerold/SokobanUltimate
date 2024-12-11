using SokobanUltimate.GameLogic.Actions;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Entities;

public class Belt : IEntity, IReactive
{
    public IntVector2 Location { get; set; }

    public IntVector2 Direction { get; set; }
    
    public bool IsActive { get; set; }

    public Action OnAction(Action action) => new();
    
    public Action React(Action foreignAction, Action ownAction)
    {
        if (foreignAction.TargetLocation == Location && IsActive)
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